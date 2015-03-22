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
	[Activity (Label = "Scout", MainLauncher = true, Icon = "@drawable/spy")]
	public class LoginActivity : Activity
	{
		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Hide menu bar
			this.RequestWindowFeature (WindowFeatures.NoTitle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Login);

			// Open the main view if clicked to proceed
			Button button = FindViewById<Button> (Resource.Id.button_main);
			button.Click += delegate {
				StartActivity(typeof(MainActivity));
			};
		}
	}
}


