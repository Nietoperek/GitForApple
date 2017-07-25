using Xamarin.Forms;
using GitForApple.Helpers;
using Android.Net;
using GitForApple.Droid.Helpers;

[assembly: Dependency(typeof(NetworkState))]
namespace GitForApple.Droid.Helpers
{

    public class NetworkState : INetworkState
    {
        public NetworkStatus getNetworkStatus()
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Android.App.Activity.ConnectivityService);
            NetworkInfo networkInfo = connectivityManager.ActiveNetworkInfo;
            if (networkInfo!=null && networkInfo.IsConnectedOrConnecting)
            {
                if (networkInfo.Type == ConnectivityType.Wifi) return NetworkStatus.ReachableViaWiFiNetwork;
                else if (networkInfo.Type == ConnectivityType.Mobile) return NetworkStatus.ReachableViaCarrierDataNetwork;
            }
            return NetworkStatus.NotConnected;
        }        
        
    }

}