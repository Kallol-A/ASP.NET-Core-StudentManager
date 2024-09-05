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
        public DbSet<User> Users { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        // Other DbSet properties for additional entities

        // Your DbContext configuration goes here

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure your entity relationships and constraints here
            modelBuilder.Entity<Role>().ToTable("TB_ROLE");
            modelBuilder.Entity<Permission>().ToTable("TB_PERMISSION");
            modelBuilder.Entity<RolePermission>().ToTable("TB_ROLE_PERMISSION");
            modelBuilder.Entity<User>().ToTable("TB_USER");
            //modelBuilder.Entity<Student>().ToTable("TB_USER");
            //modelBuilder.Entity<StudentCategory>().ToTable("TB_STUDENT_CATEGORY");
            //modelBuilder.Entity<StudentDetails>().ToTable("TB_STUDENT_DETAILS");
            //modelBuilder.Entity<StudentFees>().ToTable("TB_STUDENT_FEES");
            //modelBuilder.Entity<Student>()
            //    .HasOne(s => s.StudentDetails)
            //    .WithOne(sd => sd.Student)
            //    .HasForeignKey<StudentDetails>(sd => sd.id_student);
            //modelBuilder.Entity<Student>()
            //    .HasMany(s => s.StudentFees)  // A student has many fees
            //    .WithOne(sf => sf.Student)    // Each fee entry is associated with one student
            //    .HasForeignKey(sf => sf.id_student); // Foreign key in StudentFees table
        }
    }
}
