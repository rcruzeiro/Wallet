namespace Wallet.Entities
{
    public class Transaction : BaseEntity
    {
        public string CPF { get; set; }
        public string AccountID { get; set; }
        public string LocationID { get; set; }
        public OperationType OperationType { get; set; }
        public EventType EventType { get; set; }
        public decimal Value { get; set; }
        public string Hash { get; set; }
        public virtual Account Account { get; set; }
    }
}
