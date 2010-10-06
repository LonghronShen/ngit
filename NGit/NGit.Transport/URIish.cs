using System;
using System.Text;
using NGit;
using Sharpen;

namespace NGit.Transport
{
	/// <summary>
	/// This URI like construct used for referencing Git archives over the net, as
	/// well as locally stored archives.
	/// </summary>
	/// <remarks>
	/// This URI like construct used for referencing Git archives over the net, as
	/// well as locally stored archives. The most important difference compared to
	/// RFC 2396 URI's is that no URI encoding/decoding ever takes place. A space or
	/// any special character is written as-is.
	/// </remarks>
	[System.Serializable]
	public class URIish
	{
		private const long serialVersionUID = 1L;

		private static readonly Sharpen.Pattern FULL_URI = Sharpen.Pattern.Compile("^(?:([a-z][a-z0-9+-]+)://"
			 + "(?:([^/]+?)(?::([^/]+?))?@)?" + "(?:([^/]+?))?(?::(\\d+))?)?" + "((?:[A-Za-z]:)?"
			 + "(?:\\.\\.)?" + "/.+)$");

		private static readonly Sharpen.Pattern SCP_URI = Sharpen.Pattern.Compile("^(?:([^@]+?)@)?([^:]+?):(.+)$"
			);

		private string scheme;

		private string path;

		private string user;

		private string pass;

		private int port = -1;

		private string host;

		/// <summary>
		/// Parse and construct an
		/// <see cref="URIish">URIish</see>
		/// from a string
		/// </summary>
		/// <param name="s"></param>
		/// <exception cref="Sharpen.URISyntaxException">Sharpen.URISyntaxException</exception>
		public URIish(string s)
		{
			// optional http://
			// optional user:password@
			// optional example.com:1337
			// optional drive-letter:
			// optionally a relative path
			// /anything
			s = s.Replace('\\', '/');
			Matcher matcher = FULL_URI.Matcher(s);
			if (matcher.Matches())
			{
				scheme = matcher.Group(1);
				user = matcher.Group(2);
				pass = matcher.Group(3);
				host = matcher.Group(4);
				if (matcher.Group(5) != null)
				{
					port = System.Convert.ToInt32(matcher.Group(5));
				}
				path = matcher.Group(6);
				if (path.Length >= 3 && path[0] == '/' && path[2] == ':' && (path[1] >= 'A' && path
					[1] <= 'Z' || path[1] >= 'a' && path[1] <= 'z'))
				{
					path = Sharpen.Runtime.Substring(path, 1);
				}
				else
				{
					if (scheme != null && path.Length >= 2 && path[0] == '/' && path[1] == '~')
					{
						path = Sharpen.Runtime.Substring(path, 1);
					}
				}
			}
			else
			{
				matcher = SCP_URI.Matcher(s);
				if (matcher.Matches())
				{
					user = matcher.Group(1);
					host = matcher.Group(2);
					path = matcher.Group(3);
				}
				else
				{
					throw new URISyntaxException(s, JGitText.Get().cannotParseGitURIish);
				}
			}
		}

		/// <summary>Construct a URIish from a standard URL.</summary>
		/// <remarks>Construct a URIish from a standard URL.</remarks>
		/// <param name="u">the source URL to convert from.</param>
		public URIish(Uri u)
		{
			scheme = u.Scheme;
			path = u.AbsolutePath;
			string ui = u.UserInfo;
			if (ui != null)
			{
				int d = ui.IndexOf(':');
				user = d < 0 ? ui : Sharpen.Runtime.Substring(ui, 0, d);
				pass = d < 0 ? null : Sharpen.Runtime.Substring(ui, d + 1);
			}
			port = u.Port;
			host = u.Host;
		}

		/// <summary>Create an empty, non-configured URI.</summary>
		/// <remarks>Create an empty, non-configured URI.</remarks>
		public URIish()
		{
		}

		private URIish(NGit.Transport.URIish u)
		{
			// Configure nothing.
			this.scheme = u.scheme;
			this.path = u.path;
			this.user = u.user;
			this.pass = u.pass;
			this.port = u.port;
			this.host = u.host;
		}

