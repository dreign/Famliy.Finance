using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GW.Weixin.OAuths.Weixinmps.Models;
using GW.Weixin.Helpers;
using System.Collections.Specialized;

namespace Famliy.Finance.Controllers
{
    public class WeixinController : Controller
    {
        // GET: Weixin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            /*
            * 3、获取用户认证地址。(参考Login.aspx文件)
            */
            var oauth = new GW.Weixin.OAuths.Weixinmps.WeixinmpOAuth();
            if (oauth != null)
            {
                oauth.UpdateCache(DateTime.Now.AddHours(1)); //缓存当前协议
                var token = oauth.GetAccessToken();
                if (token.ret == 0)
                {
                    oauth.AccessToken = token.access_token;
                    oauth.ExpiresIn = token.expires_in;
                    oauth.UpdateCache(); //缓存认证信息

                    //var paras = oauth.GetTokenParas();
                    //var json = oauth.ApiByHttpGet("users_show", paras);
                    //var user = UtilHelper.ParseJson<WeixinMUser>(oauth.ApiByHttpPost("user_info", paras));
                    
                    //ViewBag.WeixinUser= user;
                    //Response.Redirect("./");
                }
                else
                {
                   // Response.Write(token.msg + "(" + token.errcode + ")");
                   // Response.Write("<br />" + token.response);
                }
            }
            else
            {
                //Response.Write("登录失败，找不到相对应的接口");
            }
            return View();
        }

        /*
         * 使用方法：
         * 1、实例化应用，my_app为配置文件自定义应用名称，查看Wbm.Weixinmp.config文件
         *    var oauth = GW.Weixin.OAuths.OAuthBase.CreateInstance("weixin");
         *    或
         *    var oath = new GW.Weixin.OAuths.Weixinmps.WeixinmpOAuth();
         *    
         * 
         * 2、验证认证信息缓存
         *     if (oauth!=null && oauth.HasCache){}
         *     
         * 
         * 3、获取认证参数
         *     var paras = oauth.GetTokenParas();
         *     
         * 
         * 4、请求API地址，users_show为配置文件API名称，查看Wbm.OAuthV2.config文件
         *     var json = oauth.ApiByHttpGet("users_show", paras);
         *     
         *     返回类结果
         *     var user = UtilHelper.ParseJson<WeixinMUser>(oauth.ApiByHttpPost("user_info", paras));
         *     string name = user.nickname;
         *     
         *     
         *     验证API是否错误，ret不等于0时发生错误。
         *     if (user.ret == 0){}
         *     或
         *     if (user.error_code == 0){}
         *     
         * 
         * 5、显示结果
         */

