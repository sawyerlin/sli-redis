using System.Collections.Generic;
using System.Text;

namespace sli_redis
{
    public class RedisHash
    {
        private readonly RedisClient _client;

        public RedisHash(RedisClient client)
        {
            _client = client;
        }

        public Dictionary<string, string> GetAll(string key)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            _client.SendCommand("HGETALL {0}\r\n", key);
            _client.ReadData((i, field) =>
            {
                string str = Encoding.UTF8.GetString(_client.ReadLine());
                if (i % 2 == 0)
                {
                    field = str;
                }
                else
                    result.Add(field, str);

                return field;
            }, result);

            return result;
        }

        public void Set(string key, string field, string value)
        {
            _client.SendCommand("HSET {0} {1}\r\n", key, field + " " + value);
        }

        public void SetIfNotExist(string key, string field, string value)
        {
            _client.SendCommand("HSETNX {0} {1}\r\n", key, field + " " + value);
        }
    }
}
