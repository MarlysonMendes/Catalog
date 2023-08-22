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
        public async Task Add(Item item)
        {
            await itemsCollection.InsertOneAsync(item);
        }

        public async Task <IEnumerable<Item>> GetItemsAsync()
        {   
            
            return await itemsCollection.Find( _ => true).ToListAsync();
        }

        public async Task<Item> GetItemAsync(Guid id)
        {
            var filter = Builders<Item>.Filter.Eq(item => item.Id,id);
            return await itemsCollection.Find(filter).SingleOrDefaultAsync();
            //return itemsCollection.Find( item => item.Id == id).SingleOrDefault();
        }

        public async Task DeleteItemAsync(Guid id)
        {
            var filter = Builders<Item>.Filter.Eq(item => item.Id,id);
            await itemsCollection.DeleteOneAsync(filter);
        }

        public async Task UpdateItemAsync(Item item)
        {
            var filter = Builders<Item>.Filter.Eq(existingItem => existingItem.Id, item.Id);
            
            await itemsCollection.ReplaceOneAsync(filter, item);
            
        }
    }
}