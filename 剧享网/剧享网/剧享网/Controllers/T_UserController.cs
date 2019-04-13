using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using 剧享网.Models;

namespace 剧享网.Controllers
{
    public class T_UserController : Controller
    {
        // GET: T_User
        public ActionResult Index()
        {
            ViewBag.MyHeadImg = GetMyHeadImg();
            return View("Index");
        }

        // GET: T_User/Create
        public ActionResult Create()
        {
            return View();
        }
        //邮箱内容
        public string MailContent(string U_UserEmail)
        {
            Random random = new Random();
            Session["Digital"] = random.Next(1000, 9999);
            MailAddress MessageFrom = new MailAddress("2867763143@qq.com"); //发件人邮箱地址 
            string MessageTo = U_UserEmail; //收件人邮箱地址 
            string MessageSubject = "邮箱验证"; //邮件主题 
            string MessageBody = "您通过剧享网注册了该邮箱，以下是您的验证码：" + Session["Digital"] + "，请输入该验证码进行验证，如果这不是您的操作，请忽略或者删除该邮件。";//邮件内容
            string result = "邮箱不可用";
            if (SendMail(MessageFrom, MessageTo, MessageSubject, MessageBody))
            {
                result = "邮箱可用";
            }
            return result;
        }

        public bool SendMail(MailAddress MessageFrom, string MessageTo, string MessageSubject, string MessageBody)  //发送验证邮件
        {
            MailMessage message = new MailMessage();
            message.To.Add(MessageTo);
            message.From = MessageFrom;
            message.Subject = MessageSubject;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            message.Body = MessageBody;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.IsBodyHtml = true; //是否为html格式 
            message.Priority = MailPriority.High; //发送邮件的优先等级 
            SmtpClient sc = new SmtpClient();
            sc.EnableSsl = true;//是否SSL加密
            sc.Host = "smtp.qq.com"; //指定发送邮件的服务器地址或IP 
            sc.Port = 587; //指定发送邮件端口 
            sc.Credentials = new System.Net.NetworkCredential("2867763143@qq.com", "nqsfhpboxertdgbg"); //指定登录服务器的用户名和密码(注意：这里的密码是开通上面的pop3/smtp服务提供给你的授权密码，不是你的qq密码)
            try
            {
                sc.Send(message); //发送邮件 
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        [HttpPost]
        public JsonResult GetVerificationCode(string U_UserEmail)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                //判断邮箱是否注册
                int judgemail = (from mail in db.T_User where mail.U_Email == U_UserEmail select mail).Count();
                if (judgemail != 0)
                {
                    return Json("该邮箱已被注册，请换邮箱");
                }
                else if (MailContent(U_UserEmail) == "邮箱不可用")
                {
                    return Json("邮箱不可用");
                }
                return Json("验证码已发送到您的邮箱，请获取填写");
            }
        }
        //POST: T_User/Create
        [HttpPost]
        public JsonResult /*string*/ Create(string U_UserName, string U_UserPassWord, string U_UserEmail, string Verification_Code)
        {
            try
            {
                using (剧享网Entities db = new 剧享网Entities())
                {
                    if (Verification_Code != Session["Digital"].ToString())
                    {
                        return Json("验证码错误");
                    }
                    // TODO: Add insert logic here

                    T_User user = new T_User();
                    user.R_Id = "R1";
                    user.U_RoleIsOpen = 1;
                    user.U_HeadImgPath = "dragonball.jpg";//注册时默认一个头像
                    user.U_UserName = U_UserName;
                    user.U_UserPassWord = U_UserPassWord;
                    user.U_UserTelNum = null;
                    user.U_Email = U_UserEmail;
                    user.U_SendSpace = 1024 * 1024 * 50;//默认为50M可使用
                    user.U_RegisterTime = DateTime.Now;
                    //将输入的数据放入表中
                    db.T_User.Add(user);
                    //将放入表中的数据进行保存，完成数据改动实现
                    db.SaveChanges();
                    return Json("注册成功");
                }
            }
            catch (Exception e)
            {
                string message = e.Message;
                return Json(message);
            }
        }

