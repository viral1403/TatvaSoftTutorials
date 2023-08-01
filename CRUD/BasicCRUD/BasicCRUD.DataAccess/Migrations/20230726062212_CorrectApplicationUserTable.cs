using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicCRUD.Migrations
{
    public partial class CorrectApplicationUserTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ComapanyId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ComapanyId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
