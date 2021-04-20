using Microsoft.EntityFrameworkCore.Migrations;

namespace laundry.Migrations
{
    public partial class mig2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "status",
                table: "TimeSlotModel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "TimeSlotModel",
                type: "TEXT",
                nullable: true);
        }
    }
}
