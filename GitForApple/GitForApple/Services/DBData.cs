using System.Collections.Generic;
using System.Threading.Tasks;
using SQLite;
using GitForApple.Models;
using Xamarin.Forms;
using Android.Util;
using System;

[assembly: Dependency(typeof(GitForApple.Services.DBData))]
namespace GitForApple.Services
{

    public class DBData : IDBData<Response>
    {
        SQLiteAsyncConnection database;

        bool isInitialized;

        //public DBData(string dbPath)
        //{
        //    database = new SQLiteAsyncConnection(dbPath);
        //    database.CreateTableAsync<Response>().Wait();
        //}
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
                //var test = await database.CreateTableAsync<Dbo>();

                var success1 = await database.CreateTableAsync<Owner>();
                var success = await database.CreateTableAsync<Response>();
                Log.Error("SQLITEDB", success.ToString());
                isInitialized = true;
            }
            catch (SQLiteException ex)
            {
                Log.Error("SQLite DB", ex.Message);
            }
            catch (Exception e)
            {
                Log.Error("System error", e.Message);
            }
        }
        public async Task<IEnumerable<Response>> GetItemsAsync()
        {
            await InitializeAsync();
            var dbItems = await database.Table<Response>().ToListAsync();
            return await Task.FromResult(dbItems);
        }

        public async Task<Response> GetItemAsync(int id)
        {
            await InitializeAsync();
            return await database.Table<Response>().Where(i => i.RepoId == id).FirstOrDefaultAsync();
        }

        public async Task<int> SaveItemAsync(Response item)
        {
            return await database.InsertOrReplaceAsync(item);
        }

        public async Task<int> SaveListAsync(IEnumerable<Response> items)
        {
            await InitializeAsync();
            var numberOfItems = 0;
            try
            {
                numberOfItems = await database.InsertAllAsync(items);
            }
            catch (Exception e)
            {
                Log.Error("System error", e.Message);
            }
            return numberOfItems;
        }

        public async Task<int> SaveorReplaceListAsync(List<Response> items)
        {
            await InitializeAsync();
            var numberOfItems = 0;
            await database.InsertAllAsync(items);
            if (items != null && items.Count > 0)
            {
                numberOfItems = items.Count;
                foreach (Response r in items)
                    await SaveItemAsync(r);
            }
            return await Task.FromResult(numberOfItems);
        }

        public async Task<int> DeleteItemAsync(Response item)
        {
            return await database.DeleteAsync(item);
        }
    }
}
