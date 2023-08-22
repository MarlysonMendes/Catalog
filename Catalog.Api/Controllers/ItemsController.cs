using Catalog.Dtos;
using Catalog.Model;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
    [ApiController]
    [Route("items")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemsRepository _itemsRepository;

        public ItemsController(IItemsRepository repository)
        {
            this._itemsRepository = repository;
        }
       
        [HttpGet]
        public async Task <IEnumerable<ItemDto>> GetAll()
        {
            var items = (await _itemsRepository.GetItemsAsync()).Select(item => item.AsDto());
            return items;
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetByIdAsync(Guid id)
        {
            var item = await _itemsRepository.GetItemAsync(id);
            if(item == null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        public async Task <ActionResult<CreateItemDto>> CreateItemAsync (CreateItemDto itemDto)
        {
            var item = new Item(){
                Id = Guid.NewGuid(), 
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreateDate = DateTimeOffset.UtcNow
            };
            await _itemsRepository.Add(item);
            return  CreatedAtAction(nameof(GetByIdAsync), new { id = item.Id }, item.AsDto());
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
        {
            var item = await _itemsRepository.GetItemAsync(id);
            if(item == null)
            {
                return NotFound();
            }
            var UpdateItem = item with{
                Name = itemDto.Name,
                Price = itemDto.Price
            };
            await _itemsRepository.UpdateItemAsync(UpdateItem);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteItemAsync(Guid id)
        {
            var item = await _itemsRepository.GetItemAsync(id);
            if(item == null)
            {
                return NotFound();
            }
            await _itemsRepository.DeleteItemAsync(id);
            return NoContent();
        }
    }
}