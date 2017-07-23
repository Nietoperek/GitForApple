using System.Collections.Generic;
using System.Threading.Tasks;

namespace GitForApple.Services
{
    public interface IDBData<T>
    {
        Task InitializeAsync();
        Task<T> GetItemAsync(int id);
        Task<int> SaveItemAsync(T item);
        Task<int> SaveListAsync(List<T> items);
        Task<int> SaveorReplaceListAsync(List<T> items);
        Task<int> DeleteItemAsync(T item);
        Task<IEnumerable<T>> GetItemsAsync();
    }
}
