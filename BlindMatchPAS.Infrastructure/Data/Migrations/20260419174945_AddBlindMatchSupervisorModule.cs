using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlindMatchPAS.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddBlindMatchSupervisorModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_StudentId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ResearchAreas_ResearchAreaId",
                table: "Projects");

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SupervisorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IdentityRevealed = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Matches_AspNetUsers_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Matches_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupervisorExpertises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupervisorId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ResearchAreaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupervisorExpertises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupervisorExpertises_AspNetUsers_SupervisorId",
                        column: x => x.SupervisorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SupervisorExpertises_ResearchAreas_ResearchAreaId",
                        column: x => x.ResearchAreaId,
                        principalTable: "ResearchAreas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Matches_ProjectId",
                table: "Matches",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_SupervisorId",
                table: "Matches",
                column: "SupervisorId");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorExpertises_ResearchAreaId",
                table: "SupervisorExpertises",
                column: "ResearchAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_SupervisorExpertises_SupervisorId",
                table: "SupervisorExpertises",
                column: "SupervisorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_StudentId",
                table: "Projects",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ResearchAreas_ResearchAreaId",
                table: "Projects",
                column: "ResearchAreaId",
                principalTable: "ResearchAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_StudentId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_ResearchAreas_ResearchAreaId",
                table: "Projects");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "SupervisorExpertises");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_StudentId",
                table: "Projects",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_ResearchAreas_ResearchAreaId",
                table: "Projects",
                column: "ResearchAreaId",
                principalTable: "ResearchAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
