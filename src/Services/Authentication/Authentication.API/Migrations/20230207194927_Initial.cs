using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Authentication.API.Migrations
{
	/// <inheritdoc />
	public partial class Initial : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Credentials",
				columns: table => new
				{
					UserId = table.Column<int>(type: "int", nullable: false),
					Username = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Credentials", x => x.UserId);
				});

			migrationBuilder.CreateTable(
				name: "RefreshTokens",
				columns: table => new
				{
					Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Expiry = table.Column<DateTime>(type: "datetime2", nullable: false),
					CredentialUserId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_RefreshTokens", x => x.Token);
					table.ForeignKey(
						name: "FK_RefreshTokens_Credentials_CredentialUserId",
						column: x => x.CredentialUserId,
						principalTable: "Credentials",
						principalColumn: "UserId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Roles",
				columns: table => new
				{
					Type = table.Column<int>(type: "int", nullable: false),
					CredentialUserId = table.Column<int>(type: "int", nullable: false),
					Expiry = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Roles", x => new { x.CredentialUserId, x.Type });
					table.ForeignKey(
						name: "FK_Roles_Credentials_CredentialUserId",
						column: x => x.CredentialUserId,
						principalTable: "Credentials",
						principalColumn: "UserId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Credentials_Username",
				table: "Credentials",
				column: "Username",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_RefreshTokens_CredentialUserId",
				table: "RefreshTokens",
				column: "CredentialUserId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "RefreshTokens");

			migrationBuilder.DropTable(
				name: "Roles");

			migrationBuilder.DropTable(
				name: "Credentials");
		}
	}
}
