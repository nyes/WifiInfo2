using System;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;

namespace WifiInfo2
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FindViewById<TextView>(Resource.Id.textView1).Text = HostName;
            FindViewById<TextView>(Resource.Id.textView2).Text = IpAddress;
            FindViewById<TextView>(Resource.Id.textView3).Text = Mac;
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public static string HostName
        {
            get
            {
                WifiManager wifiManager = (WifiManager)(Application.Context.GetSystemService(Context.WifiService));
                if (wifiManager != null)
                {
                    return wifiManager.ConnectionInfo.BSSID;
                }
                else
                {
                    return "SSID not found";
                }
            }
        }

        public static string IpAddress
        {
            get
            {
                WifiManager wifiManager = (WifiManager)(Application.Context.GetSystemService(Context.WifiService));
                if (wifiManager != null)
                {
                    return wifiManager.ConnectionInfo.IpAddress.ToString();
                }
                else
                {
                    return "IP not found";
                }
            }
        }

        public static string Mac
        {
            get
            {
                WifiManager wifiManager = (WifiManager)(Application.Context.GetSystemService(Context.WifiService));
                WifiInfo wifiinfo = wifiManager.ConnectionInfo;
                
                if (wifiinfo != null)
                {

                    return wifiinfo.LinkSpeed.ToString();
                }
                else
                {
                    return "MAC not found";
                }
            }
        }
    }
}

