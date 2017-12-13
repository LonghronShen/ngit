namespace Sharpen
{
	using System;
	using System.IO;
	using System.Text;

	internal class InputStreamReader : StreamReader
	{

#if NETCORE
		// TODO: Object disposal leak here?
		protected InputStreamReader(string file)
			: base(File.OpenRead(file))
		{
		}
#else
		protected InputStreamReader (string file) : base(file)
		{
		}
#endif

		public InputStreamReader(InputStream s) : base(s.GetWrappedStream())
		{
		}

		public InputStreamReader(InputStream s, string encoding) : base(s.GetWrappedStream(), Encoding.GetEncoding(encoding))
		{
		}

		public InputStreamReader(InputStream s, Encoding e) : base(s.GetWrappedStream(), e)
		{
		}
	}
}
