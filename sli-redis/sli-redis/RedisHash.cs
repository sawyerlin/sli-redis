using System.Collections.Generic;

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

            string returnedValue = _client.SendCommand("HGETALL {0}\r\n", key);

            int index = 0;
            string newKey = string.Empty;

            _client.ReadData(value =>
            {
                if (index % 2 == 0)
                    newKey = value;
                else
                    result.Add(newKey, value);

                index++;
            }, returnedValue);

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
