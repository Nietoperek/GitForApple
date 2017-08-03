using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using SQLiteNetExtensionsAsync.Extensions;
using SQLite.Extensions;
using GitForApple.Models;
using Xamarin.Forms;
using Android.Util;
using System;
using System.Diagnostics;
using System.Linq;

[assembly: Dependency(typeof(GitForApple.Services.DBData))]
namespace GitForApple.Services
{

    public class DBData : IDBData<Repo>
    {
        SQLiteAsyncConnection database;
        
        bool isInitialized;

        public async Task InitializeAsync()
        {
            if (isInitialized)
                return;
            try
            {
                if (database == null)
                {
                    database = new SQLiteAsyncConnection(DependencyService.Get<Helpers.IFileHelper>().GetLocalFilePath("GitHubApi.db3"));
                }
                var success1 = await database.CreateTableAsync<Owner>();
                var success = await database.CreateTableAsync<Repo>();
                Debug.WriteLine("SQLITEDB", "Database tables created "+success.ToString());
                isInitialized = true;
            }
            catch (SQLiteException ex)
            {
                Debug.WriteLine("SQLite DB", ex.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine("System error", e.Message);
            }
        }
        public async Task<IEnumerable<Repo>> GetItemsAsync()
        {
            await InitializeAsync();
            var sortedDbItems = (await database.GetAllWithChildrenAsync<Repo>()).OrderBy(r => r.Name);
            return await Task.FromResult(sortedDbItems);
        }

        public async Task<Repo> GetItemAsync(int id)
        {
            await InitializeAsync();
            return await database.GetWithChildrenAsync<Repo>(id);
        }

        public async Task SaveItemAsync(Repo item)
        {
            await database.InsertOrReplaceWithChildrenAsync(item);
        }

        public async Task<int> SaveListAsync(IEnumerable<Repo> items)
        {
            await InitializeAsync();
            var numberOfItems = 0;
            try
            {
                await database.InsertAllWithChildrenAsync(items);
                await database.InsertAsync(items.ToList()[0].Owner);
                //foreach(Repo r in items.ToList())
                //{
                //    await database.InsertAsync(r.Owner);
                //}
            }
            catch (Exception e)
            {
                Debug.WriteLine("System error", e.Message);
            }
            return numberOfItems;
        }

        public async Task<int> SaveorReplaceListAsync(List<Repo> items)
        {
            await InitializeAsync();
            var numberOfItems = 0;
            await database.InsertAllWithChildrenAsync(items);
            if (items != null && items.Count > 0)
            {
                numberOfItems = items.Count;
                foreach (Repo r in items)
                    await SaveItemAsync(r);
            }
            return await Task.FromResult(numberOfItems);
        }

        public async Task<int> DeleteItemAsync(Repo item)
        {
            return await database.DeleteAsync(item);
        }
    }
}
