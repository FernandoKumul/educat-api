using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace educat_api.Context
{
    public partial class AppDBContext : DbContext
    {
        public AppDBContext()
        {
        }

        public AppDBContext(DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Instructor> Instructors { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<Lesson> Lessons { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<CartWishList> CartWishList { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<LessonProgress> LessonsProgress { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.PkUser);
                entity.Property(e => e.Name)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasMaxLength(50);
                entity.Property(e => e.LastName) 
                    .IsRequired()
                    .IsUnicode(false)
                    .HasMaxLength(50);
                entity.Property(e => e.Email)
                    .IsRequired()
                    .IsUnicode(false)
                    .HasMaxLength(50);
                entity.Property(e => e.AvatarUrl)
                    .IsUnicode(false)
                    .HasMaxLength(200);
                entity.Property(e => e.Description)
                    .IsRequired()
                    .IsUnicode(false);
                entity.Property(e => e.IsInstructor)
                    .IsRequired()
                    .HasDefaultValue(false);
                entity.Property(e => e.CreationDate)
                    .IsRequired()
                    .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Instructor>(entity =>
            {
                entity.Property(e => e.Occupation)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.FacebookUser)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.YoutubeUser)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.LinkediId)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.TwitterUser)
                    .HasMaxLength(200)
                    .IsUnicode(false);
                entity.Property(e => e.EmailPaypal)
                    .HasMaxLength(100)
                    .IsUnicode(false);
                entity.Property(e => e.CreationDate)
                    .IsRequired()
                    .HasDefaultValueSql("GETDATE()");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsUnicode(false)
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Course>(entity =>
            {
                entity.Property(e => e.Title)             
                    .IsUnicode(false)
                    .HasMaxLength(150);
                entity.Property(e => e.Summary)             
                    .IsUnicode(false)
                    .HasMaxLength(255);
                entity.Property(e => e.Language)             
                    .IsUnicode(false)
                    .HasMaxLength(20);
                entity.Property(e => e.Difficulty)             
                    .IsUnicode(false)
                    .HasMaxLength(10);
                entity.Property(e => e.VideoPresentation)             
                    .IsUnicode(false)
                    .HasMaxLength(200);
                entity.Property(e => e.Cover)             
                    .IsUnicode(false)
                    .HasMaxLength(200);
                entity.Property(e => e.Requeriments)
                    .IsUnicode(false);
                entity.Property(e => e.Description)
                    .IsUnicode(false);
                entity.Property(e => e.LearnText)
                    .IsUnicode(false);
                entity.Property(e => e.Price)
                    .HasPrecision(10, 2);
                entity.Property(e => e.Tags)
                    .IsUnicode(false)
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsUnicode(false)
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Lesson>(entity =>
            {
                entity.Property(e => e.Title)
                    .IsUnicode(false)
                    .HasMaxLength(100);
                entity.Property(e => e.Type)
                    .IsUnicode(false)
                    .HasMaxLength(10);
                entity.Property(e => e.VideoUrl)
                    .IsUnicode(false)
                    .HasMaxLength(100);
                entity.Property(e => e.Text)
                    .IsUnicode(false)
                    .HasColumnType("VARCHAR(MAX)");
            });
            
            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Text)
                    .IsUnicode(false)
                    .HasMaxLength(200);
                entity.Property(e => e.Score)
                    .HasPrecision(3, 2);
            });
            
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.Property(e => e.OrderId)
                    .IsUnicode(false)
                    .HasMaxLength(255);
                entity.Property(e => e.PayerId)
                    .IsUnicode(false)
                    .HasMaxLength(255);
                entity.Property(e => e.PayerEmail)
                    .IsUnicode(false)
                    .HasMaxLength(255);
                entity.Property(e => e.Currency)
                    .IsUnicode(false)
                    .HasMaxLength(10);
                entity.Property(e => e.PaymentStatus)
                    .IsUnicode(false)
                    .HasMaxLength(30);
                entity.Property(e => e.PaymentMethod)
                    .IsUnicode(false)
                    .HasMaxLength(30);
                entity.Property(e => e.CardType)
                    .IsUnicode(false)
                    .HasMaxLength(30);
                entity.Property(e => e.PaymentAmount)
                    .HasPrecision(10, 2);

                entity.HasOne(p => p.User)
                    .WithMany(u => u.Payments)
                    .HasForeignKey(p => p.FkUser)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<CartWishList>(entity =>
            {
                entity.Property(e => e.Type)
                    .IsUnicode(false)
                    .HasMaxLength(10);

                entity.HasOne(c => c.User)
                    .WithMany(u => u.CartWishLists)
                    .HasForeignKey(c => c.FkUser)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<LessonProgress>(entity =>
            {
                entity.HasOne(l => l.Payment)
                    .WithMany(p => p.LessonsProgress)
                    .HasForeignKey(l => l.FkPayment)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.HasOne(l => l.User)
                    .WithMany(u => u.Likes)
                    .HasForeignKey(l => l.FkUser)
                    .OnDelete(DeleteBehavior.Restrict);
            });


            modelBuilder.Entity<Category>().HasData(
                new Category { PkCategory = 1, Name = "Diseño"},
                new Category { PkCategory = 2, Name = "Informática y software"},
                new Category { PkCategory = 3, Name = "Fotografía" },
                new Category { PkCategory = 4, Name = "Desarrollo" },
                new Category { PkCategory = 5, Name = "Desarrollo personal" },
                new Category { PkCategory = 6, Name = "Música" },
                new Category { PkCategory = 7, Name = "Marketing" },
                new Category { PkCategory = 8, Name = "Negocios" }
            );
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
