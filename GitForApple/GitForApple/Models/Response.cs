using Newtonsoft.Json;
using System;

namespace GitForApple.Models
{
    public class Response : BaseDataObject
    {
        [JsonProperty("id")]
        int repoId;
        [JsonProperty("description")]
        string description;
        [JsonProperty("name")]
        string name;
        [JsonProperty("owner")]
        Owner owner;
        [JsonProperty("language")]
        string language;
        [JsonProperty("updated_at")]
        string updated_at;
        [JsonProperty("forks")]
        int forks;

        public Response() { }

        public int RepoId
        {
            get { return repoId; }
            set { SetProperty(ref repoId, value); }
        }
        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }
        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }
        public Owner Owner
        {
            get { return owner; }
            set { SetProperty(ref owner, value); }
        }
        public string Language
        {
            get { return language; }
            set { SetProperty(ref language, value); }
        }
        public string Updated_at
        {
            get
            {
                var date = updated_at;
                return date.Replace("T", " ").Replace("Z", " ");
            }
            set { SetProperty(ref updated_at, value); }
        }

        public int Forks
        {
            get { return forks; }
            set { SetProperty(ref forks, value); }
        }

        public void clone(Response source)
        {
            Name = source.name;
            Description = source.description;
            Owner = source.owner;
            Language = source.language;
            Updated_at = source.updated_at;
            Forks = source.forks;
        }
    }

    public class Owner : BaseDataObject
    {
        [JsonProperty("avatar_url")]
        string avatar_url;

        public Owner() { }

        public string Avatar_url
        {
            get { return avatar_url; }
            set { SetProperty(ref avatar_url, value); }
        }
    }
}
