using EducationPortal.Core.Models.Auth;
using EducationPortal.Core.Models.Entities;
using System;
using System.Data.Entity;

namespace EducationPortal.DAL.EF.Context
{
    public partial class EDUDbContext : DbContext
    {
        public EDUDbContext() : base("EDUDbContext") { }

        public virtual DbSet<Article> Article { get; set; }
        public virtual DbSet<Book> Book { get; set; }
        public virtual DbSet<CompletedUserMaterial> CompletedUserMaterial { get; set; }
        public virtual DbSet<Course> Course { get; set; }
        public virtual DbSet<Material> Material { get; set; }
        public virtual DbSet<MaterialCourse> MaterialCourse { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Skill> Skill { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserCourse> UserCourse { get; set; }
        public virtual DbSet<UserSkill> UserSkill { get; set; }
        public virtual DbSet<Video> Video { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Article>().ToTable("Article");
            modelBuilder.Entity<Book>().ToTable("Book");
            modelBuilder.Entity<CompletedUserMaterial>().ToTable("CompletedUserMaterial");
            modelBuilder.Entity<Course>().ToTable("Course");
            modelBuilder.Entity<Material>().ToTable("Material");
            modelBuilder.Entity<MaterialCourse>().ToTable("MaterialCourse");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Skill>().ToTable("Skill");
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<UserCourse>().ToTable("UserCourse");
            modelBuilder.Entity<UserSkill>().ToTable("UserSkill");
            modelBuilder.Entity<Video>().ToTable("Video");
            
            modelBuilder.Entity<Role>()
                .HasMany(e => e.User)
                .WithRequired(e => e.Role)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasOptional(e => e.UserSkill)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete();

            base.OnModelCreating(modelBuilder);
        }
    }
}
