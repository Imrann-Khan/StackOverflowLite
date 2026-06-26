namespace StackOverflowLite.Domain.Model;


public enum VoteType
{
    Upvote = 1,
    Downvote = -1
}

public class Vote
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public VoteType Type { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    // Foreign Key
    public string UserId { get; set; } = string.Empty;
    public Guid? QuestionId { get; set; }
    public Guid? AnswerId { get; set; }
    
    public ApplicationUser User { get; set; } = null!;
    public Question? Question { get; set; }
    public Answer? Answer { get; set; }
}