using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserData.API.Migrations
{
	/// <inheritdoc />
	public partial class Initial : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Enrollments",
				columns: table => new
				{
					EnrollmentId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Completed = table.Column<DateTime>(type: "datetime2", nullable: true),
					CourseId = table.Column<int>(type: "int", nullable: false),
					StudentUserId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Enrollments", x => x.EnrollmentId);
				});

			migrationBuilder.CreateTable(
				name: "CompletedUnits",
				columns: table => new
				{
					UnitId = table.Column<int>(type: "int", nullable: false),
					EnrollmentId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_CompletedUnits", x => new { x.UnitId, x.EnrollmentId });
					table.ForeignKey(
						name: "FK_CompletedUnits_Enrollments_EnrollmentId",
						column: x => x.EnrollmentId,
						principalTable: "Enrollments",
						principalColumn: "EnrollmentId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Submissions",
				columns: table => new
				{
					SubmissionId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
					LessonTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
					CourseTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Created = table.Column<DateTime>(type: "datetime2", nullable: false),
					Deadline = table.Column<DateTime>(type: "datetime2", nullable: false),
					Ended = table.Column<DateTime>(type: "datetime2", nullable: false),
					Grade = table.Column<int>(type: "int", nullable: true),
					Graded = table.Column<DateTime>(type: "datetime2", nullable: true),
					StudentUserId = table.Column<int>(type: "int", nullable: false),
					InstructorUserId = table.Column<int>(type: "int", nullable: true),
					UnitId = table.Column<int>(type: "int", nullable: false),
					EnrollmentId = table.Column<int>(type: "int", nullable: false),
					TopicId = table.Column<int>(type: "int", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Submissions", x => x.SubmissionId);
					table.ForeignKey(
						name: "FK_Submissions_Enrollments_EnrollmentId",
						column: x => x.EnrollmentId,
						principalTable: "Enrollments",
						principalColumn: "EnrollmentId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Comments",
				columns: table => new
				{
					CommentId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Created = table.Column<DateTime>(type: "datetime2", nullable: false),
					OwnerUserId = table.Column<int>(type: "int", nullable: false),
					SubmissionId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Comments", x => x.CommentId);
					table.ForeignKey(
						name: "FK_Comments_Submissions_SubmissionId",
						column: x => x.SubmissionId,
						principalTable: "Submissions",
						principalColumn: "SubmissionId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Questions",
				columns: table => new
				{
					QuestionId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Point = table.Column<int>(type: "int", nullable: false),
					SubmissionId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Questions", x => x.QuestionId);
					table.ForeignKey(
						name: "FK_Questions_Submissions_SubmissionId",
						column: x => x.SubmissionId,
						principalTable: "Submissions",
						principalColumn: "SubmissionId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Reviews",
				columns: table => new
				{
					ReviewId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Type = table.Column<int>(type: "int", nullable: false),
					SubmissionId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Reviews", x => x.ReviewId);
					table.ForeignKey(
						name: "FK_Reviews_Submissions_SubmissionId",
						column: x => x.SubmissionId,
						principalTable: "Submissions",
						principalColumn: "SubmissionId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Choices",
				columns: table => new
				{
					ChoiceId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
					IsCorrect = table.Column<bool>(type: "bit", nullable: false),
					IsChosen = table.Column<bool>(type: "bit", nullable: true),
					QuestionId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Choices", x => x.ChoiceId);
					table.ForeignKey(
						name: "FK_Choices_Questions_QuestionId",
						column: x => x.QuestionId,
						principalTable: "Questions",
						principalColumn: "QuestionId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Choices_QuestionId",
				table: "Choices",
				column: "QuestionId");

			migrationBuilder.CreateIndex(
				name: "IX_Comments_SubmissionId",
				table: "Comments",
				column: "SubmissionId");

			migrationBuilder.CreateIndex(
				name: "IX_CompletedUnits_EnrollmentId",
				table: "CompletedUnits",
				column: "EnrollmentId");

			migrationBuilder.CreateIndex(
				name: "IX_Questions_SubmissionId",
				table: "Questions",
				column: "SubmissionId");

			migrationBuilder.CreateIndex(
				name: "IX_Reviews_SubmissionId",
				table: "Reviews",
				column: "SubmissionId");

			migrationBuilder.CreateIndex(
				name: "IX_Submissions_EnrollmentId",
				table: "Submissions",
				column: "EnrollmentId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Choices");

			migrationBuilder.DropTable(
				name: "Comments");

			migrationBuilder.DropTable(
				name: "CompletedUnits");

			migrationBuilder.DropTable(
				name: "Reviews");

			migrationBuilder.DropTable(
				name: "Questions");

			migrationBuilder.DropTable(
				name: "Submissions");

			migrationBuilder.DropTable(
				name: "Enrollments");
		}
	}
}
