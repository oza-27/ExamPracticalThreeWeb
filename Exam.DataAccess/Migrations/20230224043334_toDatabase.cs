using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exam.DataAccess.Migrations
{
    public partial class toDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StatusType",
                table: "orders",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusType",
                table: "orders");
        }
    }
}
