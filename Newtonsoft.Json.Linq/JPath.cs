using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Newtonsoft.Json.Linq
{
	internal class JPath
	{
		private readonly string _expression;

		private int _currentIndex;

		public List<object> Parts
		{
			get;
			private set;
		}

		public JPath(string expression)
		{
			ValidationUtils.ArgumentNotNull(expression, "expression");
			this._expression = expression;
			this.Parts = new List<object>();
			this.ParseMain();
		}

		private void ParseMain()
		{
			int num = this._currentIndex;
			bool flag = false;
			while (this._currentIndex < this._expression.get_Length())
			{
				char c = this._expression.get_Chars(this._currentIndex);
				char c2 = c;
				switch (c2)
				{
				case '[':
					goto IL_59;
				case '\\':
					IL_39:
					if (c2 == '(')
					{
						goto IL_59;
					}
					if (c2 == ')')
					{
						goto IL_9D;
					}
					if (c2 == '.')
					{
						if (this._currentIndex > num)
						{
							string text = this._expression.Substring(num, this._currentIndex - num);
							this.Parts.Add(text);
						}
						num = this._currentIndex + 1;
						flag = false;
						goto IL_113;
					}
					if (flag)
					{
						throw new Exception("Unexpected character following indexer: " + c);
					}
					goto IL_113;
				case ']':
					goto IL_9D;
				}
				goto IL_39;
				IL_113:
				this._currentIndex++;
				continue;
				IL_59:
				if (this._currentIndex > num)
				{
					string text2 = this._expression.Substring(num, this._currentIndex - num);
					this.Parts.Add(text2);
				}
				this.ParseIndexer(c);
				num = this._currentIndex + 1;
				flag = true;
				goto IL_113;
				IL_9D:
				throw new Exception("Unexpected character while parsing path: " + c);
			}
			if (this._currentIndex > num)
			{
				string text3 = this._expression.Substring(num, this._currentIndex - num);
				this.Parts.Add(text3);
			}
		}

		private void ParseIndexer(char indexerOpenChar)
		{
			this._currentIndex++;
			char c = (indexerOpenChar != '[') ? ')' : ']';
			int currentIndex = this._currentIndex;
			int num = 0;
			bool flag = false;
			while (this._currentIndex < this._expression.get_Length())
			{
				char c2 = this._expression.get_Chars(this._currentIndex);
				if (char.IsDigit(c2))
				{
					num++;
					this._currentIndex++;
				}
				else
				{
					if (c2 == c)
					{
						flag = true;
						break;
					}
					throw new Exception("Unexpected character while parsing path indexer: " + c2);
				}
			}
			if (!flag)
			{
				throw new Exception("Path ended with open indexer. Expected " + c);
			}
			if (num == 0)
			{
				throw new Exception("Empty path indexer.");
			}
			string text = this._expression.Substring(currentIndex, num);
			this.Parts.Add(Convert.ToInt32(text, CultureInfo.get_InvariantCulture()));
		}

		internal JToken Evaluate(JToken root, bool errorWhenNoMatch)
		{
			JToken jToken = root;
			using (List<object>.Enumerator enumerator = this.Parts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.get_Current();
					string text = current as string;
					if (text != null)
					{
						JObject jObject = jToken as JObject;
						if (jObject != null)
						{
							jToken = jObject[text];
							if (jToken == null && errorWhenNoMatch)
							{
								throw new Exception("Property '{0}' does not exist on JObject.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
								{
									text
								}));
							}
						}
						else
						{
							if (errorWhenNoMatch)
							{
								throw new Exception("Property '{0}' not valid on {1}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
								{
									text,
									jToken.GetType().get_Name()
								}));
							}
							JToken result = null;
							return result;
						}
					}
					else
					{
						int num = (int)current;
						JArray jArray = jToken as JArray;
						if (jArray != null)
						{
							if (jArray.Count <= num)
							{
								if (errorWhenNoMatch)
								{
									throw new IndexOutOfRangeException("Index {0} outside the bounds of JArray.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
									{
										num
									}));
								}
								JToken result = null;
								return result;
							}
							else
							{
								jToken = jArray[num];
							}
						}
						else
						{
							if (errorWhenNoMatch)
							{
								throw new Exception("Index {0} not valid on {1}.".FormatWith(CultureInfo.get_InvariantCulture(), new object[]
								{
									num,
									jToken.GetType().get_Name()
								}));
							}
							JToken result = null;
							return result;
						}
					}
				}
			}
			return jToken;
		}
	}
}
