using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoubleLStore.WebApp.Migrations
{
    public partial class updatetb1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_ShippingFees_ShippingFeeId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "ShippingFees");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ShippingFeeId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingFeeId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "ShippingFee",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isPaid",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isPaymentOnline",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingFee",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "isPaid",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "isPaymentOnline",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "ShippingFeeId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ShippingFees",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingFees", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ShippingFeeId",
                table: "Orders",
                column: "ShippingFeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_ShippingFees_ShippingFeeId",
                table: "Orders",
                column: "ShippingFeeId",
                principalTable: "ShippingFees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
