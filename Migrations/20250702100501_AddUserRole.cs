using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace chatbot.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Chatbot");

            // migrationBuilder.CreateTable(
            //     name: "Users",
            //     schema: "Chatbot",
            //     columns: table => new
            //     {
            //         UserId = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
            //         PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
            //         CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
            //         Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_Users", x => x.UserId);
            //     });
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Users",
                schema: "Chatbot",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "User"
            );


            // migrationBuilder.CreateTable(
            //     name: "ChatMessages",
            //     schema: "Chatbot",
            //     columns: table => new
            //     {
            //         MessageId = table.Column<int>(type: "int", nullable: false)
            //             .Annotation("SqlServer:Identity", "1, 1"),
            //         UserId = table.Column<int>(type: "int", nullable: false),
            //         MessageText = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         Sentiment = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         SentimentScore = table.Column<float>(type: "real", nullable: false),
            //         BotResponse = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //         CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("PK_ChatMessages", x => x.MessageId);
            //         table.ForeignKey(
            //             name: "FK_ChatMessages_Users_UserId",
            //             column: x => x.UserId,
            //             principalSchema: "Chatbot",
            //             principalTable: "Users",
            //             principalColumn: "UserId",
            //             onDelete: ReferentialAction.Cascade);
            //     });

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_UserId",
                schema: "Chatbot",
                table: "ChatMessages",
                column: "UserId");
        }

        /// <inheritdoc />
        // protected override void Down(MigrationBuilder migrationBuilder)
        // {
        //     migrationBuilder.DropTable(
        //         name: "ChatMessages",
        //         schema: "Chatbot");

        //     migrationBuilder.DropTable(
        //         name: "Users",
        //         schema: "Chatbot");
        // }
    }
}
