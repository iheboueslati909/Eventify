namespace eventify.Application.Concepts.Commands;

public class CreateConceptCommand
{
    public Guid MemberId { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<int> Genres { get; set; } = new();
}