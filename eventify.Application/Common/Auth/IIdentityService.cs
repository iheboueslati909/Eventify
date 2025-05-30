using eventify.SharedKernel;

namespace eventify.Application.Common;

public interface IIdentityService
{
    Task<Result<Guid>> RegisterAsync(string firstName, string lastName, string email, string password);
    Task<Result<string>> LoginAsync(string email, string password);
}
