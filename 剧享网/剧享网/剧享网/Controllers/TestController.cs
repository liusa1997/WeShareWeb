
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 剧享网.Models;

namespace 剧享网.Controllers
{
    public class TestController : Controller
    {
        [HttpPost]
        public string SubmitComment(string BeCommentedUserName, string BeCommentedInfo, string Time, string AuthorName, int W_Id,string WorkName,string ResponseInfo, string GetTime)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                //判断用户是否登录
                if (Session["UserName"] == null)
                {
                    return "Warning";
                }
                //判断该功能权限是否关闭
                var JudgePermission = from judge in db.T_Function where judge.F_SubId == "3F1" && judge.F_Function == "SendFunction" && judge.F_FatherId == "2F3" select judge;
                int F_IsOpen = 1;
                foreach (var open in JudgePermission)
                {
                    F_IsOpen = open.F_IsOpen;
                }
                if (F_IsOpen == 0)
                {
                    return "Close";
                }
                string UserName = Session["UserName"].ToString();
                string PassWord = Session["PassWord"].ToString();
                //查询出当前用户的信息表
                var SelectUserInfo = from userinfo in db.T_User where userinfo.U_UserName == UserName && userinfo.U_UserPassWord == PassWord select userinfo;
                //获取当前评论用户的Id
                int U_Id = 0;
                foreach (var info in SelectUserInfo)
                {
                    U_Id = info.U_Id;
                }  
                //查询出被评论的人的所有信息
                var SelectBeCommented = from becomment in db.T_Comment where becomment.U_UserName == BeCommentedUserName && becomment.C_Info == BeCommentedInfo && becomment.C_CommentTime == Time && becomment.C_WorkTime == GetTime select becomment;
                //获取被评论的人的编号
                int C_InfoId = 0;
                //获取本作品的发表时间，为下面插入提供
                string C_WorkTime = null;
                foreach (var getInfoId in SelectBeCommented)
                {
                    C_InfoId = getInfoId.C_InfoId;
                    C_WorkTime = getInfoId.C_WorkTime;
                }
                string NowTime = Convert.ToString(DateTime.Now);
                T_Comment t_Comment = new T_Comment();
                t_Comment.U_UserName = UserName;
                t_Comment.U_Id = U_Id;
                t_Comment.C_Id = 3;
                t_Comment.C_UserName = AuthorName;
                t_Comment.W_Id = W_Id;
                t_Comment.C_WorkName = WorkName;
                t_Comment.C_WorkTime = GetTime;
                t_Comment.C_Info = ResponseInfo;
                t_Comment.C_Response = C_InfoId;//对应对象唯一Id
                t_Comment.C_CommentTime = NowTime;
                db.T_Comment.Add(t_Comment);
                db.SaveChanges();
                ManagerController manager = new ManagerController();
                manager.NotifyInfo(ResponseInfo, AuthorName, W_Id, UserName, WorkName);
                return UserName + " ：" + ResponseInfo;
            }
        }

        [HttpPost]
        public string SelfComment(string AuthorName, int W_Id, string WorkName, string Comment,string GetTime)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                //判断用户是否登录
                if (Session["UserName"] == null)
                {
                    return "Warning";
                }
                //判断该功能权限是否关闭
                var JudgePermission = from judgepermission in db.T_Function where judgepermission.F_SubId == "3F1" && judgepermission.F_Function == "SendFunction" && judgepermission.F_FatherId == "2F3" select judgepermission;
                int F_IsOpen = 1;
                foreach (var open in JudgePermission)
                {
                    F_IsOpen = open.F_IsOpen;
                }
                if (F_IsOpen == 0)
                {
                    return "Close";
                }           
                string UserName = Session["UserName"].ToString();
                string PassWord = Session["PassWord"].ToString();
                //查询出当前用户的信息表
                var SelectUserInfo = from userinfo in db.T_User where userinfo.U_UserName == UserName && userinfo.U_UserPassWord == PassWord select userinfo;
                //获取当前评论用户的Id
                int U_Id = 0;
                string NowTime = Convert.ToString(DateTime.Now);
                foreach (var info in SelectUserInfo)
                {
                    U_Id = info.U_Id;
                }
                T_Comment t_Comment = new T_Comment();
                t_Comment.U_UserName = UserName;
                t_Comment.U_Id = U_Id;
                t_Comment.C_Id = 3;
                t_Comment.C_UserName = AuthorName;
                t_Comment.W_Id = W_Id;
                t_Comment.C_WorkName = WorkName;
                t_Comment.C_WorkTime = GetTime;
                t_Comment.C_Info = Comment;
                t_Comment.C_Response = 0;//自己评论对象是0
                t_Comment.C_CommentTime = NowTime;
                db.T_Comment.Add(t_Comment);
                db.SaveChanges();
                ManagerController manager = new ManagerController();
                if (W_Id == 1)
                {
                    manager.NotifyInfo(Comment, AuthorName, W_Id, UserName, WorkName);
                }
                else
                {
                    manager.NotifyInfo(Comment, AuthorName, W_Id, UserName, WorkName);
                }
                return "";
            }
        }

        public List<int> WebInitGetAgreeCount(string AuthorName, int W_Id, string WorkName, string WorkTime)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                var selectinfo = from info in db.T_SendWork where info.U_UserName == AuthorName && info.S_WorkName == WorkName && info.W_Id == W_Id && info.S_SendTime == WorkTime select info;
                int U_Id = 0;
                int S_InfoId = 0;
                foreach (var info in selectinfo)
                {
                    U_Id = info.U_Id;
                    S_InfoId = info.S_InfoId;
                }
                var list = new List<int>();
                var Count = from count in db.T_AgreeDisagree where count.U_Id == U_Id && count.A_InfoId == S_InfoId && count.W_Id == W_Id select count;
                int CountCal = (from countcal in db.T_AgreeDisagree where countcal.U_Id == U_Id && countcal.A_InfoId == S_InfoId && countcal.W_Id == W_Id select countcal).Count();
                if (CountCal == 0)
                {
                    list.Add(0);
                    list.Add(-1);
                    list.Add(0);
                    return list;
                }
                foreach (var count in Count)
                {
                    list.Add(count.A_AgreeCount);
                    list.Add(-1);
                    list.Add(count.A_DisAgreeCount);
                }
                return  list;
            }
        }
    }
}
