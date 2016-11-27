using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using KPcore.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace KPcore.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<TopicEntry> TopicEntries { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<StudentGroup> StudentGroups { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Topic>()
                .HasOne(t => t.Subject)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Topic>()
                .HasOne(t => t.Teacher)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<TopicEntry>()
                .HasOne(t => t.Topic)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<TopicEntry>()
                .HasOne(a => a.Author)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Group>()
                .HasOne(s => s.Subject)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<StudentGroup>()
                .HasKey(sg => new { sg.StudentId, sg.GroupId });

            builder.Entity<StudentGroup>()
                .HasOne(s => s.Student)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<StudentGroup>()
                .HasOne(g => g.Group)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
