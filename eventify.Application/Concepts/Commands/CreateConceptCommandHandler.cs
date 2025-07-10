
using eventify.Domain.ValueObjects;
using eventify.Domain.Entities;
using eventify.Domain.Enums;
using eventify.SharedKernel;
using eventify.Application.Common;


namespace eventify.Application.Concepts.Commands;

public class CreateConceptCommand : ICommand<Result<Guid>>
{
    public Guid MemberId { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public List<int> Genres { get; set; } = new();
}
public class CreateConceptHandler : ICommandHandler<CreateConceptCommand, Result<Guid>>
{
    private readonly IConceptRepository _conceptRepository;

    public CreateConceptHandler(IConceptRepository conceptRepository)
    {
        _conceptRepository = conceptRepository;
    }

   public async Task<Result<Guid>> Handle(CreateConceptCommand command, CancellationToken cancellationToken)
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

    var genreCollectionResult = MusicGenreCollection.Create(genres);
    if (genreCollectionResult.IsFailure)
        return Result<Guid>.Failure(genreCollectionResult.Error);

    var conceptResult = Concept.Create(command.MemberId, titleResult.Value, descriptionResult.Value, genreCollectionResult.Value);
    if (conceptResult.IsFailure)
        return Result<Guid>.Failure(conceptResult.Error);


    await _conceptRepository.AddAsync(conceptResult.Value);
    await _conceptRepository.SaveChangesAsync();

    return Result<Guid>.Success(conceptResult.Value.Id);
}


}
