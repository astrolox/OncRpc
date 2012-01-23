using System;
using System.IO;
using Xdr;
using System.Collections.Generic;

namespace Rpc.UdpDatagrams
{
	/// <summary>
	/// generator of UDP datagram
	/// </summary>
	public class UdpWriter : IByteWriter
	{
		private const int _max = 65535;
		private const int _maxBlock = 1024 * 4; // 4k

		private int _pos;
		private int _totalSize;
		private byte[] _currentBlock;
		private LinkedList<byte[]> _blocks;

		/// <summary>
		/// generator of UDP datagram
		/// </summary>
		public UdpWriter()
		{
			_pos = 0;
			_totalSize = 0;
			_currentBlock = new byte[_maxBlock];
			_blocks = new LinkedList<byte[]>();
		}

		/// <summary>
		/// write array of bytes
		/// </summary>
		/// <param name="buffer"></param>
		public void Write(byte[] buffer)
		{
			_totalSize += buffer.Length;
			if(_totalSize > _max)
				throw SizeIsExceeded();
			
			int offset = 0;
			while(true)
			{
				int len = buffer.Length - offset;
			
				if(len <= _maxBlock - _pos)
				{
					Array.Copy(buffer, offset, _currentBlock, _pos, len);
					_pos += len;
					if(_pos >= _maxBlock)
						CreateNextBlock();
				
					return;
				}
			
				Array.Copy(buffer, offset, _currentBlock, _pos, _maxBlock - _pos);
				offset += _maxBlock - _pos;
			
				CreateNextBlock();
			}
		}

		/// <summary>
		/// write byte
		/// </summary>
		/// <param name="b"></param>
		public void Write(byte b)
		{
			_totalSize++;
			if(_totalSize > _max)
				throw SizeIsExceeded();
			
			_currentBlock[_pos] = b;
			_pos++;
			
			if(_pos >= _maxBlock)
				CreateNextBlock();
		}
		
		private void CreateNextBlock()
		{
			_blocks.AddLast(_currentBlock);
			_currentBlock = new byte[_maxBlock];
			_pos = 0;
		}
		
		private static Exception SizeIsExceeded()
		{
			return new RpcException("UDP datagram size is exceeded");
		}

		public byte[] Build()
		{
			byte[] result = new byte[_totalSize];
			int offset = 0;
			foreach(var block in _blocks)
			{
				Array.Copy(block, 0, result, offset, _maxBlock);
				offset += _maxBlock;
			}
			
			if(_pos != 0) // _currentBlock is not empty
				Array.Copy(_currentBlock, 0, result, offset, _pos);

			return result;
		}
	}
}

