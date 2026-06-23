namespace apiCripto_tp.Models
{
    public class Transaccion
    {
        public int Id { get; set; }
        public string CryptoCode { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public decimal CryptoAmount { get; set; }
        public decimal Money { get; set; }
        public DateTime Datetime { get; set; }
    }
}
