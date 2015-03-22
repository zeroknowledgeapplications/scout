using System;

using Android.App;
using Android.Content;
using Android.Views;
using Android.Widget;
using Android.OS;

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
				//var name = "nl.hardlopenmetevy.app";
				//var packageURI = Android.Net.Uri.Parse(string.Format("package:{0}",name));
				//var uninstallIntent = new Intent(Intent.ActionDelete, packageURI);
				//StartActivity(uninstallIntent);
			};
		}
	}
}


