using System;
using System.Xml;

namespace Newtonsoft.Json.Converters
{
	internal class XmlDeclarationWrapper : XmlNodeWrapper, IXmlDeclaration, IXmlNode
	{
		private XmlDeclaration _declaration;

		public string Version
		{
			get
			{
				return this._declaration.get_Version();
			}
		}

		public string Encoding
		{
			get
			{
				return this._declaration.get_Encoding();
			}
			set
			{
				this._declaration.set_Encoding(value);
			}
		}

		public string Standalone
		{
			get
			{
				return this._declaration.get_Standalone();
			}
			set
			{
				this._declaration.set_Standalone(value);
			}
		}

		public XmlDeclarationWrapper(XmlDeclaration declaration) : base(declaration)
		{
			this._declaration = declaration;
		}
	}
}
