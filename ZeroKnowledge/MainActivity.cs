using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Webkit;

namespace ZeroKnowledge
{
	[Activity (Label = "Scout", MainLauncher = false, Icon = "@drawable/spy")]
	public class MainActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			WebView web = FindViewById<WebView> (Resource.Id.threatView);
			web.Settings.JavaScriptEnabled = true;
			var client = new WebChromeClient ();
			web.SetWebChromeClient (client);
			// Load page in WebView
			web.LoadUrl ("file:///android_asset/ThreatView.html");

			// Start main detection process
			var manager = ApplicationContext.PackageManager;
			var timer = new System.Timers.Timer (1000);
			timer.Elapsed += (sender, e) => {
				List<Connection> connections = ConnectionController.GetConnections (manager);
				ThreatClassifier t = new ThreatClassifier ();
				t.Classify (connections);
				List<Organization> organizations = OrganizationController.CreateFromConnections (connections);

				int i = 0;
				foreach (Organization organization in organizations.OrderBy((o) => -o.ThreatLevel)) {
					if (i > 7)
						break;

					SetLabel (i, organization.Name);
					SetThreatLevel (i, Math.Min (1.0, organization.ThreatLevel / 5));
					SetNumberOfConnections (i, organization.Connections.Count);

					i++;
				}
				// Hide the unneeded organisations
				SetNumberOfOrganisations(organizations.Count);

				for (; i <= 7; i++)
					SetLabel (i, "");
			};
			timer.Start ();
		}

		private void SetNumberOfOrganisations(int count)
		{
			WebView web = FindViewById<WebView> (Resource.Id.threatView);
			RunOnUiThread(() => web.LoadUrl (String.Format("javascript:window.UI.setNumberofConnections({0});", count), null));
		}

		private void SetLabel(int id, string label)
		{
			WebView web = FindViewById<WebView> (Resource.Id.threatView);
			RunOnUiThread(() => web.LoadUrl (String.Format("javascript:window.UI.setLabel({0}, \"{1}\");", id, label), null));
		}

		private void SetNumberOfConnections(int id, int connections)
		{
			WebView web = FindViewById<WebView> (Resource.Id.threatView);
			RunOnUiThread(() => web.LoadUrl (String.Format("javascript:window.UI.setNumberofConnections({0}, {1});", id, connections), null));
		}

		private void SetThreatLevel(int id, double threat)
		{
			WebView web = FindViewById<WebView> (Resource.Id.threatView);
			RunOnUiThread(() => web.LoadUrl (String.Format("javascript:window.UI.setThreatLevel({0}, {1});", id, threat), null));
		}
	}
}


