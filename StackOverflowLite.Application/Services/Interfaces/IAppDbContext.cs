using Microsoft.EntityFrameworkCore;
using StackOverflowLite.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace StackOverflowLite.Application.Services.Interfaces;

public interface IAppDbContext
{
    DbSet<ApplicationUser> Users { get; }
    DbSet<Question> Questions { get; }
    DbSet<Answer> Answers { get; }
    DbSet<Tag> Tags { get; }
    DbSet<QuestionTag> QuestionTags { get; }
    DbSet<Vote> Votes { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
