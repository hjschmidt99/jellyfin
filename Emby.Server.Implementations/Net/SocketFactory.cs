#pragma warning disable CS1591

using System;
using System.Net;
using System.Net.Sockets;
using MediaBrowser.Model.Net;

namespace Emby.Server.Implementations.Net
{
    public class SocketFactory : ISocketFactory
    {
        /// <inheritdoc />
        public Socket CreateUdpBroadcastSocket(int localPort)
        {
            if (localPort < 0)
            {
                throw new ArgumentException("localPort cannot be less than zero.", nameof(localPort));
            }

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                socket.EnableBroadcast = true;
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
                socket.Bind(new IPEndPoint(IPAddress.Any, localPort));

                return socket;
            }
            catch
            {
                socket?.Dispose();

                throw;
            }
        }

        /// <inheritdoc />
        public Socket CreateSsdpUdpSocket(IPData bindInterface, int localPort)
        {
            ArgumentNullException.ThrowIfNull(bindInterface.Address);

            if (localPort < 0)
            {
                throw new ArgumentException("localPort cannot be less than zero.", nameof(localPort));
            }

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            try
            {
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                socket.Bind(new IPEndPoint(bindInterface.Address, localPort));

                return socket;
            }
            catch
            {
                socket?.Dispose();

                throw;
            }
        }

        /// <inheritdoc />
        public Socket CreateUdpMulticastSocket(IPAddress multicastAddress, IPData bindInterface, int multicastTimeToLive, int localPort)
        {
            var bindIPAddress = bindInterface.Address;
            ArgumentNullException.ThrowIfNull(multicastAddress);
            ArgumentNullException.ThrowIfNull(bindIPAddress);

            if (multicastTimeToLive <= 0)
            {
                throw new ArgumentException("multicastTimeToLive cannot be zero or less.", nameof(multicastTimeToLive));
            }

            if (localPort < 0)
            {
                throw new ArgumentException("localPort cannot be less than zero.", nameof(localPort));
            }

            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            try
            {
                var interfaceIndex = bindInterface.Index;
                var interfaceIndexSwapped = (int)IPAddress.HostToNetworkOrder(interfaceIndex);

                socket.MulticastLoopback = false;
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.PacketInformation, true);
                socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastTimeToLive, multicastTimeToLive);
                socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.MulticastInterface, interfaceIndexSwapped);
                socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(multicastAddress, interfaceIndex));
                socket.Bind(new IPEndPoint(multicastAddress, localPort));

                return socket;
            }
            catch
            {
                socket?.Dispose();

                throw;
            }
        }
    }
}
