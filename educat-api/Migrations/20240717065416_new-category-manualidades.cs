using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace educat_api.Migrations
{
    /// <inheritdoc />
    public partial class newcategorymanualidades : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "PkCategory", "Name" },
                values: new object[] { 9, "Manualidades" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "PkCategory",
                keyValue: 9);
        }
    }
}
