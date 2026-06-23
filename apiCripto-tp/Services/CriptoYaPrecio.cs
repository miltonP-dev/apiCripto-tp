using System.Text.Json;
using System.Text.Json.Serialization;

namespace apiCripto_tp.Services
{
    public class CriptoYaPrecio
    {
        
            [JsonPropertyName("ask")] public decimal Ask { get; set; }
            [JsonPropertyName("totalAsk")] public decimal TotalAsk { get; set; }
            [JsonPropertyName("bid")] public decimal Bid { get; set; }
            [JsonPropertyName("totalBid")] public decimal TotalBid { get; set; }
            [JsonPropertyName("time")] public long Time { get; set; }
        

        public interface ICriptoYaService
        {
            Task<CriptoYaPrecio?> ObtenerPrecioAsync(string coin, string? fiat = null, string? exchange = null);
        }

        public class CriptoYaService : ICriptoYaService
        {
            private readonly HttpClient _http;
            private readonly IConfiguration _config;

            public CriptoYaService(HttpClient http, IConfiguration config)
            {
                _http = http;
                _config = config;
            }

            public async Task<CriptoYaPrecio?> ObtenerPrecioAsync(
                string coin, string? fiat = null, string? exchange = null)
            {
                exchange ??= _config["CriptoYa:Exchange"] ?? "satoshitango";
                fiat ??= _config["CriptoYa:Fiat"] ?? "ars";

                var url = $"https://criptoya.com/api/{exchange}/{coin}/{fiat}/1";

                try
                {
                    var resp = await _http.GetAsync(url);
                    if (!resp.IsSuccessStatusCode)
                        return null;

                    var json = await resp.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<CriptoYaPrecio>(json);
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
