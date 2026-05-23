namespace YandexGoClone.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string ClientId { get; set; } = "";
        public string? CourierId { get; set; }
        public string FromAddress { get; set; } = "";
        public string ToAddress { get; set; } = "";
        public string Status { get; set; } = "Новый"; // Новый, В пути, Доставлен
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public AppUser? Client { get; set; }
        public AppUser? Courier { get; set; }
    }
}