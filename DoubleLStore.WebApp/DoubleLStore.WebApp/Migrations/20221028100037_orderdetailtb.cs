using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoubleLStore.WebApp.Migrations
{
    public partial class orderdetailtb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DiscountProduct",
                table: "OrderDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "DiscountVoucher",
                table: "OrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "PriceProduct",
                table: "OrderDetails",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountProduct",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "DiscountVoucher",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "PriceProduct",
                table: "OrderDetails");
        }
    }
}
