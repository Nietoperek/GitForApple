using Xamarin.Forms;
using GitForApple.Helpers;
using Android.Net;
using GitForApple.Droid.Helpers;
using Android.Widget;

[assembly: Dependency(typeof(NetworkState))]
namespace GitForApple.Droid.Helpers
{

    public class NetworkState : INetworkState
    {
        public NetworkStatus getNetworkStatus()
        {
            ConnectivityManager connectivityManager = (ConnectivityManager)Android.App.Application.Context.GetSystemService(Android.App.Activity.ConnectivityService);
            NetworkInfo networkInfo = connectivityManager.ActiveNetworkInfo;

            if (networkInfo == null)
            {
                Toast.MakeText(Android.App.Application.Context,"No internet connection", ToastLength.Long).Show();
                return NetworkStatus.NotReachable;
            }
            bool isOnline = networkInfo.IsConnected;
            if (isOnline)
            {
                if (networkInfo.Type == ConnectivityType.Wifi) return NetworkStatus.ReachableViaWiFiNetwork;
                else if (networkInfo.IsRoaming) return NetworkStatus.ReachableViaCarrierDataNetwork;
            }          
            return NetworkStatus.NotReachable;
        }
        
    }

}