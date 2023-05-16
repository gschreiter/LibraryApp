using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LibraryApp.Services.ApiModels
{
    public class Book
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("volumeInfo")]
        public VolumeInfo VolumeInfo { get; set; }
    }
}
