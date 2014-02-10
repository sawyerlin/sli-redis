using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace sli_redis
{
    public class RedisClient
    {
        private readonly string _host;
        private readonly int _port;
        private readonly Socket _socket;
        private BufferedStream _stream;

        public RedisHash Hash { get; private set; }
        public RedisKey Key { get; private set; }

        public RedisClient(string host, int port = 6379, int db = 0)
        {
            _host = host;
            _port = port;
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                {
                    NoDelay = true,
                    SendTimeout = 1000
                };
            Hash = new RedisHash(this);
            Key = new RedisKey(this);
            SendCommand("SELECT {0}\r\n", db);
        }

        internal bool SendDataCommand(byte[] data, string cmd, params object[] args)
        {
            if (ConnectSocket())
            {
                string command = args != null && args.Length > 0 ? string.Format(cmd, args) : cmd;
                byte[] bytes = Encoding.UTF8.GetBytes(command);
                try
                {
                    _socket.Send(bytes);
                    if (data != null)
                    {
                        _socket.Send(data);
                        _socket.Send(new[] { (byte)'\r', (byte)'\n' });
                    }
                }
                catch (Exception)
                {
                    _socket.Close();
                    return false;
                }
            }
            return true;
        }

        internal bool SendCommand(string cmd, params object[] args)
        {
            if (ConnectSocket())
            {
                string command = args != null && args.Length > 0 ? string.Format(cmd, args) : cmd;
                byte[] bytes = Encoding.UTF8.GetBytes(command);
                try
                {
                    _socket.Send(bytes);
                    _socket.Send(new[] { (byte)'\r', (byte)'\n' });
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        internal void ReadData<T>(Func<int, string, string> readFunction, object param)
        {
            string str = Encoding.UTF8.GetString(ReadLine());
            if (str == "+OK")
            {
                str = Encoding.UTF8.GetString(ReadLine());
                if (str.StartsWith("*"))
                {
                    int count;
                    if (int.TryParse(str.Substring(1), out count))
                    {
                        string field = string.Empty;
                        for (int i = 0; i < count; i++)
                        {
                            str = Encoding.UTF8.GetString(ReadLine());
                            if (str.StartsWith("$"))
                            {
                                field = readFunction(i, field);
                            }
                        }
                    }
                }
            }
        }

        internal byte[] ReadLine()
        {
            List<byte> bytes = new List<byte>();
            int aByte;


            while ((aByte = _stream.ReadByte()) != -1)
            {
                if (aByte == '\r')
                    continue;
                if (aByte == '\n')
                    break;
                bytes.Add((byte)aByte);
            }

            byte[] result = new byte[bytes.Count];

            for (int index = 0; index < bytes.Count; index++)
                result[index] = bytes[index];

            return result;
        }

        private bool ConnectSocket()
        {
            if (!_socket.Connected)
            {
                _socket.Connect(_host, _port);
                if (!_socket.Connected)
                {
                    _socket.Close();
                    return false;
                }

                _stream = new BufferedStream(new NetworkStream(_socket), 16 * 1024);
                return true;
            }
            return true;
        }
    }
}
