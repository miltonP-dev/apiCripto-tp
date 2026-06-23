using System.Text.Json.Serialization;

namespace apiCripto_tp.Models.Dtos
{
    public class CrearTransaccionDto
    {
        [JsonPropertyName("crypto_code")]
        public string CryptoCode { get; set; }

        [JsonPropertyName("action")]
        public string Action { get; set; }

        [JsonPropertyName("crypto_amount")]
        public decimal CryptoAmount { get; set; }

        [JsonPropertyName("datetime")]
        public string Datetime { get; set; }
    }

    public class TransaccionDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("crypto_code")]
        public string CryptoCode { get; set; } = string.Empty;

        [JsonPropertyName("action")]
        public string Action { get; set; } = string.Empty;

        [JsonPropertyName("crypto_amount")]
        public decimal CryptoAmount { get; set; }

        [JsonPropertyName("money")]
        public decimal Money { get; set; }

        [JsonPropertyName("datetime")]
        public DateTime Datetime { get; set; }
    }

    public class EditarTransaccionDto
    {
        [JsonPropertyName("crypto_code")]
        public string? CryptoCode { get; set; }

        [JsonPropertyName("action")]
        public string? Action { get; set; }

        [JsonPropertyName("crypto_amount")]
        public decimal? CryptoAmount { get; set; }

        [JsonPropertyName("money")]
        public decimal? Money { get; set; }

        [JsonPropertyName("datetime")]
        public DateTime? Datetime { get; set; }
    }

    public class EstadoCryptoDto
    {
        [JsonPropertyName("crypto_code")]
        public string CryptoCode { get; set; } = string.Empty;

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = string.Empty;

        [JsonPropertyName("cantidad")]
        public decimal Cantidad { get; set; }

        [JsonPropertyName("dinero")]
        public decimal Dinero { get; set; }
    }

    public class EstadoActualDto
    {
        [JsonPropertyName("cryptos")]
        public List<EstadoCryptoDto> Cryptos { get; set; } = new();

        [JsonPropertyName("total")]
        public decimal Total { get; set; }
    }

}
