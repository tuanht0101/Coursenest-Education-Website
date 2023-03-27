using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Identity.API.Migrations
{
	/// <inheritdoc />
	public partial class Initial : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					UserId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Phonenumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
					FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
					AboutMe = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Gender = table.Column<int>(type: "int", nullable: true),
					DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
					Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Created = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastModified = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.UserId);
				});

			migrationBuilder.CreateTable(
				name: "Achievements",
				columns: table => new
				{
					AchievementId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Created = table.Column<DateTime>(type: "datetime2", nullable: false),
					UserId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Achievements", x => x.AchievementId);
					table.ForeignKey(
						name: "FK_Achievements_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "UserId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Avatars",
				columns: table => new
				{
					UserId = table.Column<int>(type: "int", nullable: false),
					MediaType = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Data = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Avatars", x => x.UserId);
					table.ForeignKey(
						name: "FK_Avatars_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "UserId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Experiences",
				columns: table => new
				{
					ExperienceId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Started = table.Column<DateTime>(type: "datetime2", nullable: false),
					Ended = table.Column<DateTime>(type: "datetime2", nullable: true),
					UserId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Experiences", x => x.ExperienceId);
					table.ForeignKey(
						name: "FK_Experiences_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "UserId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "FollowedTopics",
				columns: table => new
				{
					UserId = table.Column<int>(type: "int", nullable: false),
					TopicId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_FollowedTopics", x => new { x.UserId, x.TopicId });
					table.ForeignKey(
						name: "FK_FollowedTopics_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "UserId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "InterestedTopics",
				columns: table => new
				{
					UserId = table.Column<int>(type: "int", nullable: false),
					TopicId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_InterestedTopics", x => new { x.UserId, x.TopicId });
					table.ForeignKey(
						name: "FK_InterestedTopics_Users_UserId",
						column: x => x.UserId,
						principalTable: "Users",
						principalColumn: "UserId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Achievements_UserId",
				table: "Achievements",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_Experiences_UserId",
				table: "Experiences",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_Users_Email",
				table: "Users",
				column: "Email",
				unique: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Achievements");

			migrationBuilder.DropTable(
				name: "Avatars");

			migrationBuilder.DropTable(
				name: "Experiences");

			migrationBuilder.DropTable(
				name: "FollowedTopics");

			migrationBuilder.DropTable(
				name: "InterestedTopics");

			migrationBuilder.DropTable(
				name: "Users");
		}
	}
}
