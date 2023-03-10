namespace TelegramInfoBot.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstLastName { get; set; }
        public string PaymentType { get; set; }
        public string Pricing { get; set; }
        public string Strategy { get; set; }
        public string Deposit { get; set; }
    }

    public enum UserFields
    {
        FirstLastName,
        PaymentType,
        Pricing,
        Strategy,
        Deposit,
    }
}
