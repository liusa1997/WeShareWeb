using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 剧享网.Models;

namespace 剧享网.Controllers
{
    public class T_AgreeDisagreeController : Controller
    {
        //点赞功能！
        public string AgreeDisagree(string judge, string username, string workname, int W_id, string GetTime)
        {
            try
            {
                using (剧享网Entities db = new 剧享网Entities())
                {
                    //判断用户是否登录
                    if (Session["UserName"] == null)
                    {
                        return "Warning";
                    }
                    //判断该功能权限是否关闭
                    var JudgePermission = from judgepermission in db.T_Function where judgepermission.F_SubId == "3F3" && judgepermission.F_Function == "ClickFunction" && judgepermission.F_FatherId == "2F3" select judgepermission;
                    int F_IsOpen = 1;
                    foreach (var open in JudgePermission)
                    {
                        F_IsOpen = open.F_IsOpen;
                    }
                    if (F_IsOpen == 0)
                    {
                        return "Close";
                    }
                    //获取T_AgreeDisagree的A_Id
                    int AID = 0;
                    //获取T_SendWork的U_Id
                    int UID = 0;
                    //获取T_SendWork的S_InfoId
                    int InfoID = 0;
                    if (judge == "agree")
                    {
                        //确定文章的uid和infoid
                        var GetUid = from list in db.T_SendWork where list.U_UserName == username && list.S_WorkName == workname && list.W_Id == W_id && list.S_SendTime == GetTime select list;
                        foreach (var list in GetUid)
                        {
                            UID = list.U_Id;
                            InfoID = list.S_InfoId;
                        }
                        //判断点赞表里面是否有对应的点赞对象信息
                        int JudgeExist = (from Judge in db.T_AgreeDisagree where Judge.U_Id == UID && Judge.W_Id == W_id && Judge.A_InfoId == InfoID select Judge).Count();
                        if (JudgeExist == 0)
                        {
                            T_AgreeDisagree agreeDisagree = new T_AgreeDisagree();
                            agreeDisagree.U_Id = UID;
                            agreeDisagree.W_Id = W_id;
                            agreeDisagree.A_InfoId = InfoID;
                            agreeDisagree.A_AgreeCount = 0;
                            agreeDisagree.A_DisAgreeCount = 0;
                            db.T_AgreeDisagree.Add(agreeDisagree);
                            db.SaveChanges();
                        }
                        //确定文章的AID 并增加赞数
                        var GetNumAidEssay = from list in db.T_AgreeDisagree where list.W_Id == W_id && list.U_Id == UID && list.A_InfoId == InfoID select list;
                        int essayAgreeNum = 0;
                        foreach (var list in GetNumAidEssay)
                        {
                            essayAgreeNum = list.A_AgreeCount;
                            AID = list.A_Id;
                        }
                        ++essayAgreeNum;
                        var GetFindList = db.T_AgreeDisagree.Find(AID);
                        GetFindList.A_AgreeCount = essayAgreeNum;
                        //更改该作品赞数
                        db.SaveChanges();
                        string str = "agree" + "," + essayAgreeNum.ToString();
                        return str;
                    }
                    else
                    {
                        //确定文章的uid和infoid
                        var GetUid = from list in db.T_SendWork where list.U_UserName == username && list.S_WorkName == workname && list.W_Id == W_id && list.S_SendTime == GetTime select list;
                        foreach (var list in GetUid)
                        {
                            UID = list.U_Id;
                            InfoID = list.S_InfoId;
                        }
                        //判断点赞表里面是否有对应的点赞对象信息
                        int JudgeExist = (from Judge in db.T_AgreeDisagree where Judge.U_Id == UID && Judge.W_Id == W_id && Judge.A_InfoId == InfoID select Judge).Count();
                        if (JudgeExist == 0)
                        {
                            T_AgreeDisagree agreeDisagree = new T_AgreeDisagree();
                            agreeDisagree.U_Id = UID;
                            agreeDisagree.W_Id = W_id;
                            agreeDisagree.A_InfoId = InfoID;
                            agreeDisagree.A_AgreeCount = 0;
                            agreeDisagree.A_DisAgreeCount = 0;
                            db.T_AgreeDisagree.Add(agreeDisagree);
                            db.SaveChanges();
                        }
                        //确定文章的AID 并增加反对数
                        var GetNumAidEssay = from list in db.T_AgreeDisagree where list.W_Id == W_id && list.U_Id == UID && list.A_InfoId == InfoID select list;
                        int essayDisagreeNum = 0;
                        foreach (var list in GetNumAidEssay)
                        {
                            essayDisagreeNum = list.A_DisAgreeCount;
                            AID = list.A_Id;
                        }
                        ++essayDisagreeNum;
                        var GetFindList = db.T_AgreeDisagree.Find(AID);
                        GetFindList.A_DisAgreeCount = essayDisagreeNum;
                        //更改该作品赞数
                        db.SaveChanges();
                        string str = "disagree" + "," + essayDisagreeNum.ToString();
                        return str;
                    }
                }
            }
            catch
            { return ""; }
        }

