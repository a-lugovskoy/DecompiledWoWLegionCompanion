using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	public class XmlNodeConverter : JsonConverter
	{
		private const string TextName = "#text";

		private const string CommentName = "#comment";

		private const string CDataName = "#cdata-section";

		private const string WhitespaceName = "#whitespace";

		private const string SignificantWhitespaceName = "#significant-whitespace";

		private const string DeclarationName = "?xml";

		private const string JsonNamespaceUri = "http://james.newtonking.com/projects/json";

		public string DeserializeRootElementName
		{
			get;
			set;
		}

		public bool WriteArrayAttribute
		{
			get;
			set;
		}

		public bool OmitRootObject
		{
			get;
			set;
		}

		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			IXmlNode node = this.WrapXml(value);
			XmlNamespaceManager manager = new XmlNamespaceManager(new NameTable());
			this.PushParentNamespaces(node, manager);
			if (!this.OmitRootObject)
			{
				writer.WriteStartObject();
			}
			this.SerializeNode(writer, node, manager, !this.OmitRootObject);
			if (!this.OmitRootObject)
			{
				writer.WriteEndObject();
			}
		}

		private IXmlNode WrapXml(object value)
		{
			throw new ArgumentException("Value must be an XML object.", "value");
		}

		private void PushParentNamespaces(IXmlNode node, XmlNamespaceManager manager)
		{
			List<IXmlNode> list = null;
			IXmlNode xmlNode = node;
			while ((xmlNode = xmlNode.ParentNode) != null)
			{
				if (xmlNode.NodeType == 1)
				{
					if (list == null)
					{
						list = new List<IXmlNode>();
					}
					list.Add(xmlNode);
				}
			}
			if (list != null)
			{
				list.Reverse();
				using (List<IXmlNode>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IXmlNode current = enumerator.get_Current();
						manager.PushScope();
						using (IEnumerator<IXmlNode> enumerator2 = current.Attributes.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								IXmlNode current2 = enumerator2.get_Current();
								if (current2.NamespaceURI == "http://www.w3.org/2000/xmlns/" && current2.LocalName != "xmlns")
								{
									manager.AddNamespace(current2.LocalName, current2.Value);
								}
							}
						}
					}
				}
			}
		}

		private string ResolveFullName(IXmlNode node, XmlNamespaceManager manager)
		{
			string text = (node.NamespaceURI != null && (!(node.LocalName == "xmlns") || !(node.NamespaceURI == "http://www.w3.org/2000/xmlns/"))) ? manager.LookupPrefix(node.NamespaceURI) : null;
			if (!string.IsNullOrEmpty(text))
			{
				return text + ":" + node.LocalName;
			}
			return node.LocalName;
		}

		private string GetPropertyName(IXmlNode node, XmlNamespaceManager manager)
		{
			switch (node.NodeType)
			{
			case 1:
				return this.ResolveFullName(node, manager);
			case 2:
				if (node.NamespaceURI == "http://james.newtonking.com/projects/json")
				{
					return "$" + node.LocalName;
				}
				return "@" + this.ResolveFullName(node, manager);
			case 3:
				return "#text";
			case 4:
				return "#cdata-section";
			case 7:
				return "?" + this.ResolveFullName(node, manager);
			case 8:
				return "#comment";
			case 13:
				return "#whitespace";
			case 14:
				return "#significant-whitespace";
			case 17:
				return "?xml";
			}
			throw new JsonSerializationException("Unexpected XmlNodeType when getting node name: " + node.NodeType);
		}

		private bool IsArray(IXmlNode node)
		{
			IXmlNode arg_3B_0;
			if (node.Attributes != null)
			{
				IXmlNode xmlNode = Enumerable.SingleOrDefault<IXmlNode>(node.Attributes, (IXmlNode a) => a.LocalName == "Array" && a.NamespaceURI == "http://james.newtonking.com/projects/json");
				arg_3B_0 = xmlNode;
			}
			else
			{
				arg_3B_0 = null;
			}
			IXmlNode xmlNode2 = arg_3B_0;
			return xmlNode2 != null && XmlConvert.ToBoolean(xmlNode2.Value);
		}

		private void SerializeGroupedNodes(JsonWriter writer, IXmlNode node, XmlNamespaceManager manager, bool writePropertyName)
		{
			Dictionary<string, List<IXmlNode>> dictionary = new Dictionary<string, List<IXmlNode>>();
			for (int i = 0; i < node.ChildNodes.get_Count(); i++)
			{
				IXmlNode xmlNode = node.ChildNodes.get_Item(i);
				string propertyName = this.GetPropertyName(xmlNode, manager);
				List<IXmlNode> list;
				if (!dictionary.TryGetValue(propertyName, ref list))
				{
					list = new List<IXmlNode>();
					dictionary.Add(propertyName, list);
				}
				list.Add(xmlNode);
			}
			using (Dictionary<string, List<IXmlNode>>.Enumerator enumerator = dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, List<IXmlNode>> current = enumerator.get_Current();
					List<IXmlNode> value = current.get_Value();
					if (value.get_Count() == 1 && !this.IsArray(value.get_Item(0)))
					{
						this.SerializeNode(writer, value.get_Item(0), manager, writePropertyName);
					}
					else
					{
						string key = current.get_Key();
						if (writePropertyName)
						{
							writer.WritePropertyName(key);
						}
						writer.WriteStartArray();
						for (int j = 0; j < value.get_Count(); j++)
						{
							this.SerializeNode(writer, value.get_Item(j), manager, false);
						}
						writer.WriteEndArray();
					}
				}
			}
		}

		private void SerializeNode(JsonWriter writer, IXmlNode node, XmlNamespaceManager manager, bool writePropertyName)
		{
			switch (node.NodeType)
			{
			case 1:
				if (this.IsArray(node) && Enumerable.All<IXmlNode>(node.ChildNodes, (IXmlNode n) => n.LocalName == node.LocalName) && node.ChildNodes.get_Count() > 0)
				{
					this.SerializeGroupedNodes(writer, node, manager, false);
				}
				else
				{
					using (IEnumerator<IXmlNode> enumerator = node.Attributes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IXmlNode current = enumerator.get_Current();
							if (current.NamespaceURI == "http://www.w3.org/2000/xmlns/")
							{
								string text = (!(current.LocalName != "xmlns")) ? string.Empty : current.LocalName;
								manager.AddNamespace(text, current.Value);
							}
						}
					}
					if (writePropertyName)
					{
						writer.WritePropertyName(this.GetPropertyName(node, manager));
					}
					if (Enumerable.Count<IXmlNode>(this.ValueAttributes(node.Attributes)) == 0 && node.ChildNodes.get_Count() == 1 && node.ChildNodes.get_Item(0).NodeType == 3)
					{
						writer.WriteValue(node.ChildNodes.get_Item(0).Value);
					}
					else if (node.ChildNodes.get_Count() == 0 && CollectionUtils.IsNullOrEmpty<IXmlNode>(node.Attributes))
					{
						writer.WriteNull();
					}
					else
					{
						writer.WriteStartObject();
						for (int i = 0; i < node.Attributes.get_Count(); i++)
						{
							this.SerializeNode(writer, node.Attributes.get_Item(i), manager, true);
						}
						this.SerializeGroupedNodes(writer, node, manager, true);
						writer.WriteEndObject();
					}
				}
				return;
			case 2:
			case 3:
			case 4:
			case 7:
			case 13:
			case 14:
				if (node.NamespaceURI == "http://www.w3.org/2000/xmlns/" && node.Value == "http://james.newtonking.com/projects/json")
				{
					return;
				}
				if (node.NamespaceURI == "http://james.newtonking.com/projects/json" && node.LocalName == "Array")
				{
					return;
				}
				if (writePropertyName)
				{
					writer.WritePropertyName(this.GetPropertyName(node, manager));
				}
				writer.WriteValue(node.Value);
				return;
			case 8:
				if (writePropertyName)
				{
					writer.WriteComment(node.Value);
				}
				return;
			case 9:
			case 11:
				this.SerializeGroupedNodes(writer, node, manager, writePropertyName);
				return;
			case 17:
			{
				IXmlDeclaration xmlDeclaration = (IXmlDeclaration)node;
				writer.WritePropertyName(this.GetPropertyName(node, manager));
				writer.WriteStartObject();
				if (!string.IsNullOrEmpty(xmlDeclaration.Version))
				{
					writer.WritePropertyName("@version");
					writer.WriteValue(xmlDeclaration.Version);
				}
				if (!string.IsNullOrEmpty(xmlDeclaration.Encoding))
				{
					writer.WritePropertyName("@encoding");
					writer.WriteValue(xmlDeclaration.Encoding);
				}
				if (!string.IsNullOrEmpty(xmlDeclaration.Standalone))
				{
					writer.WritePropertyName("@standalone");
					writer.WriteValue(xmlDeclaration.Standalone);
				}
				writer.WriteEndObject();
				return;
			}
			}
			throw new JsonSerializationException("Unexpected XmlNodeType when serializing nodes: " + node.NodeType);
		}

		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			XmlNamespaceManager manager = new XmlNamespaceManager(new NameTable());
			IXmlDocument xmlDocument = null;
			IXmlNode xmlNode = null;
			if (xmlDocument == null || xmlNode == null)
			{
				throw new JsonSerializationException("Unexpected type when converting XML: " + objectType);
			}
			if (reader.TokenType != JsonToken.StartObject)
			{
				throw new JsonSerializationException("XmlNodeConverter can only convert JSON that begins with an object.");
			}
			if (!string.IsNullOrEmpty(this.DeserializeRootElementName))
			{
				this.ReadElement(reader, xmlDocument, xmlNode, this.DeserializeRootElementName, manager);
			}
			else
			{
				reader.Read();
				this.DeserializeNode(reader, xmlDocument, manager, xmlNode);
			}
			return xmlDocument.WrappedNode;
		}

		private void DeserializeValue(JsonReader reader, IXmlDocument document, XmlNamespaceManager manager, string propertyName, IXmlNode currentNode)
		{
			if (propertyName != null)
			{
				if (XmlNodeConverter.<>f__switch$map0 == null)
				{
					Dictionary<string, int> dictionary = new Dictionary<string, int>(4);
					dictionary.Add("#text", 0);
					dictionary.Add("#cdata-section", 1);
					dictionary.Add("#whitespace", 2);
					dictionary.Add("#significant-whitespace", 3);
					XmlNodeConverter.<>f__switch$map0 = dictionary;
				}
				int num;
				if (XmlNodeConverter.<>f__switch$map0.TryGetValue(propertyName, ref num))
				{
					switch (num)
					{
					case 0:
						currentNode.AppendChild(document.CreateTextNode(reader.Value.ToString()));
						return;
					case 1:
						currentNode.AppendChild(document.CreateCDataSection(reader.Value.ToString()));
						return;
					case 2:
						currentNode.AppendChild(document.CreateWhitespace(reader.Value.ToString()));
						return;
					case 3:
						currentNode.AppendChild(document.CreateSignificantWhitespace(reader.Value.ToString()));
						return;
					}
				}
			}
			if (!string.IsNullOrEmpty(propertyName) && propertyName.get_Chars(0) == '?')
			{
				this.CreateInstruction(reader, document, currentNode, propertyName);
			}
			else
			{
				if (reader.TokenType == JsonToken.StartArray)
				{
					this.ReadArrayElements(reader, document, propertyName, currentNode, manager);
					return;
				}
				this.ReadElement(reader, document, currentNode, propertyName, manager);
			}
		}

		private void ReadElement(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string propertyName, XmlNamespaceManager manager)
		{
			if (string.IsNullOrEmpty(propertyName))
			{
				throw new JsonSerializationException("XmlNodeConverter cannot convert JSON with an empty property name to XML.");
			}
			Dictionary<string, string> dictionary = this.ReadAttributeElements(reader, manager);
			string prefix = MiscellaneousUtils.GetPrefix(propertyName);
			IXmlElement xmlElement = this.CreateElement(propertyName, document, prefix, manager);
			currentNode.AppendChild(xmlElement);
			using (Dictionary<string, string>.Enumerator enumerator = dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, string> current = enumerator.get_Current();
					string prefix2 = MiscellaneousUtils.GetPrefix(current.get_Key());
					IXmlNode arg_A7_0;
					if (!string.IsNullOrEmpty(prefix2))
					{
						IXmlNode xmlNode = document.CreateAttribute(current.get_Key(), manager.LookupNamespace(prefix2), current.get_Value());
						arg_A7_0 = xmlNode;
					}
					else
					{
						arg_A7_0 = document.CreateAttribute(current.get_Key(), current.get_Value());
					}
					IXmlNode attributeNode = arg_A7_0;
					xmlElement.SetAttributeNode(attributeNode);
				}
			}
			if (reader.TokenType == JsonToken.String)
			{
				xmlElement.AppendChild(document.CreateTextNode(reader.Value.ToString()));
			}
			else if (reader.TokenType == JsonToken.Integer)
			{
				xmlElement.AppendChild(document.CreateTextNode(XmlConvert.ToString((long)reader.Value)));
			}
			else if (reader.TokenType == JsonToken.Float)
			{
				xmlElement.AppendChild(document.CreateTextNode(XmlConvert.ToString((double)reader.Value)));
			}
			else if (reader.TokenType == JsonToken.Boolean)
			{
				xmlElement.AppendChild(document.CreateTextNode(XmlConvert.ToString((bool)reader.Value)));
			}
			else if (reader.TokenType == JsonToken.Date)
			{
				DateTime dateTime = (DateTime)reader.Value;
				xmlElement.AppendChild(document.CreateTextNode(XmlConvert.ToString(dateTime, DateTimeUtils.ToSerializationMode(dateTime.get_Kind()))));
			}
			else if (reader.TokenType != JsonToken.Null)
			{
				if (reader.TokenType != JsonToken.EndObject)
				{
					manager.PushScope();
					this.DeserializeNode(reader, document, manager, xmlElement);
					manager.PopScope();
				}
			}
		}

		private void ReadArrayElements(JsonReader reader, IXmlDocument document, string propertyName, IXmlNode currentNode, XmlNamespaceManager manager)
		{
			string prefix = MiscellaneousUtils.GetPrefix(propertyName);
			IXmlElement xmlElement = this.CreateElement(propertyName, document, prefix, manager);
			currentNode.AppendChild(xmlElement);
			int num = 0;
			while (reader.Read() && reader.TokenType != JsonToken.EndArray)
			{
				this.DeserializeValue(reader, document, manager, propertyName, xmlElement);
				num++;
			}
			if (this.WriteArrayAttribute)
			{
				this.AddJsonArrayAttribute(xmlElement, document);
			}
			if (num == 1 && this.WriteArrayAttribute)
			{
				IXmlElement element = Enumerable.Single<IXmlElement>(xmlElement.ChildNodes.CastValid<IXmlElement>(), (IXmlElement n) => n.LocalName == propertyName);
				this.AddJsonArrayAttribute(element, document);
			}
		}

		private void AddJsonArrayAttribute(IXmlElement element, IXmlDocument document)
		{
			element.SetAttributeNode(document.CreateAttribute("json:Array", "http://james.newtonking.com/projects/json", "true"));
		}

		private Dictionary<string, string> ReadAttributeElements(JsonReader reader, XmlNamespaceManager manager)
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			bool flag = false;
			bool flag2 = false;
			if (reader.TokenType != JsonToken.String && reader.TokenType != JsonToken.Null && reader.TokenType != JsonToken.Boolean && reader.TokenType != JsonToken.Integer && reader.TokenType != JsonToken.Float && reader.TokenType != JsonToken.Date && reader.TokenType != JsonToken.StartConstructor)
			{
				while (!flag && !flag2 && reader.Read())
				{
					JsonToken tokenType = reader.TokenType;
					if (tokenType != JsonToken.PropertyName)
					{
						if (tokenType != JsonToken.EndObject)
						{
							throw new JsonSerializationException("Unexpected JsonToken: " + reader.TokenType);
						}
						flag2 = true;
					}
					else
					{
						string text = reader.Value.ToString();
						if (!string.IsNullOrEmpty(text))
						{
							char c = text.get_Chars(0);
							char c2 = c;
							if (c2 != '$')
							{
								if (c2 != '@')
								{
									flag = true;
								}
								else
								{
									text = text.Substring(1);
									reader.Read();
									string text2 = reader.Value.ToString();
									dictionary.Add(text, text2);
									string text3;
									if (this.IsNamespaceAttribute(text, out text3))
									{
										manager.AddNamespace(text3, text2);
									}
								}
							}
							else
							{
								text = text.Substring(1);
								reader.Read();
								string text2 = reader.Value.ToString();
								string text4 = manager.LookupPrefix("http://james.newtonking.com/projects/json");
								if (text4 == null)
								{
									int? num = default(int?);
									while (manager.LookupNamespace("json" + num) != null)
									{
										num = new int?(num.GetValueOrDefault() + 1);
									}
									text4 = "json" + num;
									dictionary.Add("xmlns:" + text4, "http://james.newtonking.com/projects/json");
									manager.AddNamespace(text4, "http://james.newtonking.com/projects/json");
								}
								dictionary.Add(text4 + ":" + text, text2);
							}
						}
						else
						{
							flag = true;
						}
					}
				}
			}
			return dictionary;
		}

		private void CreateInstruction(JsonReader reader, IXmlDocument document, IXmlNode currentNode, string propertyName)
		{
			if (propertyName == "?xml")
			{
				string version = null;
				string encoding = null;
				string standalone = null;
				while (reader.Read() && reader.TokenType != JsonToken.EndObject)
				{
					string text = reader.Value.ToString();
					if (text != null)
					{
						if (XmlNodeConverter.<>f__switch$map1 == null)
						{
							Dictionary<string, int> dictionary = new Dictionary<string, int>(3);
							dictionary.Add("@version", 0);
							dictionary.Add("@encoding", 1);
							dictionary.Add("@standalone", 2);
							XmlNodeConverter.<>f__switch$map1 = dictionary;
						}
						int num;
						if (XmlNodeConverter.<>f__switch$map1.TryGetValue(text, ref num))
						{
							switch (num)
							{
							case 0:
								reader.Read();
								version = reader.Value.ToString();
								continue;
							case 1:
								reader.Read();
								encoding = reader.Value.ToString();
								continue;
							case 2:
								reader.Read();
								standalone = reader.Value.ToString();
								continue;
							}
						}
					}
					throw new JsonSerializationException("Unexpected property name encountered while deserializing XmlDeclaration: " + reader.Value);
				}
				IXmlNode newChild = document.CreateXmlDeclaration(version, encoding, standalone);
				currentNode.AppendChild(newChild);
			}
			else
			{
				IXmlNode newChild2 = document.CreateProcessingInstruction(propertyName.Substring(1), reader.Value.ToString());
				currentNode.AppendChild(newChild2);
			}
		}

		private IXmlElement CreateElement(string elementName, IXmlDocument document, string elementPrefix, XmlNamespaceManager manager)
		{
			IXmlElement arg_28_0;
			if (!string.IsNullOrEmpty(elementPrefix))
			{
				IXmlElement xmlElement = document.CreateElement(elementName, manager.LookupNamespace(elementPrefix));
				arg_28_0 = xmlElement;
			}
			else
			{
				arg_28_0 = document.CreateElement(elementName);
			}
			return arg_28_0;
		}

		private void DeserializeNode(JsonReader reader, IXmlDocument document, XmlNamespaceManager manager, IXmlNode currentNode)
		{
			JsonToken tokenType;
			while (true)
			{
				tokenType = reader.TokenType;
				switch (tokenType)
				{
				case JsonToken.StartConstructor:
				{
					string propertyName2 = reader.Value.ToString();
					while (reader.Read() && reader.TokenType != JsonToken.EndConstructor)
					{
						this.DeserializeValue(reader, document, manager, propertyName2, currentNode);
					}
					goto IL_17E;
				}
				case JsonToken.PropertyName:
				{
					if (currentNode.NodeType == 9 && document.DocumentElement != null)
					{
						goto Block_4;
					}
					string propertyName = reader.Value.ToString();
					reader.Read();
					if (reader.TokenType == JsonToken.StartArray)
					{
						int num = 0;
						while (reader.Read() && reader.TokenType != JsonToken.EndArray)
						{
							this.DeserializeValue(reader, document, manager, propertyName, currentNode);
							num++;
						}
						if (num == 1 && this.WriteArrayAttribute)
						{
							IXmlElement element = Enumerable.Single<IXmlElement>(currentNode.ChildNodes.CastValid<IXmlElement>(), (IXmlElement n) => n.LocalName == propertyName);
							this.AddJsonArrayAttribute(element, document);
						}
					}
					else
					{
						this.DeserializeValue(reader, document, manager, propertyName, currentNode);
					}
					goto IL_17E;
				}
				case JsonToken.Comment:
					currentNode.AppendChild(document.CreateComment((string)reader.Value));
					goto IL_17E;
				}
				break;
				IL_17E:
				if (reader.TokenType != JsonToken.PropertyName && !reader.Read())
				{
					return;
				}
			}
			if (tokenType != JsonToken.EndObject && tokenType != JsonToken.EndArray)
			{
				throw new JsonSerializationException("Unexpected JsonToken when deserializing node: " + reader.TokenType);
			}
			return;
			Block_4:
			throw new JsonSerializationException("JSON root object has multiple properties. The root object must have a single property in order to create a valid XML document. Consider specifing a DeserializeRootElementName.");
		}

		private bool IsNamespaceAttribute(string attributeName, out string prefix)
		{
			if (attributeName.StartsWith("xmlns", 4))
			{
				if (attributeName.get_Length() == 5)
				{
					prefix = string.Empty;
					return true;
				}
				if (attributeName.get_Chars(5) == ':')
				{
					prefix = attributeName.Substring(6, attributeName.get_Length() - 6);
					return true;
				}
			}
			prefix = null;
			return false;
		}

		private IEnumerable<IXmlNode> ValueAttributes(IEnumerable<IXmlNode> c)
		{
			return Enumerable.Where<IXmlNode>(c, (IXmlNode a) => a.NamespaceURI != "http://james.newtonking.com/projects/json");
		}

		public override bool CanConvert(Type valueType)
		{
			return false;
		}
	}
}
