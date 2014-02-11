namespace sli_redis.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            RedisClient client = new RedisClient("192.168.102.49");
            client.Hash.Set("test", "a", "b");
            var result = client.Key.GetAll("*");
        }
    }
}
