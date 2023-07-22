using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryV2.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Readers_CurrentReaderId",
                table: "Books");

            migrationBuilder.AlterColumn<byte[]>(
                name: "CurrentReaderId",
                table: "Books",
                type: "BLOB",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "BLOB");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Readers_CurrentReaderId",
                table: "Books",
                column: "CurrentReaderId",
                principalTable: "Readers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Readers_CurrentReaderId",
                table: "Books");

            migrationBuilder.AlterColumn<byte[]>(
                name: "CurrentReaderId",
                table: "Books",
                type: "BLOB",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "BLOB",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Readers_CurrentReaderId",
                table: "Books",
                column: "CurrentReaderId",
                principalTable: "Readers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
