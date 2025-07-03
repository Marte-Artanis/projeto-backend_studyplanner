   using Microsoft.EntityFrameworkCore;
   using StudyPlanner.Models;

   namespace StudyPlanner
   {
       public class PlannerContext : DbContext
       {
           public DbSet<StudySession> Sessions => Set<StudySession>();
           public DbSet<Subject> Subjects => Set<Subject>();
           public DbSet<Goal> Goals => Set<Goal>();

           public PlannerContext() { }
           public PlannerContext(DbContextOptions<PlannerContext> options): base(options) { }

           protected override void OnConfiguring(DbContextOptionsBuilder options)
           {
               if (!options.IsConfigured)
               {
                   options.UseSqlite("Data Source=studyplanner.db");
               }
           }

           protected override void OnModelCreating(ModelBuilder modelBuilder)
           {
               modelBuilder.Entity<Subject>()
                           .HasIndex(s => s.Name)
                           .IsUnique();
           }
       }
   }