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

    public async Task<List<TimeTableSlot>> GetConflictingSlotsForArtistAsync(Guid artistId, TimeSpan time, CancellationToken cancellationToken = default)
    {
        return await _context.TimeTableSlots
            .Where(slot =>
                slot.StartTime <= time &&
                slot.EndTime > time &&
            slot.ArtistProfiles.Any(ap => ap.Id == artistId))
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    
}
