namespace UnitTestProject1
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using System.Threading.Tasks;
    using Moq;

    public class Order
    {
        public string State { get; set; }

        public override string ToString()
        {
            return $"Order<State:{State}>";
        }
    }

    public interface IOrderService
    {
        Task UpdateOrderAsync(Order order);
    }

    public class Program
    {
        public async Task RunAsync(IOrderService orderService)
        {
            var order = new Order();

            order.State = "new";
            await orderService.UpdateOrderAsync(order);

            order.State = "open";
            await orderService.UpdateOrderAsync(order);
        }
    }

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var mock = new Mock<IOrderService>();
            await new Program().RunAsync(mock.Object);
            mock.Verify(x => x.UpdateOrderAsync(It.IsAny<Order>()), Times.Exactly(2));
        }

        [TestMethod]
        public async Task TestMethod2()
        {
            var mock = new Mock<IOrderService>();
            await new Program().RunAsync(mock.Object);
            mock.Verify(x => x.UpdateOrderAsync(It.Is<Order>(o => o.State == "new")), Times.Once);
            mock.Verify(x => x.UpdateOrderAsync(It.Is<Order>(o => o.State == "open")), Times.Once);
        }

        [TestMethod]
        public async Task TestMethod3()
        {
            var mock = new Mock<IOrderService>();
            await new Program().RunAsync(mock.Object);

            mock.Verify(x => x.UpdateOrderAsync(It.Is<Order>(o => o.State == "new")), Times.Once);
            mock.Verify(x => x.UpdateOrderAsync(It.Is<Order>(o => o.State == "open")), Times.Once);
        }

        [TestMethod]
        public async Task TestMethod4()
        {
            var mock = new Mock<IOrderService>();
            mock.Setup(x => x.UpdateOrderAsync(It.Is<Order>(o => o.State == "new"))).Returns(Task.FromResult(true));
            mock.Setup(x => x.UpdateOrderAsync(It.Is<Order>(o => o.State == "open"))).Returns(Task.FromResult(false));

            await new Program().RunAsync(mock.Object);

            mock.Verify(x => x.UpdateOrderAsync(It.Is<Order>(o => o.State == "new")), Times.Once);
            mock.Verify(x => x.UpdateOrderAsync(It.Is<Order>(o => o.State == "open")), Times.Once);
        }
    }
}
