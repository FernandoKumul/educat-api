using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace educat_api.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    PkCategory = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.PkCategory);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    PkUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    AvatarUrl = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    IsInstructor = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.PkUser);
                });

            migrationBuilder.CreateTable(
                name: "Instructors",
                columns: table => new
                {
                    PkInstructor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkUser = table.Column<int>(type: "int", nullable: false),
                    Occupation = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    FacebookUser = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    YoutubeUser = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    LinkediId = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    TwitterUser = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    EmailPaypal = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instructors", x => x.PkInstructor);
                    table.ForeignKey(
                        name: "FK_Instructors_Users_FkUser",
                        column: x => x.FkUser,
                        principalTable: "Users",
                        principalColumn: "PkUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    PkCourse = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FKInstructor = table.Column<int>(type: "int", nullable: false),
                    FkCategory = table.Column<int>(type: "int", nullable: true),
                    Title = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: false),
                    Summary = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Language = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Difficulty = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    VideoPresentation = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Cover = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: true),
                    Requeriments = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    LearnText = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    Tags = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: false),
                    CretionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.PkCourse);
                    table.ForeignKey(
                        name: "FK_Courses_Categories_FkCategory",
                        column: x => x.FkCategory,
                        principalTable: "Categories",
                        principalColumn: "PkCategory");
                    table.ForeignKey(
                        name: "FK_Courses_Instructors_FKInstructor",
                        column: x => x.FKInstructor,
                        principalTable: "Instructors",
                        principalColumn: "PkInstructor",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartWishList",
                columns: table => new
                {
                    PkCartWishList = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkCourse = table.Column<int>(type: "int", nullable: false),
                    FkUser = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartWishList", x => x.PkCartWishList);
                    table.ForeignKey(
                        name: "FK_CartWishList_Courses_FkCourse",
                        column: x => x.FkCourse,
                        principalTable: "Courses",
                        principalColumn: "PkCourse",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartWishList_Users_FkUser",
                        column: x => x.FkUser,
                        principalTable: "Users",
                        principalColumn: "PkUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PkPayment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkCourse = table.Column<int>(type: "int", nullable: false),
                    FkUser = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    PayerId = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true),
                    PayerEmail = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    PaymentAmount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    PaymentStatus = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    PaymentMethod = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: false),
                    CardType = table.Column<string>(type: "varchar(30)", unicode: false, maxLength: 30, nullable: true),
                    Archived = table.Column<bool>(type: "bit", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PkPayment);
                    table.ForeignKey(
                        name: "FK_Payments_Courses_FkCourse",
                        column: x => x.FkCourse,
                        principalTable: "Courses",
                        principalColumn: "PkCourse",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_Users_FkUser",
                        column: x => x.FkUser,
                        principalTable: "Users",
                        principalColumn: "PkUser");
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    PkUnit = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkCourse = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.PkUnit);
                    table.ForeignKey(
                        name: "FK_Units_Courses_FkCourse",
                        column: x => x.FkCourse,
                        principalTable: "Courses",
                        principalColumn: "PkCourse",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    PkLesson = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fkunit = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    Type = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    VideoUrl = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Text = table.Column<string>(type: "VARCHAR(MAX)", unicode: false, nullable: true),
                    TimeDuration = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CretionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.PkLesson);
                    table.ForeignKey(
                        name: "FK_Lessons_Units_Fkunit",
                        column: x => x.Fkunit,
                        principalTable: "Units",
                        principalColumn: "PkUnit",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    PkComment = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkUser = table.Column<int>(type: "int", nullable: false),
                    FkCourse = table.Column<int>(type: "int", nullable: true),
                    FkLesson = table.Column<int>(type: "int", nullable: true),
                    Text = table.Column<string>(type: "varchar(200)", unicode: false, maxLength: 200, nullable: false),
                    Score = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: false),
                    CretionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.PkComment);
                    table.ForeignKey(
                        name: "FK_Comments_Courses_FkCourse",
                        column: x => x.FkCourse,
                        principalTable: "Courses",
                        principalColumn: "PkCourse");
                    table.ForeignKey(
                        name: "FK_Comments_Lessons_FkLesson",
                        column: x => x.FkLesson,
                        principalTable: "Lessons",
                        principalColumn: "PkLesson");
                    table.ForeignKey(
                        name: "FK_Comments_Users_FkUser",
                        column: x => x.FkUser,
                        principalTable: "Users",
                        principalColumn: "PkUser",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LessonsProgress",
                columns: table => new
                {
                    PkLessonProgress = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkLesson = table.Column<int>(type: "int", nullable: false),
                    FkPayment = table.Column<int>(type: "int", nullable: false),
                    Done = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonsProgress", x => x.PkLessonProgress);
                    table.ForeignKey(
                        name: "FK_LessonsProgress_Lessons_FkLesson",
                        column: x => x.FkLesson,
                        principalTable: "Lessons",
                        principalColumn: "PkLesson",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonsProgress_Payments_FkPayment",
                        column: x => x.FkPayment,
                        principalTable: "Payments",
                        principalColumn: "PkPayment",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    PkLike = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkUser = table.Column<int>(type: "int", nullable: false),
                    FkComment = table.Column<int>(type: "int", nullable: false),
                    IsLike = table.Column<bool>(type: "bit", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => x.PkLike);
                    table.ForeignKey(
                        name: "FK_Likes_Comments_FkComment",
                        column: x => x.FkComment,
                        principalTable: "Comments",
                        principalColumn: "PkComment",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Likes_Users_FkUser",
                        column: x => x.FkUser,
                        principalTable: "Users",
                        principalColumn: "PkUser",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "PkCategory", "Name" },
                values: new object[,]
                {
                    { 1, "Diseño" },
                    { 2, "Informática y software" },
                    { 3, "Fotografía" },
                    { 4, "Desarrollo" },
                    { 5, "Desarrollo personal" },
                    { 6, "Música" },
                    { 7, "Marketing" },
                    { 8, "Negocios" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartWishList_FkCourse",
                table: "CartWishList",
                column: "FkCourse");

            migrationBuilder.CreateIndex(
                name: "IX_CartWishList_FkUser",
                table: "CartWishList",
                column: "FkUser");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_FkCourse",
                table: "Comments",
                column: "FkCourse");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_FkLesson",
                table: "Comments",
                column: "FkLesson");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_FkUser",
                table: "Comments",
                column: "FkUser");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_FkCategory",
                table: "Courses",
                column: "FkCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_FKInstructor",
                table: "Courses",
                column: "FKInstructor");

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_FkUser",
                table: "Instructors",
                column: "FkUser",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_Fkunit",
                table: "Lessons",
                column: "Fkunit");

            migrationBuilder.CreateIndex(
                name: "IX_LessonsProgress_FkLesson",
                table: "LessonsProgress",
                column: "FkLesson");

            migrationBuilder.CreateIndex(
                name: "IX_LessonsProgress_FkPayment",
                table: "LessonsProgress",
                column: "FkPayment");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_FkComment",
                table: "Likes",
                column: "FkComment");

            migrationBuilder.CreateIndex(
                name: "IX_Likes_FkUser",
                table: "Likes",
                column: "FkUser");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_FkCourse",
                table: "Payments",
                column: "FkCourse");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_FkUser",
                table: "Payments",
                column: "FkUser");

            migrationBuilder.CreateIndex(
                name: "IX_Units_FkCourse",
                table: "Units",
                column: "FkCourse");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartWishList");

            migrationBuilder.DropTable(
                name: "LessonsProgress");

            migrationBuilder.DropTable(
                name: "Likes");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Instructors");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
