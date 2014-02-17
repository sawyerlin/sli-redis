using System;
using System.Collections.Generic;

namespace sli_redis.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            RedisClient client = new RedisClient("192.168.102.49");

            // Hash
            //client.Hash.Set("test", "a", "b");
            //var result = client.Key.GetAll("*");

            // List
            client.List.EnQueue("mylist", new List<string> { "Test1", "Test2", "Test3" });
            client.List.EnQueue("mylist", "Test4");
            for (int i = 0; i < 10; i++)
            {
                var result = client.List.DeQueue("mylist");
                if (result == null)
                    break;

                Console.WriteLine(result);
            }


            Console.Read();
        }
    }
}
