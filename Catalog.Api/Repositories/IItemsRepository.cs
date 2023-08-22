using Catalog.Model;

namespace Catalog.Repositories
{
    public interface IItemsRepository
    {
        Task Add(Item item);
        Task<IEnumerable<Item>> GetItemsAsync();
        Task<Item> GetItemAsync(Guid id);
        Task DeleteItemAsync(Guid id);
        Task UpdateItemAsync(Item item);

	}
}
