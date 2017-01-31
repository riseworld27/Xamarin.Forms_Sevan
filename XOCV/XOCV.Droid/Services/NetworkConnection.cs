using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.Net;
using Xamarin.Forms;
using XOCV.Droid.Services;
using XOCV.Interfaces;

[assembly:Dependency(typeof(NetworkConnection))]
namespace XOCV.Droid.Services
{
    class NetworkConnection : INetworkConnection
    {
        public bool IsConnected
        {
            get
            {
                ConnectivityManager cm =
                    (ConnectivityManager)Forms.Context.GetSystemService(Context.ConnectivityService);

                return cm.ActiveNetworkInfo != null && cm.ActiveNetworkInfo.IsConnectedOrConnecting;
            }
        }

        public void CheckNetworkConnection()
        {
            throw new NotImplementedException();
        }
    }
}