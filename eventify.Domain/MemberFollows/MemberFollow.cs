namespace Eventify.Domain.Members;
using eventify.SharedKernel;

public class MemberFollow
{
    public Guid Id { get; private set; }
    public Guid MemberId { get; private set; }

    public Guid TargetId { get; private set; } // ID of the followed entity
    public FollowTargetType TargetType { get; private set; } //enum

    public DateTime CreatedAt { get; private set; }
    public bool IsMuted { get; private set; }
    public FollowNotificationType NotificationType { get; private set; }

    private MemberFollow() { }

    private MemberFollow(
        Guid memberId, 
        Guid targetId, 
        FollowTargetType targetType, 
        bool isMuted, 
        FollowNotificationType notificationType)
        {
            Id = Guid.NewGuid();
            MemberId = memberId;
            TargetId = targetId;
            TargetType = targetType;
            CreatedAt = DateTime.UtcNow;
            IsMuted = isMuted;
            NotificationType = notificationType;
        }

    public static Result<MemberFollow> Create(
        Guid memberId, 
        Guid targetId, 
        FollowTargetType targetType, 
        bool isMuted, 
        FollowNotificationType notificationType, 
        Func<Guid, Guid, bool> ownsCheck = null)
    {
        if (memberId == Guid.Empty) 
            return Result.Failure<MemberFollow>("Member ID cannot be empty.");
        if (targetId == Guid.Empty) 
            return Result.Failure<MemberFollow>("Target ID cannot be empty.");

        // Domain rule: cannot follow own concept (could be extended to artist/event later)
        if (targetType == FollowTargetType.Concept && ownsCheck != null && ownsCheck(memberId, targetId))
            return Result.Failure<MemberFollow>("Member cannot follow their own concept.");

        return Result.Success(new MemberFollow(memberId, targetId, targetType, isMuted, notificationType));
    }

    public void Mute() => IsMuted = true;
    public void Unmute() => IsMuted = false;

    public Result SetNotificationType(FollowNotificationType type)
    {
        NotificationType = type;
        return Result.Success();
    }
}
