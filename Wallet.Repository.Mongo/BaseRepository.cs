using System;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Wallet.Entities;

namespace Wallet.Repository.Mongo
{
    public abstract class BaseRepository<T>
        where T : class
    {
        readonly string _collection;
        readonly IMongoDatabase _database;

        protected IMongoCollection<T> Collection
        { get { return _database.GetCollection<T>(_collection); } }

        protected BaseRepository(string connstring, string database, string collection)
        {
            _collection = collection;
            MongoClient client = new MongoClient(connstring);
            RegisterDiscriminators();
            _database = client.GetDatabase(database);
        }

        void RegisterDiscriminators()
        {
            try
            {
                if (!BsonClassMap.IsClassMapRegistered(typeof(Transaction)))
                    BsonClassMap.RegisterClassMap<Transaction>(bcm =>
                {
                    bcm.AutoMap();
                    bcm.SetIdMember(bcm.GetMemberMap(c => c.ID));
                });
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
