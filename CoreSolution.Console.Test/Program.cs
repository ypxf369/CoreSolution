using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using CoreSolution.AutoMapper;
using CoreSolution.AutoMapper.Configuration;
using CoreSolution.AutoMapper.Extensions;
using CoreSolution.AutoMapper.Startup;
using CoreSolution.Domain.Entities;
using CoreSolution.Dto;
using CoreSolution.EntityFrameworkCore;
using CoreSolution.IService;
using CoreSolution.RabbitMQ;
using CoreSolution.Service;
using Microsoft.EntityFrameworkCore;

namespace CoreSolution.Console.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //var p = new Person()
            //{
            //    Name = "yepeng",
            //    Age = 18
            //};
            //MessageHandler.Producer<Person>("person1", p);

            MessageHandler.Consumer((byte[] a) =>
            {
                var b = a;
                var c = MessageHandler.Deserializer<Person>(b);
                System.Console.WriteLine(c.Name);
            }, "person1");
            System.Console.ReadKey();


            System.Console.WriteLine("Hello World!");
        }



    }
    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
