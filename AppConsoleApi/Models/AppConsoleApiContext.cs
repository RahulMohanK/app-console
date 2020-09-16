using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AppConsoleApi.Models
{
    public partial class AppConsoleApiContext : DbContext
    {
        public AppConsoleApiContext()
        {
        }

        public AppConsoleApiContext(DbContextOptions<AppConsoleApiContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Application> Application { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Project> Project { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=DESKTOP-ROBHQ7Q;Database=Console Database;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>(entity =>
            {
                entity.HasKey(e => e.AppId);

                entity.Property(e => e.AppId).HasColumnName("App_Id");

                entity.Property(e => e.CategoryId).HasColumnName("Category_Id");

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasColumnName("File_Name")
                    .HasMaxLength(100);

                entity.Property(e => e.ProjectId).HasColumnName("Project_Id");

                entity.Property(e => e.UploadedDate)
                    .HasColumnName("Uploaded_Date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Application)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Application_Category");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Application)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_Application_Project");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasColumnName("Category_Name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.BundleIdentifier)
                    .IsRequired()
                    .HasColumnName("Bundle_Identifier")
                    .HasMaxLength(100);

                entity.Property(e => e.ProjectName)
                    .IsRequired()
                    .HasColumnName("Project_Name")
                    .HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