        //设计该用户名称的全局变量是必须的，为了在HTML里面的session能做判断
        string UserName = null;
        public ActionResult Load()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Load(FormCollection collection)
        {
            try
            {
                using (剧享网Entities db = new 剧享网Entities())
                {
                    UserName = collection["U_UserName"];
                    string PassWord = collection["U_UserPassWord"];
                    string Email = collection["U_UserEmail"];
                    //将邮箱存入session使用
                    Session["Email"] = Email;
                    //将密码存入session使用
                    Session["PassWord"] = PassWord;
                    //当用户登录成功后获取用户的角色
                    var GetRole = from exist in db.T_User where exist.U_UserName == UserName && exist.U_UserPassWord == PassWord && exist.U_Email == Email select exist;
                    //获取该用户是否有登录权限
                    int U_RoleIsOpen = (from exist in db.T_User where exist.U_RoleIsOpen == 1 && exist.U_UserName == UserName && exist.U_UserPassWord == PassWord && exist.U_Email == Email select exist).Count();
                    //获取该用户的密码和用户名是否匹配
                    var UserJudge = (from exist in db.T_User where exist.U_UserName == UserName && exist.U_UserPassWord == PassWord && exist.U_Email == Email select exist).Count();
                    if (UserJudge != 1)
                    {
                        return Content("<script>alert('登录失败');history.go(-1);</script>");
                    }
                    else
                    {
                        //获取角色字段
                        string Role = null;
                        foreach (var getrole in GetRole)
                        {
                            Role = getrole.R_Id;
                        }
                        //方法一：(只满足一次HTTP请求，之后会置空)
                        //TempData["Role"] = Role;
                        //方法二 ：
                        Session["Role"] = Role;
                        if (U_RoleIsOpen != 1)
                        {
                            return Content("<script>alert('该用户已被禁止登录');history.go(-1);</script>");
                        }
                        else
                        {
                            //修改状态
                            db.SaveChanges();
                            //通过session来暂时保存用户名的值（默认20Min）
                            Session["UserName"] = UserName;
                            return RedirectToAction("Index");
                        }
                    }
                }
            }
            catch
            {
                return View();
            }
        }
        //GET
        public ActionResult ChangePWD()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ChangePWD(FormCollection collection)
        {
            try
            {
                using (剧享网Entities db = new 剧享网Entities())
                {
                    string Name = Session["UserName"].ToString();
                    string OldPWD = collection["OldPassWord"];
                    string TelNum = collection["U_UserTelNum"];
                    string NewPWD = collection["U_UserPassWord"];
                    //用户名和密码判断是否与数据库匹配合适
                    var JudgeSuit = (from suit in db.T_User where suit.U_UserName == Name && suit.U_UserPassWord == OldPWD && suit.U_UserTelNum == TelNum select suit).Count();
                    if (JudgeSuit != 1)
                    {
                        return Content("<script>alert('用户名或原密码或电话号码不正确');history.go(-1)</script>");
                    }
                    var GetList = from suit in db.T_User where suit.U_UserName == Name && suit.U_UserPassWord == OldPWD select suit;
                    int Id = 0;
                    foreach (var getlist in GetList)
                    {
                        Id = getlist.U_Id;
                    }
                    //获取通过该Id查询的所有相关列
                    var GetFindList = db.T_User.Find(Id);
                    //在该列集合中修改密码
                    GetFindList.U_UserPassWord = NewPWD;
                    //保存即可影响数据库的数据
                    db.SaveChanges();
                    return RedirectToAction("Load");
                }
            }
            catch
            {
                return View();
            }
        }
        public ActionResult FindPWD()
        {
            return View();
        }
        [HttpPost]
        public ActionResult FindPWD(FormCollection collection)
        {
            try
            {
                using (剧享网Entities db = new 剧享网Entities())
                {
                    string Name = collection["U_UserName"];
                    string TelNum = collection["U_UserTelNum"];
                    string NewPWD = collection["U_UserPassWord"];
                    string PwdAgain = collection["passWordAgain"];
                    //用户名和手机号判断是否与数据库匹配合适
                    var JudgeSuit = (from suit in db.T_User where suit.U_UserName == Name && suit.U_UserTelNum == TelNum select suit).Count();
                    if (JudgeSuit != 1)
                    {
                        return Content("<script>alert('用户名或手机号不正确');history.go(-1)</script>");
                    }
                    //判断两次密码是否相等
                    if (PwdAgain != NewPWD)
                    {
                        return Content("<script>alert('两次密码不等，请重新输入！');history.go(-1)</script>");
                    }
                    var GetList = from suit in db.T_User where suit.U_UserName == Name && suit.U_UserTelNum == TelNum select suit;
                    int Id = 0;
                    foreach (var getlist in GetList)
                    {
                        Id = getlist.U_Id;
                    }
                    //获取通过该Id查询的所有相关列
                    var GetFindList = db.T_User.Find(Id);
                    //在该列集合中修改密码
                    GetFindList.U_UserPassWord = NewPWD;
                    //保存即可影响数据库的数据
                    db.SaveChanges();
                    return RedirectToAction("Load");
                }
            }
            catch
            {
                return View();
            }
        }
        public ActionResult LogOut()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LogOut(FormCollection collection)
        {
            try
            {
                using (剧享网Entities db = new 剧享网Entities())
                {
                    string username = (string)Session["UserName"];
                    string password = (string)Session["PassWord"];
                    //通过对应用户名和密码选出对应用户的所有信息
                    var SelectUser = from user in db.T_User where user.U_UserName == username && user.U_UserPassWord == password select user;
                    int U_Id = 0;
                    //获取选出的用户的ID
                    foreach (var user in SelectUser)
                    {
                        U_Id = user.U_Id;
                    }
                    //保存修改记录
                    db.SaveChanges();
                    Session["UserName"] = null;
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        /// <summary>
        /// 获取访问客户端的IPV4地址
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public void GetClientIPv4Address(string EnTime, string QuitTime)
        {
            //try
            //{
            string ipv4 = String.Empty;
            foreach (IPAddress ip in Dns.GetHostAddresses(GetClientIP()))
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }
            if (ipv4 != String.Empty)
            {
                return;
            }
            // 利用 Dns.GetHostEntry 方法，由获取的 IPv6 位址反查 DNS 纪录，
            // 再逐一判断何者为 IPv4 协议，即可转为 IPv4 位址。
            foreach (IPAddress ip in Dns.GetHostEntry(GetClientIP()).AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    ipv4 = ip.ToString();
                    break;
                }
            }
            //调用该方法
            InsertIp(EnTime, QuitTime, ipv4);
            //}
            //catch { }
        }
        //不是静态的话，当要使用这个方法的话，需要对类实例化然后其对象进行调用，而静态的直接类调用
        //节省了一定时间，但是在B/S中static是个类似的session的机制见（慎用）：
        //https://www.cnblogs.com/jianglan/archive/2012/12/06/2804974.html
        //如果一个类中有static类型的变量,那么这个类所有实例都共享这个变量.
        public static string GetClientIP()
        {
            ////获取代理服务器 IP（就是不是本地服务器）
            //if (null == System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"])
            //{
            //    //发出请求的远程主机的IP地址
            //    return System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //}
            //else
            //{
            //    //用户的真实 IP，经过多个代理服务器时
            //    return System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //}
            string requestClientIpAddress = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(requestClientIpAddress))
                requestClientIpAddress = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            if (string.IsNullOrEmpty(requestClientIpAddress))
                requestClientIpAddress = System.Web.HttpContext.Current.Request.UserHostAddress;
            return requestClientIpAddress;
        }
        //向IpReport表插入信息
        public void InsertIp(string EnTime, string QuitTime, string ipv4)
        {
            try
            {
                using (剧享网Entities db = new 剧享网Entities())
                {
                    //当对应Ip进入网页则取出其对应的Id
                    var SelectId = from id in db.T_IpReport where id.I_Ipv4 == ipv4 select id;
                    int Id = 0;
                    foreach (var id in SelectId)
                    {
                        Id = id.I_Id;
                    }
                    //通过Id取出该Ip退出网页的时间
                    var GetTime = from time in db.T_IpReport where time.I_Id == Id select time;
                    //随便赋予的一直时间值，可以不用修改该值了，其目的是获得退出时间
                    DateTime QTime = Convert.ToDateTime("2018-11-21 20:13:50");
                    //获取当前时间
                    DateTime NowTime = DateTime.Now;
                    foreach (var gettime in GetTime)
                    {
                        QTime = (DateTime)gettime.I_QuitTime;
                    }
                    //获取两时间的跨度
                    TimeSpan ts = NowTime - QTime;
                    //通过totalseconds获取跨度的秒数差，还有totaldays等
                    double days = Math.Round(ts.TotalSeconds, 0);
                    //当该Ip距离上次访问间隔7天及以上便可插入当前访问的数据等
                    if (days >= 3)
                    {
                        T_IpReport ipReport = new T_IpReport();
                        ipReport.I_Ipv4 = ipv4;
                        ipReport.C_TypeId = 1;//在该控制器里面，向该表插入的是对主页的访问Ip
                        ipReport.I_EnTime = Convert.ToDateTime(EnTime);
                        ipReport.I_QuitTime = Convert.ToDateTime(QuitTime);
                        db.T_IpReport.Add(ipReport);
                        db.SaveChanges();
                    }
                }
            }
            catch
            { }
        }
        //个人中心界面
        public ActionResult UserCenter()
        {
            ViewBag.MyHeadImg = GetMyHeadImg();
            return View();
        }
        //我的文章
        public ActionResult MyEassy()
        {
            try
            {
                using (剧享网Entities db = new 剧享网Entities())
                {
                    ViewBag.MyHeadImg = GetMyHeadImg();
                    //string UserName = "liu";
                    //string PassWord = "123456";
                    //string Email = "1984713349@qq.com";
                    string UserName = Session["UserName"].ToString();
                    string PassWord = Session["PassWord"].ToString();
                    string Email = Session["Email"].ToString();
                    //获取对应用户的信息
                    var selectinfo = from info in db.T_User where info.U_Email == Email && info.U_UserName == UserName && info.U_UserPassWord == PassWord select info;
                    //获取用户Id
                    int U_Id = 0;
                    foreach (var info in selectinfo)
                    {
                        U_Id = info.U_Id;
                    }
                    //获取用户可发送大小
                    int SendSpace = 0;
                    foreach (var info in selectinfo)
                    {
                        U_Id = info.U_Id;
                        SendSpace = info.U_SendSpace;
                    }
                    var HadSentSize = from size in db.T_SendWork where size.U_Id == U_Id select size;
                    //获取已发送的视频的总大小
                    int EassySize = 0;
                    foreach (var size in HadSentSize)
                    {
                        EassySize += size.S_WorkSize;
                    }
                    if ((ViewBag.EassySize = (SendSpace - EassySize) / (1024 * 1024)) <= 0)
                    {
                        ViewBag.EassySize = 0;//还能使用的MB
                    }
                    //获取作品表里面的该作者的作品数量
                    int SendWorkInfoCount = (from workinfocount in db.T_SendWork where workinfocount.U_UserName == UserName && workinfocount.U_Id == U_Id && workinfocount.W_Id == 1 select workinfocount).Count();
                    //响应前台
                    ViewBag.WorkCount = SendWorkInfoCount;
                    //有作品数量时才进行以下
                    if (SendWorkInfoCount != 0)
                    {
                        //获取作品表里面的信息
                        var selectSendWorkInfo = from workinfo in db.T_SendWork where workinfo.U_UserName == UserName && workinfo.U_Id == U_Id && workinfo.W_Id == 1 select workinfo;
                        //存放作品名（多个,前台用）
                        List<string> WorkName = new List<string>();
                        //存放作品路径（多个,获取图片及文章内容）
                        List<string> WorkPath = new List<string>();
                        //存放作品发布时间（多个，前台用）
                        List<string> WorkTime = new List<string>();
                        foreach (var info in selectSendWorkInfo)
                        {
                            WorkName.Add(info.S_WorkName);
                            WorkPath.Add(info.S_WorkPath);
                            WorkTime.Add(info.S_SendTime);
                        }
                        //存放图片的路径（多个,前台用）
                        List<string> ImgPath = new List<string>();
                        //存放文章部分内容（多个,前台用）
                        List<string> EassyContent = new List<string>();
                        for (int i = 0; i < SendWorkInfoCount; ++i)
                        {
                            //获取文章图片的名字
                            string eassyimgName = null;
                            string Example = WorkPath[i];
                            for (int j = 10; j < WorkPath[i].Length; ++j)
                            {
                                eassyimgName += Example[j];
                            }
                            FileStream fsEassy = new FileStream(Server.MapPath(WorkPath[i] + "\\" + eassyimgName + ".txt"), FileMode.Open, FileAccess.Read);
                            StreamReader readerEassy = new StreamReader(fsEassy);
                            string eassycontent = readerEassy.ReadToEnd();
                            string partialEassyContent = null;
                            if (eassycontent.Length > 10)
                            {
                                for (int m = 0; m < 10; ++m)
                                {
                                    partialEassyContent += eassycontent[m];
                                }
                            }
                            else
                            {
                                partialEassyContent = eassycontent;
                            }
                            EassyContent.Add(partialEassyContent + "..");
                            //根据相对路径，读取所有文件，并且不查找子文件夹。之后筛选格式是图片的文件
                            var strs = System.IO.Directory.GetFiles(Server.MapPath(WorkPath[i]), "*.*", System.IO.SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".jpg") || s.EndsWith(".gif") || s.EndsWith(".bmp") || s.EndsWith(".png"));
                            foreach (string imgpath in strs)
                            {
                                FileInfo fileInfo = new FileInfo(imgpath);
                                //记住一定不能是~/MyEassy/，我们要相对路径
                                ImgPath.Add("/MyEassy/" + eassyimgName + "/" + fileInfo.Name);
                            }
                        }
                        ViewBag.WorkName = WorkName;
                        ViewBag.ImgPath = ImgPath;
                        ViewBag.WorkTime = WorkTime;
                        ViewBag.EassyContent = EassyContent;
                    }
                }
                return View();
            }
            catch { return View("/Shared/Error/"); }
        }

