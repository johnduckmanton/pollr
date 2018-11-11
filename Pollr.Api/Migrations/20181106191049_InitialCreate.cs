using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Pollr.Api.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PollDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Theme = table.Column<string>(nullable: true),
                    Owner = table.Column<string>(nullable: true),
                    IsPublished = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PollDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Polls",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true),
                    Handle = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false),
                    IsPublished = table.Column<bool>(nullable: false),
                    ExpiryDate = table.Column<DateTime>(nullable: true),
                    PollDate = table.Column<DateTime>(nullable: false),
                    CurrentQuestion = table.Column<short>(nullable: false),
                    PollDefinitionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Polls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Polls_PollDefinitions_PollDefinitionId",
                        column: x => x.PollDefinitionId,
                        principalTable: "PollDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuestionDefinition",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QuestionText = table.Column<string>(nullable: true),
                    HasCorrectAnswer = table.Column<bool>(nullable: false),
                    IsDisabled = table.Column<bool>(nullable: false),
                    PollDefinitionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionDefinition", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuestionDefinition_PollDefinitions_PollDefinitionId",
                        column: x => x.PollDefinitionId,
                        principalTable: "PollDefinitions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    QuestionText = table.Column<string>(nullable: true),
                    IsDisabled = table.Column<bool>(nullable: false),
                    TotalVotes = table.Column<int>(nullable: false),
                    PollId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Question_Polls_PollId",
                        column: x => x.PollId,
                        principalTable: "Polls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CandidateAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AnswerText = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    IsCorrectAnswer = table.Column<bool>(nullable: false),
                    IsDisabled = table.Column<bool>(nullable: false),
                    QuestionDefinitionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CandidateAnswer_QuestionDefinition_QuestionDefinitionId",
                        column: x => x.QuestionDefinitionId,
                        principalTable: "QuestionDefinition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AnswerText = table.Column<string>(nullable: true),
                    ImagePath = table.Column<string>(nullable: true),
                    IsCorrectAnswer = table.Column<bool>(nullable: false),
                    IsDisabled = table.Column<bool>(nullable: false),
                    VoteCount = table.Column<int>(nullable: false),
                    QuestionId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Answer_Question_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Question",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answer_QuestionId",
                table: "Answer",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateAnswer_QuestionDefinitionId",
                table: "CandidateAnswer",
                column: "QuestionDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Polls_PollDefinitionId",
                table: "Polls",
                column: "PollDefinitionId");

            migrationBuilder.CreateIndex(
                name: "IX_Question_PollId",
                table: "Question",
                column: "PollId");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionDefinition_PollDefinitionId",
                table: "QuestionDefinition",
                column: "PollDefinitionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "CandidateAnswer");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "QuestionDefinition");

            migrationBuilder.DropTable(
                name: "Polls");

            migrationBuilder.DropTable(
                name: "PollDefinitions");
        }
    }
}
