using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SGMS.Migrations
{
    public partial class ModAtt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "QualificationName",
                table: "Attachments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Attachments",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QualificationName",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Attachments");
        }
    }
}
