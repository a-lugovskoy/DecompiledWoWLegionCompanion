using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Newtonsoft.Json.Bson
{
	internal class BsonBinaryWriter
	{
		private static readonly Encoding Encoding = Encoding.get_UTF8();

		private readonly BinaryWriter _writer;

		private byte[] _largeByteBuffer;

		private int _maxChars;

		public DateTimeKind DateTimeKindHandling
		{
			get;
			set;
		}

		public BsonBinaryWriter(Stream stream)
		{
			this.DateTimeKindHandling = 1;
			this._writer = new BinaryWriter(stream);
		}

		public void Flush()
		{
			this._writer.Flush();
		}

		public void Close()
		{
			this._writer.Close();
		}

		public void WriteToken(BsonToken t)
		{
			this.CalculateSize(t);
			this.WriteTokenInternal(t);
		}

		private void WriteTokenInternal(BsonToken t)
		{
			switch (t.Type)
			{
			case BsonType.Number:
			{
				BsonValue bsonValue = (BsonValue)t;
				this._writer.Write(Convert.ToDouble(bsonValue.Value, CultureInfo.get_InvariantCulture()));
				return;
			}
			case BsonType.String:
			{
				BsonString bsonString = (BsonString)t;
				this.WriteString((string)bsonString.Value, bsonString.ByteCount, new int?(bsonString.CalculatedSize - 4));
				return;
			}
			case BsonType.Object:
			{
				BsonObject bsonObject = (BsonObject)t;
				this._writer.Write(bsonObject.CalculatedSize);
				using (IEnumerator<BsonProperty> enumerator = bsonObject.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BsonProperty current = enumerator.get_Current();
						this._writer.Write((sbyte)current.Value.Type);
						this.WriteString((string)current.Name.Value, current.Name.ByteCount, default(int?));
						this.WriteTokenInternal(current.Value);
					}
				}
				this._writer.Write(0);
				return;
			}
			case BsonType.Array:
			{
				BsonArray bsonArray = (BsonArray)t;
				this._writer.Write(bsonArray.CalculatedSize);
				int num = 0;
				using (IEnumerator<BsonToken> enumerator2 = bsonArray.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						BsonToken current2 = enumerator2.get_Current();
						this._writer.Write((sbyte)current2.Type);
						this.WriteString(num.ToString(CultureInfo.get_InvariantCulture()), MathUtils.IntLength(num), default(int?));
						this.WriteTokenInternal(current2);
						num++;
					}
				}
				this._writer.Write(0);
				return;
			}
			case BsonType.Binary:
			{
				BsonValue bsonValue2 = (BsonValue)t;
				byte[] array = (byte[])bsonValue2.Value;
				this._writer.Write(array.Length);
				this._writer.Write(0);
				this._writer.Write(array);
				return;
			}
			case BsonType.Undefined:
			case BsonType.Null:
				return;
			case BsonType.Oid:
			{
				BsonValue bsonValue3 = (BsonValue)t;
				byte[] array2 = (byte[])bsonValue3.Value;
				this._writer.Write(array2);
				return;
			}
			case BsonType.Boolean:
			{
				BsonValue bsonValue4 = (BsonValue)t;
				this._writer.Write((bool)bsonValue4.Value);
				return;
			}
			case BsonType.Date:
			{
				BsonValue bsonValue5 = (BsonValue)t;
				long num2;
				if (bsonValue5.Value is DateTime)
				{
					DateTime dateTime = (DateTime)bsonValue5.Value;
					if (this.DateTimeKindHandling == 1)
					{
						dateTime = dateTime.ToUniversalTime();
					}
					else if (this.DateTimeKindHandling == 2)
					{
						dateTime = dateTime.ToLocalTime();
					}
					num2 = JsonConvert.ConvertDateTimeToJavaScriptTicks(dateTime, false);
				}
				else
				{
					DateTimeOffset dateTimeOffset = (DateTimeOffset)bsonValue5.Value;
					num2 = JsonConvert.ConvertDateTimeToJavaScriptTicks(dateTimeOffset.get_UtcDateTime(), dateTimeOffset.get_Offset());
				}
				this._writer.Write(num2);
				return;
			}
			case BsonType.Regex:
			{
				BsonRegex bsonRegex = (BsonRegex)t;
				this.WriteString((string)bsonRegex.Pattern.Value, bsonRegex.Pattern.ByteCount, default(int?));
				this.WriteString((string)bsonRegex.Options.Value, bsonRegex.Options.ByteCount, default(int?));
				return;
			}
			case BsonType.Integer:
			{
				BsonValue bsonValue6 = (BsonValue)t;
				this._writer.Write(Convert.ToInt32(bsonValue6.Value, CultureInfo.get_InvariantCulture()));
				return;
			}
			case BsonType.Long:
			{
				BsonValue bsonValue7 = (BsonValue)t;
				this._writer.Write(Convert.ToInt64(bsonValue7.Value, CultureInfo.get_InvariantCulture()));
				return;
			}
			}
			throw new ArgumentOutOfRangeException("t", "Unexpected token when writing BSON: {0}".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
			{
				t.Type
			}));
		}

		private void WriteString(string s, int byteCount, int? calculatedlengthPrefix)
		{
			if (calculatedlengthPrefix.get_HasValue())
			{
				this._writer.Write(calculatedlengthPrefix.get_Value());
			}
			if (s != null)
			{
				if (this._largeByteBuffer == null)
				{
					this._largeByteBuffer = new byte[256];
					this._maxChars = 256 / BsonBinaryWriter.Encoding.GetMaxByteCount(1);
				}
				if (byteCount <= 256)
				{
					BsonBinaryWriter.Encoding.GetBytes(s, 0, s.get_Length(), this._largeByteBuffer, 0);
					this._writer.Write(this._largeByteBuffer, 0, byteCount);
				}
				else
				{
					int num = 0;
					int num2;
					for (int i = s.get_Length(); i > 0; i -= num2)
					{
						num2 = ((i <= this._maxChars) ? i : this._maxChars);
						int bytes = BsonBinaryWriter.Encoding.GetBytes(s, num, num2, this._largeByteBuffer, 0);
						this._writer.Write(this._largeByteBuffer, 0, bytes);
						num += num2;
					}
				}
			}
			this._writer.Write(0);
		}

		private int CalculateSize(int stringByteCount)
		{
			return stringByteCount + 1;
		}

		private int CalculateSizeWithLength(int stringByteCount, bool includeSize)
		{
			int num = (!includeSize) ? 1 : 5;
			return num + stringByteCount;
		}

		private int CalculateSize(BsonToken t)
		{
			switch (t.Type)
			{
			case BsonType.Number:
				return 8;
			case BsonType.String:
			{
				BsonString bsonString = (BsonString)t;
				string text = (string)bsonString.Value;
				bsonString.ByteCount = ((text == null) ? 0 : BsonBinaryWriter.Encoding.GetByteCount(text));
				bsonString.CalculatedSize = this.CalculateSizeWithLength(bsonString.ByteCount, bsonString.IncludeLength);
				return bsonString.CalculatedSize;
			}
			case BsonType.Object:
			{
				BsonObject bsonObject = (BsonObject)t;
				int num = 4;
				using (IEnumerator<BsonProperty> enumerator = bsonObject.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						BsonProperty current = enumerator.get_Current();
						int num2 = 1;
						num2 += this.CalculateSize(current.Name);
						num2 += this.CalculateSize(current.Value);
						num += num2;
					}
				}
				num++;
				bsonObject.CalculatedSize = num;
				return num;
			}
			case BsonType.Array:
			{
				BsonArray bsonArray = (BsonArray)t;
				int num3 = 4;
				int num4 = 0;
				using (IEnumerator<BsonToken> enumerator2 = bsonArray.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						BsonToken current2 = enumerator2.get_Current();
						num3++;
						num3 += this.CalculateSize(MathUtils.IntLength(num4));
						num3 += this.CalculateSize(current2);
						num4++;
					}
				}
				num3++;
				bsonArray.CalculatedSize = num3;
				return bsonArray.CalculatedSize;
			}
			case BsonType.Binary:
			{
				BsonValue bsonValue = (BsonValue)t;
				byte[] array = (byte[])bsonValue.Value;
				bsonValue.CalculatedSize = 5 + array.Length;
				return bsonValue.CalculatedSize;
			}
			case BsonType.Undefined:
			case BsonType.Null:
				return 0;
			case BsonType.Oid:
				return 12;
			case BsonType.Boolean:
				return 1;
			case BsonType.Date:
				return 8;
			case BsonType.Regex:
			{
				BsonRegex bsonRegex = (BsonRegex)t;
				int num5 = 0;
				num5 += this.CalculateSize(bsonRegex.Pattern);
				num5 += this.CalculateSize(bsonRegex.Options);
				bsonRegex.CalculatedSize = num5;
				return bsonRegex.CalculatedSize;
			}
			case BsonType.Integer:
				return 4;
			case BsonType.Long:
				return 8;
			}
			throw new ArgumentOutOfRangeException("t", "Unexpected token when writing BSON: {0}".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
			{
				t.Type
			}));
		}
	}
}
