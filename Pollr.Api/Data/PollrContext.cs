using Microsoft.EntityFrameworkCore;
using Pollr.Api.Models;
using Pollr.Api.Models.PollDefinitions;
using Pollr.Api.Models.Polls;
using System;
using System.Data;

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

            // Unique constraint for Poll handle
            modelBuilder.Entity<Poll>()
                .HasIndex(p => p.Handle)
                .IsUnique();
        }

        public DbConnectionInfo GetConnectionInfo()
        {
            var info = new DbConnectionInfo
            {
                State = "Disconnected"
            };

            using (this)
            {
                try
                {
                    var connection = this.Database.GetDbConnection();
                    connection.Open();
                    if (connection.State == ConnectionState.Open)
                    {
                        info.State = connection.State.ToString();
                        info.Database = connection.Database;
                        info.DataSource = connection.DataSource;
                        info.ServerVersion = connection.ServerVersion;
                        info.ConnectionTimeout = connection.ConnectionTimeout;
                        connection.Close();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return info;
        }

        public DbSet<PollDefinition> PollDefinitions { get; set; }
        public DbSet<Poll> Polls { get; set; }
    }
}
