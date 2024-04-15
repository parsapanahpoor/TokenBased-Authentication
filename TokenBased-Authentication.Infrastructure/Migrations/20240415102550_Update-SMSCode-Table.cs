using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TokenBased_Authentication.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSMSCodeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MobileModel",
                table: "UserTokens",
                newName: "LastestSignInPlatformName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastestSignInPlatformName",
                table: "UserTokens",
                newName: "MobileModel");
        }
    }
}
