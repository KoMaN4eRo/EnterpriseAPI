using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace EnterpriseAPI.Migrations
{
    public partial class Inital : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "organization",
                columns: table => new
                {
                    organizationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    organizationCode = table.Column<int>(nullable: false),
                    organizationName = table.Column<string>(nullable: false),
                    organizationType = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_organization", x => x.organizationId);
                });

            migrationBuilder.CreateTable(
                name: "country",
                columns: table => new
                {
                    countryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    countryCode = table.Column<int>(nullable: false),
                    countryName = table.Column<string>(nullable: false),
                    organizationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_country", x => x.countryId);
                    table.ForeignKey(
                        name: "FK_country_organization_organizationId",
                        column: x => x.organizationId,
                        principalTable: "organization",
                        principalColumn: "organizationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "business",
                columns: table => new
                {
                    businessId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    businessName = table.Column<string>(nullable: false),
                    countryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_business", x => x.businessId);
                    table.ForeignKey(
                        name: "FK_business_country_countryId",
                        column: x => x.countryId,
                        principalTable: "country",
                        principalColumn: "countryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "family",
                columns: table => new
                {
                    familyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    businessId = table.Column<int>(nullable: false),
                    familyName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_family", x => x.familyId);
                    table.ForeignKey(
                        name: "FK_family_business_businessId",
                        column: x => x.businessId,
                        principalTable: "business",
                        principalColumn: "businessId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "offering",
                columns: table => new
                {
                    offeringId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    familyId = table.Column<int>(nullable: false),
                    offeringName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_offering", x => x.offeringId);
                    table.ForeignKey(
                        name: "FK_offering_family_familyId",
                        column: x => x.familyId,
                        principalTable: "family",
                        principalColumn: "familyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "department",
                columns: table => new
                {
                    departmentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    departmentName = table.Column<string>(nullable: false),
                    offeringId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_department", x => x.departmentId);
                    table.ForeignKey(
                        name: "FK_department_offering_offeringId",
                        column: x => x.offeringId,
                        principalTable: "offering",
                        principalColumn: "offeringId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_business_countryId",
                table: "business",
                column: "countryId");

            migrationBuilder.CreateIndex(
                name: "IX_country_organizationId",
                table: "country",
                column: "organizationId");

            migrationBuilder.CreateIndex(
                name: "IX_department_offeringId",
                table: "department",
                column: "offeringId");

            migrationBuilder.CreateIndex(
                name: "IX_family_businessId",
                table: "family",
                column: "businessId");

            migrationBuilder.CreateIndex(
                name: "IX_offering_familyId",
                table: "offering",
                column: "familyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "department");

            migrationBuilder.DropTable(
                name: "offering");

            migrationBuilder.DropTable(
                name: "family");

            migrationBuilder.DropTable(
                name: "business");

            migrationBuilder.DropTable(
                name: "country");

            migrationBuilder.DropTable(
                name: "organization");
        }
    }
}
