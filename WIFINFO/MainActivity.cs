using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using Android.App;
using Android.Content;
using Android.Net;
using Android.Net.Wifi;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;
using NetworkAccess = Xamarin.Essentials.NetworkAccess;

namespace WIFINFO
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        public string ssid, protocol, bssid, ipaddress, ip2, gateway, dns1, dns2, dhcp, mask;
        public Int32 linkspeed, rssi, leaseduration;
        public float frequency;
        public IPAddress[] ip1;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
            FloatingActionButton fab2 = FindViewById<FloatingActionButton>(Resource.Id.fab2);
            fab2.Click += Fab2OnClick;

            Run();

        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            Run();
        }

        private void Fab2OnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionWifiSettings));
        }

        public void Run()
        {
            WifiInfo();
            DHCPInfo();
            MacAddress();
            Connection();
        }

        public void Connection()
        {
            var current = Connectivity.NetworkAccess;
            var profiles = Connectivity.Profiles;

            if (current == NetworkAccess.Internet)
            {
                if (profiles.Contains(ConnectionProfile.WiFi))
                {
                    FindViewById<TextView>(Resource.Id.textView1).Text = "Connected to " + ssid;
                    FindViewById<TextView>(Resource.Id.textView1).SetTextColor(Android.Graphics.Color.Green);
                    ShowWiFi();
                }
                else
                {
                    FindViewById<TextView>(Resource.Id.textView1).Text = "Mobile data connection";
                    FindViewById<TextView>(Resource.Id.textView1).SetTextColor(Android.Graphics.Color.Yellow);
                    ShowMobileData();
                }

            }
            else
            {
                FindViewById<TextView>(Resource.Id.textView1).Text = "Disconnected";
                FindViewById<TextView>(Resource.Id.textView1).SetTextColor(Android.Graphics.Color.Red);

                for (int i = 2; i <= 27; i++)
                {
                    int id = 2131230861 + i;
                    FindViewById<TextView>(id).Visibility = ViewStates.Gone;
                }
            }
        }

        public void ShowWiFi()
        {
            for (int i = 2; i <= 27; i++)
            {
                int id = 2131230861 + i;
                FindViewById<TextView>(id).Visibility = ViewStates.Visible;
            }

            FindViewById<TextView>(Resource.Id.textView15).Text = bssid;
            FindViewById<TextView>(Resource.Id.textView16).Text = linkspeed + " Mbps";
            FindViewById<TextView>(Resource.Id.textView17).Text = rssi + " dBm";
            FindViewById<TextView>(Resource.Id.textView18).Text = MacAddress();
            FindViewById<TextView>(Resource.Id.textView19).Text = "" + ip1[0];
            FindViewById<TextView>(Resource.Id.textView20).Text = ip2;
            FindViewById<TextView>(Resource.Id.textView21).Text = String.Format("{0:N1}", frequency) + " GHz";
            FindViewById<TextView>(Resource.Id.textView22).Text = mask;
            FindViewById<TextView>(Resource.Id.textView23).Text = gateway;
            FindViewById<TextView>(Resource.Id.textView24).Text = dns1;
            FindViewById<TextView>(Resource.Id.textView25).Text = dns2;
            FindViewById<TextView>(Resource.Id.textView26).Text = dhcp;
            FindViewById<TextView>(Resource.Id.textView27).Text = leaseduration / 3600 + " h";
        }

        public void ShowMobileData()
        {
            for (int i = 2; i <= 27; i++)
            {
                int id = 2131230861 + i;
                FindViewById<TextView>(id).Visibility = ViewStates.Gone;
            }

            FindViewById<TextView>(Resource.Id.textView5).Visibility = ViewStates.Visible;
            FindViewById<TextView>(Resource.Id.textView6).Visibility = ViewStates.Visible;
            FindViewById<TextView>(Resource.Id.textView18).Visibility = ViewStates.Visible;
            FindViewById<TextView>(Resource.Id.textView19).Visibility = ViewStates.Visible;
            FindViewById<TextView>(Resource.Id.textView18).Text = MacAddress();
            FindViewById<TextView>(Resource.Id.textView19).Text = "" + ip1[0];
        }

        public void DHCPInfo()
        {
            WifiManager wifiManager = (WifiManager)(Application.Context.GetSystemService(Context.WifiService));
            DhcpInfo dhcpinfo = wifiManager.DhcpInfo;

            ip2 = Android.Text.Format.Formatter.FormatIpAddress(dhcpinfo.IpAddress);
            mask = Android.Text.Format.Formatter.FormatIpAddress(dhcpinfo.Netmask);
            gateway = Android.Text.Format.Formatter.FormatIpAddress(dhcpinfo.Gateway);
            dns1 = Android.Text.Format.Formatter.FormatIpAddress(dhcpinfo.Dns1);
            dns2 = Android.Text.Format.Formatter.FormatIpAddress(dhcpinfo.Dns2);
            dhcp = Android.Text.Format.Formatter.FormatIpAddress(dhcpinfo.ServerAddress);
            leaseduration = dhcpinfo.LeaseDuration;
        }

        public void WifiInfo()
        {
            WifiManager wifiManager = (WifiManager)(Application.Context.GetSystemService(Context.WifiService));
            WifiInfo wifiinfo = wifiManager.ConnectionInfo;

            bssid = wifiinfo.BSSID;
            ssid = wifiinfo.SSID;
            rssi = wifiinfo.Rssi;
            linkspeed = wifiinfo.LinkSpeed;
            int freq = (wifiinfo.Frequency);
            frequency = freq / 1000f;
            ip1 = Dns.GetHostAddresses(Dns.GetHostName());
        }

        public string MacAddress()
        {
            foreach (var netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (netInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                netInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet)
                {
                    var address = netInterface.GetPhysicalAddress();
                    return BitConverter.ToString(address.GetAddressBytes());
                }
            }
            return "No Mac";
        }

    }
}

