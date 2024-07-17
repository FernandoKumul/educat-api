using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace educat_api.Migrations
{
    /// <inheritdoc />
    public partial class AddNullFKCoursePaymentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Courses_FkCourse",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "FkCourse",
                table: "Payments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Courses_FkCourse",
                table: "Payments",
                column: "FkCourse",
                principalTable: "Courses",
                principalColumn: "PkCourse");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Courses_FkCourse",
                table: "Payments");

            migrationBuilder.AlterColumn<int>(
                name: "FkCourse",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Courses_FkCourse",
                table: "Payments",
                column: "FkCourse",
                principalTable: "Courses",
                principalColumn: "PkCourse",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
