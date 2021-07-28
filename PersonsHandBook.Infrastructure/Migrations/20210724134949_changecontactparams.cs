using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonsHandBook.Infrastructure.Migrations
{
    public partial class changecontactparams : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Contacts",
                newName: "OfficeNumber");

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomePhoneNumber",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MobileNumber",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "HomePhoneNumber",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "MobileNumber",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "OfficeNumber",
                table: "Contacts",
                newName: "PhoneNumber");
        }
    }
}
