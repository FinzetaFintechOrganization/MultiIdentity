public interface ISubscriptionService
{
	Task StartSubscriptionAsync(SubscriptionDTO dto);
	Task ExtendSubscriptionAsync(ExtendSubscriptionDTO dto);
}
