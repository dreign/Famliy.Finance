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
using NetMQ.zmq;

namespace GW.Utils.MQ
{     
    public enum MQServerType
    {
        Response = ZmqSocketType.Rep,
        Publisher = ZmqSocketType.Pub,
        Router = ZmqSocketType.Router,
        Stream = ZmqSocketType.Stream,
        Push = ZmqSocketType.Push,
        XPublisher = ZmqSocketType.Xpub
    }

    public enum MQClientType
    {
        Request = ZmqSocketType.Req,
        Subscriber = ZmqSocketType.Sub,
        Dealer = ZmqSocketType.Dealer,
        Stream = ZmqSocketType.Stream,
        Pull = ZmqSocketType.Pull,
        XSubscriber = ZmqSocketType.Xsub
    }
      
 
    //public class MQDataEventArgs<NetMQSocket, NetMQMessage> : EventArgs
    //    where NetMQSocket : new()
    //    where NetMQMessage : new() { }

    //public class MQDataEventArgs2 : EventArgs
    //{
    //    public NetMQSocket Socket;
    //    public NetMQMessage Message;
    //}
}