        //文章点赞
        [HttpPost]
        public string EssayAgreeDisagree(string judge, string username, string workname, string GetTime)
        {
            //文章编号是1
            int W_id = 1;
            string result = AgreeDisagree(judge, username, workname, W_id, GetTime);
            return result;
        }

        //视频点赞
        [HttpPost]
        public string VideoAgreeDisagree(string judge, string username, string workname, string GetTime)
        {
            //视频编号是2
            int W_id = 2;
            string result = AgreeDisagree(judge, username, workname, W_id, GetTime);
            return result;
        }

        //评论点赞
        [HttpPost]
        public JsonResult CommentaryAgreeorDisagree(string judge, string username, string C_UserName, int W_Id, string workname, string commentary,string GetTime,string CommentTime)
        {
            try
            {
                using (剧享网Entities db = new 剧享网Entities())
                {
                    //判断用户是否登录
                    if (Session["UserName"] == null)
                    {
                        return Json("Warning");
                    }
                    //判断该功能权限是否关闭
                    var JudgePermission = from judgepermission in db.T_Function where judgepermission.F_SubId == "3F3" && judgepermission.F_Function == "ClickFunction" && judgepermission.F_FatherId == "2F3" select judgepermission;
                    int F_IsOpen = 1;
                    foreach (var open in JudgePermission)
                    {
                        F_IsOpen = open.F_IsOpen;
                    }
                    if (F_IsOpen == 0)
                    {
                        return Json("Close");
                    }
                    var SelectInfo = from info in db.T_Comment where info.U_UserName == username && info.C_UserName == C_UserName && info.W_Id == W_Id && info.C_WorkName == workname && info.C_Info == commentary && info.C_CommentTime == CommentTime && info.C_WorkTime == GetTime select info;
                    //如果评论表没有对应信息则退出
                    int SelectInfoCount = (from infocount in db.T_Comment where infocount.U_UserName == username && infocount.C_UserName == C_UserName && infocount.W_Id == W_Id && infocount.C_WorkName == workname && infocount.C_Info == commentary && infocount.C_CommentTime == CommentTime && infocount.C_WorkTime == GetTime select infocount).Count();
                    if (SelectInfoCount == 0)
                    {
                        return Json(0);
                    }
                    //获取T_Comment的U_Id
                    int U_Id = 0;
                    //获取T_Comment的C_Id
                    var C_Id = 3;
                    //获取T_Comment的C_InfoId
                    int C_InfoId = 0;
                    //获取T_AgreeDisagree的A_Id
                    int A_Id = 0;
                    foreach (var info in SelectInfo)
                    {
                        U_Id = info.U_Id;
                        C_InfoId = info.C_InfoId;
                    }
                    int CheckInfo = (from check in db.T_AgreeDisagree where check.U_Id == U_Id && check.W_Id == C_Id && check.A_InfoId == C_InfoId select check).Count();
                    if (CheckInfo == 0)
                    {
                        //建议格式：
                        T_AgreeDisagree agreeDisagree = new T_AgreeDisagree
                        {
                            U_Id = U_Id,
                            W_Id = C_Id,
                            A_InfoId = C_InfoId,
                            A_AgreeCount = 0,
                            A_DisAgreeCount = 0
                        };
                        db.T_AgreeDisagree.Add(agreeDisagree);
                        db.SaveChanges();
                    }
                    if (judge == "agree")
                    {
                        //确定文章的AID 并增加赞数
                        var GetNumAidEssay = from list in db.T_AgreeDisagree where list.W_Id == C_Id && list.U_Id == U_Id && list.A_InfoId == C_InfoId select list;
                        int essayAgreeNum = 0;
                        foreach (var list in GetNumAidEssay)
                        {
                            essayAgreeNum = list.A_AgreeCount;
                            A_Id = list.A_Id;
                        }
                        ++essayAgreeNum;
                        var GetFindList = db.T_AgreeDisagree.Find(A_Id);
                        GetFindList.A_AgreeCount = essayAgreeNum;
                        //更改该作品赞数
                        db.SaveChanges();
                        //建议格式：
                        string str = $"agree,{essayAgreeNum}";
                        //string str = "agree" + "," + essayAgreeNum.ToString();
                        return Json(str);
                    }
                    else
                    {
                        //确定文章的AID 并增加反对数
                        var GetNumAidEssay = from list in db.T_AgreeDisagree where list.W_Id == C_Id && list.U_Id == U_Id && list.A_InfoId == C_InfoId select list;
                        int essayDisagreeNum = 0;
                        foreach (var list in GetNumAidEssay)
                        {
                            essayDisagreeNum = list.A_DisAgreeCount;
                            A_Id = list.A_Id;
                        }
                        ++essayDisagreeNum;
                        var GetFindList = db.T_AgreeDisagree.Find(A_Id);
                        GetFindList.A_DisAgreeCount = essayDisagreeNum;
                        //更改该作品赞数
                        db.SaveChanges();
                        string str = "disagree" + "," + essayDisagreeNum.ToString();
                        return Json(str);
                    }
                }
            }
            catch
            {
                return Json(0);
            }
        }
    }
}
