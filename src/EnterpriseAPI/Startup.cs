using System;
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
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Security.Claims;
using EnterpriseAPI.Models.UserModel;
using EnterpriseAPI.Validation;

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
                builder.AddApplicationInsightsSettings(developerMode: true);
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            string con = @"Server=(localdb)\mssqllocaldb;Database=EnterpriseAPIdb;Trusted_Connection=True;";
            services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(con));
            services.AddApplicationInsightsTelemetry(Configuration);
            services.AddMvc();
            services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.CookieName = ".MyApplication";
            });
            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(container));
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(container));
            services.AddAuthentication(
    options => options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseSession();
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                LoginPath = new PathString("/api/Account/Info"),
                LogoutPath = new PathString("/api/Account/Logout")
            });

            //LimkedIn authentication
            app.UseOAuthAuthentication(new OAuthOptions
            {
                AuthenticationScheme = "LinkedIn",
                ClientId = Configuration["linkedin:clientId"],
                ClientSecret = Configuration["linkedin:clientSecret"],
                CallbackPath = new PathString("/signin-linkedin"),  
                AuthorizationEndpoint = "https://www.linkedin.com/oauth/v2/authorization",
                TokenEndpoint = "https://www.linkedin.com/oauth/v2/accessToken",
                UserInformationEndpoint = "https://api.linkedin.com/v1/people/~:(id,firstName,lastName,email-address,picture-url)",
                Scope = { "r_basicprofile", "r_emailaddress" },
                Events = new OAuthEvents
                {
                    OnCreatingTicket = async context =>
                    {
                        var request = new HttpRequestMessage(HttpMethod.Get, context.Options.UserInformationEndpoint);
                        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", context.AccessToken);
                        request.Headers.Add("x-li-format", "json"); 
                        var response = await context.Backchannel.SendAsync(request, context.HttpContext.RequestAborted);
                        response.EnsureSuccessStatusCode();
                        var user = JObject.Parse(await response.Content.ReadAsStringAsync());

                        var userId = user.Value<string>("id");
                        if (!string.IsNullOrEmpty(userId))
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId, ClaimValueTypes.String, context.Options.ClaimsIssuer));
                        }

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

                        var email = user.Value<string>("emailAddress");
                        if (!string.IsNullOrEmpty(email))
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.Email, email, ClaimValueTypes.String,
                                context.Options.ClaimsIssuer));
                        }
                        
                        //var pictureUrl = user.Value<string>("pictureUrl");
                        //if (!string.IsNullOrEmpty(email))
                        //{
                        //    context.Identity.AddClaim(new Claim("profile-picture", pictureUrl, ClaimValueTypes.String,
                        //        context.Options.ClaimsIssuer));
                        //}
                    }
                }
            });

            app.UseSimpleInjectorAspNetRequestScoping(container);

            container.Options.DefaultScopedLifestyle = new AspNetRequestLifestyle();

            InitializeContainer(app);
            
            container.Verify();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            
            app.UseApplicationInsightsRequestTelemetry();

            app.UseApplicationInsightsExceptionTelemetry();
            app.UseMvc();
        }

        private void InitializeContainer(IApplicationBuilder app)
        {
            // Add application presentation components:
            container.RegisterMvcControllers(app);
            container.RegisterMvcViewComponents(app);

            // Add application services. For instance:
            container.Register(() => ApplicationContextFactory.Create(), Lifestyle.Scoped);
            container.Register<IOrganizationService, OrganizationService>(Lifestyle.Transient);
            container.Register<IOrganizationRepository, OrganizationRepository>(Lifestyle.Singleton);
            container.Register<ICountryService, CountryService>(Lifestyle.Transient);
            container.Register<ICountryRepository, CountryRepository>(Lifestyle.Singleton);
            container.Register<IBusinessService, BusinessService>(Lifestyle.Transient);
            container.Register<IBusinessRepository, BusinessRepository>(Lifestyle.Singleton);
            container.Register<IFamilyService, FamilyService>(Lifestyle.Transient);
            container.Register<IFamilyRepository, FamilyRepository>(Lifestyle.Singleton);
            container.Register<IOfferingService, OfferingService>(Lifestyle.Transient);
            container.Register<IOfferingRepository, OfferingRepository>(Lifestyle.Singleton);
            container.Register<IDepartmentService, DepartmentService>(Lifestyle.Transient);
            container.Register<IDepartmentRepository, DepartmentRepository>(Lifestyle.Singleton);
            container.Register<IUser, User>(Lifestyle.Singleton);
            container.Register<IValidation, ModelValidation>(Lifestyle.Transient);
            container.RegisterSingleton(app.ApplicationServices.GetService<ILoggerFactory>());

            // Cross-wire ASP.NET services (if any). For instance:
            // NOTE: Prevent cross-wired instances as much as possible.
            // See: https://simpleinjector.org/blog/2016/07/
        }
    }
}
