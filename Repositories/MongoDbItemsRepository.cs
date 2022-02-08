using Catalog.Model;
using System.Collections.Generic;
using Catalog.Model;
using MongoDB.Driver;

namespace Catalog.Repositories
{

    public class MongoDbItemsRepository : IItemsRepository
    {
        private readonly string databaseName = "catalog";
        private readonly string collectionName = "items";
        private readonly IMongoCollection<Item> itemsCollection;
        public MongoDbItemsRepository(IMongoClient mongoClient)
        {
            IMongoDatabase database = mongoClient.GetDatabase(databaseName);
            itemsCollection = database.GetCollection<Item>(collectionName);

        }
        public void Add(Item item)
        {
            itemsCollection.InsertOne(item);
        }

        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Item> GetAll()
        {
            throw new NotImplementedException();
        }

        public Item GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Remove(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(Item item)
        {
            throw new NotImplementedException();
        }
    }
}