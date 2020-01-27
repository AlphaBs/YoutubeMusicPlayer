using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public SearchListResponse Search(string query, string pagetoken)
        {
            var q = Service.Search.List("id,snippet");
            q.MaxResults = 10;
            q.PageToken = pagetoken;
            q.Q = query;
            q.Type = "video,playlist";

            var res = q.Execute();
            return res;
        }
    }
}
