using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MyShop.Api.DTOs.Products;
using MyShop.Api.Entities;
using MyShop.Api.Repositories.Interfaces;
using MyShop.Api.Services;

namespace MyShop.Tests.Unit;

public class ProductServiceTests
{
    [Fact]
    public async Task GetByIdAsync_WhenProductExists_ReturnsProductDto()
    {
        var repositoryMock = new Mock<IProductRepository>();
        var loggerMock = new Mock<ILogger<ProductService>>();

        var product = new Product
        {
            Id = 1,
            Name = "Keyboard",
            Price = 100,
            Stock = 5,
            Description = "Mechanical keyboard",
            CreatedAtUtc = DateTime.UtcNow
        };

        repositoryMock
            .Setup(x => x.GetByIdAsync(1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        var service = new ProductService(
            repositoryMock.Object,
            loggerMock.Object);

        ProductDto result = await service.GetByIdAsync(
            1,
            CancellationToken.None);

        result.Should().NotBeNull();

        result.Id.Should().Be(1);
        result.Name.Should().Be("Keyboard");
        result.Price.Should().Be(100);
        result.Stock.Should().Be(5);
        result.Description.Should().Be("Mechanical keyboard");
    }
}