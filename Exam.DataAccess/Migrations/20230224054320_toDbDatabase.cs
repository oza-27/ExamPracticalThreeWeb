using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exam.DataAccess.Migrations
{
    public partial class toDbDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TotalAmount",
                table: "orders",
                type: "double",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "DiscountAmount",
                table: "orders",
                type: "double",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_orderItems_OrderId",
                table: "orderItems",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_orderItems_orders_OrderId",
                table: "orderItems",
                column: "OrderId",
                principalTable: "orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_orderItems_orders_OrderId",
                table: "orderItems");

            migrationBuilder.DropIndex(
                name: "IX_orderItems_OrderId",
                table: "orderItems");

            migrationBuilder.AlterColumn<int>(
                name: "TotalAmount",
                table: "orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DiscountAmount",
                table: "orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(double),
                oldType: "double",
                oldNullable: true);
        }
    }
}
