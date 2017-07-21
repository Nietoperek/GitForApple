using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitForApple.Services
{
    public interface IGitHubData<T>
    {
        Task InitializeAsync();
        Task<bool> getContent(string url, int numberOfpages);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);

    }
}
