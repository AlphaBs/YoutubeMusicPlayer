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

        public YoutubeAPI()
        {
            Service = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = Key
            });
        }

        public YouTubeService Service { get; private set; }

        public Music[] Search(string query, string pagetoken)
        {
            var count = 10;
            var list = new List<Music>(count);

            var q = Service.Search.List("id");
            q.MaxResults = count;
            q.PageToken = pagetoken;
            q.Q = query;
            q.Type = "video,playlist";

            var searchVideos = new List<string>(count);
            var searchPlaylist = new List<string>(count);
            foreach (var item in q.Execute().Items)
            {
                if (item.Id.Kind == "youtube#video")
                    searchVideos.Add(item.Id.VideoId);
                else if (item.Id.Kind == "youtube#playlist")
                    searchPlaylist.Add(item.Id.PlaylistId);
            }

            var r = Service.Videos.List("id,snippet,contentDetails,statistics");
            r.MaxResults = count;
            r.Id = string.Join(",", searchVideos);

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
                }) ;
            }

            return list.ToArray();
        }
    }
}
