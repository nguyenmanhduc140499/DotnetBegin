using Microsoft.EntityFrameworkCore.Migrations;

namespace DemoDotnet.Migrations
{
    public partial class Table_Emp_Items : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StudentName",
                table: "Person",
                newName: "Student_Address");

            migrationBuilder.AddColumn<string>(
                name: "ItemID",
                table: "Product",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployesID",
                table: "Person",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Person",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "items",
                columns: table => new
                {
                    ItemID = table.Column<string>(type: "TEXT", nullable: false),
                    itemName = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_items", x => x.ItemID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "items");

            migrationBuilder.DropColumn(
                name: "ItemID",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "EmployesID",
                table: "Person");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Person");

            migrationBuilder.RenameColumn(
                name: "Student_Address",
                table: "Person",
                newName: "StudentName");
        }
    }
}
