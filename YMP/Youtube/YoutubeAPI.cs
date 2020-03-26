using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMP.Model;
using YMP.Util;

namespace YMP.Youtube
{
    public class YoutubeAPI
    {
        // Please input your YouTube Data API KEY
        private static readonly string Key = YoutubeDataAPI.KEY;
        public const string YoutubeVideoKind = "youtube#video";
        public const string YoutubePlayListKind = "youtube#playlist";

        public YoutubeAPI()
        {
            Service = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = Key
            });
        }

        public YouTubeService Service { get; private set; }
        public int MaxResult { get; set; } = 20;

        public Tuple<string[], string[]> Search(string query, ref string pagetoken)
        {
            var q = Service.Search.List("id");
            q.MaxResults = MaxResult;
            q.PageToken = pagetoken;
            q.Q = query;
            q.Type = "video,playlist";

            var r = q.Execute();
            pagetoken = r.NextPageToken;

            var v = new List<string>(MaxResult);
            var p = new List<string>(MaxResult);

            foreach (var item in r.Items)
            {
                if (item.Id.Kind == YoutubeVideoKind)
                    v.Add(item.Id.VideoId);
                else if (item.Id.Kind == YoutubePlayListKind)
                    p.Add(item.Id.PlaylistId);
            }

            return new Tuple<string[], string[]>(v.ToArray(), p.ToArray());
        }

        public Music[] Videos(string[] ids)
        {
            if (ids.Length == 0)
                return new Music[0];

            var list = new List<Music>(ids.Length);

            var r = Service.Videos.List("id,snippet,contentDetails,statistics");
            r.MaxResults = ids.Length;
            r.Id = string.Join(",", ids);

            var videos = r.Execute();
            foreach (var item in videos.Items)
            {
                list.Add(new Music()
                {
                    YoutubeID = item.Id,
                    Title = item.Snippet.Title,
                    Artists = item.Snippet.ChannelTitle,
                    PublishAt = item.Snippet.PublishedAt ?? DateTime.Now,
                    Duration = StringFormat.FromISO8601Str(item.ContentDetails.Duration),
                    Thumbnail = item.Snippet.Thumbnails.Medium.Url,
                    HighResThumbnail = item.Snippet.Thumbnails.High.Url,
                    Views = item.Statistics.ViewCount ?? 0
                });
            }

            return list.ToArray();
        }

        public PlayListMetadata[] Playlists(string[] ids)
        {
            if (ids.Length == 0)
                return new PlayListMetadata[0];

            var list = new List<PlayListMetadata>(ids.Length);

            var l = Service.Playlists.List("id,snippet,contentDetails");
            l.MaxResults = ids.Length;
            l.Id = string.Join(",", ids);

            var playlists = l.Execute();
            foreach (var item in playlists.Items)
            {
                list.Add(new PlayListMetadata()
                {
                    ID = item.Id,
                    Title = item.Snippet.Title,
                    Count = item.ContentDetails.ItemCount ?? 0,
                    Thumbnail = item.Snippet.Thumbnails.Medium.Url,
                    Creator = item.Snippet.ChannelTitle
                });
            }

            return list.ToArray();
        }

        public Music[] PlaylistItem(string playlistid, ref string pagetoken)
        {
            var r = Service.PlaylistItems.List("snippet");
            r.PlaylistId = playlistid;
            r.PageToken = pagetoken;
            r.MaxResults = MaxResult;

            var res = r.Execute();
            pagetoken = res.NextPageToken;

            var ids = res.Items.Select(x => x.Snippet.ResourceId.VideoId).ToArray();
            return Videos(ids);
        }

        public string GetVideoUrl(string id)
        {
            return "https://www.youtube.com/watch?v=" + id;
        }

        public string GetPlayListUrl(string id)
        {
            return "https://www.youtube.com/playlist?list=" + id;
        }
    }
}
