using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YMP.Model;

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

        public Tuple<string[], string[]> Search(string query, string pagetoken)
        {
            var count = 10;

            var q = Service.Search.List("id");
            q.MaxResults = count;
            q.PageToken = pagetoken;
            q.Q = query;
            q.Type = "video,playlist";

            var r = q.Execute();
            var v = new List<string>(count);
            var p = new List<string>(count);

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

            var list = new List<Music>(10);

            var r = Service.Videos.List("id,snippet,contentDetails,statistics");
            r.MaxResults = 10;
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
                    Duration = item.ContentDetails.Duration,
                    Thumbnail = item.Snippet.Thumbnails.Medium.Url,
                    Views = item.Statistics.ViewCount ?? 0
                });
            }

            return list.ToArray();
        }

        public PlayListMetadata[] Playlists(string[] ids)
        {
            if (ids.Length == 0)
                return new PlayListMetadata[0];

            var list = new List<PlayListMetadata>(10);

            var l = Service.Playlists.List("id,snippet,contentDetails");
            l.MaxResults = 10;
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
    }
}
