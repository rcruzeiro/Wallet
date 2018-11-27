using System.Threading.Tasks;
using Wallet.Objects;

namespace Wallet.Repository
{
    public interface IWalletBlockchainRepository
    {
        Task<WalletBlockchain> GetBlockchain();
        Task Create(WalletBlockchain blockchain);
        Task<bool> Update(WalletBlockchain blockchain);
    }
}
