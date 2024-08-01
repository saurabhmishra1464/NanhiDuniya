using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NanhiDuniya.Data.Migrations
{
    /// <inheritdoc />
    public partial class addIndexToIsRevokedColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserRefreshTokens_IsRevoked",
                table: "UserRefreshTokens",
                column: "IsRevoked");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserRefreshTokens_IsRevoked",
                table: "UserRefreshTokens");
        }
    }
}
