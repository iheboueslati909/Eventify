using eventify.Domain.Entities;
using eventify.Application.Common.Interfaces;

public interface IMemberRepository : IRepository<Member>
{
    Task<Member?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> EmailExistsAsync(string email, CancellationToken cancellationToken = default);
}