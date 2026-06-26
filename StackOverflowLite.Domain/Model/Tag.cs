namespace StackOverflowLite.Domain.Model;
public class Tag
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty; // unique
    public ICollection<QuestionTag> QuestionTags { get; set; } = [];
}