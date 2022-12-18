using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoubleLStore.WebApp.Migrations
{
    public partial class updsalary1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfNotWorking",
                table: "SalaryStaff",
                newName: "NumberOfWorking");

            migrationBuilder.RenameColumn(
                name: "ListDayNotWorking",
                table: "SalaryStaff",
                newName: "ListDayWorking");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NumberOfWorking",
                table: "SalaryStaff",
                newName: "NumberOfNotWorking");

            migrationBuilder.RenameColumn(
                name: "ListDayWorking",
                table: "SalaryStaff",
                newName: "ListDayNotWorking");
        }
    }
}
