using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoubleLStore.WebApp.Migrations
{
    public partial class updatchat1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isNewMessageAdmin",
                table: "ChatUsers",
                type: "bit",
                nullable: false);
                //defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isNewMessageAdmin",
                table: "ChatUsers");
        }
    }
}
