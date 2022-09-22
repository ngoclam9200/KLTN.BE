using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoubleLStore.WebApp.Migrations
{
    public partial class updatetableproductimageproductcostprod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "price",
                table: "CostProduct",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "count",
                table: "CostProduct",
                newName: "Count");

            migrationBuilder.AddColumn<bool>(
                name: "isDefaut",
                table: "ImageProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "CostProduct",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Count",
                table: "CostProduct",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<double>(
                name: "TotalCost",
                table: "CostProduct",
                type: "float",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDefaut",
                table: "ImageProducts");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "CostProduct",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Count",
                table: "CostProduct",
                newName: "count");

            migrationBuilder.AlterColumn<string>(
                name: "TotalCost",
                table: "CostProduct",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "price",
                table: "CostProduct",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "count",
                table: "CostProduct",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
