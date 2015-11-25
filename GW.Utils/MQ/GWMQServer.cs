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
using System.Threading;

namespace GW.Utils.MQ
{   
    /// <summary>
    /// Mq服务端
    /// </summary>
    public class GWMQServer : IDisposable
    {
        public delegate void GWMQMessageHandler(NetMQSocket socket, NetMQMessage message);
        public event GWMQMessageHandler OnReceiveMessage;
      
        private int _port;
        private NetMQSocket _serverSocket;
        private MQServerType _type;
        private NetMQContext _context;
        private Task task;
        private bool _isStart = false;
           
        public void Init(int port, MQServerType type)
        {
            _type = type;
            _port = port;
            _isStart = true;
            _context = NetMQContext.Create();
            CreateServer();
        }

        public void Dispose()
        {
            _isStart = false;
            _serverSocket.Dispose();
            _context.Dispose();
            if (task != null)
            {
                task.Dispose();
            }
        }

        protected void CreateServer()
        {
            switch (_type)
            {
                case MQServerType.Response:
                    _serverSocket = _context.CreateResponseSocket(); break;
                case MQServerType.Publisher:
                    _serverSocket = _context.CreatePublisherSocket(); break;
                case MQServerType.Router:
                    _serverSocket = _context.CreateRouterSocket(); break;
                case MQServerType.Stream:
                    _serverSocket = _context.CreateStreamSocket(); break;
                case MQServerType.Push:
                    _serverSocket = _context.CreatePushSocket(); break;
                case MQServerType.XPublisher:
                    _serverSocket = _context.CreateXPublisherSocket(); break;
                default:
                    _serverSocket = _context.CreateResponseSocket(); break;
            }

            _serverSocket.Bind("tcp://*:" + _port);
            
           }
        public void StartAsyncReceive()
        {
            task = Task.Factory.StartNew(() =>
                 AsyncRead(_serverSocket), TaskCreationOptions.LongRunning);
        }

        protected void AsyncRead(NetMQSocket serverSocket)
        {
            OnReceiveMessage += new GWMQMessageHandler(GWMQServer_OnReceiveMessage);
            while (_isStart)
            {
                if (serverSocket.HasIn)
                {
                    var message = serverSocket.ReceiveMessage();
                    OnReceiveMessage(serverSocket, message);
                    //Thread.Sleep(1);
                }
            }
        }

        protected virtual void GWMQServer_OnReceiveMessage(NetMQSocket socket, NetMQMessage message)
        {
            //TODO
            throw new NotImplementedException();
        }

        public NetMQSocket Server
        {
            get { return _serverSocket; }
        }

        public void Send(NetMQMessage msg)
        {
            _serverSocket.SendMessage(msg);
        }

        public NetMQMessage CreateMessage()
        {
            return new NetMQMessage();
        }
    }
}
