﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using educat_api.Context;

#nullable disable

namespace educat_api.Migrations
{
    [DbContext(typeof(AppDBContext))]
    [Migration("20240602001005_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.CartWishList", b =>
                {
                    b.Property<int>("PkCartWishList")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PkCartWishList"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("FkCourse")
                        .HasColumnType("int");

                    b.Property<int>("FkUser")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.HasKey("PkCartWishList");

                    b.HasIndex("FkCourse");

                    b.HasIndex("FkUser");

                    b.ToTable("CartWishList");
                });

            modelBuilder.Entity("Domain.Entities.Category", b =>
                {
                    b.Property<int>("PkCategory")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PkCategory"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("PkCategory");

                    b.ToTable("Categories");

                    b.HasData(
                        new
                        {
                            PkCategory = 1,
                            Name = "Diseño"
                        },
                        new
                        {
                            PkCategory = 2,
                            Name = "Informática y software"
                        },
                        new
                        {
                            PkCategory = 3,
                            Name = "Fotografía"
                        },
                        new
                        {
                            PkCategory = 4,
                            Name = "Desarrollo"
                        },
                        new
                        {
                            PkCategory = 5,
                            Name = "Desarrollo personal"
                        },
                        new
                        {
                            PkCategory = 6,
                            Name = "Música"
                        },
                        new
                        {
                            PkCategory = 7,
                            Name = "Marketing"
                        },
                        new
                        {
                            PkCategory = 8,
                            Name = "Negocios"
                        });
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.Property<int>("PkComment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PkComment"));

                    b.Property<DateTime>("CretionDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("FkCourse")
                        .HasColumnType("int");

                    b.Property<int?>("FkLesson")
                        .HasColumnType("int");

                    b.Property<int>("FkUser")
                        .HasColumnType("int");

                    b.Property<decimal>("Score")
                        .HasPrecision(3, 2)
                        .HasColumnType("decimal(3,2)");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.HasKey("PkComment");

                    b.HasIndex("FkCourse");

                    b.HasIndex("FkLesson");

                    b.HasIndex("FkUser");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Domain.Entities.Course", b =>
                {
                    b.Property<int>("PkCourse")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PkCourse"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Cover")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime>("CretionDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<string>("Difficulty")
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<int>("FKInstructor")
                        .HasColumnType("int");

                    b.Property<int?>("FkCategory")
                        .HasColumnType("int");

                    b.Property<string>("Language")
                        .HasMaxLength(20)
                        .IsUnicode(false)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("LearnText")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<decimal?>("Price")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("Requeriments")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<string>("Summary")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Tags")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(150)
                        .IsUnicode(false)
                        .HasColumnType("varchar(150)");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("VideoPresentation")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.HasKey("PkCourse");

                    b.HasIndex("FKInstructor");

                    b.HasIndex("FkCategory");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("Domain.Entities.Instructor", b =>
                {
                    b.Property<int>("PkInstructor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PkInstructor"));

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("EmailPaypal")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("FacebookUser")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<int>("FkUser")
                        .HasColumnType("int");

                    b.Property<string>("LinkediId")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("Occupation")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("TwitterUser")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<string>("YoutubeUser")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.HasKey("PkInstructor");

                    b.HasIndex("FkUser")
                        .IsUnique();

                    b.ToTable("Instructors");
                });

            modelBuilder.Entity("Domain.Entities.Lesson", b =>
                {
                    b.Property<int>("PkLesson")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PkLesson"));

                    b.Property<DateTime>("CretionDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Fkunit")
                        .HasColumnType("int");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .IsUnicode(false)
                        .HasColumnType("VARCHAR(MAX)");

                    b.Property<int>("TimeDuration")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<string>("VideoUrl")
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.HasKey("PkLesson");

                    b.HasIndex("Fkunit");

                    b.ToTable("Lessons");
                });

            modelBuilder.Entity("Domain.Entities.LessonProgress", b =>
                {
                    b.Property<int>("PkLessonProgress")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PkLessonProgress"));

                    b.Property<bool>("Done")
                        .HasColumnType("bit");

                    b.Property<int>("FkLesson")
                        .HasColumnType("int");

                    b.Property<int>("FkPayment")
                        .HasColumnType("int");

                    b.HasKey("PkLessonProgress");

                    b.HasIndex("FkLesson");

                    b.HasIndex("FkPayment");

                    b.ToTable("LessonsProgress");
                });

            modelBuilder.Entity("Domain.Entities.Like", b =>
                {
                    b.Property<int>("PkLike")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PkLike"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("FkComment")
                        .HasColumnType("int");

                    b.Property<int>("FkUser")
                        .HasColumnType("int");

                    b.Property<bool>("IsLike")
                        .HasColumnType("bit");

                    b.HasKey("PkLike");

                    b.HasIndex("FkComment");

                    b.HasIndex("FkUser");

                    b.ToTable("Likes");
                });

            modelBuilder.Entity("Domain.Entities.Payment", b =>
                {
                    b.Property<int>("PkPayment")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PkPayment"));

                    b.Property<bool>("Archived")
                        .HasColumnType("bit");

                    b.Property<string>("CardType")
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false)
                        .HasColumnType("varchar(10)");

                    b.Property<int>("FkCourse")
                        .HasColumnType("int");

                    b.Property<int>("FkUser")
                        .HasColumnType("int");

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PayerEmail")
                        .IsRequired()
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("PayerId")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<decimal>("PaymentAmount")
                        .HasPrecision(10, 2)
                        .HasColumnType("decimal(10,2)");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<string>("PaymentStatus")
                        .IsRequired()
                        .HasMaxLength(30)
                        .IsUnicode(false)
                        .HasColumnType("varchar(30)");

                    b.Property<DateTime>("TransactionDate")
                        .HasColumnType("datetime2");

                    b.HasKey("PkPayment");

                    b.HasIndex("FkCourse");

                    b.HasIndex("FkUser");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("Domain.Entities.Unit", b =>
                {
                    b.Property<int>("PkUnit")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PkUnit"));

                    b.Property<int>("FkCourse")
                        .HasColumnType("int");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("PkUnit");

                    b.HasIndex("FkCourse");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<int>("PkUser")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PkUser"));

                    b.Property<string>("AvatarUrl")
                        .HasMaxLength(200)
                        .IsUnicode(false)
                        .HasColumnType("varchar(200)");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<bool>("IsInstructor")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("PkUser");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Domain.Entities.CartWishList", b =>
                {
                    b.HasOne("Domain.Entities.Course", "Course")
                        .WithMany("CartWishList")
                        .HasForeignKey("FkCourse")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("CartWishLists")
                        .HasForeignKey("FkUser")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.HasOne("Domain.Entities.Course", "Course")
                        .WithMany("Comments")
                        .HasForeignKey("FkCourse");

                    b.HasOne("Domain.Entities.Lesson", "Lesson")
                        .WithMany("Comments")
                        .HasForeignKey("FkLesson");

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("Comments")
                        .HasForeignKey("FkUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("Lesson");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Course", b =>
                {
                    b.HasOne("Domain.Entities.Instructor", "Instructor")
                        .WithMany("Courses")
                        .HasForeignKey("FKInstructor")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Category", "Category")
                        .WithMany("Courses")
                        .HasForeignKey("FkCategory");

                    b.Navigation("Category");

                    b.Navigation("Instructor");
                });

            modelBuilder.Entity("Domain.Entities.Instructor", b =>
                {
                    b.HasOne("Domain.Entities.User", "User")
                        .WithOne("Instructor")
                        .HasForeignKey("Domain.Entities.Instructor", "FkUser")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Lesson", b =>
                {
                    b.HasOne("Domain.Entities.Unit", "Unit")
                        .WithMany("Lessons")
                        .HasForeignKey("Fkunit")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Unit");
                });

            modelBuilder.Entity("Domain.Entities.LessonProgress", b =>
                {
                    b.HasOne("Domain.Entities.Lesson", "Lesson")
                        .WithMany("LessonsProgress")
                        .HasForeignKey("FkLesson")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.Payment", "Payment")
                        .WithMany("LessonsProgress")
                        .HasForeignKey("FkPayment")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Lesson");

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("Domain.Entities.Like", b =>
                {
                    b.HasOne("Domain.Entities.Comment", "Comment")
                        .WithMany("Likes")
                        .HasForeignKey("FkComment")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("Likes")
                        .HasForeignKey("FkUser")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Comment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Payment", b =>
                {
                    b.HasOne("Domain.Entities.Course", "Course")
                        .WithMany("Payments")
                        .HasForeignKey("FkCourse")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("Payments")
                        .HasForeignKey("FkUser")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Course");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Domain.Entities.Unit", b =>
                {
                    b.HasOne("Domain.Entities.Course", "Course")
                        .WithMany("Units")
                        .HasForeignKey("FkCourse")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("Domain.Entities.Category", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("Domain.Entities.Comment", b =>
                {
                    b.Navigation("Likes");
                });

            modelBuilder.Entity("Domain.Entities.Course", b =>
                {
                    b.Navigation("CartWishList");

                    b.Navigation("Comments");

                    b.Navigation("Payments");

                    b.Navigation("Units");
                });

            modelBuilder.Entity("Domain.Entities.Instructor", b =>
                {
                    b.Navigation("Courses");
                });

            modelBuilder.Entity("Domain.Entities.Lesson", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("LessonsProgress");
                });

            modelBuilder.Entity("Domain.Entities.Payment", b =>
                {
                    b.Navigation("LessonsProgress");
                });

            modelBuilder.Entity("Domain.Entities.Unit", b =>
                {
                    b.Navigation("Lessons");
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Navigation("CartWishLists");

                    b.Navigation("Comments");

                    b.Navigation("Instructor");

                    b.Navigation("Likes");

                    b.Navigation("Payments");
                });
#pragma warning restore 612, 618
        }
    }
}
