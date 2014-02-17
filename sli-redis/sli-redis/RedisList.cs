namespace sli_redis
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class RedisList
    {
        private readonly RedisClient _client;

        public RedisList(RedisClient client)
        {
            _client = client;
        }

        public void EnQueue(string key, List<string> values)
        {
            string value = string.Empty;
            value = values.Aggregate(value, (current, next) => current + (" " + next));
            _client.SendCommand("RPUSH {0}{1}\r\n", key, value);
        }

        public void EnQueue(string key, string value)
        {
            _client.SendCommand("RPUSH {0} {1}\r\n", key, value);
        }

        public string DeQueue(string key)
        {
            string result = string.Empty;
            _client.SendCommand("LPOP {0}\r\n", key);
            _client.ReadData((i, field) =>
                {
                    result = i == -1 ? null : Encoding.UTF8.GetString(_client.ReadLine());
                    return result;
                }, result);
            return result;
        }
    }
}