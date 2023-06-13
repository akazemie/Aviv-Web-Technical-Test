using listingapi.Controllers;
using listingapi.Infrastructure.Database;
using listingapi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text.RegularExpressions;
using Xunit;

namespace listingapi.unittests
{
    public class ListingsTests
    {
        private ListingsController _listingsController;
        private readonly Mock<ILogger<ListingsController>> _mockLogger;

        public ListingsTests()
        {
            _mockLogger = new Mock<ILogger<ListingsController>>();

            var options = new DbContextOptionsBuilder<ListingsContext>().Options;
            var listingContextMock = new Mock<ListingsContext>(options);
            listingContextMock.Setup(m => m.Listings).Returns(new Mock<DbSet<Infrastructure.Database.Models.Listing>>().Object);
            listingContextMock.Setup(m => m.ListingPriceHistories)
                .Returns(new Mock<DbSet<Infrastructure.Database.Models.ListingPriceHistory>>().Object);
            _listingsController = new ListingsController(_mockLogger.Object, listingContextMock.Object);
        }

        [Fact]
        public async Task TestCreateListingValid()
        {
            var listing = new Listing
            {
                Name = "Name",
                BedroomsCount = 3,
                BuildingType = RealEstateListingBuildingType.HOUSE,
                ContactPhoneNumber = "0751272285",
                Description = "Description",
                PostalAddress = new PostalAddress
                {
                    City = "City",
                    Country = "FR",
                    PostalCode = "13007",
                    StreetAddress = "47 quai de rive neuve"
                },
                LatestPriceEur = 130000,
                RoomsCount = 4,
                SurfaceAreaM2 = 87
            };
            var actionResult = await _listingsController.PostListingAsync(listing, new CancellationToken()) as ObjectResult;
            Assert.NotNull(actionResult);
        }

        [Fact]
        public async Task TesttCreateListingBadRequest()
        {
            var listing = new Listing
            {
                Name = "Name",
                BedroomsCount = 3,
                BuildingType = RealEstateListingBuildingType.HOUSE,
                ContactPhoneNumber = "0751272285",
                Description = "Description",
                RoomsCount = 4,
                SurfaceAreaM2 = 87
            };
            var actionResult = await _listingsController.PostListingAsync(listing, new CancellationToken()) as BadRequestResult;
            Assert.NotNull(actionResult);
        }

        [Fact]
        public async Task TestUpdatetListingBadRequest()
        {
            var listing = new Listing
            {
                Name = "Name",
                BedroomsCount = 3,
                BuildingType = RealEstateListingBuildingType.HOUSE,
                ContactPhoneNumber = "0751272285",
                Description = "Description",
                RoomsCount = 4,
                SurfaceAreaM2 = 87
            };
            var actionResult = await _listingsController.PutListingAsync(0, listing, new CancellationToken()) as BadRequestResult;
            Assert.NotNull(actionResult);
        }

        [Fact]
        public async Task TestGetListingPriceHistoryValid()
        {
            // Get the price history for the new listing
            // Arrange
            int listingId = 1;
            var listingPriceHistories = new List<Infrastructure.Database.Models.ListingPriceHistory>
            {
                new Infrastructure.Database.Models.ListingPriceHistory { Id = 1, ListingPriceId = listingId, UpdateDate = DateTime.Now.AddDays(-1), Price = 9.99 },
                new Infrastructure.Database.Models.ListingPriceHistory { Id = 2, ListingPriceId = listingId, UpdateDate = DateTime.Now, Price = 19.99 }
            };
            var expectedPrices = new List<PriceReadOnly>
            {
                new PriceReadOnly { CreatedDate = listingPriceHistories[0].UpdateDate, PriceEur = (int)listingPriceHistories[0].Price },
                new PriceReadOnly { CreatedDate = listingPriceHistories[1].UpdateDate, PriceEur = (int)listingPriceHistories[1].Price }
            };

            var mockDbSet = new Mock<DbSet<Infrastructure.Database.Models.ListingPriceHistory>>();
            mockDbSet.As<IQueryable<Infrastructure.Database.Models.ListingPriceHistory>>().Setup(m => m.Provider).Returns(listingPriceHistories.AsQueryable().Provider);
            mockDbSet.As<IQueryable<Infrastructure.Database.Models.ListingPriceHistory>>().Setup(m => m.Expression).Returns(listingPriceHistories.AsQueryable().Expression);
            mockDbSet.As<IQueryable<Infrastructure.Database.Models.ListingPriceHistory>>().Setup(m => m.ElementType).Returns(listingPriceHistories.AsQueryable().ElementType);
            mockDbSet.As<IQueryable<Infrastructure.Database.Models.ListingPriceHistory>>().Setup(m => m.GetEnumerator()).Returns(listingPriceHistories.AsQueryable().GetEnumerator());


            var options = new DbContextOptionsBuilder<ListingsContext>().Options;
            var listingContextMock = new Mock<ListingsContext>(options);

            listingContextMock.Setup(m => m.Listings)
                .Returns(new Mock<DbSet<Infrastructure.Database.Models.Listing>>().Object);

            listingContextMock.Setup(c => c.ListingPriceHistories).Returns(mockDbSet.Object);

            var controller = new ListingsController(_mockLogger.Object, listingContextMock.Object);

            // Act
            var actionResult = controller.GetListingPriceHistory(listingId);

            // Assert
            Assert.NotNull(actionResult);

            var okResult = Assert.IsType<OkObjectResult>(actionResult);
            var actualPrices = Assert.IsAssignableFrom<IEnumerable<PriceReadOnly>>(okResult.Value);
            Assert.Equal(expectedPrices.Count, actualPrices.Count());
            Assert.Equal(expectedPrices[0].CreatedDate, actualPrices.ElementAt(0).CreatedDate);
            Assert.Equal(expectedPrices[0].PriceEur, actualPrices.ElementAt(0).PriceEur);
            Assert.Equal(expectedPrices[1].CreatedDate, actualPrices.ElementAt(1).CreatedDate);
            Assert.Equal(expectedPrices[1].PriceEur, actualPrices.ElementAt(1).PriceEur);
        }

        [Fact]
        public void TestPhoneNumberValid()
        {
            var contactPhoneNumber = "+33751272285";
            var regex = new Regex("^\\+[1-9]\\d{1,14}$");
            var result = regex.IsMatch(contactPhoneNumber);
            Assert.True(result);
        }

        [Fact]
        public void TestPostalCodeValid()
        {
            var postalCode = "13007";
            Assert.True(postalCode.Length == 5);
        }
    }
}
