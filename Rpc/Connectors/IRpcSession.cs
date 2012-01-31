using System;

namespace Rpc.Connectors
{
	public interface IRpcSession : ITicketOwner
	{
		void AsyncSend(ITicket ticket);
		void Close(Exception ex);

		event Action<IRpcSession, Exception> OnExcepted;
		event Action<IRpcSession> OnSended;
	}
}
