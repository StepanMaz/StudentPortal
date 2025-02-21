using StudentPortal.EventBus;

namespace StudentPortal.Events;

public record UserAccountDeleted(Guid UserId) : IntegrationEvent;