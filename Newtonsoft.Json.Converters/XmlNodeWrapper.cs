using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	internal class XmlNodeWrapper : IXmlNode
	{
		private readonly XmlNode _node;

		public object WrappedNode
		{
			get
			{
				return this._node;
			}
		}

		public XmlNodeType NodeType
		{
			get
			{
				return this._node.get_NodeType();
			}
		}

		public string Name
		{
			get
			{
				return this._node.get_Name();
			}
		}

		public string LocalName
		{
			get
			{
				return this._node.get_LocalName();
			}
		}

		public IList<IXmlNode> ChildNodes
		{
			get
			{
				return Enumerable.ToList<IXmlNode>(Enumerable.Select<XmlNode, IXmlNode>(Enumerable.Cast<XmlNode>(this._node.get_ChildNodes()), (XmlNode n) => this.WrapNode(n)));
			}
		}

		public IList<IXmlNode> Attributes
		{
			get
			{
				if (this._node.get_Attributes() == null)
				{
					return null;
				}
				return Enumerable.ToList<IXmlNode>(Enumerable.Select<XmlAttribute, IXmlNode>(Enumerable.Cast<XmlAttribute>(this._node.get_Attributes()), (XmlAttribute a) => this.WrapNode(a)));
			}
		}

		public IXmlNode ParentNode
		{
			get
			{
				XmlNode xmlNode = (!(this._node is XmlAttribute)) ? this._node.get_ParentNode() : ((XmlAttribute)this._node).get_OwnerElement();
				if (xmlNode == null)
				{
					return null;
				}
				return this.WrapNode(xmlNode);
			}
		}

		public string Value
		{
			get
			{
				return this._node.get_Value();
			}
			set
			{
				this._node.set_Value(value);
			}
		}

		public string Prefix
		{
			get
			{
				return this._node.get_Prefix();
			}
		}

		public string NamespaceURI
		{
			get
			{
				return this._node.get_NamespaceURI();
			}
		}

		public XmlNodeWrapper(XmlNode node)
		{
			this._node = node;
		}

		private IXmlNode WrapNode(XmlNode node)
		{
			XmlNodeType nodeType = node.get_NodeType();
			if (nodeType == 1)
			{
				return new XmlElementWrapper((XmlElement)node);
			}
			if (nodeType != 17)
			{
				return new XmlNodeWrapper(node);
			}
			return new XmlDeclarationWrapper((XmlDeclaration)node);
		}

		public IXmlNode AppendChild(IXmlNode newChild)
		{
			XmlNodeWrapper xmlNodeWrapper = (XmlNodeWrapper)newChild;
			this._node.AppendChild(xmlNodeWrapper._node);
			return newChild;
		}
	}
}
