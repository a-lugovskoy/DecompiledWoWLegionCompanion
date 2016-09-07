using bnet.protocol;
using System;
using System.Text;

namespace bgs
{
	public class ContentHandle
	{
		public string Region
		{
			get;
			set;
		}

		public string Usage
		{
			get;
			set;
		}

		public string Sha256Digest
		{
			get;
			set;
		}

		public static ContentHandle FromProtocol(bnet.protocol.ContentHandle contentHandle)
		{
			if (contentHandle == null || !contentHandle.IsInitialized)
			{
				return null;
			}
			return new ContentHandle
			{
				Region = new FourCC(contentHandle.Region).ToString(),
				Usage = new FourCC(contentHandle.Usage).ToString(),
				Sha256Digest = ContentHandle.ByteArrayToString(contentHandle.Hash)
			};
		}

		public override string ToString()
		{
			return string.Format("Region={0} Usage={1} Sha256={2}", this.Region, this.Usage, this.Sha256Digest);
		}

		public static string ByteArrayToString(byte[] ba)
		{
			StringBuilder stringBuilder = new StringBuilder(ba.Length * 2);
			for (int i = 0; i < ba.Length; i++)
			{
				byte b = ba[i];
				stringBuilder.AppendFormat("{0:x2}", b);
			}
			return stringBuilder.ToString();
		}
	}
}
