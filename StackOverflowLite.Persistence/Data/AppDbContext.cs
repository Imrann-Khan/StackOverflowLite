using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Domain.Entities;

using StackOverflowLite.Application.Services.Interfaces;

namespace StackOverflowLite.Persistence.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser>(options), IAppDbContext
{
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Answer> Answers => Set<Answer>();
    public DbSet<Vote> Votes => Set<Vote>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<QuestionTag> QuestionTags => Set<QuestionTag>();

    // MUST be override — without this keyword EF Core never calls this method!
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // QuestionTag — composite primary key (many-to-many join table)
        builder.Entity<QuestionTag>()
            .HasKey(qt => new { qt.QuestionId, qt.TagId });

        // Tag name must be unique across the platform
        builder.Entity<Tag>()
            .HasIndex(t => t.Name)
            .IsUnique();

        // Question → AcceptedAnswer: nullable FK, use SetNull not Cascade
        // (Cascade here would cause circular delete cycle with Answer → Question)
        builder.Entity<Question>()
            .HasOne(q => q.AcceptedAnswer)
            .WithMany()
            .HasForeignKey(q => q.AcceptedAnswerId)
            .OnDelete(DeleteBehavior.SetNull);

        // Answer → Question: cascade delete answers when question deleted
        builder.Entity<Answer>()
            .HasOne(a => a.Question)
            .WithMany(q => q.Answers)
            .HasForeignKey(a => a.QuestionId)
            .OnDelete(DeleteBehavior.Cascade);

        // Vote → Question: nullable FK (vote is on question OR answer, not both)
        builder.Entity<Vote>()
            .HasOne(v => v.Question)
            .WithMany(q => q.Votes)
            .HasForeignKey(v => v.QuestionId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);

        // Vote → Answer: nullable FK
        builder.Entity<Vote>()
            .HasOne(v => v.Answer)
            .WithMany(a => a.Votes)
            .HasForeignKey(v => v.AnswerId)
            .OnDelete(DeleteBehavior.Cascade)
            .IsRequired(false);
    }
}
