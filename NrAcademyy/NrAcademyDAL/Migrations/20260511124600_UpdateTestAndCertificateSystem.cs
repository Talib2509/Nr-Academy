using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NrAcademyDAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTestAndCertificateSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Tests");

            migrationBuilder.AddColumn<int>(
                name: "DurationInMinutes",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DurationInMinutes",
                table: "Tests");

            migrationBuilder.AddColumn<string>(
                name: "Duration",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
