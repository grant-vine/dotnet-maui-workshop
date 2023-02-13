using System.Net.Http;
using System.Net.Http.Json;

namespace MonkeyFinder.Services;

public class MonkeyService
{
    DatabaseService database;
    //HttpClient httpClient;
    public MonkeyService(DatabaseService database)
    {
        this.database = database;
        //this.httpClient = new HttpClient();
    }

    List<Monkey> monkeyList;

    public async Task<List<Monkey>> GetMonkeys()
    {
        if (monkeyList?.Count > 0)
            return monkeyList;

        // Online
        //var response = await httpClient.GetAsync("https://www.montemagno.com/monkeys.json");
        //if (response.IsSuccessStatusCode)
        //{
        //    monkeyList = await response.Content.ReadFromJsonAsync<List<Monkey>>();
        //}

        var response = await database.From<Monkey>();

        if (response.Count > 0)
        {
            monkeyList = response.ToList();
        }

        // Offline
        /*using var stream = await FileSystem.OpenAppPackageFileAsync("monkeydata.json");
        using var reader = new StreamReader(stream);
        var contents = await reader.ReadToEndAsync();
        monkeyList = JsonSerializer.Deserialize<List<Monkey>>(contents);
        */
        return monkeyList;
    }
}
