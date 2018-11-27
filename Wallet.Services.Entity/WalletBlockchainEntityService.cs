using System;
using System.Threading.Tasks;
using Core.Framework.Blockchain;
using Wallet.Objects;
using Wallet.Repository;

namespace Wallet.Services.Entity
{
    public sealed class WalletBlockchainEntityService
    {
        readonly IWalletBlockchainRepository _repository;

        public WalletBlockchainEntityService(IWalletBlockchainRepository repository)
        {
            _repository = repository;
        }

        public async Task<WalletBlockchain> GetBlockchain()
        {
            try
            {
                return await _repository.GetBlockchain();
            }
            catch (Exception ex)
            { throw ex; }
        }

        public async Task AddBlock(Block block)
        {
            try
            {
                var blockchain = await GetBlockchain();

                if (blockchain == null)
                {
                    blockchain = new WalletBlockchain();
                    blockchain.AddBlock(block);
                    await _repository.Create(blockchain);
                }
                else
                {
                    blockchain.AddBlock(block);
                    await _repository.Update(blockchain);
                }
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
