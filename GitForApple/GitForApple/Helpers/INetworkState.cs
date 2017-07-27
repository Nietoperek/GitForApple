namespace GitForApple.Helpers
{
    public interface INetworkState
    {
        NetworkStatus getNetworkStatus();
    }
    public enum NetworkStatus
    {
        NotConnected,
        NotReachable,
        ReachableViaCarrierDataNetwork,
        ReachableViaWiFiNetwork
    }
}
