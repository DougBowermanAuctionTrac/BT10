using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;
using Android.App;
using Android.Widget;
using Android.OS;

namespace BT10
{
    [Activity(Label = "BT10", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        //public SongDB mySongDB = new SongDB();
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            MobileCenter.Start("a6221245-5ab0-4421-8e51-9efbafaa2641",
                   typeof(Analytics), typeof(Crashes));

            // Set our view from the "main" layout resource
            SetContentView (Resource.Layout.activity_main);

            SongDB.Instance.InitSongDB();
            SongDB.Instance.FillSongDB();
        }

        //public SongDB getSongDB()
        //{
        //    return mySongDB;
        //}
    }
}

