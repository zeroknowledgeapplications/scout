using System;
using System.IO;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Webkit;

namespace ZeroKnowledge
{
	[Activity (Label = "ZeroKnowledge", MainLauncher = false, Icon = "@drawable/spy")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			WebView web = FindViewById<WebView> (Resource.Id.threatView);
			web.Settings.JavaScriptEnabled = true;
			web.SetWebChromeClient (new WebChromeClient ());

			// Start main detection process
			var manager = ApplicationContext.PackageManager;
			List<Connection> connections = ConnectionController.GetConnections (manager);
			ThreatClassifier t = new ThreatClassifier ();
			t.Classify (connections);
			List<Organization> organizations = OrganizationController.CreateFromConnections (connections);

			//DEBUG
			foreach (Organization organization in organizations) {
				Console.WriteLine (organization);
			}
			foreach (Connection connection in connections) {
				Console.WriteLine (string.Format ("Connection = {0} Threat = {1}",
					connection, connection.ThreatLevel
				));
			}

			// Load main UI in WebView
			web.LoadUrl ("file:///android_asset/ThreatView.html");
		}
	}
}


