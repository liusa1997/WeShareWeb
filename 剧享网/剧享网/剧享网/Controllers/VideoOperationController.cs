using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 剧享网.Models;

namespace 剧享网.Controllers
{
    public class VideoOperationController : Controller
    {
        public ActionResult SendVideo()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SendVideo(FormCollection collection)
        {
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

                string UserName = Session["UserName"].ToString();
                string PassWord = Session["PassWord"].ToString();
                string Email = Session["Email"].ToString();
                //string UserName = "liu";
                //string PassWord = "123456";
                //string Email = "1984713349@qq.com";
                string NowTime = Convert.ToString(DateTime.Now);
                //视频发送时间
                string VideoSendTime = null;

                //获取对应用户的信息
                var selectinfo = from info in db.T_User where info.U_Email == Email && info.U_UserName == UserName && info.U_UserPassWord == PassWord select info;
                //获取用户Id
                int U_Id = 0;
                foreach (char signal in NowTime)
                {
                    if (signal != ':' && signal != '/')
                    {
                        VideoSendTime += signal;
                    }
                }
                string VideoName = collection["VideoName"];
                string VideoIntroduction = collection["VideoIntroduction"];
                HttpFileCollectionBase hfcb = Request.Files;
                HttpPostedFileBase hpfbVideo = hfcb["video"];
                HttpPostedFileBase hpfbImg = hfcb["img"];
                string DirectoryPath = UserName + "视频" + VideoName + VideoSendTime;

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
                if ((SendSpace - VideoSize) <= (hpfbVideo.ContentLength + hpfbImg.ContentLength))
                {
                    return Json("Fail");//空间不够发送失败
                }

                try
                {
                    //获取视频名称
                    string videoinfo = System.IO.Path.GetFileName(hpfbVideo.FileName);
                    string getVideoExtension = System.IO.Path.GetExtension(videoinfo);
                    string VideoFileName = DirectoryPath + getVideoExtension;
                    //获取图片名称
                    string imginfo = System.IO.Path.GetFileName(hpfbImg.FileName);
                    string getImgExtension = System.IO.Path.GetExtension(imginfo);
                    string ImgFileName = DirectoryPath + getImgExtension;
                    if ((hpfbVideo.ContentLength + hpfbImg.ContentLength) > (1024 * 1024 * 30))
                    {
                        return Json("30M");
                    }
                    //创建父文件夹
                    if (!System.IO.Directory.Exists(Server.MapPath("~/MyVideo/" + DirectoryPath)))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("~/MyVideo/" + DirectoryPath));
                    }
                    //写入视频介绍内容
                    FileStream fs = new FileStream(Server.MapPath("~/MyVideo/" + DirectoryPath + "\\" + DirectoryPath + ".txt"), FileMode.Create, FileAccess.ReadWrite);
                    StreamWriter writer = new StreamWriter(fs);
                    writer.Write(VideoIntroduction);
                    writer.Close();
                    fs.Close();
                    //保存封面图片到指定文件夹下
                    hpfbImg.SaveAs(Server.MapPath("~/MyVideo/" + DirectoryPath + "\\" + ImgFileName));
                    //保存上传的视频到指定文件夹下
                    hpfbVideo.SaveAs(Server.MapPath("~/MyVideo/" + DirectoryPath + "\\" + VideoFileName));

                    T_SendWork sendWork = new T_SendWork();
                    sendWork.U_UserName = UserName;
                    sendWork.U_Id = U_Id;
                    sendWork.W_Id = 2;
                    sendWork.S_WorkName = VideoName;
                    sendWork.S_WorkPath = "~/MyVideo/" + DirectoryPath;
                    sendWork.S_WorkSize = (hpfbVideo.ContentLength + hpfbImg.ContentLength);
                    sendWork.S_SendTime = NowTime;
                    db.T_SendWork.Add(sendWork);
                    db.SaveChanges();
                }
                catch
                { }
            }
            return Json("OK");
        }

        public ActionResult OpenVideo(string AuthorName, int W_Id, string WorkName, string WorkTime)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                剧享网.Comment_cs.FatherNode father = new Comment_cs.FatherNode();
                List<T_Comment> FatherNode = new List<T_Comment>();
                FatherNode = father.FatherInfo(AuthorName, W_Id, WorkName, WorkTime);
                ViewBag.FatherNode = FatherNode;

                TestController test = new TestController();
                List<int> list = new List<int>();
                list = test.WebInitGetAgreeCount(AuthorName, W_Id, WorkName, WorkTime);
                ViewBag.List = list;

                //获取视频内容
                var GetCurrentInfo = from currentinfo in db.T_SendWork where currentinfo.U_UserName == AuthorName && currentinfo.W_Id == W_Id && currentinfo.S_WorkName == WorkName && currentinfo.S_SendTime == WorkTime select currentinfo;
                ViewBag.Title = "MyVideo";
                ViewBag.VideoName = WorkName;
                ViewBag.Author = AuthorName;
                ViewBag.VideoSendTime = WorkTime;
                string WorkPath = null;
                foreach (var info in GetCurrentInfo)
                {
                    WorkPath = info.S_WorkPath;
                }
                //视频名（内容介绍名和视频封面图片名及父文件夹名都是一样的）
                string VideoName = null;
                for (int i = 10; i < WorkPath.Length; ++i)
                {
                    VideoName += WorkPath[i];
                }
                //获取介绍的内容
                FileStream fs = new FileStream(Server.MapPath(WorkPath + "\\" + VideoName + ".txt"), FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(fs);
                ViewBag.VideoContent = reader.ReadToEnd();
                //获取当前打开的视频的路径
                var getVideoInfo = System.IO.Directory.GetFiles(Server.MapPath("/MyVideo/" + VideoName), "*.*", SearchOption.TopDirectoryOnly).Where(s => s.EndsWith(".mp4") || s.EndsWith(".flv") || s.EndsWith(".avi") || s.EndsWith(".wmv"));
                foreach (string info in getVideoInfo)
                {
                    FileInfo fileInfo = new FileInfo(info);
                    ViewBag.VideoPath = "/MyVideo/" + VideoName + "/" + fileInfo.Name;
                }
            }
            return View();
        }
    }
}
