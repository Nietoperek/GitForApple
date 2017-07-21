using GitForApple.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(GitForApple.Services.GitHubApi))]
namespace GitForApple.Services
{
    public class GitHubApi : IGitHubData<Response>
    {
        bool isInitialized;
        List<Response> repos;
        HttpClient client;
        static string _UserAgent;
        static string _UserToken;
        static string _RepoURL;

        public async Task InitializeAsync()
        {
            if (isInitialized)
                return;
            _UserAgent = "GitForApple";
            _UserToken = "token 611b564fad7e549912f40ece9b33c8d8dccdd104";
            _RepoURL = "https://api.github.com/users/apple/repos";
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", _UserToken);
            client.DefaultRequestHeaders.Add("User-Agent", _UserAgent);

            await getContent(_RepoURL, 1);

            isInitialized = true;
        }

        public async Task<bool> getContent(string url, int numberOfpages)
        {
            var uri = new Uri(string.Format(url, string.Empty));
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                int lastPage = numberOfpages;
                var content = await response.Content.ReadAsStringAsync();
                var responseList = JsonConvert.DeserializeObject<List<Response>>(content);
                if (repos == null) { repos = responseList; }
                else { repos.AddRange(responseList); }
                IEnumerable<string> values;
                string nextUrl = String.Empty;
                // if more pages in repos then there is header Link
                if (response.Headers.TryGetValues("Link", out values))
                {
                    Match match = Regex.Match(values.FirstOrDefault(), @"^.*<(.*?page=)([0-9]*)>; rel=.last.", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        nextUrl = match.Groups[1].Value;
                        Int32.TryParse(match.Groups[2].Value, out lastPage);
                        if (lastPage > numberOfpages)
                        {
                            numberOfpages++;
                            await getContent(nextUrl + numberOfpages, numberOfpages);
                        }
                    }
                }
            }
            return await Task.FromResult(true);
        }

        public async Task<IEnumerable<Response>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeAsync();

            return await Task.FromResult(repos);
        }
    }
}
