using Newtonsoft.Json;

namespace GitForApple.Models
{
    public class UpdateResponse
    {
        [JsonProperty("total_count")]
        int total_count;
        [JsonProperty("incomplete_results")]
        bool incomplete_results;
        [JsonProperty("items")]
        Repo []items;

        public UpdateResponse() {  }

        public int Total_count { get => total_count; set => total_count = value; }
        public bool Incomplete_results { get => incomplete_results; set => incomplete_results = value; }
        public Repo[] Items { get => items; set => items = value; }
    }
}
