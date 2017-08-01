using Newtonsoft.Json;
using SQLite;
using System;

namespace GitForApple.Models
{
    public class Dbo : BaseDataObject
    {
        
        int repoId = -1;
        string description = String.Empty;
        string name = String.Empty;
        string language = String.Empty;
        string updated_at = String.Empty;
        int forks = -1;

        public Dbo() { }

        [PrimaryKey]
        public int RepoId { get => repoId; set => repoId = value; }
        [JsonProperty("description")]
        public string Description { get => description; set => description = value; }
        public string Name { get => name; set => name = value; }
        public string Language { get => language; set => language = value; }
        public string Updated_at { get => updated_at; set => updated_at = value; }
        public int Forks { get => forks; set => forks = value; }
    }
   
}
