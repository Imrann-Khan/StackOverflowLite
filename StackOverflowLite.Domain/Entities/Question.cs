namespace StackOverflowLite.Domain.Entities;

public class Question
{
    public Guid Id {get; set;} = Guid.NewGuid();
    public string Title {get; set;} = string.Empty;
    public string Description {get; set;} = string.Empty;
    public int ViewCount { get; set; } = 0;  // tracked in Redis, persisted here
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public DateTime? UpdatedAt {get; set;}

    // Foreign Key
    public string AuthorId {get; set;} = string.Empty;
    public Guid? AcceptedAnswerId {get; set;}

    public ApplicationUser Author {get; set;} = null!;
    public Answer? AcceptedAnswer {get; set;}
    public ICollection<Answer> Answers {get; set;} = [];
    public ICollection<Vote> Votes { get; set; } = [];
    public ICollection<QuestionTag> QuestionTags {get; set;} = [];
}