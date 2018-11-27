namespace Wallet.DTO.Transactions
{
    public class TransactionDTO
    {
        public string LocationID { get; set; }
        public int OperationType { get; set; }
        public int EventType { get; set; }
        public decimal Value { get; set; }
    }
}
