using eventify.Domain.Entities;

namespace eventify.Application.Common.Interfaces;

public interface ITimeTableSlotRepository : IRepository<TimeTableSlot>
{
    public Task<List<TimeTableSlot>> GetConflictingSlotsForArtistAsync(Guid artistId, DateTime startTime, DateTime endTime, CancellationToken cancellationToken = default);

}
