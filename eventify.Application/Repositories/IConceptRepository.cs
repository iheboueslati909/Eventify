using eventify.Domain.Entities;

public interface IConceptRepository
{
    Task<IEnumerable<Concept>> GetActiveConceptsAsync();
}