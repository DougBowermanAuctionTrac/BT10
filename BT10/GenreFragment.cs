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
    public class GenreFragment : ListFragment
    {
        private int _currentPlayId;
        private bool _isDualPane;
        //private SongDB mySongDB;
        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            var topsongsFrame = Activity.FindViewById<View>(Resource.Id.topsongs);

            // If running on a tablet, then the layout in Resources/Layout-Large will be loaded. 
            // That layout uses fragments, and defines the detailsFrame. We use the visiblity of 
            // detailsFrame as this distinguisher between tablet and phone.
            _isDualPane = topsongsFrame != null && topsongsFrame.Visibility == ViewStates.Visible;
            //mySongDB = ((MainActivity)this.Activity).getSongDB();

            var adapter = new ArrayAdapter<String>(Activity, Android.Resource.Layout.SimpleListItemChecked, SongDB.Instance.GetGenres());
            ListAdapter = adapter;

            if (savedInstanceState != null)
            {
                _currentPlayId = savedInstanceState.GetInt("current_play_id", 0);
            }

            if (_isDualPane)
            {
                ListView.ChoiceMode = ChoiceMode.Single;
                ShowTopSongs(_currentPlayId);
            }
        }

        public override void OnSaveInstanceState(Bundle outState)
        {
            base.OnSaveInstanceState(outState);
            outState.PutInt("current_play_id", _currentPlayId);
        }

        public override void OnListItemClick(ListView l, View v, int position, long id)
        {
            ShowTopSongs(position);
        }

        private void ShowTopSongs(int playId)
        {
            _currentPlayId = playId;
            if (_isDualPane)
            {
                // We can display everything in-place with fragments.
                // Have the list highlight this item and show the data.
                ListView.SetItemChecked(playId, true);

                // Check what fragment is shown, replace if needed.
                var topsongs = FragmentManager.FindFragmentById(Resource.Id.topsongs) as TopSongsFragment;
                if (topsongs == null || topsongs.ShownPlayId != playId)
                {
                    // Make new fragment to show this selection.
                    topsongs = TopSongsFragment.NewInstance(playId);

                    // Execute a transaction, replacing any existing
                    // fragment with this one inside the frame.
                    var ft = FragmentManager.BeginTransaction();
                    ft.Replace(Resource.Id.topsongs, topsongs);
                    ft.SetTransition(FragmentTransit.FragmentFade);
                    ft.Commit();
                }
            }
            else
            {
                // Otherwise we need to launch a new activity to display
                // the dialog fragment with selected text.
                var intent = new Intent();

                intent.SetClass(Activity, typeof(TopSongsActivity));
                intent.PutExtra("current_play_id", playId);
                //intent.PutExtra("current_songdb", mySongDB);

                StartActivity(intent);
            }
        }
    }
}