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
			// Load page in WebView
			web.LoadUrl ("file:///android_asset/ThreatView.html");

			// Start main detection process
			var manager = ApplicationContext.PackageManager;
			List<Connection> connections = ConnectionController.GetConnections (manager);
			ThreatClassifier t = new ThreatClassifier ();
			t.Classify (connections);
			List<Organization> organizations = OrganizationController.CreateFromConnections (connections);

			int i = 0;
			foreach (Organization organization in organizations) {
				if(i > 7)
					break;

				SetLabel(i, organization.Name);
				SetThreatLevel(i, Math.Min(1.0, organization.ThreatLevel / 5));
				SetNumberOfConnections(i, organization.Connections.Count);

				i++;
			}

			for(;i <= 7; i++)
				SetLabel(i, "");
		}

		private void SetLabel(int id, string label)
		{
			WebView web = FindViewById<WebView> (Resource.Id.threatView);
			web.EvaluateJavascript (String.Format("window.UI.setLabel({0}, {1});", id, label), null);
		}

		private void SetNumberOfConnections(int id, int connections)
		{
			WebView web = FindViewById<WebView> (Resource.Id.threatView);
			web.EvaluateJavascript (String.Format("window.UI.setConnections({0}, {1});", id, connections), null);
		}

		private void SetThreatLevel(int id, double threat)
		{
			WebView web = FindViewById<WebView> (Resource.Id.threatView);
			web.EvaluateJavascript (String.Format("window.UI.setThreat({0}, {1});", id, threat), null);
		}
	}
}


