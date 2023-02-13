using System;
using Microsoft.Extensions.Logging;
using Supabase.Interfaces;
using Supabase.Gotrue;
using Newtonsoft.Json;

namespace MonkeyFinder.Providers
{
	public class CustomSupabaseSessionHandler : ISupabaseSessionHandler
    {
        private string supabaseCacheFilename = ".supabase.cache";
        private readonly ILogger<CustomSupabaseSessionHandler> logger;

        public CustomSupabaseSessionHandler(
            ILogger<CustomSupabaseSessionHandler> logger
        )
        {
            Console.WriteLine("------------------- CONSTRUCTOR -------------------");
            this.logger = logger;
        }

        public Task<bool> SessionPersistor<TSession>(TSession session) where TSession : Session
        {
            try
            {
                var cacheDir = FileSystem.CacheDirectory;
                var path = Path.Join(cacheDir, supabaseCacheFilename);
                var str = JsonConvert.SerializeObject(session);

                using (StreamWriter file = new StreamWriter(path))
                {
                    file.Write(str);
                    file.Dispose();
                    return Task.FromResult(true);

                };
            }
            catch (Exception err)
            {
                Debug.WriteLine("Unable to write cache file.");
                throw err;
            }
        }

        public async Task<TSession?> SessionRetriever<TSession>() where TSession : Session
        {
            var tsc = new TaskCompletionSource<Session>();
            try
            {
                var cacheDir = FileSystem.CacheDirectory;
                var path = Path.Join(cacheDir, supabaseCacheFilename);

                if (File.Exists(path))
                {
                    using (StreamReader file = new StreamReader(path))
                    {
                        var str = file.ReadToEnd();
                        if (!String.IsNullOrEmpty(str))
                            tsc.SetResult(JsonConvert.DeserializeObject<Session>(str));
                        else
                            tsc.SetResult(null);
                        file.Dispose();
                    };
                }
                else
                {
                    tsc.SetResult(null);
                }
            }
            catch
            {
                Debug.WriteLine("Unable to read cache file.");
                tsc.SetResult(null);
            }
            //return await localStorage.GetItemAsync<Session>(SESSION_KEY);
            return (TSession?)await tsc.Task;

        }

        public Task<bool> SessionDestroyer()
        {
            try
            {
                var cacheDir = FileSystem.CacheDirectory;
                var path = Path.Join(cacheDir, supabaseCacheFilename);
                if (File.Exists(path))
                    File.Delete(path);
                return Task.FromResult(true);
            }
            catch (Exception err)
            {
                Debug.WriteLine("Unable to delete cache file.");
                return Task.FromResult(false);
            }
        }
    }
}