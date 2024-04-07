using Moq;
using QuoterApp.DataAccess;
using QuoterApp.Model;
using QuoterApp.Services;

namespace QuoterApp.Unit.Tests
{
    [TestFixture]
    public class QuoterServiceTests
    {
        [Test]
        public void GetQuote_WithNoOrders_ThrowsArgumentException()
        {
            var mockRepository = new Mock<IOrdersRepository>();
            mockRepository.Setup(repo => repo.GetInstrumentOrders(It.IsAny<string>())).Returns(new List<MarketOrder>());

            var service = new QuoterService(mockRepository.Object);

            Assert.Throws<ArgumentException>(() => service.GetQuote("instrumentId", 10));
        }

        [Test]
        public void GetQuote_ReturnsCorrectQuote()
        {
            var orders = new List<MarketOrder>
            {
                new MarketOrder { InstrumentId = "instrumentId", Quantity = 10, Price = 20 },
                new MarketOrder { InstrumentId = "instrumentId", Quantity = 10, Price = 30 },
                new MarketOrder { InstrumentId = "instrumentId", Quantity = 10, Price = 25 }
            };

            var ordersRepositoryMock = new Mock<IOrdersRepository>();
            ordersRepositoryMock.Setup(repo => repo.GetInstrumentOrders("instrumentId")).Returns(orders);

            var quoterService = new QuoterService(ordersRepositoryMock.Object);

            var quote = quoterService.GetQuote("instrumentId", 10);

            Assert.AreEqual(20, quote);
        }

        [Test]
        public void GetVolumeWeightedAveragePrice_WithNoOrders_ThrowsArgumentException()
        {
            var mockRepository = new Mock<IOrdersRepository>();
            mockRepository.Setup(repo => repo.GetInstrumentOrders(It.IsAny<string>())).Returns(new List<MarketOrder>());

            var service = new QuoterService(mockRepository.Object);

            Assert.Throws<ArgumentException>(() => service.GetVolumeWeightedAveragePrice("instrumentId"));
        }

        [Test]
        public void GetVolumeWeightedAveragePrice_CalculatesCorrectly()
        {
            var orders = new List<MarketOrder>
            {
                new MarketOrder { InstrumentId = "instrumentId", Quantity = 10, Price = 20 },
                new MarketOrder { InstrumentId = "instrumentId", Quantity = 20, Price = 30 },
                new MarketOrder { InstrumentId = "instrumentId", Quantity = 30, Price = 25 }
            };

            var ordersRepositoryMock = new Mock<IOrdersRepository>();
            ordersRepositoryMock.Setup(repo => repo.GetInstrumentOrders("instrumentId")).Returns(orders);

            var quoterService = new QuoterService(ordersRepositoryMock.Object);

            var averagePrice = quoterService.GetVolumeWeightedAveragePrice("instrumentId");

            Assert.AreEqual(25.82, averagePrice, 0.1);
        }
    }
}