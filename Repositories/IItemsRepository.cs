using Catalog.Model;

namespace Catalog.Repositories
{
    public interface IItemsRepository
    {
        void Add(Item item);
        IEnumerable<Item> GetAll();
        Item GetById(Guid id);
        void Remove(Guid id);
        void Update(Item item);
        void Delete(Guid id);
    }
}
