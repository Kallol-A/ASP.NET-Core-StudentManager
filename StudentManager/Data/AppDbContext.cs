using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using WebApi.Models;

namespace WebApi.Data
{
    public class AppDbContext : DbContext
    {
        // Constructor
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Student> Students { get; set; }
        public DbSet<StudentDetails> StudentDetails { get; set; }
        public DbSet<StudentCategory> StudentCategories { get; set; }
        public DbSet<StudentFees> StudentFees { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        // Other DbSet properties for additional entities

        // Your DbContext configuration goes here

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure your entity relationships and constraints here
            modelBuilder.Entity<Role>().ToTable("tb_role");
            modelBuilder.Entity<Permission>().ToTable("tb_permission");
            modelBuilder.Entity<RolePermission>().ToTable("tb_role_permission");
            modelBuilder.Entity<User>().ToTable("tb_user");
            modelBuilder.Entity<StudentCategory>().ToTable("tb_student_category");
            modelBuilder.Entity<StudentDetails>().ToTable("tb_student_details");
            modelBuilder.Entity<StudentFees>().ToTable("tb_student_fees");
            modelBuilder.Entity<Student>().ToTable("tb_student");
            modelBuilder.Entity<Student>()
                .HasOne(s => s.StudentDetails)
                .WithOne(sd => sd.Student)
                .HasForeignKey<StudentDetails>(sd => sd.id_student);
            modelBuilder.Entity<Student>()
                .HasMany(s => s.StudentFees)  // A student has many fees
                .WithOne(sf => sf.Student)    // Each fee entry is associated with one student
                .HasForeignKey(sf => sf.id_student); // Foreign key in StudentFees table
        }
    }
}