        //我的个人信息
        public ActionResult MyInfo()
        {
            ViewBag.MyHeadImg = GetMyHeadImg();
            return View();
        }
        [HttpPost]
        public JsonResult MyInfo(FormCollection collection)
        {
            try
            {
                string UpdateName = collection["Name"];
                string UpdateTel = collection["Phone"];
                string Verification_Code = collection["Verification_Code"];
                string qqEmail = collection["qqEmail"];
                //返回的图片的字符串以及返回的错误异常
                string pic = "", error = "", result = "";
                using (剧享网Entities db = new 剧享网Entities())
                {
                    string Name = Session["UserName"].ToString();
                    string PWD = Session["PassWord"].ToString();
                    string Email = Session["Email"].ToString();
                    //string Name = "liu";
                    //string PWD = "123456";
                    //string Email = "1984713349@qq.com";
                    //选出当前用户
                    var GetUserInfo = from info in db.T_User where info.U_Email == Email && info.U_UserName == Name && info.U_UserPassWord == PWD select info;
                    //获取当前用户的头像路径
                    string headimgpath = null;
                    int id = 0;
                    foreach (var info in GetUserInfo)
                    {
                        headimgpath = info.U_HeadImgPath;
                        id = info.U_Id;
                    }
                    //找到当前用户的信息，准备修改
                    var ModifyInfo = db.T_User.Find(id);
                    //获取前台的文件信息
                    HttpFileCollectionBase files = Request.Files;
                    //获取文件中图片的信息
                    HttpPostedFileBase file = files["img"];
                    try
                    {
                        //获取图片的所有信息
                        string fileInfo = System.IO.Path.GetFileName(file.FileName);
                        //只获取名字没有扩展名
                        string fileonlyname = System.IO.Path.GetFileNameWithoutExtension(fileInfo);
                        //获取图片的扩展名
                        string fileExtension = System.IO.Path.GetExtension(fileInfo);
                        //自定义图片名
                        string fileName = Name + PWD + Email + fileonlyname;
                        //存放图片到指定的文件中
                        string filePhysicalPath = Server.MapPath("~/head_img/" + fileName + fileExtension);
                        //保存存片到指定文件位置
                        file.SaveAs(filePhysicalPath);
                        //修改存入头像的路径
                        ModifyInfo.U_HeadImgPath = fileName + fileExtension;
                        db.SaveChanges();
                        //返回图片名，供前台使用
                        pic = "/head_img/" + fileName + fileExtension;
                    }
                    catch (Exception ex)
                    {
                        error = ex.Message;
                    }

                    var GetList = from suit in db.T_User where suit.U_UserName == Name && suit.U_UserPassWord == PWD && suit.U_Email == Email select suit;
                    int Id = 0;
                    foreach (var getlist in GetList)
                    {
                        Id = getlist.U_Id;
                    }
                    //获取通过该Id查询的所有相关列
                    var GetFindList = db.T_User.Find(Id);
                    //没填写用户名或者电话则不修改
                    if (UpdateName != null && UpdateName != "")
                    {
                        GetFindList.U_UserName = UpdateName;
                        //更新当前session内容，确保当前用户能使用其余功能
                        Session["UserName"] = UpdateName;
                        result = "OK";
                    }
                    if (UpdateTel != null && UpdateTel != "")
                    {
                        GetFindList.U_UserTelNum = UpdateTel;
                        result = "OK";
                    }
                    //邮箱修改
                    if (Verification_Code == Session["Digital"].ToString())
                    {
                        GetFindList.U_Email = qqEmail;
                        Session["Email"] = qqEmail;
                        result = "OK";
                    }

                    //保存即可影响数据库的数据
                    db.SaveChanges();
                }
                return Json(new
                {
                    pic = pic,
                    result = result,
                    error = error
                });
            }
            catch
            {
                return Json(new
                {
                    pic = "",
                    result = "",
                });
            }
        }

