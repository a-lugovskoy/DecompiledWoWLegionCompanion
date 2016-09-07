using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using System;
using System.IO;

namespace bgs
{
	public class FileUtil
	{
		private static readonly uint BNET_COMPRESSED_MAGIC = 1131245658u;

		private static readonly uint BNET_COMPRESSED_VERSION = 0u;

		private static readonly byte[] BNET_COMPRESSED_MAGIC_BYTES = BitConverter.GetBytes(FileUtil.BNET_COMPRESSED_MAGIC);

		private static readonly byte[] BNET_COMPRESSED_VERSION_BYTES = BitConverter.GetBytes(FileUtil.BNET_COMPRESSED_VERSION);

		private static readonly int BNET_COMPRESSED_HEADER_SIZE = FileUtil.BNET_COMPRESSED_MAGIC_BYTES.Length + FileUtil.BNET_COMPRESSED_VERSION_BYTES.Length + 8;

		public static bool StoreToDrive(byte[] data, string filePath, bool overwrite, bool compress)
		{
			if (data == null)
			{
				return false;
			}
			try
			{
				bool flag = File.Exists(filePath);
				if (flag && !overwrite)
				{
					bool result = false;
					return result;
				}
				byte[] array = data;
				if (compress)
				{
					array = FileUtil.Compress(array);
					if (array == null)
					{
						bool result = false;
						return result;
					}
				}
				if (flag && overwrite)
				{
					File.Delete(filePath);
				}
				if (!compress && data.Length == 0)
				{
					File.Create(filePath).Close();
					bool result = true;
					return result;
				}
				FileStream fileStream = File.Create(filePath, array.Length);
				fileStream.Write(array, 0, array.Length);
				fileStream.Flush();
				fileStream.Close();
			}
			catch (Exception)
			{
				bool result = false;
				return result;
			}
			return true;
		}

		public static bool LoadFromDrive(string filePath, out byte[] data)
		{
			data = null;
			try
			{
				if (!File.Exists(filePath))
				{
					bool result = false;
					return result;
				}
				FileStream fileStream = File.OpenRead(filePath);
				byte[] array = new byte[fileStream.get_Length()];
				fileStream.Read(array, 0, array.Length);
				fileStream.Close();
				if (FileUtil.IsCompressedStream(array))
				{
					byte[] array2 = FileUtil.Decompress(array);
					if (array2 == null)
					{
						bool result = false;
						return result;
					}
					data = array2;
				}
				else
				{
					data = array;
				}
			}
			catch (Exception)
			{
				bool result = false;
				return result;
			}
			return true;
		}

		private static byte[] Compress(byte[] data)
		{
			byte[] array;
			try
			{
				Deflater deflater = new Deflater();
				MemoryStream memoryStream = new MemoryStream(data.Length);
				DeflaterOutputStream deflaterOutputStream = new DeflaterOutputStream(memoryStream, deflater);
				deflaterOutputStream.Write(data, 0, data.Length);
				deflaterOutputStream.Flush();
				deflaterOutputStream.Finish();
				array = memoryStream.ToArray();
			}
			catch (Exception)
			{
				return null;
			}
			byte[] bytes = BitConverter.GetBytes((ulong)((long)data.Length));
			int num = array.Length + FileUtil.BNET_COMPRESSED_HEADER_SIZE;
			byte[] array2 = new byte[num];
			int num2 = 0;
			Array.Copy(FileUtil.BNET_COMPRESSED_MAGIC_BYTES, 0, array2, num2, FileUtil.BNET_COMPRESSED_MAGIC_BYTES.Length);
			num2 += FileUtil.BNET_COMPRESSED_MAGIC_BYTES.Length;
			Array.Copy(FileUtil.BNET_COMPRESSED_VERSION_BYTES, 0, array2, num2, FileUtil.BNET_COMPRESSED_VERSION_BYTES.Length);
			num2 += FileUtil.BNET_COMPRESSED_VERSION_BYTES.Length;
			Array.Copy(bytes, 0, array2, num2, bytes.Length);
			num2 += bytes.Length;
			Array.Copy(array, 0, array2, num2, array.Length);
			return array2;
		}

		private static byte[] Decompress(byte[] data)
		{
			MemoryStream memoryStream = new MemoryStream(data, FileUtil.BNET_COMPRESSED_HEADER_SIZE, data.Length - FileUtil.BNET_COMPRESSED_HEADER_SIZE);
			int num = (int)FileUtil.GetDecompressedLength(data);
			memoryStream.Seek(0L, 0);
			Inflater inflater = new Inflater(false);
			InflaterInputStream inflaterInputStream = new InflaterInputStream(memoryStream, inflater);
			byte[] array = new byte[num];
			int num2 = 0;
			int num3 = array.Length;
			try
			{
				while (true)
				{
					int num4 = inflaterInputStream.Read(array, num2, num3);
					if (num4 <= 0)
					{
						break;
					}
					num2 += num4;
					num3 -= num4;
				}
			}
			catch (Exception)
			{
				byte[] result = null;
				return result;
			}
			if (num2 != num)
			{
				return null;
			}
			return array;
		}

		private static bool IsCompressedStream(byte[] data)
		{
			try
			{
				if (data.Length < FileUtil.BNET_COMPRESSED_HEADER_SIZE)
				{
					bool result = false;
					return result;
				}
				if (BitConverter.ToUInt32(data, 0) != FileUtil.BNET_COMPRESSED_MAGIC)
				{
					bool result = false;
					return result;
				}
				if (BitConverter.ToUInt32(data, FileUtil.BNET_COMPRESSED_MAGIC_BYTES.Length) != FileUtil.BNET_COMPRESSED_VERSION)
				{
					bool result = false;
					return result;
				}
			}
			catch (Exception)
			{
				bool result = false;
				return result;
			}
			return true;
		}

		private static ulong GetDecompressedLength(byte[] data)
		{
			return BitConverter.ToUInt64(data, FileUtil.BNET_COMPRESSED_MAGIC_BYTES.Length + FileUtil.BNET_COMPRESSED_VERSION_BYTES.Length);
		}
	}
}
