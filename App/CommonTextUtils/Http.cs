using System.Net.Http;
using System.Threading.Tasks;

public static class Http
{
    /// <summary>
    /// Скачивание избражения с ресурса доступого по URL
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static async Task<byte[]> DownloadImage(this HttpClient client, string url)
    {
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        await using var ms = await response.Content.ReadAsStreamAsync();

        byte[] data = new byte[ms.Length];
        ms.Read(data);
        return data;
    }
}
