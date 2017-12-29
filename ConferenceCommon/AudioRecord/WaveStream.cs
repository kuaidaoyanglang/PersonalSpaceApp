
using System;
using System.IO;

namespace ConferenceCommon.AudioRecord
{
	public class WaveStream : Stream, IDisposable
	{
		private Stream m_Stream;
		private long m_DataPos;
		private long m_Length;

		private WaveFormat m_Format;

		public WaveFormat Format
		{
			get { return m_Format; }
		}

		private string ReadChunk(BinaryReader reader)
		{
			byte[] ch = new byte[4];
			reader.Read(ch, 0, ch.Length);
			return System.Text.Encoding.ASCII.GetString(ch);
		}

		private void ReadHeader()
		{
			BinaryReader Reader = new BinaryReader(m_Stream);
			if (ReadChunk(Reader) != "RIFF")
				throw new Exception("Invalid file format");

            Reader.ReadInt32(); // 文件长度减去第一个8字节的段的描述，我们不使用它

			if (ReadChunk(Reader) != "WAVE")
				throw new Exception("Invalid file format");

			if (ReadChunk(Reader) != "fmt ")
				throw new Exception("Invalid file format");

			int len = Reader.ReadInt32();
			if (len < 16) // bad format chunk length
				throw new Exception("Invalid file format");

			m_Format = new WaveFormat(22050, 16, 2); // initialize to any format
			m_Format.wFormatTag = Reader.ReadInt16();
			m_Format.nChannels = Reader.ReadInt16();
			m_Format.nSamplesPerSec = Reader.ReadInt32();
			m_Format.nAvgBytesPerSec = Reader.ReadInt32();
			m_Format.nBlockAlign = Reader.ReadInt16();
			m_Format.wBitsPerSample = Reader.ReadInt16();

            // 在流提前跳过波浪格式块
            len -= 16; // 最小尺寸
			while (len > 0)
			{
				Reader.ReadByte();
				len--;
			}

            // 假设数据块对齐
			while(m_Stream.Position < m_Stream.Length && ReadChunk(Reader) != "data")
				;

			if (m_Stream.Position >= m_Stream.Length)
				throw new Exception("Invalid file format");

			m_Length = Reader.ReadInt32();
			m_DataPos = m_Stream.Position;

			Position = 0;
		}

		public WaveStream(string fileName) : this(new FileStream(fileName, FileMode.Open))
		{
		}
		public WaveStream(Stream S)
		{
			m_Stream = S;
			ReadHeader();
		}
		~WaveStream()
		{
			m_Dispose();
		}
		public void m_Dispose()
		{
			if (m_Stream != null)
				m_Stream.Close();
			GC.SuppressFinalize(this);
		}

		public override bool CanRead
		{
			get { return true; }
		}
		public override bool CanSeek
		{
			get { return true; }
		}
		public override bool CanWrite
		{
			get { return false; }
		}
		public override long Length
		{
			get { return m_Length; }
		}
		public override long Position
		{
			get { return m_Stream.Position - m_DataPos; }
			set { Seek(value, SeekOrigin.Begin); }
		}
		public override void Close()
		{
			m_Dispose();
		}
		public override void Flush()
		{
		}
		public override void SetLength(long len)
		{
			throw new InvalidOperationException();
		}
		public override long Seek(long pos, SeekOrigin o)
		{
			switch(o)
			{
				case SeekOrigin.Begin:
					m_Stream.Position = pos + m_DataPos;
					break;
				case SeekOrigin.Current:
					m_Stream.Seek(pos, SeekOrigin.Current);
					break;
				case SeekOrigin.End:
					m_Stream.Position = m_DataPos + m_Length - pos;
					break;
			}
			return this.Position;
		}
		public override int Read(byte[] buf, int ofs, int count)
		{
			int toread = (int)Math.Min(count, m_Length - Position);
			return m_Stream.Read(buf, ofs, toread);
		}
		public override void Write(byte[] buf, int ofs, int count)
		{
			throw new InvalidOperationException();
		}
	}
}
