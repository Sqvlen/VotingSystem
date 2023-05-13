using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ExtendedEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountVote",
                table: "Voting",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Voting",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Details",
                table: "Voting",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Voting",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Voting",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isClosed",
                table: "Voting",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Vote",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VotingId",
                table: "Vote",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Voting_UserId",
                table: "Voting",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_UserId",
                table: "Vote",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_VotingId",
                table: "Vote",
                column: "VotingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Users_UserId",
                table: "Vote",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Voting_VotingId",
                table: "Vote",
                column: "VotingId",
                principalTable: "Voting",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Voting_Users_UserId",
                table: "Voting",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Users_UserId",
                table: "Vote");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Voting_VotingId",
                table: "Vote");

            migrationBuilder.DropForeignKey(
                name: "FK_Voting_Users_UserId",
                table: "Voting");

            migrationBuilder.DropIndex(
                name: "IX_Voting_UserId",
                table: "Voting");

            migrationBuilder.DropIndex(
                name: "IX_Vote_UserId",
                table: "Vote");

            migrationBuilder.DropIndex(
                name: "IX_Vote_VotingId",
                table: "Vote");

            migrationBuilder.DropColumn(
                name: "CountVote",
                table: "Voting");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Voting");

            migrationBuilder.DropColumn(
                name: "Details",
                table: "Voting");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Voting");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Voting");

            migrationBuilder.DropColumn(
                name: "isClosed",
                table: "Voting");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Vote");

            migrationBuilder.DropColumn(
                name: "VotingId",
                table: "Vote");
        }
    }
}
