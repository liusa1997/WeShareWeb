﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Mvc;
using 剧享网.Models;

namespace 剧享网.Controllers
{
    public class EassyOperationController : Controller
    {
        //发布文章
        public ActionResult SendMyEassy()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SendMyEassy(FormCollection collection)
        {
            string UserName = Session["UserName"].ToString();
            string PassWord = Session["PassWord"].ToString();
            string Email = Session["Email"].ToString();
            //string UserName = "liu";
            //string PassWord = "123456";
            //string Email = "1984713349@qq.com";
            using (剧享网Entities db = new 剧享网Entities())
            {
                //判断该功能权限是否关闭
                var JudgePermission = from judgepermission in db.T_Function where judgepermission.F_SubId == "3F1" && judgepermission.F_Function == "SendFunction" && judgepermission.F_FatherId == "2F3" select judgepermission;
                int F_IsOpen = 1;
                foreach (var open in JudgePermission)
                {
                    F_IsOpen = open.F_IsOpen;
                }
                if (F_IsOpen == 0)
                {
                    return Json("Close");
                }
                //获取对应用户的信息
                var selectinfo = from info in db.T_User where info.U_Email == Email && info.U_UserName == UserName && info.U_UserPassWord == PassWord select info;
                //获取用户Id
                int U_Id = 0;
                foreach (var info in selectinfo)
                {
                    U_Id = info.U_Id;
                }

                //
                string EassyName = collection["EassyName"].Trim();
                //
                string EassyContent = collection["EassyContent"]; 
                string NowTime = Convert.ToString(DateTime.Now);
                //
                string EassySendTime = null;
                foreach (char signal in NowTime)
                {
                    if (signal != ':' && signal != '/')
                    {
                        EassySendTime += signal;
                    }
                }
                //
                string EassyTxtName = UserName + "文章" + EassyName + EassySendTime;
                //
                string EassyImgName = UserName + "文章" + EassyName + EassySendTime;
                //
                string FatherFilePath = "~/MyEassy/" + UserName + "文章" + EassyName + EassySendTime;
                //获取前台的文件信息
                HttpFileCollectionBase httpFile = Request.Files;
                //获取文件中图片的信息,这个参数是对应前台的input的name值
                HttpPostedFileBase fileBase = httpFile["img"];

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
                if ((SendSpace - VideoSize) <= fileBase.ContentLength )
                {
                    return Json("Fail");//空间不够发送失败
                }

                try
                {
                    string imginfo = System.IO.Path.GetFileName(fileBase.FileName);
                    string imgextension = System.IO.Path.GetExtension(imginfo);
                    EassyImgName += imgextension;
                    //如果用户的文章文件夹不存在，则创建
                    if (!Directory.Exists(Server.MapPath(FatherFilePath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(FatherFilePath));
                    }
                    //
                    FileStream fs = new FileStream(Server.MapPath(FatherFilePath+ "\\" + EassyTxtName + ".txt"), FileMode.Create, FileAccess.ReadWrite);
                    StreamWriter mysw = new StreamWriter(fs);
                    //只存入文章的内容
                    mysw.Write(EassyContent);
                    mysw.Close();
                    fs.Close();
                    //
                    string imgPath = Server.MapPath(FatherFilePath+"\\"+ EassyImgName);
                    fileBase.SaveAs(imgPath);

                    //将发布的文章信息保存到数据库
                    T_SendWork sendWork = new T_SendWork();
                    sendWork.U_UserName = UserName;
                    sendWork.U_Id = U_Id;
                    sendWork.W_Id = 1;
                    sendWork.S_WorkName = EassyName;
                    sendWork.S_WorkPath = FatherFilePath;
                    sendWork.S_WorkSize = fileBase.ContentLength;//给发布的文章大小赋值
                    sendWork.S_SendTime = NowTime;
                    db.T_SendWork.Add(sendWork);
                    db.SaveChanges();
                    return Json("OK");
                }
                catch { return Json(""); }
            }
        }

        //修改文章
        public ActionResult ModiftMyEassy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ModiftMyEassy(FormCollection collection)
        {
            return View();
        }

        //打开文章
        public ActionResult OpenMyEassy(string AuthorName, string W_Id, string WorkName, string WorkTime)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                //AuthorName = "liu";
                //W_Id = "1";
                //WorkName = "你好";
                //WorkTime = "2018/12/14 22:10:00";
                int w_id = Convert.ToInt32(W_Id);
                剧享网.Comment_cs.FatherNode father = new Comment_cs.FatherNode();
                List<T_Comment> FatherNode = new List<T_Comment>();
                FatherNode = father.FatherInfo(AuthorName, w_id, WorkName, WorkTime);
                ViewBag.FatherNode = FatherNode;

                TestController test = new TestController();
                List<int> list = new List<int>();
                list = test.WebInitGetAgreeCount(AuthorName, w_id, WorkName, WorkTime);
                ViewBag.List = list;

                //获取文章内容
                var GetCurrentInfo = from currentinfo in db.T_SendWork where currentinfo.U_UserName == AuthorName && currentinfo.W_Id == w_id && currentinfo.S_WorkName == WorkName && currentinfo.S_SendTime == WorkTime select currentinfo;
                ViewBag.Title = "MyEassy";
                ViewBag.EassyName = WorkName;
                ViewBag.Author = AuthorName;
                ViewBag.EassySendTime = WorkTime;
                string WorkPath = null;
                foreach (var info in GetCurrentInfo)
                {
                    WorkPath = info.S_WorkPath;
                }
                string EassyName = null;
                for (int i = 10; i < WorkPath.Length; ++i)
                {
                    EassyName += WorkPath[i];
                }
                FileStream fs = new FileStream(Server.MapPath(WorkPath + "\\" + EassyName + ".txt"), FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(fs);
                ViewBag.EassyContent = reader.ReadToEnd();

            }
            return View();
        }
        [HttpPost]
        public ActionResult OpenMyEassy(FormCollection collection)
        {
            
            return View();
        }
    }
}
