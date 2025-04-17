namespace Eventify.Domain.Members;
using eventify.SharedKernel;

public class MemberFollow
{
    public Guid Id { get; private set; }
    public Guid MemberId { get; private set; }

    public Guid TargetId { get; private set; } // ID of the followed entity
    public FollowTargetType TargetType { get; private set; }

    public DateTime CreatedAt { get; private set; }
    public bool IsMuted { get; private set; }
    public FollowNotificationType NotificationType { get; private set; }

    private MemberFollow() { }

    private MemberFollow(Guid memberId, Guid targetId, FollowTargetType targetType, bool isMuted, FollowNotificationType notificationType)
    {
        Id = Guid.NewGuid();
        MemberId = memberId;
        TargetId = targetId;
        TargetType = targetType;
        CreatedAt = DateTime.UtcNow;
        IsMuted = isMuted;
        NotificationType = notificationType;
    }

    public static MemberFollow Create(Guid memberId, Guid targetId, FollowTargetType targetType, bool isMuted, FollowNotificationType notificationType, Func<Guid, Guid, bool> ownsCheck = null)
    {
        if (memberId == Guid.Empty) throw new ArgumentNullException(nameof(memberId));
        if (targetId == Guid.Empty) throw new ArgumentNullException(nameof(targetId));

        // Domain rule: cannot follow own concept (could be extended to artist/event later)
        if (targetType == FollowTargetType.Concept && ownsCheck != null && ownsCheck(memberId, targetId))
            throw new InvalidOperationException("Member cannot follow their own concept.");

        return new MemberFollow(memberId, targetId, targetType, isMuted, notificationType);
    }

    public void Mute() => IsMuted = true;
    public void Unmute() => IsMuted = false;
    public void SetNotificationType(FollowNotificationType type) => NotificationType = type;
}
