using GitForApple.Helpers;
using GitForApple.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        static string _UserAgent = "Mobile Application GitForApple Agent";
        //static string _UserToken;
        //static string _UserToken = "token revoked";
        static string _RepoURL = "https://api.github.com/users/apple/repos";
        static string _SearchURL = "https://api.github.com/search/repositories?q=user:apple+pushed:%3E=";


        public async Task InitializeAsync()
        {
            if (isInitialized)
                return;
            if (client == null)
            {
                client = new HttpClient();
                //client.DefaultRequestHeaders.Add("Authorization", _UserToken);
                client.DefaultRequestHeaders.Add("User-Agent", _UserAgent);
            }
            await getContent(_RepoURL, 1);

            isInitialized = true;
        }
        public async Task<bool> isSiteReachable(string url)
        {
            HttpResponseMessage response = null;
            if (client == null)
            {
                client = new HttpClient();
                //client.DefaultRequestHeaders.Add("Authorization", _UserToken);
                client.DefaultRequestHeaders.Add("User-Agent", _UserAgent);
            }
            try
            {
                var uri = new Uri(string.Format(_RepoURL, string.Empty));
                response = await client.GetAsync(uri);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to connect to site.",
                    Cancel = "OK"
                }, "SiteUnreachable");
            }
            if (response != null && response.IsSuccessStatusCode)
                return await Task.FromResult(true);

            return await Task.FromResult(false);
        }

        public async Task<bool> getContent(string url, int numberOfpages)
        {
            HttpResponseMessage response = null;
            try
            {
                var uri = new Uri(string.Format(url, string.Empty));
                response = await client.GetAsync(uri);
            }
            catch (HttpRequestException e)
            {
                Debug.WriteLine(e);
                MessagingCenter.Send(new MessagingCenterAlert
                {
                    Title = "Error",
                    Message = "Unable to connect to site.",
                    Cancel = "OK"
                }, "SiteUnreachable");
            }
            if (response != null && response.IsSuccessStatusCode)
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
            bool updated = false;
            //TODO LastUpdateTime
            DateTime now = DateTime.Now.ToLocalTime();
            var uri = new Uri(string.Format(_SearchURL + now.ToString("yyyy") + "-" + now.ToString("MM") + "-" + now.ToString("dd"), string.Empty));
            var response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var updateResponse = JsonConvert.DeserializeObject<UpdateResponse>(content);
                var array = updateResponse.Items;
                var updatedRepos = array.ToList();
                List<Response> newItems = updatedRepos.Where(n => repos.All(d => n.RepoId != d.RepoId)).ToList();
                List<Response> toBeUpdated = repos.Where(c => updatedRepos.Any(d => c.RepoId == d.RepoId && c.Updated_at != d.Updated_at)).ToList();
                //toBeUpdated.Add(new Response { RepoId = 64889661, Name = "Updated item", Description="Test no image update" });
                if (toBeUpdated != null && toBeUpdated.Count > 0)
                {
                    updated = true;
                    foreach (Response u in toBeUpdated)
                    {
                        var repo = repos.First(i => i.RepoId == u.RepoId);
                        repo.clone(u);
                    }
                }
                //newItems.Add(new Response {Name="Test", Description="Test"});
                if (newItems != null && newItems.Count() > 0)
                {
                    updated = true;
                    repos.AddRange(newItems);
                }
            }

            return await Task.FromResult(updated);
        }
        public async Task<IEnumerable<Response>> GetItemsAsync(bool refresh = false)
        {
            if (repos != null && refresh)
            {
                if (!await getContentUpdate())
                    return await Task.FromResult(Enumerable.Empty<Response>());
            }
            await InitializeAsync();
            return await Task.FromResult(repos);
        }
    }
}
