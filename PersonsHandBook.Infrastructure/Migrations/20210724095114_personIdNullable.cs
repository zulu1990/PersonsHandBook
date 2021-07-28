using Microsoft.EntityFrameworkCore.Migrations;

namespace PersonsHandBook.Infrastructure.Migrations
{
    public partial class personIdNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Persons_PersonId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_PersonId",
                table: "Photos");

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Photos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Photos_PersonId",
                table: "Photos",
                column: "PersonId",
                unique: true,
                filter: "[PersonId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Persons_PersonId",
                table: "Photos",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Photos_Persons_PersonId",
                table: "Photos");

            migrationBuilder.DropIndex(
                name: "IX_Photos_PersonId",
                table: "Photos");

            migrationBuilder.AlterColumn<int>(
                name: "PersonId",
                table: "Photos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Photos_PersonId",
                table: "Photos",
                column: "PersonId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Photos_Persons_PersonId",
                table: "Photos",
                column: "PersonId",
                principalTable: "Persons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
