using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using GW.Mail.Entity;

namespace GW.Mail
{
    public class MailQueueManage
    {
        //默认5分钟
        private int _interval = 1000 * 60 * 5;
        private static Timer _timer;
        public static Queue<MailInfo> Q_MailInfo = new Queue<MailInfo>();
        private object _obj = new object();
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="Interval">定时处理时间，单位毫秒</param>
        public MailQueueManage(int Interval)
        {
            _interval = Interval;
        }
        public void Run()
        {
            _timer = new Timer(_interval);
            _timer.Enabled = true;
            _timer.Elapsed += new ElapsedEventHandler(doTimer);
            _timer.Start();
        }
        private void doTimer(object source, ElapsedEventArgs e)
        {
            if (Q_MailInfo.Count>0)
            {
                lock (this._obj)
                {
                    MailInfo item=Q_MailInfo.Dequeue();
                    while (Q_MailInfo.Count>0)
                    {
                        MailManager.Send(item,false);
                        item = Q_MailInfo.Dequeue();
                    }
                }
               
            }
        }
    }
}
