using Xamarin.Forms;
using GitForApple.iOS.Helpers;
using GitForApple.Helpers;

[assembly: Dependency(typeof(NetworkState))]
namespace GitForApple.iOS.Helpers
{
    public class NetworkState : INetworkState
    {
        public NetworkStatus getNetworkStatus()
        {
            return Reachability.InternetConnectionStatus();
        }
    }
}