using System;
using System.Collections.Generic;
using System.Threading;

namespace sli_redis.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            RedisClient client = new RedisClient("192.168.102.49");

            // Hash
            //client.Hash.Set("test", "a", "b");
            //client.Hash.SetIfNotExist("test", "a", "c");
            //client.Hash.SetIfNotExist("test", "b", "c");
            //var test = client.Hash.GetAll("test");
            //Console.WriteLine(test);
            //client.Hash.Set("test1", "a", "b");

            // Key
            //var test = client.Key.GetAll("*");
            //Console.WriteLine(test);

            // List
            int index = 0;
            while (true)
            {
                client.List.EnQueue("mylist", "Test " + index);
                Thread.Sleep(1000);
                index++;
            }
            //client.List.EnQueue("mylist", new List<string> { "Test1", "Test2", "Test3" });
            //client.List.EnQueue("mylist", "Test4");
            //for (int i = 0; i < 10; i++)
            //{
            //    var result = client.List.DeQueue("mylist");
            //    if (result == null)
            //        break;

            //    Console.WriteLine(result);
            //}
            //string length = client.List.GetLength("mylist");
            //List<string> result = client.List.GetAll("mylist");
            //Console.WriteLine(length);
            //Console.WriteLine(result);
        }
    }
}
