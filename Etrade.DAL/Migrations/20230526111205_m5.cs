using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Etrade.DAL.Migrations
{
    /// <inheritdoc />
    public partial class m5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdressTitle",
                table: "Orders",
                newName: "AddressTitle");

            migrationBuilder.RenameColumn(
                name: "Adress",
                table: "Orders",
                newName: "Address");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddressTitle",
                table: "Orders",
                newName: "AdressTitle");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Orders",
                newName: "Adress");
        }
    }
}
