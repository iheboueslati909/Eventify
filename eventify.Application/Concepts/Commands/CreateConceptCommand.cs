
using eventify.Domain.ValueObjects;
using eventify.Domain.Entities;
using eventify.Domain.Enums;
using eventify.SharedKernel;


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
    if (titleResult.IsFailure)
        return Result<Guid>.Failure(titleResult.Error);

    var descriptionResult = Description.Create(command.Description);
    if (descriptionResult.IsFailure)
        return Result<Guid>.Failure(descriptionResult.Error);

    var genres = new List<MusicGenre>();

    foreach (var genreValue in command.Genres)
    {
        if (!Enum.IsDefined(typeof(MusicGenre), genreValue))
            return Result<Guid>.Failure($"Invalid genre value: {genreValue}");

        genres.Add((MusicGenre)genreValue);
    }

    var genreCollection = MusicGenreCollection.Create(genres);

    if (genreCollection.IsFailure)
        return Result<Guid>.Failure(genreCollection.Error);

    var concept = Concept.Create(command.MemberId, titleResult.Value, descriptionResult.Value, genreCollection.Value);

    if (!concept.IsSuccess)
        return Result<Guid>.Failure(concept.Error);
    await _conceptRepository.AddAsync(concept.Value);
    await _conceptRepository.SaveChangesAsync();

    return Result<Guid>.Success(concept.Value.Id);
}


}
