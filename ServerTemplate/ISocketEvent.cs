using System.Net.Sockets;


namespace ServerTemplate
{
    public interface ISocketEvent
    {
        void AcceptEvent(Socket client);
        void ReceiveEvent(int count);
        void SendEvent(int count);
        void ClientCloseEvent();
    }
}
