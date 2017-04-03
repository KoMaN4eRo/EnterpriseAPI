using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using EnterpriseAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Controllers;
using SimpleInjector.Integration.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore;
using EnterpriseAPI.Models.OrganizationModel;
using EnterpriseAPI.Models.CountryModel;
using EnterpriseAPI.Models.BusinessModel;
using EnterpriseAPI.Models.FamilyModel;
using EnterpriseAPI.Models.OfferingModel;
using EnterpriseAPI.Models.DepartmentModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using System.Globalization;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using EnterpriseAPI.Models.UserModel;
using Microsoft.AspNetCore.Session;

namespace EnterpriseAPI
{
    public class Startup
    {
        private Container container = new Container();

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsEnvironment("Development"))
            {
                // This will push telemetry data through Application Insights pipeline faster, allowing you to view results immediately.
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            string con = @"Server=(localdb)\mssqllocaldb;Database=EnterpriseAPIdb;Trusted_Connection=True;";
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(con));
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddMvc();
            //services.AddCaching();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.CookieName = ".MyApplication";
            });

            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(container));
            services.AddAuthentication(
    options => options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseSession();
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                LoginPath = new PathString("/login"),
                LogoutPath = new PathString("/logout")
            });

            app.UseOAuthAuthentication(new OAuthOptions
            {
                // We need to specify an Authentication Scheme
                AuthenticationScheme = "LinkedIn",
                // Configure the LinkedIn Client ID and Client Secret
                ClientId = Configuration["linkedin:clientId"],
                ClientSecret = Configuration["linkedin:clientSecret"],
                // Set the callback path, so LinkedIn will call back to http://APP_URL/signin-linkedin 
                // Also ensure that you have added the URL as an Authorized Redirect URL in your LinkedIn application
                CallbackPath = new PathString("/signin-linkedin"),
                // Configure the LinkedIn endpoints                
                AuthorizationEndpoint = "https://www.linkedin.com/oauth/v2/authorization",
                TokenEndpoint = "https://www.linkedin.com/oauth/v2/accessToken",
                //UserInformationEndpoint = "https://api.linkedin.com/v1/people/~:(id,formatted-name,email-address,picture-url)",
                UserInformationEndpoint = "https://api.linkedin.com/v1/people/~:(id,firstName,lastName,email-address,picture-url)",
                Scope = { "r_basicprofile", "r_emailaddress" },
                Events = new OAuthEvents
                {
                    // The OnCreatingTicket event is called after the user has been authenticated and the OAuth middleware has 
                    // created an auth ticket. We need to manually call the UserInformationEndpoint to retrieve the user's information,
                    // parse the resulting JSON to extract the relevant information, and add the correct claims.
                    OnCreatingTicket = async context =>
                    {
                        // Retrieve user info
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                        request.Headers.Add("x-li-format", "json"); // Tell LinkedIn we want the result in JSON, otherwise it will return XML
                        var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();
                        // Extract the user info object
                        var user = JObject.Parse(await response.Content.ReadAsStringAsync());
                        // Add the Name Identifier claim
                        var userId = user.Value<string>("id");
                        if (!string.IsNullOrEmpty(userId))
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId, ClaimValueTypes.String, context.Options.ClaimsIssuer));
                        }
                        // Add the Name claim
                        var firstName = user.Value<string>("firstName");
                        if (!string.IsNullOrEmpty(firstName))
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.Name, firstName, ClaimValueTypes.String, context.Options.ClaimsIssuer));
                        }

                        var lastName = user.Value<string>("lastName");
                        if (!string.IsNullOrEmpty(lastName))
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.Surname, lastName, ClaimValueTypes.String, context.Options.ClaimsIssuer));
                        }

                        // Add the email address claim
                        var email = user.Value<string>("emailAddress");
                        if (!string.IsNullOrEmpty(email))
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String,
                                context.Options.ClaimsIssuer));
                        }
                        // Add the Profile Picture claim
                        var pictureUrl = user.Value<string>("pictureUrl");
                        if (!string.IsNullOrEmpty(email))
                        {
                            context.Identity.AddClaim(new Claim("profile-picture", pictureUrl, ClaimValueTypes.String,
                                context.Options.ClaimsIssuer));
                        }
                    }
                }
            });
            // Listen for requests on the /login path, and issue a challenge to log in with the LinkedIn middleware
            app.Map("/login", builder =>
            {
                builder.Run(async context =>
                {
                    // Return a challenge to invoke the LinkedIn authentication scheme
                    await context.Authentication.ChallengeAsync("LinkedIn", properties: new AuthenticationProperties() { RedirectUri = "/api/Account/Login" });
                });
            });
            // Listen for requests on the /logout path, and sign the user out
            app.Map("/logout", builder =>
            {
                builder.Run(async context =>
                {
                    // Sign the user out of the authentication middleware (i.e. it will clear the Auth cookie)
                    await context.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    // Redirect the user to the home page after signing out
                    context.Response.Redirect("/api/Account/Logout");
                });
            });

            app.UseSimpleInjectorAspNetRequestScoping(container);

            container.Options.DefaultScopedLifestyle = new AspNetRequestLifestyle();

            InitializeContainer(app);
            
            container.Verify();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            
            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();
            //app.UseJwtBearerAuthentication();
            //app.UseLinkedInAuthentication()
            //{

            //}
            app.UseMvc();
            
        }

        private void InitializeContainer(IApplicationBuilder app)
        {
            // Add application presentation components:
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);

            // Add application services. For instance:
            container.Register(() => ApplicationContextFactory.Create(), Lifestyle.Scoped);
            container.Register<IOrganization, Organization>(Lifestyle.Singleton);
            container.Register<ICountry, Country>(Lifestyle.Singleton);
            container.Register<IBusiness, Business>(Lifestyle.Singleton);
            container.Register<IFamily, Family>(Lifestyle.Singleton);
            container.Register<IOffering, Offering>(Lifestyle.Singleton);
            container.Register<IDepartment, Department>(Lifestyle.Singleton);
            container.Register<IUser, User>(Lifestyle.Singleton);
            container.RegisterSingleton(app.ApplicationServices.GetService<ILoggerFactory>());

            // Cross-wire ASP.NET services (if any). For instance:
            // NOTE: Prevent cross-wired instances as much as possible.
            // See: https://simpleinjector.org/blog/2016/07/
        }
    }
}
