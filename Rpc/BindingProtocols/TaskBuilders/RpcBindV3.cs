using System.Threading;
using System.Threading.Tasks;

namespace Rpc.BindingProtocols.TaskBuilders
{
	/// <summary>
	/// RPCBIND Version 3
	/// 
	/// RPCBIND is contacted by way of an assigned address specific to the transport being used.
	/// For TCP/IP and UDP/IP, for example, it is port number 111. Each transport has such an assigned, well-known address.
	/// http://tools.ietf.org/html/rfc1833#section-2.2.1
	/// </summary>
	public sealed class RpcBindV3 : BaseRpcBind
	{
		/// <summary>
		/// RPCBIND Version 3
		/// 
		/// RPCBIND is contacted by way of an assigned address specific to the transport being used.
		/// For TCP/IP and UDP/IP, for example, it is port number 111. Each transport has such an assigned, well-known address.
		/// http://tools.ietf.org/html/rfc1833#section-2.2.1
		/// </summary>
		/// <param name="conn">instance of connector</param>
		/// <param name="token">cancellation token</param>
		/// <param name="attachedToParent">attache created task to parent task</param>
		public RpcBindV3(IRpcClient conn, CancellationToken token, bool attachedToParent)
			: base(conn, token, attachedToParent)
		{
		}
		
		/// <summary>
		/// Gets the version of protocol
		/// </summary>
		protected override uint Version
		{
			get
			{
				return 3u;
			}
		}

		/// <summary>
		/// This procedure allows a caller to call another remote procedure on the same machine without knowing the remote procedure's universal
		/// address.  It is intended for supporting broadcasts to arbitrary remote programs via RPCBIND's universal address.  The parameters
		/// "prog", "vers", "proc", and args are the program number, version number, procedure number, and parameters of the remote procedure.
		/// Note - This procedure only sends a response if the procedure was successfully executed and is silent (no response) otherwise.
		/// The procedure returns the remote program's universal address, and the results of the remote procedure.
		/// </summary>
		public Task<rpcb_rmtcallres> CallIt(rpcb_rmtcallargs arg)
		{
			return CreateTask<rpcb_rmtcallargs, rpcb_rmtcallres>(5u, arg);
		}
	}
}
