using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CoreSolution.RabbitMQ.Test
{
    [TestClass]
    class RabbitMqTest
    {
        [TestMethod]
        public void ProducerTest()
        {
            var p = new Person()
            {
                Name = "yepeng",
                Age = 18
            };
            MessageHandler.Producer<Person>("person", p);
            Assert.IsTrue(true);
        }
    }

    [Serializable]
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
