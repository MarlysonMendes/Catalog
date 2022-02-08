using System.Collections.Generic;
using Catalog.Model;
namespace Catalog.Repositories
{
    

    public class InMemItemsRepository : IItemsRepository
    {
        public readonly List<Item> items = new()
        {
            new Item { Id = Guid.NewGuid(), Name = "Item 1", Price = 10.5m, CreateDate = DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Iron Sword", Price = 20.5m, CreateDate = DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Bronze Shild", Price = 30.5m, CreateDate = DateTimeOffset.UtcNow },
            new Item { Id = Guid.NewGuid(), Name = "Bronze Sword", Price = 40.5m, CreateDate = DateTimeOffset.UtcNow },
        };
        public void Add(Item item)
        {
            items.Add(item);
        }
        public IEnumerable<Item> GetAll()
        {
            return items;
        }
        public Item GetById(Guid id)
        {
            return items.Find(i => i.Id == id);
        }
        public void Remove(Guid id)
        {
            items.RemoveAll(i => i.Id == id);
        }
        public void Update(Item item)
        {
            var index = items.FindIndex(i => i.Id == item.Id);
            items[index] = item;
        }
        public void Delete(Guid id)
        {
            items.RemoveAll(i => i.Id == id);
        }
    }
}