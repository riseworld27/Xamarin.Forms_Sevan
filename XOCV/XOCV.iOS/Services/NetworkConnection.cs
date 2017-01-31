using System;
using System.Net;
using CoreFoundation;
using SystemConfiguration;
using XOCV.Interfaces;
using XOCV.iOS.Services;

[assembly: Xamarin.Forms.Dependency (typeof (NetworkConnection))]
namespace XOCV.iOS.Services
{
    public class NetworkConnection : INetworkConnection
    {
        private NetworkReachability defaultRouteReachability;
        private NetworkReachability adHocWiFiNetworkReachability;

        public bool IsConnected { get; set; }

        private event EventHandler ReachabilityChanged;

        public NetworkConnection ()
        {
            CheckNetworkConnection ();
            UpdateNetworkStatus ();
        }

        public void CheckNetworkConnection ()
        {
            InternetConnectionStatus ();
        }

        private void UpdateNetworkStatus ()
        {
            if (InternetConnectionStatus ())
            {
                IsConnected = true;
            } else if (LocalWifiConnectionStatus ())
            {
                IsConnected = true;
            }
            else
            {
                IsConnected = false;
            }
        }

        private void OnChange (NetworkReachabilityFlags flags)
        {
            var h = ReachabilityChanged;
            if (h != null)
                h (null, EventArgs.Empty);
        }

        private bool IsNetworkAvailable (out NetworkReachabilityFlags flags)
        {
            if (defaultRouteReachability == null)
            {
                defaultRouteReachability = new NetworkReachability (new IPAddress (0));
                defaultRouteReachability.SetNotification (OnChange);
                defaultRouteReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
            }
            if (!defaultRouteReachability.TryGetFlags (out flags))
                return false;
            return IsReachableWithoutRequiringConnection (flags);
        }

        private bool IsAdHocWiFiNetworkAvailable (out NetworkReachabilityFlags flags)
        {
            if (adHocWiFiNetworkReachability == null)
            {
                adHocWiFiNetworkReachability = new NetworkReachability (new IPAddress (new byte [] { 169, 254, 0, 0 }));
                adHocWiFiNetworkReachability.SetNotification (OnChange);
                adHocWiFiNetworkReachability.Schedule (CFRunLoop.Current, CFRunLoop.ModeDefault);
            }

            if (!adHocWiFiNetworkReachability.TryGetFlags (out flags))
                return false;

            return IsReachableWithoutRequiringConnection (flags);
        }

        public static bool IsReachableWithoutRequiringConnection (NetworkReachabilityFlags flags)
        {
            // Is it reachable with the current network configuration?
            bool isReachable = (flags & NetworkReachabilityFlags.Reachable) != 0;

            // Do we need a connection to reach it?
            bool noConnectionRequired = (flags & NetworkReachabilityFlags.ConnectionRequired) == 0;

            // Since the network stack will automatically try to get the WAN up,
            // probe that
            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
                noConnectionRequired = true;

            return isReachable && noConnectionRequired;
        }

        private bool InternetConnectionStatus ()
        {
            NetworkReachabilityFlags flags;
            bool defaultNetworkAvailable = IsNetworkAvailable (out flags);
            if (defaultNetworkAvailable && ((flags & NetworkReachabilityFlags.IsDirect) != 0))
            {
                return false;
            }
            if ((flags & NetworkReachabilityFlags.IsWWAN) != 0)
            {
                return true;
            }
            if (flags == 0)
            {
                return false;
            }

            return true;
        }

        private bool LocalWifiConnectionStatus ()
        {
            NetworkReachabilityFlags flags;
            if (IsAdHocWiFiNetworkAvailable (out flags))
            {
                if ((flags & NetworkReachabilityFlags.IsDirect) != 0)
                    return true;
            }
            return false;
        }
    }
}