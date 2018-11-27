namespace Wallet.Entities
{
    public enum AccountType
    {
        Voucher = 1,
        Giftcard
    }

    public enum OperationType
    {
        Credit = 1,
        Debit
    }

    public enum EventType
    {
        Create = 1,
        Charge,
        Consume
    }
}
