using Famliy.Finance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Famliy.Finance.DAL
{
    public class BankModelInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<BankModel>
    {
        protected override void Seed(BankModel context)
        {
            //context = new BankModel();
            var userList = new List<sys_user>
            {
                new sys_user { user_name="admin@admin.com", password="123", nick_name="God"},
                new sys_user { user_name="dongruijun@gw.com.cn",password="123",nick_name="rjdong"},
               new sys_user { user_name="dreign@qq.com",password="123",nick_name="dreign"}
            };
            userList.ForEach(s => context.sys_users.Add(s));
            context.SaveChanges();
        }
    }
}
