using eventify.Domain.Entities;
using eventify.Application.Common.Interfaces;

public interface IConceptRepository : IRepository<Concept>
{
    Task<IList<Concept>> GetActiveConceptsAsync();
}