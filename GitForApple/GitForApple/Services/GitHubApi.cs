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
        //static string _UserToken;
        static string _RepoURL;
        static string _SearchURL;

        public async Task InitializeAsync()
        {
            if (isInitialized)
                return;
            _UserAgent = "GitForApple";
            //_UserToken = "token revoked";
            _RepoURL = "https://api.github.com/users/apple/repos";
            _SearchURL = "https://api.github.com/search/repositories?q=user:apple+pushed:%3E="; //2017-07-21
            client = new HttpClient();
            //client.DefaultRequestHeaders.Add("Authorization", _UserToken);
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
        public async Task<bool> getContentUpdate()
        {
            DateTime now = DateTime.Now.ToLocalTime();
            var uri = new Uri(string.Format(_SearchURL + now.ToString("yyyy") + "-" + now.ToString("MM") + "-" + now.ToString("dd"), string.Empty));
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var updateResponse = JsonConvert.DeserializeObject<UpdateResponse>(content);
                var array = updateResponse.Items;
                var updatedRepos = array.ToList();
                List<Response> toBeUpdated = repos.Where(c => updatedRepos.Any(d => c.RepoId == d.RepoId && c.Updated_at == d.Updated_at)).ToList();
                if (toBeUpdated != null && toBeUpdated.Count > 0)
                    foreach (Response u in toBeUpdated)
                    {
                        var repo = repos.First(i => i.RepoId == u.RepoId);
                        repo = u;
                    }
            }

            return await Task.FromResult(true);
        }
        public async Task<IEnumerable<Response>> GetItemsAsync(bool forceRefresh = false)
        {
            await InitializeAsync();
            if (forceRefresh) await getContentUpdate();

            return await Task.FromResult(repos);
        }
    }
}
