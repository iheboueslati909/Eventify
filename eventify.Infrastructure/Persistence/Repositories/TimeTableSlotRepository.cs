using eventify.Application.Common.Interfaces;
using eventify.Domain.Entities;
using eventify.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

namespace eventify.Infrastructure.Persistence.Repositories;

public class TimeTableSlotRepository : BaseRepository<TimeTableSlot>, ITimeTableSlotRepository
{
    public TimeTableSlotRepository(EventsDbContext context): base(context)
    {
    }

    public async Task<List<TimeTableSlot>> GetConflictingSlotsForArtistAsync(
        Guid artistId,
        DateTime StartTime,
        DateTime EndTime,
        CancellationToken cancellationToken = default)
    {
        return await _context.TimeTableSlots
            .Where(slot =>
                slot.ArtistProfiles.Any(ap => ap.Id == artistId) &&
                slot.StartTime < EndTime &&
                slot.EndTime > StartTime)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    
}
