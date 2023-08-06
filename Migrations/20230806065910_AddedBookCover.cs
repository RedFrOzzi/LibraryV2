using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryV2.Migrations
{
    /// <inheritdoc />
    public partial class AddedBookCover : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "BookCoverId",
                table: "Books",
                type: "BLOB",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BookCovers",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "BLOB", nullable: false),
                    ImageName = table.Column<string>(type: "TEXT", nullable: false),
                    ImageData = table.Column<byte[]>(type: "BLOB", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookCovers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookCoverId",
                table: "Books",
                column: "BookCoverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookCovers_BookCoverId",
                table: "Books",
                column: "BookCoverId",
                principalTable: "BookCovers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookCovers_BookCoverId",
                table: "Books");

            migrationBuilder.DropTable(
                name: "BookCovers");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookCoverId",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookCoverId",
                table: "Books");
        }
    }
}
