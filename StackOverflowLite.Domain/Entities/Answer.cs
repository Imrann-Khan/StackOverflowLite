namespace StackOverflowLite.Domain.Entities;

public class Answer
{
    public Guid Id {get; set;} = Guid.NewGuid();
    public string Content {get; set;} = string.Empty;
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public bool IsAccepted {get; set;} = false;
    public DateTime UpdatedAt {get; set;}

    // Foreign Key
    public string AuthorId {get; set;} = string.Empty;
    public Guid QuestionId {get; set;}

    public ApplicationUser Author {get; set;} = null!;
    public Question Question {get; set;} = null!;
    public ICollection<Vote> Votes {get; set;} = [];
}