using GitForApple.Helpers;
using Newtonsoft.Json;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace GitForApple.Models
{
    //[Table("Repos")]
    public class Repo : ObservableObject
    {
        [JsonProperty("id")]
        int repoId = -1;
        [JsonProperty("description")]
        string description = String.Empty;
        [JsonProperty("name")]
        string name = String.Empty;
        [JsonProperty("owner")]
        Owner owner;
        [JsonProperty("language")]
        string language = String.Empty;
        [JsonProperty("updated_at")]
        string updated_at = String.Empty;
        [JsonProperty("forks")]
        int forks = -1;
        //int ownerId;
        public Repo() { }

        [PrimaryKey]
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
        [ForeignKey(typeof(Owner))]
        public int OwnerId_ { get; set; }

        [OneToOne]
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

        public void Clone(Repo source)
        {
            Name = source.name;
            Description = source.description;
            Owner = source.owner;
            Language = source.language;
            Updated_at = source.updated_at;
            Forks = source.forks;
        }
    }

    //[Table("Owners")]
    public class Owner : ObservableObject
    {
        [JsonProperty("avatar_url")]
        string avatar_url = String.Empty;
        
        [JsonProperty("id"), PrimaryKey]
        public int OwnerId { get; set; }

        public Owner() { }        

        public string Avatar_url
        {
            get { return avatar_url; }
            set { SetProperty(ref avatar_url, value); }
        }
    }
}
