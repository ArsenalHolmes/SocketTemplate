using System.Net.Sockets;


namespace ClientTemplate
{
    public interface ISocketEvent
    {
        void ConnectEvent();
        void ReceiveEvent(int count);
        void SendEvent(int count);
        void ClientCloseEvent();
    }
}
