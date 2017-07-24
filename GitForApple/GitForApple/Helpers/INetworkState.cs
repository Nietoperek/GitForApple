namespace GitForApple.Helpers
{
    public interface INetworkState
    {
        NetworkStatus getNetworkStatus();
    }
    public enum NetworkStatus
    {
        NotReachable,
        ReachableViaCarrierDataNetwork,
        ReachableViaWiFiNetwork
    }
}
