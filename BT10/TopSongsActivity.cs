using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace BT10
{
    [Activity(Label = "TopSongsActivity")]
    public class TopSongsActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            base.OnCreate(savedInstanceState);
            var index = Intent.Extras.GetInt("current_play_id", 0);
            //SongDB mySongDB = Intent.GetObject<SongDB>()
            //new Intent(this, typeof(Activity2));
            //activity2.PutExtra("MyData", "Data from Activity1");

            var topsongs = TopSongsFragment.NewInstance(index); // DetailsFragment.NewInstance is a factory method to create a Details Fragment
            var fragmentTransaction = FragmentManager.BeginTransaction();
            fragmentTransaction.Add(Android.Resource.Id.Content, topsongs);
            fragmentTransaction.Commit();
        }
    }
}