		/// <returns>true if this URI references a repository on another system.</returns>
		public virtual bool IsRemote()
		{
			return GetHost() != null;
		}

		/// <returns>host name part or null</returns>
		public virtual string GetHost()
		{
			return host;
		}

		/// <summary>Return a new URI matching this one, but with a different host.</summary>
		/// <remarks>Return a new URI matching this one, but with a different host.</remarks>
		/// <param name="n">the new value for host.</param>
		/// <returns>a new URI with the updated value.</returns>
		public virtual NGit.Transport.URIish SetHost(string n)
		{
			NGit.Transport.URIish r = new NGit.Transport.URIish(this);
			r.host = n;
			return r;
		}

		/// <returns>protocol name or null for local references</returns>
		public virtual string GetScheme()
		{
			return scheme;
		}

		/// <summary>Return a new URI matching this one, but with a different scheme.</summary>
		/// <remarks>Return a new URI matching this one, but with a different scheme.</remarks>
		/// <param name="n">the new value for scheme.</param>
		/// <returns>a new URI with the updated value.</returns>
		public virtual NGit.Transport.URIish SetScheme(string n)
		{
			NGit.Transport.URIish r = new NGit.Transport.URIish(this);
			r.scheme = n;
			return r;
		}

		/// <returns>path name component</returns>
		public virtual string GetPath()
		{
			return path;
		}

		/// <summary>Return a new URI matching this one, but with a different path.</summary>
		/// <remarks>Return a new URI matching this one, but with a different path.</remarks>
		/// <param name="n">the new value for path.</param>
		/// <returns>a new URI with the updated value.</returns>
		public virtual NGit.Transport.URIish SetPath(string n)
		{
			NGit.Transport.URIish r = new NGit.Transport.URIish(this);
			r.path = n;
			return r;
		}

		/// <returns>user name requested for transfer or null</returns>
		public virtual string GetUser()
		{
			return user;
		}

		/// <summary>Return a new URI matching this one, but with a different user.</summary>
		/// <remarks>Return a new URI matching this one, but with a different user.</remarks>
		/// <param name="n">the new value for user.</param>
		/// <returns>a new URI with the updated value.</returns>
		public virtual NGit.Transport.URIish SetUser(string n)
		{
			NGit.Transport.URIish r = new NGit.Transport.URIish(this);
			r.user = n;
			return r;
		}

		/// <returns>password requested for transfer or null</returns>
		public virtual string GetPass()
		{
			return pass;
		}

		/// <summary>Return a new URI matching this one, but with a different password.</summary>
		/// <remarks>Return a new URI matching this one, but with a different password.</remarks>
		/// <param name="n">the new value for password.</param>
		/// <returns>a new URI with the updated value.</returns>
		public virtual NGit.Transport.URIish SetPass(string n)
		{
			NGit.Transport.URIish r = new NGit.Transport.URIish(this);
			r.pass = n;
			return r;
		}

		/// <returns>port number requested for transfer or -1 if not explicit</returns>
		public virtual int GetPort()
		{
			return port;
		}

		/// <summary>Return a new URI matching this one, but with a different port.</summary>
		/// <remarks>Return a new URI matching this one, but with a different port.</remarks>
		/// <param name="n">the new value for port.</param>
		/// <returns>a new URI with the updated value.</returns>
		public virtual NGit.Transport.URIish SetPort(int n)
		{
			NGit.Transport.URIish r = new NGit.Transport.URIish(this);
			r.port = n > 0 ? n : -1;
			return r;
		}