        //我的视频
        public ActionResult MyVideo()
        {
            try
            {
                using (剧享网Entities db = new 剧享网Entities())
                {
                    ViewBag.MyHeadImg = GetMyHeadImg();
                    //string UserName = "liu";
                    //string PassWord = "123456";
                    //string Email = "1984713349@qq.com";
                    string UserName = Session["UserName"].ToString();
                    string PassWord = Session["PassWord"].ToString();
                    string Email = Session["Email"].ToString();
                    //获取对应用户的信息
                    var selectinfo = from info in db.T_User where info.U_Email == Email && info.U_UserName == UserName && info.U_UserPassWord == PassWord select info;
                    //获取用户Id
                    int U_Id = 0;
                    //获取用户可发送大小
                    int SendSpace = 0;
                    foreach (var info in selectinfo)
                    {
                        U_Id = info.U_Id;
                        SendSpace = info.U_SendSpace;
                    }
                    var HadSentSize = from size in db.T_SendWork where size.U_Id == U_Id select size;
                    //获取已发送的总大小
                    int VideoSize = 0;
                    foreach (var size in HadSentSize)
                    {
                        VideoSize += size.S_WorkSize;
                    }
                    if ((ViewBag.VideoSize = (SendSpace - VideoSize) / (1024 * 1024)) <= 0)
                    {
                        ViewBag.VideoSize = 0;//还能使用的MB
                    }
                    //获取作品表里面的该作者的作品数量
                    int SendWorkInfoCount = (from workinfocount in db.T_SendWork where workinfocount.U_UserName == UserName && workinfocount.U_Id == U_Id && workinfocount.W_Id == 2 select workinfocount).Count();
                    //响应前台
                    ViewBag.WorkCount = SendWorkInfoCount;
                    //有作品数量时才进行以下
                    if (SendWorkInfoCount != 0)
                    {
                        //获取作品表里面的信息
                        var selectSendWorkInfo = from workinfo in db.T_SendWork where workinfo.U_UserName == UserName && workinfo.U_Id == U_Id && workinfo.W_Id == 2 select workinfo;
                        //存放作品名（多个,前台用）
                        List<string> WorkName = new List<string>();
                        //存放作品路径（多个,获取图片及文章内容）
                        List<string> WorkPath = new List<string>();
                        //存放作品发布时间（多个，前台用）
                        List<string> WorkTime = new List<string>();
                        foreach (var info in selectSendWorkInfo)
                        {
                            WorkName.Add(info.S_WorkName);
                            WorkPath.Add(info.S_WorkPath);
                            WorkTime.Add(info.S_SendTime);
                        }
                        //存放图片的路径（多个,前台用）
                        List<string> ImgPath = new List<string>();
                        //存放视频介绍的部分内容（多个,前台用）
                        List<string> PartialVideoContent = new List<string>();
                        for (int i = 0; i < SendWorkInfoCount; ++i)
                        {
                            //获取视频图片的名字
                            string videoimgName = null;
                            //取出当前文件夹的路径
                            string Example = WorkPath[i];
                            for (int j = 10; j < WorkPath[i].Length; ++j)
                            {
                                videoimgName += Example[j];
                            }
                            //读取视频介绍内容
                            FileStream fsVideo = new FileStream(Server.MapPath(WorkPath[i] + "\\" + videoimgName + ".txt"), FileMode.Open, FileAccess.Read);
                            StreamReader readerVideo = new StreamReader(fsVideo);
                            string videocontent = readerVideo.ReadToEnd();
                            string partialVideoContent = null;
                            //取出介绍内容的前10个字符
                            if (videocontent.Length > 10)
                            {
                                for (int m = 0; m < 10; ++m)
                                {
                                    partialVideoContent += videocontent[m];
                                }
                            }
                            else
                            {
                                partialVideoContent = videocontent;
                            }
                            PartialVideoContent.Add(partialVideoContent + "..");
                            //根据相对路径，读取所有文件，并且不查找子文件夹。之后筛选格式是图片的文件
                            var getImg = System.IO.Directory.GetFiles(Server.MapPath(WorkPath[i]), "*.*", System.IO.SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".jpg") || s.EndsWith(".gif") || s.EndsWith(".bmp") || s.EndsWith(".png"));
                            foreach (string imgpath in getImg)
                            {
                                FileInfo fileInfo = new FileInfo(imgpath);
                                //记住一定不能是~/MyEassy/，我们要相对路径
                                ImgPath.Add("/MyVideo/" + videoimgName + "/" + fileInfo.Name);
                            }
                        }
                        ViewBag.WorkName = WorkName;
                        ViewBag.ImgPath = ImgPath;
                        ViewBag.WorkTime = WorkTime;
                        ViewBag.VideoContent = PartialVideoContent;
                    }
                }
                return View();
            }
            catch { return View("/Shared/Error/"); }
        }

        //关注信息
        public ActionResult FollowInfo()
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                string CurrentUserName = Session["UserName"].ToString();
                string Email = Session["Email"].ToString();
                string PassWord = Session["PassWord"].ToString();
                //string CurrentUserName = "li";
                //string Email = "1984713345@qq.com";
                //string PassWord = "123456";
                ViewBag.MyHeadImg = GetMyHeadImg();
                //获取当前用户的Id/
                var selectId = from info in db.T_User where info.U_UserName == CurrentUserName && info.U_UserPassWord == PassWord && info.U_Email == Email select info;
                int CurrentUserId = 0;
                foreach (var info in selectId)
                {
                    CurrentUserId = info.U_Id;
                }
                //找当前用户的粉丝数量
                int selectCurrentUserFansCount = (from fansCount in db.T_FollowUsers where fansCount.F_BeFollowerId == CurrentUserId select fansCount).Count();
                //找当前用户的关注人的数量
                int selectCurrentUserFollowersCount = (from FollowersCount in db.T_FollowUsers where FollowersCount.F_FollowerId == CurrentUserId select FollowersCount).Count();
                ViewBag.FansCount = selectCurrentUserFansCount;
                ViewBag.FollowersCount = selectCurrentUserFollowersCount;
            }
            return View();
        }
        [HttpPost]
        public JsonResult FollowInfo(string ShowStatus)
        {
            string CurrentUserName = Session["UserName"].ToString();
            string Email = Session["Email"].ToString();
            string PassWord = Session["PassWord"].ToString();
            //string CurrentUserName = "li";
            //string Email = "1984713345@qq.com";
            //string PassWord = "123456";
            using (剧享网Entities db = new 剧享网Entities())
            {
                //获取当前用户的Id/
                var selectId = from info in db.T_User where info.U_UserName == CurrentUserName && info.U_UserPassWord == PassWord && info.U_Email == Email select info;
                int CurrentUserId = 0;
                foreach (var info in selectId)
                {
                    CurrentUserId = info.U_Id;
                }
                //如果点击的是显示粉丝
                if (ShowStatus == "ShowMyFans")
                {
                    //找当前用户的粉丝
                    var selectCurrentUserFans = from fans in db.T_FollowUsers where fans.F_BeFollowerId == CurrentUserId select fans;
                    //存放粉丝的ID
                    List<int> FansId = new List<int>();
                    foreach (var id in selectCurrentUserFans)
                    {
                        FansId.Add(id.F_FollowerId);
                    }
                    //没有粉丝
                    if (FansId.Count == 0)
                    {
                        return Json("None");
                    }
                    //存放粉丝的信息
                    List<string> FansInfo = new List<string>();
                    //通过id获取粉丝的信息
                    foreach (int GetFansInfoById in FansId)
                    {
                        //先获取用户表的信息
                        var fansbaseinfo = from info in db.T_User where info.U_Id == GetFansInfoById select info;
                        foreach (var info in fansbaseinfo)
                        {
                            FansInfo.Add(info.U_HeadImgPath);
                            FansInfo.Add(info.U_UserName);
                            FansInfo.Add(info.R_Id);
                            FansInfo.Add(info.U_Email);
                        }
                    }
                    //先把数组序列化放到前端
                    string serializedInfo = JsonConvert.SerializeObject(FansInfo);
                    return Json(serializedInfo);
                }
                //如果点击的是显示我的关注
                else
                {
                    //找当前用户的关注人
                    var selectCurrentUserFollowers = from followers in db.T_FollowUsers where followers.F_FollowerId == CurrentUserId select followers;
                    //存放关注人的ID
                    List<int> BeFollowersId = new List<int>();
                    foreach (var id in selectCurrentUserFollowers)
                    {
                        BeFollowersId.Add(id.F_BeFollowerId);
                    }
                    //没有关注任何用户
                    if (BeFollowersId.Count == 0)
                    {
                        return Json("None");
                    }
                    //存放关注人的信息
                    List<string> FollowersInfo = new List<string>();
                    //通过id获取关注人的信息
                    foreach (int GetFansInfoById in BeFollowersId)
                    {
                        //先获取用户表的信息
                        var followersbaseinfo = from info in db.T_User where info.U_Id == GetFansInfoById select info;
                        foreach (var info in followersbaseinfo)
                        {
                            FollowersInfo.Add(info.U_HeadImgPath);
                            FollowersInfo.Add(info.U_UserName);
                            FollowersInfo.Add(info.R_Id);
                            FollowersInfo.Add(info.U_Email);
                        }
                    }
                    //先把数组序列化放到前端
                    string serializedInfo = JsonConvert.SerializeObject(FollowersInfo);
                    return Json(serializedInfo);
                }
            }
        }

        //用户中心每个页面加载对应用户的头像
        public string GetMyHeadImg()
        {
            try
            {
                using (剧享网Entities db = new 剧享网Entities())
                {
                    string Name = Session["UserName"].ToString();
                    string PWD = Session["PassWord"].ToString();
                    string Email = Session["Email"].ToString();
                    //选出当前用户
                    var GetUserInfo = from info in db.T_User where info.U_Email == Email && info.U_UserName == Name && info.U_UserPassWord == PWD select info;
                    //获取当前用户的头像路径
                    string headimgpath = null;
                    foreach (var info in GetUserInfo)
                    {
                        headimgpath = info.U_HeadImgPath;
                    }
                    var img = System.IO.Directory.GetFiles(Server.MapPath("~/head_img/"), "*.*", SearchOption.TopDirectoryOnly).Where(type => type.EndsWith(".jpg") || type.EndsWith(".png") || type.EndsWith(".gif") || type.EndsWith(".bmp"));
                    string MyHeadImg = null;
                    foreach (var imginfo in img)
                    {
                        FileInfo fi = new FileInfo(imginfo);
                        if (fi.Name == headimgpath)
                        {
                            MyHeadImg = "/head_img/" + fi.Name;
                            break;
                        }
                    }
                    return MyHeadImg;
                }
            }
            catch { return "/ALL_IMAGE/Gao_Fu/h1.png"; }
        }
    }
}
