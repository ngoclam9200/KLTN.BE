using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoubleLStore.WebApp.Migrations
{
    public partial class updatevouchercount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Count",
                table: "Vouchers",
                newName: "AmountRemaining");

            migrationBuilder.AddColumn<int>(
                name: "AmountInput",
                table: "Vouchers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountInput",
                table: "Vouchers");

            migrationBuilder.RenameColumn(
                name: "AmountRemaining",
                table: "Vouchers",
                newName: "Count");
        }
    }
}
