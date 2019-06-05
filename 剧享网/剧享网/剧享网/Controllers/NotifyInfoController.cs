using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 剧享网.Models;

namespace 剧享网.Controllers
{
    public class NotifyInfoController : Controller
    {
        // GET: NotifyInfo
        public ActionResult NotifyIndex()
        {
            try
            {
                using (剧享网Entities db = new 剧享网Entities())
                {
                    //Session["UserName"] = "li";
                    string UserName = Session["UserName"].ToString();
                    //选出系统对自己所有未读通知(包括关注通知)
                    var SelectSystemInfo = from sysinfo in db.T_NotifyInfo where sysinfo.N_ReplyObject == UserName && sysinfo.N_ReadStatus == 0 && sysinfo.W_Id == 0 select sysinfo;
                    string Time1 = null;
                    var SysList = new List<string>();
                    foreach (var sysinfo in SelectSystemInfo)
                    {
                        Time1 = Convert.ToString(sysinfo.N_Time);
                        SysList.Add(sysinfo.N_ReplyUserName);
                        SysList.Add(sysinfo.N_SystemContent);
                        SysList.Add(Time1);
                        ViewBag.SysList = SysList;
                    }
                    //选出用户对自己动态响应的所有未读通知
                    var SelectResponseInfo = from resinfo in db.T_NotifyInfo where resinfo.N_ReplyObject == UserName && resinfo.N_ReadStatus == 0 && resinfo.W_Id != 0 select resinfo;
                    string Time2 = null;
                    string str1 = "回复了你的";
                    var ResList = new List<string>();
                    foreach (var resinfo in SelectResponseInfo)
                    {
                        Time2 = Convert.ToString(resinfo.N_Time);
                        ResList.Add(resinfo.N_ReplyUserName);
                        ResList.Add(str1);
                        if (resinfo.W_Id == 1)
                        {
                            ResList.Add("文章");
                            ResList.Add("《" + resinfo.N_WorkName + "》");
                        }
                        else if (resinfo.W_Id == 2)
                        {
                            ResList.Add("视频");
                            ResList.Add("《" + resinfo.N_WorkName + "》");
                        }
                        else if (resinfo.W_Id == 3)
                        {
                            ResList.Add("评论");
                            ResList.Add("“" + resinfo.N_SystemContent + "”");
                        }
                        ResList.Add("：");
                        ResList.Add(resinfo.N_SystemContent);
                        ResList.Add(Time2);
                        ViewBag.ResList = ResList;
                    }
                    //选出所有已读的系统通知
                    var SelectReadInfo = from readinfo in db.T_NotifyInfo where readinfo.N_ReplyObject == UserName && readinfo.N_ReadStatus == 1 && readinfo.W_Id == 0 select readinfo;
                    var SysReadList = new List<string>();
                    foreach (var sysreadinfo in SelectReadInfo)
                    {
                        Time1 = Convert.ToString(sysreadinfo.N_Time);
                        SysReadList.Add(sysreadinfo.N_ReplyUserName);
                        SysReadList.Add(sysreadinfo.N_SystemContent);
                        SysReadList.Add(Time1);
                        ViewBag.SysReadList = SysReadList;
                    }
                    //选出所有已读的动态通知
                    var SelectResponseReadInfo = from resreadinfo in db.T_NotifyInfo where resreadinfo.N_ReplyObject == UserName && resreadinfo.N_ReplyObject == UserName && resreadinfo.N_ReadStatus == 1 && resreadinfo.W_Id != 0 select resreadinfo;
                    var ResReadList = new List<string>();
                    foreach (var resreadinfo in SelectResponseReadInfo)
                    {
                        Time2 = Convert.ToString(resreadinfo.N_Time);
                        ResReadList.Add(resreadinfo.N_ReplyUserName);
                        ResReadList.Add(str1);
                        if (resreadinfo.W_Id == 1)
                        {
                            ResReadList.Add("文章");
                            ResReadList.Add("《" + resreadinfo.N_WorkName + "》");
                        }
                        else if (resreadinfo.W_Id == 2)
                        {
                            ResReadList.Add("视频");
                            ResReadList.Add("《" + resreadinfo.N_WorkName + "》");
                        }
                        else if (resreadinfo.W_Id == 3)
                        {
                            ResReadList.Add("评论");
                            ResReadList.Add("“" + resreadinfo.N_SystemContent + "”");
                        }
                        ResReadList.Add("：");
                        ResReadList.Add(resreadinfo.N_SystemContent);
                        ResReadList.Add(Time2);
                        ViewBag.ResReadList = ResReadList;
                    }
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        //页面加载成功就把状态修改为已读（1）
        public void UpdateStatus()
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                if (Session["UserName"] == null)
                {
                    return;
                }
                string UserName = Session["UserName"].ToString();
                //选出对自己所有通知
                var SelectInfo = from info in db.T_NotifyInfo where info.N_ReplyObject == UserName && info.N_ReadStatus ==0 select info;
                int Id = 0;
                foreach (var info in SelectInfo)
                {
                    Id = info.N_Id;
                    var UpdateStatus = db.T_NotifyInfo.Find(Id);
                    UpdateStatus.N_ReadStatus = 1;
                }
                db.SaveChanges();
            }
        }

        [HttpPost]
        //计算对自己所有的未读通知数量
        public int InfoCount()
        {
            try
            {
                using (剧享网Entities db = new 剧享网Entities())
                {
                    string UserName = Session["UserName"].ToString();
                    //计算对自己所有的未读通知数量
                    int UnreadInfoCount = (from info in db.T_NotifyInfo where info.N_ReplyObject == UserName && info.N_ReadStatus == 0 select info).Count();
                    return UnreadInfoCount;
                }
            }
            catch
            { return 0; }
        }

        //删除通知信息
        public void DeleteNotifyInfo(string ReplyUserName,string SystemContent,string Time)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                //获取当前的用户名
                string UserName = Session["UserName"].ToString();
                //获取想删除信息的ID
                var getId = from info in db.T_NotifyInfo where info.N_ReplyUserName == ReplyUserName && info.N_SystemContent == SystemContent && info.N_Time == Time && info.N_ReplyObject == UserName select info;
                int DeleteId = 0;
                foreach (var info in getId)
                {
                    DeleteId = info.N_Id;
                }
                //根据id查找信息
                var FindInfo = db.T_NotifyInfo.Find(DeleteId);
                //删除查找的信息
                db.T_NotifyInfo.Remove(FindInfo);
                db.SaveChanges();
            }
        }
    }
}










