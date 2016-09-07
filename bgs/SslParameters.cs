using System;

namespace bgs
{
	public class SslParameters
	{
		public bool useSsl = true;

		public SslCertBundleSettings bundleSettings;

		public SslParameters()
		{
			this.bundleSettings = new SslCertBundleSettings();
		}
	}
}
