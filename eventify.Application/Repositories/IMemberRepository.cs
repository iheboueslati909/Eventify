using eventify.Domain.Entities;
using eventify.Application.Common.Interfaces;

public interface IMemberRepository : IRepository<Member>
{
    Task<Member> GetByEmailAsync(string email);
    Task<bool> EmailExistsAsync(string email);
}