        /// <summary>
        /// 发送客服消息
        /// </summary>
        /// <param name="touser">普通用户openid</param>
        /// <param name="content">文本消息内容</param>
        /// <returns></returns>
        public static void MessageCustomSendText(string touser, string content)
        {

            var body = new
            {
                touser = touser,
                msgtype = "text",
                text = new
                {
                    content = content
                }
            };
            try
            {
                MessageCustomSend(UtilHelper.ParseJson(body));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送图片消息
        /// </summary>
        /// <param name="touser">普通用户openid</param>
        /// <param name="media_id">发送的图片的媒体ID</param>
        public static void MessageCustomSendImage(string touser, int media_id)
        {
            var body = new
            {
                touser = touser,
                msgtype = "image",
                image = new
                {
                    media_id = media_id

                }
            };
            try
            {
                MessageCustomSend(UtilHelper.ParseJson(body));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送语音消息
        /// </summary>
        /// <param name="touser">普通用户openid</param>
        /// <param name="media_id">发送的语音的媒体ID</param>
        public static void MessageCustomSendVoice(string touser, int media_id)
        {
            var body = new
            {
                touser = touser,
                msgtype = "voice",
                voice = new
                {
                    media_id = media_id
                }
            };
            try
            {
                MessageCustomSend(UtilHelper.ParseJson(body));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 发送音乐消息
        /// </summary>
        /// <param name="touser">普通用户openid</param>
        /// <param name="musicurl">音乐链接</param>
        /// <param name="hqmusicurl">高品质音乐链接，wifi环境优先使用该链接播放音乐</param>
        /// <param name="thumb_media_id">缩略图的媒体ID</param>
        /// <param name="title">音乐标题</param>
        /// <param name="description">音乐描述</param>
        public static void MessageCustomSendMusic(string touser, int media_id, string musicurl, string hqmusicurl, int thumb_media_id, string title = "", string description = "")
        {
            var body = new
            {
                touser = touser,
                msgtype = "music",
                music = new
                {
                    media_id = media_id,
                    title = title,
                    description = description,
                    musicurl = musicurl,
                    hqmusicurl = hqmusicurl,
                    thumb_media_id = thumb_media_id
                }
            };
            try
            {
                MessageCustomSend(UtilHelper.ParseJson(body));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 发送视频消息
        /// </summary>
        /// <param name="touser">普通用户openid</param>
        /// <param name="media_id">发送的视频的媒体ID</param>
        /// <param name="title">视频消息的标题</param>
        /// <param name="description">视频消息的描述</param>
        public static void MessageCustomSendVideo(string touser, int media_id, string title = "", string description = "")
        {
            var body = new
            {
                touser = touser,
                msgtype = "video",
                video = new
                {
                    media_id = media_id,
                    title = title,
                    description = description
                }
            };
            try
            {
                MessageCustomSend(UtilHelper.ParseJson(body));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送客服消息
        /// </summary>
        /// <param name="body">发送的消息体</param>
        /// <returns></returns>
        public static void MessageCustomSendNews(string touser, object[] articles)
        {
            /*
            var articles = new object[10];
            articles[0] = new
            {
                title = "",
                description = "",
                url = "",
                picurl = ""
            };
             */
            var body = new
            {
                touser = "",
                msgtype = "news",
                news = new
                {
                    articles = articles
                }
            };
            try
            {
                MessageCustomSend(UtilHelper.ParseJson(body));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送客服消息
        /// </summary>
        /// <param name="body">发送的消息体</param>
        /// <returns></returns>
        public static void MessageCustomSend(string body)
        {

            try
            {
                var oauth = GetCurrentOAuth();
                var url = string.Format("{0}?access_token={1}", oauth.ApiUrl("message_custom_send"), oauth.AccessToken);
                string response = oauth.ApiUrlByHttpPost(url, body);
                var data = GW.Weixin.Helpers.UtilHelper.ParseJson<WeixinMError>(response);
                if (data.errcode != 0)
                {
                    throw new Exception(string.Format("{0}:{1}", data.errcode, data.errmsg));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 上传多媒体文件接口
        /// </summary>
        /// <param name="type">媒体文件类型，分别有图片（image）、语音（voice）、视频（video）和缩略图（thumb）</param>
        /// <param name="filename">文件名</param>
        public static WeixinMMedia MediaUpload(string type, string filename)
        {
            try
            {
                var oauth = GetCurrentOAuth();
                NameValueCollection paras = oauth.GetTokenParas();
                paras.Add("type", "image");
                NameValueCollection files = oauth.GetEmptyParas();
                files.Add("media", filename);
                string response = oauth.ApiByHttpPostWithPic("media_upload", paras, files);
                var data = GW.Weixin.Helpers.UtilHelper.ParseJson<WeixinMMedia>(response);
                if (data.errcode != 0)
                {
                    throw new Exception(string.Format("{0}:{1}", data.errcode, data.errmsg));
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 分组管理接口
        /// </summary>
        /// <param name="name">组名</param>
        public static WeixinMGroup GroupsCreate(string name)
        {

            //{"group":{"name":"test"}}
            var body = new
            {
                group = new
                {
                    name = name
                }
            };
            try
            {
                var oauth = GetCurrentOAuth();
                var url = string.Format("{0}?access_token={1}", oauth.ApiUrl("groups_create"), oauth.AccessToken);
                string response = oauth.ApiUrlByHttpPost(url, UtilHelper.ParseJson(body));
                var data = GW.Weixin.Helpers.UtilHelper.ParseJson<WeixinMGroup>(response);
                if (data.errcode != 0)
                {
                    throw new Exception(string.Format("{0}:{1}", data.errcode, data.errmsg));
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询所有分组
        /// </summary>
        public static WeixinMGroupItem[] GroupsGet()
        {
            try
            {
                var oauth = GetCurrentOAuth();
                NameValueCollection paras = oauth.GetTokenParas();
                string response = oauth.ApiByHttpGet("groups_get", paras);
                var data = GW.Weixin.Helpers.UtilHelper.ParseJson<WeixinMGroups>(response);
                if (data.errcode != 0)
                {
                    throw new Exception(string.Format("{0}:{1}", data.errcode, data.errmsg));
                }
                return data.groups;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过用户的OpenID查询其所在的GroupID
        /// </summary>
        public static WeixinMGroupId GroupsGetId(string openid)
        {
            //{"openid":"od8XIjsmk6QdVTETa9jLtGWA6KBc"}
            var body = new { openid = openid };
            try
            {
                var oauth = GetCurrentOAuth();
                var url = string.Format("{0}?access_token={1}", oauth.ApiUrl("groups_getid"), oauth.AccessToken);
                string response = oauth.ApiUrlByHttpPost(url, UtilHelper.ParseJson(body));
                //{"groupid": 102}
                var data = GW.Weixin.Helpers.UtilHelper.ParseJson<WeixinMGroupId>(response);
                if (data.errcode != 0)
                {
                    throw new Exception(string.Format("{0}:{1}", data.errcode, data.errmsg));
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改分组名
        /// </summary>
        public static void GroupsUpdate(string id, string name)
        {
            //{"group":{"id":108,"name":"test2_modify2"}}
            var body = new
            {
                group = new
                {
                    id = id,
                    name = name
                }
            };

            try
            {
                var oauth = GetCurrentOAuth();
                var url = string.Format("{0}?access_token={1}", oauth.ApiUrl("groups_update"), oauth.AccessToken);
                string response = oauth.ApiUrlByHttpPost(url, UtilHelper.ParseJson(body));
                //{"errcode": 0, "errmsg": "ok"}
                var data = GW.Weixin.Helpers.UtilHelper.ParseJson<WeixinMError>(response);
                if (data.errcode != 0)
                {
                    throw new Exception(string.Format("{0}:{1}", data.errcode, data.errmsg));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 移动用户分组
        /// </summary>
        public static void GroupsMembersUpdate(string openid, string to_groupid)
        {
            //{"openid":"oDF3iYx0ro3_7jD4HFRDfrjdCM58","to_groupid":108}
            var body = new
            {
                openid = openid,
                to_groupid = to_groupid
            };
            try
            {
                var oauth = GetCurrentOAuth();
                var url = string.Format("{0}?access_token={1}", oauth.ApiUrl("groups_members_update"), oauth.AccessToken);
                string response = oauth.ApiUrlByHttpPost(url, UtilHelper.ParseJson(body));
                var data = GW.Weixin.Helpers.UtilHelper.ParseJson<WeixinMError>(response);
                if (data.errcode != 0)
                {
                    throw new Exception(string.Format("{0}:{1}", data.errcode, data.errmsg));
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取用户基本信息
        /// </summary>
        public static WeixinMUser UserInfo(string openid)
        {
            try
            {
                var oauth = GetCurrentOAuth();

                NameValueCollection paras = oauth.GetTokenParas();
                paras.Add("openid", openid);
                string response = oauth.ApiByHttpPost("user_info", paras);
                var data = GW.Weixin.Helpers.UtilHelper.ParseJson<WeixinMUser>(response);
                if (data.errcode != 0)
                {
                    throw new Exception(string.Format("{0}:{1}", data.errcode, data.errmsg));
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 获取关注者列表
        /// </summary>
        public static WeixinMOpenIds UserGet(string next_openid)
        {
            try
            {
                var oauth = GetCurrentOAuth();

                NameValueCollection paras = oauth.GetTokenParas();
                //第一个拉取的OPENID，不填默认从头开始拉取
                paras.Add("next_openid", next_openid);
                string response = oauth.ApiByHttpPost("user_get", paras);
                //{"total":2,"count":2,"data":{"openid":["","OPENID1","OPENID2"]},"next_openid":"NEXT_OPENID"}    
                var data = GW.Weixin.Helpers.UtilHelper.ParseJson<WeixinMOpenIds>(response);
                if (data.errcode != 0)
                {
                    throw new Exception(string.Format("{0}:{1}", data.errcode, data.errmsg));
                }
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取当前缓存OAuth
        /// </summary>
        /// <returns></returns>
        public static GW.Weixin.OAuths.Weixinmps.WeixinmpOAuth GetCurrentOAuth()
        {
            try
            {
                var oauth = new GW.Weixin.OAuths.Weixinmps.WeixinmpOAuth();
                if (oauth != null && oauth.HasCache)
                {
                    return oauth;
                }
                else
                {
                    throw new Exception("发送失败，找不到相对应的OAuth缓存或登陆超时");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}