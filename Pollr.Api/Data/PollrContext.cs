using Microsoft.EntityFrameworkCore;
using Pollr.Api.Models.PollDefinitions;
using Pollr.Api.Models.Polls;

namespace Pollr.Api.Data
{
    public class PollrContext : DbContext
    {
        public PollrContext(DbContextOptions<PollrContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Cascade Delete on Poll Definition to QuestionDefinition
            modelBuilder.Entity<QuestionDefinition>()
                .HasOne(p => p.PollDefinition)
                .WithMany(b => b.Questions)
                .OnDelete(DeleteBehavior.Cascade);

            // Cascade Delete on QuestionDefinition to Candidate Answer
            modelBuilder.Entity<CandidateAnswer>()
                .HasOne(p => p.QuestionDefinition)
                .WithMany(b => b.Answers)
                .OnDelete(DeleteBehavior.Cascade);

            // Cascade Delete on Poll to Question
            modelBuilder.Entity<Question>()
                .HasOne(p => p.Poll)
                .WithMany(b => b.Questions)
                .OnDelete(DeleteBehavior.Cascade);

            // Cascade Delete on Question to Answer
            modelBuilder.Entity<Answer>()
                .HasOne(p => p.Question)
                .WithMany(b => b.Answers)
                .OnDelete(DeleteBehavior.Cascade);
        }

        public bool Ping()
        {
            // TODO: Add database keepalive code
            return true;
        }

        public DbSet<PollDefinition> PollDefinitions { get; set; }
        public DbSet<Poll> Polls { get; set; }
    }
}
