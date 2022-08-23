using Nuclio.Sdk;
using NUnit.Framework;

namespace FunctionTests
{
    public class Tests
    {
        function f; 
        [SetUp]
        public void Setup()
        {
            f = new function();
        }

        [Test]
        public void Test1()
        {
            Context c = new Context();
            Event e = new Event
            {
                Body = System.Text.Encoding.UTF8.GetBytes("12345"),
                Trigger = new Trigger
                {
                    Kind = "Nats"
                }
            };

            var result = f.execute(c, e);
            Assert.IsNotNull(result);
            Assert.IsInstanceOf(typeof(Response), result);
            Assert.AreEqual((result as Response).Body,"nats: 12345");
            
        }
    }
}