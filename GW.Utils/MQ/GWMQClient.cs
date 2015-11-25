//系统名      :   通用Web框架
//-----------<类 说 明>-------------------------------------------------------------------------- 
//功能概况    :   NetMQ （ZeroMQ to .Net）消息队列
//编写作者    :   董瑞军
//联系方式    :   rjdong@gw.com.cn
//编写日期    :   2015-01-30
//-----------<修改记录>--------------------------------------------------------------------------
//修改日期  修改者  修改内容
//
//----------------------------------------------------------------------------------------------- 
//Copyright (C) 2015,Shanghai Great Wisdom Co.,Ltd. All Rights Reserved.
//-----------------------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetMQ;
using System.Threading.Tasks;

namespace GW.Utils.MQ
{  
    /// <summary>
    /// MQ客户端
    /// </summary>
    public class GWMQClient : IDisposable
    {
        public delegate void GWMQMessageHandler(NetMQSocket socket, NetMQMessage message);
        public event GWMQMessageHandler OnReceiveMessage;
  
        //public event EventHandler<MQDataEventArgs<NetMQSocket, NetMQMessage>> OnReceive;

        //protected virtual void OnOnReceive(MQDataEventArgs<NetMQSocket, NetMQMessage> e)
        //{
        //    EventHandler<MQDataEventArgs<NetMQSocket, NetMQMessage>> handler = OnReceive;
        //    if (handler != null) handler(this, e);
        //}

        private int _port;
        private NetMQSocket _clientSocket;
        private MQClientType _type;
        private NetMQContext _context;
        private string _ip;
        private Task task;
        bool _isStart = false;

        public void Init(string ip, int port, MQClientType type)
        {
            _type = type;
            _ip = ip;
            _port = port;
            _isStart = true;
            _context = NetMQContext.Create();
            CreateClient();
        }

        public void Dispose()
        {
            _isStart = false;
            _clientSocket.Dispose();
            _context.Dispose();
            if (task != null)
            {
                task.Dispose();
            }
        }

        void CreateClient()
        {
            switch (_type)
            {
                case MQClientType.Request:
                    _clientSocket = _context.CreateRequestSocket();
                    break;
                case MQClientType.Subscriber:
                    _clientSocket = _context.CreateSubscriberSocket();
                    break;
                case MQClientType.Dealer:
                    _clientSocket = _context.CreateDealerSocket();
                    break;
                case MQClientType.Stream:
                    _clientSocket = _context.CreateStreamSocket();
                    break;
                case MQClientType.Pull:
                    _clientSocket = _context.CreatePullSocket();
                    break;
                case MQClientType.XSubscriber:
                    _clientSocket = _context.CreateXSubscriberSocket();
                    break;
                default:
                    _clientSocket = _context.CreateRequestSocket();
                    break;
            }
            _clientSocket.Connect("tcp://" + _ip + ":" + _port);
        }

        public void StartAsyncReceive()
        {
            task = Task.Factory.StartNew(() =>
                AsyncRead(_clientSocket), TaskCreationOptions.LongRunning);
        }

        private void AsyncRead(NetMQSocket socket)
        {
            OnReceiveMessage += new GWMQMessageHandler(GWMQClient_OnReceiveMessage);
            while (_isStart)
            {
                if (socket.HasIn)
                {
                    var message = socket.ReceiveMessage();
                    OnReceiveMessage(socket, message);
                }
            }
        }

        protected virtual void GWMQClient_OnReceiveMessage(NetMQSocket socket, NetMQMessage message)
        {
            //TODO
            throw new NotImplementedException();
        }

        public NetMQSocket Client
        {
            get { return _clientSocket; }
        }

        public T GetClient<T>() where T : NetMQSocket
        {
            return (T)_clientSocket;
        }

        public void Send(NetMQMessage msg)
        {
            _clientSocket.SendMessage(msg);
        }

        public NetMQMessage CreateMessage()
        {
            return new NetMQMessage();
        }

        public NetMQMessage ReceiveMessage()
        {
            return _clientSocket.ReceiveMessage();
        }

       
    }
}
