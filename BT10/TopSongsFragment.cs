using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace BT10
{
    internal class TopSongsFragment : ListFragment
    {
        public static TopSongsFragment NewInstance(int playId)
        {
            var topsongsFrag = new TopSongsFragment { Arguments = new Bundle() };
            topsongsFrag.Arguments.PutInt("current_play_id", playId);
            return topsongsFrag;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public int ShownPlayId
        {
            get { return Arguments.GetInt("current_play_id", 0); }
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            if (container == null)
            {
                // Currently in a layout without a container, so no reason to create our view.
                return null;
            }
            //View rootView = inflater.inflate(Resource.Layout.activity_main, container,false);

            //SongDB mySongDB = ((MainActivity)this.Activity).getSongDB();

            var topsongsListView = new ListView(Activity);
            ListAdapter = new ArrayAdapter<String>(Activity, Android.Resource.Layout.SimpleListItem1, SongDB.Instance.GetTopTen(ShownPlayId));
            
            return topsongsListView;
            //var scroller = new ScrollView(Activity);
            //var text = new TextView(Activity);
            //var padding = Convert.ToInt32(TypedValue.ApplyDimension(ComplexUnitType.Dip, 4, Activity.Resources.DisplayMetrics));
            //text.SetPadding(padding, padding, padding, padding);
            //text.TextSize = 24;
            //text.Text = Shakespeare.Dialogue[ShownPlayId];
            //scroller.AddView(text);
            //return scroller;
        }

        //public override void OnListItemClick(ListView l, View v, int position, long id)
        //{
        //    ShowDetails(position);
        //}
    }
}