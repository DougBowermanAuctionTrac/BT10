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
using System.Net;
using System.Threading.Tasks;
using System.IO;

namespace BT10
{
    public class SongDB
    {

        bool popsongsfetched;
        bool rbhhsongsfetched;
        bool latinsongsfetched;
        bool dnceelcsongsfetched;
        bool countrysongsfetched;
        bool rocksongsfetched;

        string[] Genres = { "Pop", "R&B/HipHop", "Latin", "Dance/Elec", "Country", "Rock" };
        string[] popsongs;
        string[] rbhhsongs;
        string[] latinsongs;
        string[] dnceelcsongs;
        string[] countrysongs;
        string[] rocksongs;

        private SongDB()
        {

        }
        private static SongDB _instance;
        public static SongDB Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SongDB();
                }
                return _instance;
            }
        }

        public void InitSongDB()
        {
            popsongs     = new string[50];
            for (int i = 0; i < 50; i++)
                popsongs[i] = "fetching";
            popsongsfetched = false;

            rbhhsongs = new string[50];
            for (int i = 0; i < 50; i++)
                rbhhsongs[i] = "fetching";
            rbhhsongsfetched = false;

            latinsongs = new string[50];
            for (int i = 0; i < 50; i++)
                latinsongs[i] = "fetching";
            latinsongsfetched = false;

            dnceelcsongs = new string[50];
            for (int i = 0; i < 50; i++)
                dnceelcsongs[i] = "fetching";
            dnceelcsongsfetched = false;

            countrysongs = new string[50];
            for (int i = 0; i < 50; i++)
                countrysongs[i] = "fetching";
            countrysongsfetched = false;

            rocksongs = new string[50];
            for (int i = 0; i < 50; i++)
                rocksongs[i] = "fetching";
            rocksongsfetched = false;

        }
        public async void FillSongDB()
        {
            popsongsfetched = await FetchLists("http://www.billboard.com/charts/pop-songs", popsongs);
            rbhhsongsfetched = await FetchLists("http://www.billboard.com/charts/r-b-hip-hop-songs", rbhhsongs);
            latinsongsfetched = await FetchLists("http://www.billboard.com/charts/latin-songs", latinsongs);
            dnceelcsongsfetched = await FetchLists("http://www.billboard.com/charts/dance-electronic-songs", dnceelcsongs);
            countrysongsfetched = await FetchLists("http://www.billboard.com/charts/country-songs", countrysongs);
            rocksongsfetched = await FetchLists("http://www.billboard.com/charts/rock-songs", rocksongs);
        }
        public string[] GetGenres()
        {
            string[] myGenres = new string[6];
            for (int GenreNum = 0; GenreNum < 6; GenreNum++)
            {
                myGenres[GenreNum] = Genres[GenreNum];
            }
            return myGenres;
        }
        public string[] GetTopTen(int GenreNum)
        {
            string[] myTopTen = new string[50];
            for (int i = 0; i < 50; i++)
                myTopTen[i] = (i+1).ToString();
            if (GenreNum == 0)
            {
                if ((popsongs != null) && (popsongsfetched))
                {
                    for (int TopTenNum = 0; TopTenNum < popsongs.Length; TopTenNum++)
                    {
                        myTopTen[TopTenNum] = (TopTenNum+1).ToString() + ". " + popsongs[TopTenNum];
                    }
                }
            }
            if (GenreNum == 1)
            {
                if ((rbhhsongs != null) && (rbhhsongsfetched))
                {
                    for (int TopTenNum = 0; TopTenNum < rbhhsongs.Length; TopTenNum++)
                    {
                        myTopTen[TopTenNum] = (TopTenNum + 1).ToString() + ". " + rbhhsongs[TopTenNum];
                    }
                }
            }
            if (GenreNum == 2)
            {
                if ((latinsongs != null) && (latinsongsfetched))
                {
                    for (int TopTenNum = 0; TopTenNum < latinsongs.Length; TopTenNum++)
                    {
                        myTopTen[TopTenNum] = (TopTenNum + 1).ToString() + ". " + latinsongs[TopTenNum];
                    }
                }
            }
            if (GenreNum == 3)
            {
                if ((dnceelcsongs != null) && (dnceelcsongsfetched))
                {
                    for (int TopTenNum = 0; TopTenNum < dnceelcsongs.Length; TopTenNum++)
                    {
                        myTopTen[TopTenNum] = (TopTenNum + 1).ToString() + ". " + dnceelcsongs[TopTenNum];
                    }
                }
            }
            if (GenreNum == 4)
            {
                if ((countrysongs != null) && (countrysongsfetched))
                {
                    for (int TopTenNum = 0; TopTenNum < countrysongs.Length; TopTenNum++)
                    {
                        myTopTen[TopTenNum] = (TopTenNum + 1).ToString() + ". " + countrysongs[TopTenNum];
                    }
                }
            }
            if (GenreNum == 5)
            {
                if ((rocksongs != null) && (rocksongsfetched))
                {
                    for (int TopTenNum = 0; TopTenNum < rocksongs.Length; TopTenNum++)
                    {
                        myTopTen[TopTenNum] = (TopTenNum + 1).ToString() + ". " + rocksongs[TopTenNum];
                    }
                }
            }
            return myTopTen;
        }


        private async Task<bool> FetchLists(string url, string [] chartsongs)
        {
            // Create an HTTP web request using the URL:
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.ContentType = "application/json";
            request.Method = "GET";

            //// Send the request to the server and wait for the response:
            using (WebResponse response = await request.GetResponseAsync())
            {
                Stream ReceiveStream = response.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                StreamReader readStream = new StreamReader(ReceiveStream, encode);

                int SongNum = 0;
                int SongCount = 50;
                string sLine;
                string Title = "";
                string Artist = "";
                string TitleTag = "data-songtitle=\"";
                //string ArtistTag = ">";
                bool bGotTitle = false;
                bool bGotArtist = false;
                
                while (((sLine = readStream.ReadLine()) != null) && (SongNum < SongCount))
                {
                    bGotTitle = false;
                    bGotArtist = false;

                    if (sLine.Contains("<article class=\"chart-row"))
                    {
                        bGotTitle = true;
                        //Console.WriteLine(sLine);
                        int TitlePos = sLine.IndexOf(TitleTag) + TitleTag.Length;

                        Title = sLine.Substring(TitlePos, sLine.IndexOf('"', TitlePos) - TitlePos);
                        while (bGotArtist == false)
                        {
                            sLine = readStream.ReadLine();
                            if (sLine.Contains("chart-row__artist"))
                            {
                                bGotArtist = true;
                                //Console.WriteLine(sLine);

                                Artist = readStream.ReadLine();
                                break;
                            }
                        }
                        if ((bGotTitle) && (bGotArtist))
                        {
                            //Console.WriteLine(Title + ":" + Artist);
                            chartsongs[SongNum] = WebUtility.HtmlDecode(Title) + ":" + WebUtility.HtmlDecode(Artist);
                            SongNum++;
                            continue;
                        }

                    }

                }

            }
            return true;
        }
}

}