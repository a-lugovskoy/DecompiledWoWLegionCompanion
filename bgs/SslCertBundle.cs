using System;

namespace bgs
{
	public class SslCertBundle
	{
		private bool isUsingCertBundle;

		public bool isCertBundleSigned = true;

		private byte[] certBundleBytes;

		public bool IsUsingCertBundle
		{
			get
			{
				return this.isUsingCertBundle;
			}
		}

		public byte[] CertBundleBytes
		{
			get
			{
				return this.certBundleBytes;
			}
			set
			{
				this.certBundleBytes = value;
				this.isUsingCertBundle = (this.certBundleBytes != null);
			}
		}

		public SslCertBundle(byte[] certBundleBytes)
		{
			this.CertBundleBytes = certBundleBytes;
		}
	}
}
