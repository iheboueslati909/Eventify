
using eventify.Domain.ValueObjects;
using eventify.Domain.Entities;
using eventify.Domain.Enums;


namespace eventify.Application.Concepts.Commands;

public class CreateConceptCommand
{
    public Guid MemberId { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<int> Genres { get; set; } = new();
}
public class CreateConceptHandler
{
    private readonly IConceptRepository _conceptRepository;

    public CreateConceptHandler(IConceptRepository conceptRepository)
    {
        _conceptRepository = conceptRepository;
    }

   public async Task<Result<Guid>> Handle(CreateConceptCommand command)
{
    var titleResult = Title.Create(command.Title);
    if (!titleResult.IsSuccess)
        return Result<Guid>.Failure(titleResult.Error);

    var descriptionResult = Description.Create(command.Description);
    if (!descriptionResult.IsSuccess)
        return Result<Guid>.Failure(descriptionResult.Error);

    var genres = new List<MusicGenre>();

    foreach (var genreValue in command.Genres)
    {
        if (!Enum.IsDefined(typeof(MusicGenre), genreValue))
            return Result<Guid>.Failure($"Invalid genre value: {genreValue}");

        genres.Add((MusicGenre)genreValue);
    }

    var genreCollection = new MusicGenreCollection(genres);

    var concept = Concept.Create(command.MemberId, titleResult.Value, descriptionResult.Value, genreCollection);

    await _conceptRepository.AddAsync(concept);
    await _conceptRepository.SaveChangesAsync();

    return Result<Guid>.Success(concept.Id);
}


}
