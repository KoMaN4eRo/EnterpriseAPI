using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using EnterpriseAPI.Models;

namespace EnterpriseAPI.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20170329154801_Initial2")]
    partial class Initial2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("EnterpriseAPI.Models.BusinessModel.Business", b =>
                {
                    b.Property<int>("businessId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("businessName")
                        .IsRequired();

                    b.Property<int>("countryId");

                    b.HasKey("businessId");

                    b.HasIndex("countryId");

                    b.ToTable("business");
                });

            modelBuilder.Entity("EnterpriseAPI.Models.CountryModel.Country", b =>
                {
                    b.Property<int>("countryId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("countryCode");

                    b.Property<string>("countryName")
                        .IsRequired();

                    b.Property<int>("organizationId");

                    b.HasKey("countryId");

                    b.HasIndex("organizationId");

                    b.ToTable("country");
                });

            modelBuilder.Entity("EnterpriseAPI.Models.DepartmentModel.Department", b =>
                {
                    b.Property<int>("departmentId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("departmentName")
                        .IsRequired();

                    b.Property<int>("offeringId");

                    b.HasKey("departmentId");

                    b.HasIndex("offeringId");

                    b.ToTable("department");
                });

            modelBuilder.Entity("EnterpriseAPI.Models.FamilyModel.Family", b =>
                {
                    b.Property<int>("familyId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("businessId");

                    b.Property<string>("familyName")
                        .IsRequired();

                    b.HasKey("familyId");

                    b.HasIndex("businessId");

                    b.ToTable("family");
                });

            modelBuilder.Entity("EnterpriseAPI.Models.OfferingModel.Offering", b =>
                {
                    b.Property<int>("offeringId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("familyId");

                    b.Property<string>("offeringName")
                        .IsRequired();

                    b.HasKey("offeringId");

                    b.HasIndex("familyId");

                    b.ToTable("offering");
                });

            modelBuilder.Entity("EnterpriseAPI.Models.OrganizationModel.Organization", b =>
                {
                    b.Property<int>("organizationId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Owner");

                    b.Property<int>("organizationCode");

                    b.Property<string>("organizationName")
                        .IsRequired();

                    b.Property<string>("organizationType")
                        .IsRequired();

                    b.HasKey("organizationId");

                    b.ToTable("organization");
                });

            modelBuilder.Entity("EnterpriseAPI.Models.BusinessModel.Business", b =>
                {
                    b.HasOne("EnterpriseAPI.Models.CountryModel.Country")
                        .WithMany("business")
                        .HasForeignKey("countryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EnterpriseAPI.Models.CountryModel.Country", b =>
                {
                    b.HasOne("EnterpriseAPI.Models.OrganizationModel.Organization")
                        .WithMany("country")
                        .HasForeignKey("organizationId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EnterpriseAPI.Models.DepartmentModel.Department", b =>
                {
                    b.HasOne("EnterpriseAPI.Models.OfferingModel.Offering")
                        .WithMany("department")
                        .HasForeignKey("offeringId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EnterpriseAPI.Models.FamilyModel.Family", b =>
                {
                    b.HasOne("EnterpriseAPI.Models.BusinessModel.Business")
                        .WithMany("family")
                        .HasForeignKey("businessId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("EnterpriseAPI.Models.OfferingModel.Offering", b =>
                {
                    b.HasOne("EnterpriseAPI.Models.FamilyModel.Family")
                        .WithMany("offering")
                        .HasForeignKey("familyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
