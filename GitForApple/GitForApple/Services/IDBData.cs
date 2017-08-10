using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitForApple.Services
{
    public interface IDBData<T>
    {
        Task InitializeAsync();
        Task<T> GetItemAsync(int id);
        Task SaveItemAsync(T item);
        Task SaveListAsync(IEnumerable<T> items);
        Task<int> SaveorReplaceListAsync(List<T> items);
        Task<int> DeleteItemAsync(T item);
        Task<IEnumerable<T>> GetItemsAsync();
    }
}
