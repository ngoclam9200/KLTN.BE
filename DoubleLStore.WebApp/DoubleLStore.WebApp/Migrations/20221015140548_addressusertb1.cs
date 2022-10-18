using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoubleLStore.WebApp.Migrations
{
    public partial class addressusertb1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "lat",
                table: "AddressUsers");

            migrationBuilder.DropColumn(
                name: "lng",
                table: "AddressUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "lat",
                table: "AddressUsers",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "lng",
                table: "AddressUsers",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
