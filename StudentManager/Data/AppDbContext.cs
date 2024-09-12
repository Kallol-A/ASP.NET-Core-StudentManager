using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using StudentManager.Models;

namespace StudentManager.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<StudentCategory> StudentCategory { get; set; }
        public DbSet<StudentDetails> StudentDetails { get; set; }
        public DbSet<StudentFees> StudentFees { get; set; }

        // Your DbContext configuration goes here
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure your entity relationships and constraints here
            modelBuilder.Entity<Role>().ToTable("TB_ROLE");
            modelBuilder.Entity<Permission>().ToTable("TB_PERMISSION");
            modelBuilder.Entity<RolePermission>().ToTable("TB_ROLE_PERMISSION");
            modelBuilder.Entity<User>().ToTable("TB_USER");
            modelBuilder.Entity<StudentCategory>().ToTable("TB_STUDENT_CATEGORY");
            modelBuilder.Entity<StudentDetails>().ToTable("TB_STUDENT_DETAILS");
            modelBuilder.Entity<StudentFees>().ToTable("TB_STUDENT_FEES");
            // User - StudentDetails: One-to-One Relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.StudentDetails)
                .WithOne(sd => sd.Student)
                .HasForeignKey<StudentDetails>(sd => sd.id_user);

            // User - StudentFees: One-to-Many Relationship
            modelBuilder.Entity<User>()
                .HasMany(u => u.StudentFees)
                .WithOne(sf => sf.Student)
                .HasForeignKey(sf => sf.id_user);

            // StudentDetails - StudentCategory: One-to-One Relationship
            modelBuilder.Entity<StudentDetails>()
                .HasOne(sd => sd.StudentCategory)
                .WithOne(sc => sc.StudentDetails)
                .HasForeignKey<StudentDetails>(sd => sd.id_student_category);

            // Configuring the required and optional fields, if necessary
            modelBuilder.Entity<StudentDetails>()
                .Property(sd => sd.student_first_name)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.user_email)
                .IsRequired();

            // Configure other properties, relationships, indexes, etc., as needed.

            base.OnModelCreating(modelBuilder);
        }
    }
}
