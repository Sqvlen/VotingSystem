using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedNameEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Names_Voting_VotingId",
                table: "Names");

            migrationBuilder.DropIndex(
                name: "IX_Names_VotingId",
                table: "Names");

            migrationBuilder.DropColumn(
                name: "VotingId",
                table: "Names");

            migrationBuilder.CreateTable(
                name: "NameVoting",
                columns: table => new
                {
                    NamesId = table.Column<int>(type: "INTEGER", nullable: false),
                    VotingsId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NameVoting", x => new { x.NamesId, x.VotingsId });
                    table.ForeignKey(
                        name: "FK_NameVoting_Names_NamesId",
                        column: x => x.NamesId,
                        principalTable: "Names",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NameVoting_Voting_VotingsId",
                        column: x => x.VotingsId,
                        principalTable: "Voting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NameVoting_VotingsId",
                table: "NameVoting",
                column: "VotingsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NameVoting");

            migrationBuilder.AddColumn<int>(
                name: "VotingId",
                table: "Names",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Names_VotingId",
                table: "Names",
                column: "VotingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Names_Voting_VotingId",
                table: "Names",
                column: "VotingId",
                principalTable: "Voting",
                principalColumn: "Id");
        }
    }
}
