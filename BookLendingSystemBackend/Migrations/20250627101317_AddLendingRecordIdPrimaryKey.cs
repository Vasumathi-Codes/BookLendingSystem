using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookLendingSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddLendingRecordIdPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LendingRecords_UserId",
                table: "LendingRecords");

            migrationBuilder.CreateIndex(
                name: "IX_LendingRecords_UserId_BookId_ReturnDate",
                table: "LendingRecords",
                columns: new[] { "UserId", "BookId", "ReturnDate" },
                unique: true,
                filter: "\"ReturnDate\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LendingRecords_UserId_BookId_ReturnDate",
                table: "LendingRecords");

            migrationBuilder.CreateIndex(
                name: "IX_LendingRecords_UserId",
                table: "LendingRecords",
                column: "UserId");
        }
    }
}
