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
        public DbSet<Department> Departments { get; set; }
        public DbSet<StudentDetails> StudentDetails { get; set; }
        public DbSet<FacultyDetails> FacultyDetails { get; set; }
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
            modelBuilder.Entity<Department>().ToTable("TB_DEPARTMENT");
            modelBuilder.Entity<StudentDetails>().ToTable("TB_STUDENT_DETAILS");
            modelBuilder.Entity<FacultyDetails>().ToTable("TB_FACULTY_DETAILS");
            modelBuilder.Entity<StudentFees>().ToTable("TB_STUDENT_FEES");
            
            // User - StudentDetails: One-to-One Relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.StudentDetails)
                .WithOne(sd => sd.Student)
                .HasForeignKey<StudentDetails>(sd => sd.id_user);

            // User - FacultyDetails: One-to-One Relationship
            modelBuilder.Entity<User>()
                .HasOne(u => u.FacultyDetails)
                .WithOne(sd => sd.Faculty)
                .HasForeignKey<FacultyDetails>(sd => sd.id_user);

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

            // FacultyDetails - Department: One-to-One Relationship
            modelBuilder.Entity<FacultyDetails>()
                .HasOne(sd => sd.Department)
                .WithOne(sc => sc.FacultyDetails)
                .HasForeignKey<FacultyDetails>(sd => sd.id_department);

            // Configure the RolePermission join table relationship
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => rp.id_role_permission);  // Primary key for RolePermission

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Role)
                .WithMany(r => r.RolePermissions)
                .HasForeignKey(rp => rp.id_role)
                .OnDelete(DeleteBehavior.Restrict);  // Optional: avoid cascading delete

            modelBuilder.Entity<RolePermission>()
                .HasOne(rp => rp.Permission)
                .WithMany(p => p.RolePermissions)
                .HasForeignKey(rp => rp.id_permission)
                .OnDelete(DeleteBehavior.Restrict);  // Optional: avoid cascading delete

            // Configuring the required and optional fields, if necessary
            modelBuilder.Entity<StudentDetails>()
                .Property(sd => sd.student_first_name)
                .IsRequired();
            modelBuilder.Entity<StudentDetails>()
                .Property(sd => sd.student_last_name)
                .IsRequired();

            // Configuring the required and optional fields, if necessary
            modelBuilder.Entity<FacultyDetails>()
                .Property(sd => sd.faculty_first_name)
                .IsRequired();
            modelBuilder.Entity<FacultyDetails>()
                .Property(sd => sd.faculty_last_name)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.user_email)
                .IsRequired();

            // Configure other properties, relationships, indexes, etc., as needed.

            base.OnModelCreating(modelBuilder);
        }
    }
}
