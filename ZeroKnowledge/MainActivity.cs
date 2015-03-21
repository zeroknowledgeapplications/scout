﻿using System;
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
	[Activity (Label = "ZeroKnowledge", MainLauncher = true, Icon = "@drawable/icon")]
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

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			var manager = ApplicationContext.PackageManager;

			button.Click += delegate {

				List<Connection> connections = ConnectionController.GetConnections (manager);

				ThreatClassifier t = new ThreatClassifier ();
				t.Classify (connections);

				List<Organization> organizations = OrganizationController.CreateFromConnections (connections);

				//button.Text = string.Format ("{0} clicks!", count++);
				foreach (Organization organization in organizations) {
					Console.WriteLine (organization);
				}
				foreach (Connection connection in connections) {
					Console.WriteLine (string.Format ("Connection = {0} Threat = {1}",
						connection, connection.ThreatLevel
					));
				}

				// Load page in WebView
				web.LoadUrl ("file:///android_asset/ThreatView.html");
			};
		}
	}
}


