using System;

namespace bgs
{
	public class SslCertBundleSettings
	{
		public SslCertBundle bundle;

		public UrlDownloaderConfig bundleDownloadConfig;

		public SslCertBundleSettings()
		{
			this.bundle = new SslCertBundle(null);
			this.bundleDownloadConfig = new UrlDownloaderConfig();
		}
	}
}
