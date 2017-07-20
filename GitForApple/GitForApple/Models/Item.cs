using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitForApple.Models
{
    public class Item : BaseDataObject
    {
        string avatar = string.Empty;
        string name = string.Empty;
        string language = string.Empty;
        string lastUpdatedAt = string.Empty;
        string description = string.Empty;
        int forks = -1;

        string text = string.Empty;
        public string Avatar
        {
            get { return avatar; }
            set { SetProperty(ref avatar, value); }
        }

        public string Name
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public string Language
        {
            get { return language; }
            set { SetProperty(ref language, value); }
        }
        public string LastUpdatedAt
        {
            get { return lastUpdatedAt; }
            set { SetProperty(ref lastUpdatedAt, value); }
        }

        public string Description
        {
            get { return description; }
            set { SetProperty(ref description, value); }
        }

        public int Forks
        {
            get { return forks; }
            set { SetProperty(ref forks, value); }
        }
    }
}
