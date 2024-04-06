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
        public void GetQuote_WithOrders_ReturnsMinimumPrice()
        {
            var orders = new List<MarketOrder>
            {
                new MarketOrder { Price = 100 },
                new MarketOrder { Price = 150 },
                new MarketOrder { Price = 120 }
            };

            var mockRepository = new Mock<IOrdersRepository>();
            mockRepository.Setup(repo => repo.GetInstrumentOrders(It.IsAny<string>())).Returns(orders);

            var service = new QuoterService(mockRepository.Object);

            var quote = service.GetQuote("instrumentId", 10);

            Assert.AreEqual(100, quote);
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
        public void GetVolumeWeightedAveragePrice_WithOrders_ReturnsCorrectValue()
        {
            var orders = new List<MarketOrder>
            {
                new MarketOrder { Price = 100, Quantity = 10 },
                new MarketOrder { Price = 150, Quantity = 5 },
                new MarketOrder { Price = 120, Quantity = 8 }
            };

            var mockRepository = new Mock<IOrdersRepository>();
            mockRepository.Setup(repo => repo.GetInstrumentOrders(It.IsAny<string>())).Returns(orders);

            var service = new QuoterService(mockRepository.Object);

            var vwamp = service.GetVolumeWeightedAveragePrice("instrumentId");

            Assert.AreEqual(117.89, vwamp, 0.1);
        }
    }
}