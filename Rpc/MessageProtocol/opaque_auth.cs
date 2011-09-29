using System;
using Xdr;

namespace Rpc.MessageProtocol
{
	/// <summary>
	/// 
	/// http://tools.ietf.org/html/rfc5531#section-8.2
	/// </summary>
	public class opaque_auth
	{
		/// <summary>
		/// Authentication flavor
		/// </summary>
		[Order(0)]
		public auth_flavor flavor;
		
		/// <summary>
		/// The interpretation and semantics of the data contained within the 
		/// authentication fields are specified by individual, independent
		/// authentication protocol specifications.
		/// </summary>
		[Order(1), Var(400)]
		public byte[] body;
	};
}