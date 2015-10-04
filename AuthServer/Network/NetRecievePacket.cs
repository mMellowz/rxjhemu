using common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Network
{
    public abstract class NetRecievePacket
    {
        public BinaryReader Reader;
        public NetSession _Session;

        public void Process(NetSession sess)
        {
            _Session = sess;

            using (Reader = new BinaryReader(new MemoryStream(_Session.Buffer)))
            {
                Read();
            }

            try
            {
                Process();
            }
            catch (Exception ex)
            {
                Log.WarnException("ARecvPacket", ex);
            }
        }

        public abstract void Read();

        public abstract void Process();

        protected int ReadD()
        {
            try
            {
                return Reader.ReadInt32();
            }
            catch (Exception)
            {
                Log.Warn("Missing D for: {0}", GetType());
            }
            return 0;
        }

        protected int ReadC()
        {
            try
            {
                return Reader.ReadByte() & 0xFF;
            }
            catch (Exception)
            {
                Log.Warn("Missing C for: {0}", GetType());
            }
            return 0;
        }

        protected int ReadH()
        {
            try
            {
                return Reader.ReadInt16() & 0xFFFF;
            }
            catch (Exception)
            {
                Log.Warn("Missing H for: {0}", GetType());
            }
            return 0;
        }

        protected double ReadDf()
        {
            try
            {
                return Reader.ReadDouble();
            }
            catch (Exception)
            {
                Log.Warn("Missing DF for: {0}", GetType());
            }
            return 0;
        }

        protected float ReadF()
        {
            try
            {
                return Reader.ReadSingle();
            }
            catch (Exception)
            {
                Log.Warn("Missing F for: {0}", GetType());
            }
            return 0;
        }

        protected long ReadQ()
        {
            try
            {
                return Reader.ReadInt64();
            }
            catch (Exception)
            {
                Log.Warn("Missing Q for: {0}", GetType());
            }
            return 0;
        }

        protected internal string ReadS()
        {
            string str = "";
            try
            {
                int len = ReadH();
                str = ReadS(len); ;
            }
            catch (Exception ex)
            {
                Log.ErrorException("while reading string from packet", ex);
            }
            return str;
        }

        protected internal string ReadS(int Length)
        {
            string str = "";
            try
            {
                str = Encoding.Default.GetString(ReadB(Length));
            }
            catch (Exception ex)
            {
                Log.ErrorException("while reading string from packet", ex);
            }
            return str;
        }

        protected byte[] ReadB(int length)
        {
            byte[] result = new byte[length];
            try
            {
                Reader.Read(result, 0, length);
            }
            catch (Exception)
            {
                Log.Warn("Missing byte[] for: {0}", GetType());
            }
            return result;
        }
    }
}
