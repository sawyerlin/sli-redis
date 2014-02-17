namespace sli_redis
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class RedisList
    {
        private const string StartIndex = "0";

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
            string returnedValue = _client.SendCommand("LPOP {0}\r\n", key);
            _client.ReadData(value =>
                {
                    result = value;
                }, returnedValue);
            return result;
        }

        public string GetLength(string key)
        {
            string result = StartIndex;

            string returnedValue = _client.SendCommand("LLEN {0}\r\n", key);
            _client.ReadData(value =>
                {
                    result = value;
                }, returnedValue);

            return result;
        }

        public List<string> GetAll(string key)
        {
            List<string> result = new List<string>();

            string length = GetLength(key);

            string returnedValue = _client.SendCommand("LRANGE {0} {1} {2}\r\n", key, StartIndex, length);

            _client.ReadData(result.Add, returnedValue);

            return result;
        }
    }
}