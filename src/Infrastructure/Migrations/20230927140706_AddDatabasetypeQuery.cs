using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDatabasetypeQuery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DatabaseTypeId",
                table: "AssessmentQuery",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("BDEA958C-BE24-44FC-98A8-44353849B132"));

            migrationBuilder.AddColumn<bool>(
                name: "RenderGenericView",
                table: "AssessmentQuery",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentQuery_DatabaseTypeId",
                table: "AssessmentQuery",
                column: "DatabaseTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AssessmentQuery_DatabaseType_DatabaseTypeId",
                table: "AssessmentQuery",
                column: "DatabaseTypeId",
                principalTable: "DatabaseType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssessmentQuery_DatabaseType_DatabaseTypeId",
                table: "AssessmentQuery");

            migrationBuilder.DropIndex(
                name: "IX_AssessmentQuery_DatabaseTypeId",
                table: "AssessmentQuery");

            migrationBuilder.DropColumn(
                name: "DatabaseTypeId",
                table: "AssessmentQuery");

            migrationBuilder.DropColumn(
                name: "RenderGenericView",
                table: "AssessmentQuery");
        }
    }
}
