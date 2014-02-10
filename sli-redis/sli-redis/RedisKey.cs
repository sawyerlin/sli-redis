using System.Collections.Generic;
using System.Text;

namespace sli_redis
{
    public class RedisKey
    {
        private readonly RedisClient _client;

        public RedisKey(RedisClient client)
        {
            _client = client;
        }

        public List<string> GetAll(string pattern)
        {
            List<string> result = new List<string>();

            _client.SendCommand("KEYS {0}\r\n", pattern);
            _client.ReadData<List<string>>((i, field) =>
            {
                string str = Encoding.UTF8.GetString(_client.ReadLine());
                result.Add(str);

                return str;
            }, result);

            return result;
        }
    }
}
