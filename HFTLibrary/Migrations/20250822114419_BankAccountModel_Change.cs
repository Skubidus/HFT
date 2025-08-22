using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HFTLibrary.Migrations
{
    /// <inheritdoc />
    public partial class BankAccountModel_Change : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "BankAccounts");

            migrationBuilder.AddColumn<string>(
                name: "BankName",
                table: "BankAccounts",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BankName",
                table: "BankAccounts");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "BankAccounts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
