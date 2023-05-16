using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Services.ApiModels
{
    public class ImageLinks
    {
        [JsonProperty("smallThumbnail")]
        public string SmallThumbnail { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }
    }
}