		public override int GetHashCode()
		{
			int hc = 0;
			if (GetScheme() != null)
			{
				hc = hc * 31 + GetScheme().GetHashCode();
			}
			if (GetUser() != null)
			{
				hc = hc * 31 + GetUser().GetHashCode();
			}
			if (GetPass() != null)
			{
				hc = hc * 31 + GetPass().GetHashCode();
			}
			if (GetHost() != null)
			{
				hc = hc * 31 + GetHost().GetHashCode();
			}
			if (GetPort() > 0)
			{
				hc = hc * 31 + GetPort();
			}
			if (GetPath() != null)
			{
				hc = hc * 31 + GetPath().GetHashCode();
			}
			return hc;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is NGit.Transport.URIish))
			{
				return false;
			}
			NGit.Transport.URIish b = (NGit.Transport.URIish)obj;
			if (!Eq(GetScheme(), b.GetScheme()))
			{
				return false;
			}
			if (!Eq(GetUser(), b.GetUser()))
			{
				return false;
			}
			if (!Eq(GetPass(), b.GetPass()))
			{
				return false;
			}
			if (!Eq(GetHost(), b.GetHost()))
			{
				return false;
			}
			if (GetPort() != b.GetPort())
			{
				return false;
			}
			if (!Eq(GetPath(), b.GetPath()))
			{
				return false;
			}
			return true;
		}

		private static bool Eq(string a, string b)
		{
			if (a == b)
			{
				return true;
			}
			if (a == null || b == null)
			{
				return false;
			}
			return a.Equals(b);
		}

		/// <summary>Obtain the string form of the URI, with the password included.</summary>
		/// <remarks>Obtain the string form of the URI, with the password included.</remarks>
		/// <returns>the URI, including its password field, if any.</returns>
		public virtual string ToPrivateString()
		{
			return Format(true);
		}

		public override string ToString()
		{
			return Format(false);
		}

		private string Format(bool includePassword)
		{
			StringBuilder r = new StringBuilder();
			if (GetScheme() != null)
			{
				r.Append(GetScheme());
				r.Append("://");
			}
			if (GetUser() != null)
			{
				r.Append(GetUser());
				if (includePassword && GetPass() != null)
				{
					r.Append(':');
					r.Append(GetPass());
				}
			}
			if (GetHost() != null)
			{
				if (GetUser() != null)
				{
					r.Append('@');
				}
				r.Append(GetHost());
				if (GetScheme() != null && GetPort() > 0)
				{
					r.Append(':');
					r.Append(GetPort());
				}
			}
			if (GetPath() != null)
			{
				if (GetScheme() != null)
				{
					if (!GetPath().StartsWith("/"))
					{
						r.Append('/');
					}
				}
				else
				{
					if (GetHost() != null)
					{
						r.Append(':');
					}
				}
				r.Append(GetPath());
			}
			return r.ToString();
		}

		/// <summary>Get the "humanish" part of the path.</summary>
		/// <remarks>
		/// Get the "humanish" part of the path. Some examples of a 'humanish' part
		/// for a full path:
		/// <table>
		/// <tr>
		/// <th>Path</th>
		/// <th>Humanish part</th>
		/// </tr>
		/// <tr>
		/// <td><code>/path/to/repo.git</code></td>
		/// <td rowspan="4"><code>repo</code></td>
		/// </tr>
		/// <tr>
		/// <td><code>/path/to/repo.git/</code></td>
		/// </tr>
		/// <tr>
		/// <td><code>/path/to/repo/.git</code></td>
		/// </tr>
		/// <tr>
		/// <td><code>/path/to/repo/</code></td>
		/// </tr>
		/// <tr>
		/// <td><code>/path//to</code></td>
		/// <td>an empty string</td>
		/// </tr>
		/// </table>
		/// </remarks>
		/// <returns>
		/// the "humanish" part of the path. May be an empty string. Never
		/// <code>null</code>
		/// .
		/// </returns>
		/// <exception cref="System.ArgumentException">
		/// if it's impossible to determine a humanish part, or path is
		/// <code>null</code>
		/// or empty
		/// </exception>
		/// <seealso cref="GetPath()">GetPath()</seealso>
		public virtual string GetHumanishName()
		{
			if (string.Empty.Equals(GetPath()) || GetPath() == null)
			{
				throw new ArgumentException();
			}
			string[] elements = GetPath().Split("/");
			if (elements.Length == 0)
			{
				throw new ArgumentException();
			}
			string result = elements[elements.Length - 1];
			if (Constants.DOT_GIT.Equals(result))
			{
				result = elements[elements.Length - 2];
			}
			else
			{
				if (result.EndsWith(Constants.DOT_GIT_EXT))
				{
					result = Sharpen.Runtime.Substring(result, 0, result.Length - Constants.DOT_GIT_EXT
						.Length);
				}
			}
			return result;
		}
	}
}