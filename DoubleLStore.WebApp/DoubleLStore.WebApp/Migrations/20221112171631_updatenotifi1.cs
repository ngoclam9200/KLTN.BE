using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoubleLStore.WebApp.Migrations
{
    public partial class updatenotifi1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isNewNotifi",
                table: "Notifi",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isNewNotifi",
                table: "Notifi");
        }
    }
}
