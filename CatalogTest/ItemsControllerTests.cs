using Moq;
using Catalog.Repositories;
using Catalog.Model;
using Amazon.Runtime.Internal.Util;
using Catalog.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using Catalog.Dtos;
using FluentAssertions;

namespace CatalogTest
{
	public class ItemsControllerTests
	{
		[Fact]
		public async void GetItemAsync_WithUnexistingItem_ReturnsNotFound()
		{
			var repositoryMoq = new Mock<IItemsRepository>();
			repositoryMoq.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
				.ReturnsAsync((Item)null);


			var controller = new ItemsController(repositoryMoq.Object);

			var result = await controller.GetByIdAsync(Guid.NewGuid());

			Assert.IsType<NotFoundResult>(result.Result);
		}

		[Fact]
		public async Task GetItemAsync_WithExistingItem_ReturnExpectedItem()
		{
			var expectedItem = CreateRandomItem();
			var repositoryMoq = new Mock<IItemsRepository>();

			repositoryMoq.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
				.ReturnsAsync(expectedItem);


			var controller = new ItemsController(repositoryMoq.Object);

			var result = await controller.GetByIdAsync(Guid.NewGuid());

			result.Value.Should().BeEquivalentTo(
				expectedItem, 
				opt => opt.ComparingByMembers<Item>());
		}


		[Fact]
		public async Task CreateAsync_WithExistingItem_ReturnCreatedItem()
		{
			var itemToCreate = new CreateItemDto
			{
				Name = Guid.NewGuid().ToString(),
				Price = 1
			};

			var repositoryMoq = new Mock<IItemsRepository>();

			repositoryMoq.Setup(repo => repo.Add(It.IsAny<Item>()));


			var controller = new ItemsController(repositoryMoq.Object);

			var result = await controller.CreateItemAsync(itemToCreate);

			var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;

			result.Value.Should().BeEquivalentTo(
				createdItem,
				opt => opt.ComparingByMembers<Item>().ExcludingMissingMembers());

			createdItem.Id.Should().NotBeEmpty();

			createdItem.CreateDate.Should().Be(DateTimeOffset.UtcNow);
		}

		[Fact]
		public async Task UpdateAsync_WithExistingItem_ReturnNoContent()
		{
			var expectedItem = CreateRandomItem();
			var repositoryMoq = new Mock<IItemsRepository>();

			repositoryMoq.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
				.ReturnsAsync(expectedItem);

			var itemId = expectedItem.Id;

			var itemUpdate = new UpdateItemDto 
			{ 
				Name = Guid.NewGuid().ToString(),
				Price = 1
			};

			var controller = new ItemsController(repositoryMoq.Object);

			var result = await controller.UpdateItemAsync(itemId,itemUpdate);

			result.Should().BeOfType<NoContentResult>();
		}

		[Fact]
		public async Task DeleteAsync_WithExistingItem_ReturnNoContent()
		{
			var expectedItem = CreateRandomItem();
			var repositoryMoq = new Mock<IItemsRepository>();

			repositoryMoq.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
				.ReturnsAsync(expectedItem);

			var itemId = expectedItem.Id;

			var itemUpdate = new UpdateItemDto
			{
				Name = Guid.NewGuid().ToString(),
				Price = 1
			};

			var controller = new ItemsController(repositoryMoq.Object);

			var result = await controller.DeleteItemAsync(itemId);

			result.Should().BeOfType<NoContentResult>();
		}

		[Fact]
		public async Task GetAllItemAsync_WithExistingItem_ReturnExpectedAllItem()
		{
			var expectedItems = new[] { CreateRandomItem(), CreateRandomItem()};
			var repositoryMoq = new Mock<IItemsRepository>();

			repositoryMoq.Setup(repo => repo.GetItemsAsync())
				.ReturnsAsync(expectedItems);


			var controller = new ItemsController(repositoryMoq.Object);

			var result = await controller.GetAll();

			result.Should().BeEquivalentTo(expectedItems);
		}

		public Item CreateRandomItem()
		{
			return new Item 
			{ 
				Id = Guid.NewGuid(),
				Name = "Test",
				Price = 0,
				CreateDate = DateTime.Now
			};
		}
	}
}