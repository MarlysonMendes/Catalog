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
        public IEnumerable<ItemDto> GetAll()
        {
            var items = _itemsRepository.GetAll().Select(item => item.AsDto());
            return items;
        }
        [HttpGet("{id}")]
        public ActionResult<ItemDto> GetById(Guid id)
        {
            var item = _itemsRepository.GetById(id);
            if(item == null)
            {
                return NotFound();
            }
            return item.AsDto();
        }

        [HttpPost]
        public ActionResult<ItemDto> CreateItem (CreateItemDto itemDto)
        {
            var item = new Item(){
                Id = Guid.NewGuid(), 
                Name = itemDto.Name,
                Price = itemDto.Price,
                CreateDate = DateTimeOffset.UtcNow
            };
            _itemsRepository.Add(item);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item.AsDto());
        }
    }
}