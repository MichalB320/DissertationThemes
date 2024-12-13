using System.Net.Http;
using System.Text.Json;
using System.Windows;

namespace DissertationThemes.ViewerApp.Services;

public class DataService
{
    private readonly HttpClient _httpClient;

    public DataService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<T> LoadDataAsync<T> (string apiURL)
    {
        try
        {
            var response = await _httpClient.GetAsync(apiURL);

            if (typeof(T) == typeof(byte[]))
            {
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                return (T)(object)fileBytes;
            }
            else
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var dataResponse = JsonSerializer.Deserialize<T>(jsonResponse);
                return dataResponse;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
            return default;
        }
    }
}
