using System;
using Core.Framework.Entities;

namespace Wallet.Entities
{
    public abstract class BaseEntity : IEntity
    {
        public int ID { get; set; }
        public string ClientID { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
