using eventify.Domain.Entities;
using eventify.Application.Repositories;
using eventify.Infrastructure.Extensions;

namespace eventify.Infrastructure.Persistence.Repositories;

public class ClubRepository : BaseRepository<Club>, IClubRepository
{
    public ClubRepository(EventsDbContext context) : base(context)
    {
    }
}
