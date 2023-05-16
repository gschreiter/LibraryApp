using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Services.ApiModels
{
    public class VolumeInfo
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("subtitle")]
        public string SubTitle { get; set; }

        [JsonProperty("authors")]
        public List<string> Authors { get; set; }

        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        [JsonProperty("publishedDate")]
        public string PublishedDate { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("pageCount")]
        public int PageCount { get; set; }

        [JsonProperty("printType")]
        public string PrintType { get; set; }

        [JsonProperty("categories")]
        public List<string> Categories { get; set; }

        [JsonProperty("averageRating")]
        public double AverageRating { get; set; }

        [JsonProperty("ratingsCount")]
        public int RatingsCount { get; set; }

        [JsonProperty("imageLinks")]
        public ImageLinks ImageLinks { get; set; }

        [JsonProperty("industryIdentifiers")]
        public List<ISBN> ISBNList { get; set; }
    }
}

