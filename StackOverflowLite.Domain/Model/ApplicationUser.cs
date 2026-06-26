namespace StackOverflowLite.Domain.Model;
using Microsoft.AspNetCore.Identity;

public class ApplicationUser : IdentityUser
{
    public int Reputation {get; set;} = 0;
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

    public ICollection<Question> Questions {get; set;} = [];
    public ICollection<Answer> Answers {get; set;} = [];
    public ICollection<Vote> Votes {get; set;} = [];
}