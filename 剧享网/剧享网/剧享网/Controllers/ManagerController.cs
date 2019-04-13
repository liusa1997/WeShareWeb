using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 剧享网.Models;

namespace 剧享网.Controllers
{
    public class ManagerController : Controller
    {
        public ActionResult MainPage()
        {
            return View();
        }
        [HttpPost]
        //string sendstatus对应页面的name值要一致，获取的是value
        public ActionResult MainPage(string send, string check,string click, string delete, string notify, string modify, string OptionRadio,FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                if (send != null)
                {
                    SendFunction(send, collection);
                }
                if (check != null)
                {
                    CheckFunction(check, collection);
                }
                if (click != null)
                {
                    ClickFunction(click, collection);
                }
                if (delete != null)
                {
                    DeleteFunction(delete, collection);
                }
                if (notify != null)
                {
                    Notify(notify, collection);
                }
                //只修改全员功能权限
                if (modify != null)
                {
                    ModifyRole(modify,collection);
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        //向数据库插入管理员操作的日志
        public void GetManInfo(string FuncName, string Object, string Content)
        {
            string Name = Session["UserName"].ToString();
            string EnterTime = DateTime.Now.ToString();
            using (剧享网Entities db = new 剧享网Entities())
            {
                T_ManOperLog operLog = new T_ManOperLog();
                operLog.U_UserName = Name;
                operLog.M_OperFunc = FuncName;
                operLog.M_OperObject = Object;
                operLog.M_OperContent = Content;
                operLog.M_EnterTime = Convert.ToDateTime(EnterTime);
                operLog.M_QuitTime = null;
                db.T_ManOperLog.Add(operLog);
                db.SaveChanges();
            }
        }
        [HttpPost]
        //管理员退出管理界面将退出的时间为null的全部修改
        //因为现在是ajax写的提交表单，所以不需要担心一点按钮就刷新出现异常的退出时间
        public ActionResult UpdateQuitTime(string QuitTime)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                string UserName = Session["UserName"].ToString();
                var name = from listinfo in db.T_ManOperLog where listinfo.U_UserName == UserName && listinfo.M_QuitTime == null select listinfo;
                int updatecount = (from listinfo in db.T_ManOperLog where listinfo.U_UserName == UserName && listinfo.M_QuitTime == null select listinfo).Count();
                int[] listcount = new int[updatecount];
                int i = 0;
                foreach (var info in name)
                {
                    listcount[i] = info.M_Id;
                    ++i;
                }
                for (i = 0; i < updatecount; ++i)
                {
                    var Id = db.T_ManOperLog.Find(listcount[i]);
                    Id.M_QuitTime = Convert.ToDateTime(QuitTime);
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }

        public void SendFunction(string send,FormCollection collection)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                //获取send点击事件进行操作
                var GetSend = from list in db.T_Function where list.F_SubId == "3F1" && list.F_Function == "SendFunction" && list.F_FatherId == "2F3" select list;
                Int32 SendId = 0;
                foreach (var list in GetSend)
                {
                    SendId = list.F_Id;
                }
                //此时Find()里面的Id 不能加引号，不然会去找"Id"这个值不是Id了，而且页面不会报错只是不会往下面运行了
                var GetFindSend = db.T_Function.Find(SendId);
                if (send == "启用")
                {
                    GetFindSend.F_IsOpen = 1;
                    GetManInfo("SendFunction",null,"1");
                    NotifyInfo("发送功能被管理员启用,现在可以发送文章、视频、评论", "系统公告：", 0,null, null);
                }
                if (send == "禁止")
                {
                    GetFindSend.F_IsOpen = 0;
                    GetManInfo("SendFunction", null, "0");
                    NotifyInfo("发送功能被管理员禁用,现在禁止发送文章、视频、评论", "系统公告：", 0, null, null);
                }
                db.SaveChanges();
            }
        }
        public void CheckFunction(string check, FormCollection collection)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                //获取审查事件进行操作
                var GetCheck = from list in db.T_Function where list.F_SubId == "3F2" && list.F_Function == "CheckFunction" && list.F_FatherId == "2F3" select list;
                Int32 CheckId = 0;
                foreach (var list in GetCheck)
                {
                    CheckId = list.F_Id;
                }
                //此时Find()里面的Id 不能加引号，不然会去找"Id"这个值不是Id了，而且页面不会报错只是不会往下面运行了
                var GetFindCheck = db.T_Function.Find(CheckId);
                if (check == "启用")
                {
                    GetFindCheck.F_IsOpen = 1;
                    GetManInfo("CheckFunction", null, "1");
                    NotifyInfo("审查功能被管理员启用,现在管理员用户可以审查用户各种信息合法规范", "系统公告：", 0, null, null);
                }
                if (check == "禁止")
                {
                    GetFindCheck.F_IsOpen = 0;
                    GetManInfo("CheckFunction", null, "0");
                    NotifyInfo("审查功能被管理员禁用,现在禁止审查用户的信息", "系统公告：", 0, null, null);
                }
                db.SaveChanges();
            }
        }
        public void ClickFunction(string click, FormCollection collection)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                //获取send点击事件进行操作
                var GetClick = from list in db.T_Function where list.F_SubId == "3F3" && list.F_Function == "ClickFunction" && list.F_FatherId == "2F3" select list;
                Int32 ClickId = 0;
                foreach (var list in GetClick)
                {
                    ClickId = list.F_Id;
                }
                //此时Find()里面的Id 不能加引号，不然会去找"Id"这个值不是Id了，而且页面不会报错只是不会往下面运行了
                var GetFindClick = db.T_Function.Find(ClickId);
                if (click == "启用")
                {
                    GetFindClick.F_IsOpen = 1;
                    GetManInfo("ClickFunction", null, "1");
                    NotifyInfo("点赞功能被管理员启用,现在所有用户可以为用户的发布信息进行点赞反对", "系统公告：", 0, null, null);
                }
                if (click == "禁止")
                {
                    GetFindClick.F_IsOpen = 0;
                    GetManInfo("ClickFunction", null, "0");
                    NotifyInfo("点赞功能被管理员禁用,现在所有用户禁止为用户的发布信息进行点赞反对", "系统公告：", 0, null, null);
                }
                db.SaveChanges();
            }
        }
        public void DeleteFunction(string delete, FormCollection collection)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                //获取send点击事件进行操作
                var GetDelete = from list in db.T_Function where list.F_SubId == "3F4" && list.F_Function == "DeleteFunction" && list.F_FatherId == "2F3" select list;
                Int32 DeleteId = 0;
                foreach (var list in GetDelete)
                {
                    DeleteId = list.F_Id;
                }
                //此时Find()里面的Id 不能加引号，不然会去找"Id"这个值不是Id了，而且页面不会报错只是不会往下面运行了
                var GetFindDelete = db.T_Function.Find(DeleteId);
                if (delete == "启用")
                {
                    GetFindDelete.F_IsOpen = 1;
                    GetManInfo("DeleteFunction", null, "1");
                    NotifyInfo("删除功能被管理员启用,现在VIP用户可以删除其他用户的评论，管理员可以删除其他用户的评论以及删除不合乎规范的发布信息", "系统公告：", 0, null, null);
                }
                if (delete == "禁止")
                {
                    GetFindDelete.F_IsOpen = 0;
                    GetManInfo("DeleteFunction", null, "0");
                    NotifyInfo("删除功能被管理员禁用,现在禁止VIP用户删除其他用户的评论，管理员禁止删除其他用户的评论以及删除不合乎规范的发布信息", "系统公告：", 0, null, null);
                }
                db.SaveChanges();
            }
        }
        public void Notify(string notify, FormCollection collection)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                //获取send点击事件进行操作
                var GetNotify = from list in db.T_Function where list.F_SubId == "3F5" && list.F_Function == "Notify" && list.F_FatherId == "2F3" select list;
                Int32 NotifyId = 0;
                foreach (var list in GetNotify)
                {
                    NotifyId = list.F_Id;
                }
                //此时Find()里面的Id 不能加引号，不然会去找"Id"这个值不是Id了，而且页面不会报错只是不会往下面运行了
                var GetFindNotifyId = db.T_Function.Find(NotifyId);
                if (notify == "启用")
                {
                    GetFindNotifyId.F_IsOpen = 1;
                    GetManInfo("Notify", null, "1");
                    NotifyInfo("通知功能被管理员启用,现在管理员可以发布通知信息", "系统公告：", 0, null, null);
                }
                if (notify == "禁止")
                {
                    GetFindNotifyId.F_IsOpen = 0;
                    GetManInfo("Notify", null, "0");
                    NotifyInfo("通知功能被管理员禁用,现在管理员禁止发布通知信息", "系统公告：", 0, null, null);
                }
                db.SaveChanges();
            }
        }
        [HttpPost]
        public string NotifyInfo(string content,string ReplyUserName,int W_Id,string UserName,string WorkName)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                //Session["UserName"] = "liu"; 
                var GetNotify = from list in db.T_Function where list.F_SubId == "3F5" && list.F_Function == "Notify" && list.F_FatherId == "2F3" select list;
                Int32 NotifyId = 0;
                foreach (var list in GetNotify)
                {
                    NotifyId = list.F_Id;
                }
                //此时Find()里面的Id 不能加引号，不然会去找"Id"这个值不是Id了，而且页面不会报错只是不会往下面运行了
                var GetFindNotifyId = db.T_Function.Find(NotifyId);
                if (GetFindNotifyId.F_IsOpen == 0)
                {
                    return "该功能被禁用";
                }
                //当UserName!=null就是关注通知和动态通知
                if (UserName != null)
                {
                    //取对应用户进行通知
                    T_NotifyInfo notifyInfo = new T_NotifyInfo();
                    notifyInfo.N_ReplyUserName = UserName;
                    notifyInfo.N_ReplyObject = ReplyUserName;
                    notifyInfo.W_Id = W_Id;
                    notifyInfo.N_WorkName = WorkName;
                    notifyInfo.N_Time = Convert.ToString(DateTime.Now);
                    notifyInfo.N_SystemContent = content;
                    notifyInfo.N_ReadStatus = 0;
                    db.T_NotifyInfo.Add(notifyInfo);
                    db.SaveChanges();
                }
                else
                {
                    //向通知记录表里面插入通知记录信息
                    string name = Session["UserName"].ToString();
                    var time = DateTime.Now;
                    T_NotifyInfoRecord InfoRecord = new T_NotifyInfoRecord();
                    InfoRecord.U_UserName = name;
                    InfoRecord.N_Content = content;
                    InfoRecord.N_Time = time;
                    db.T_NotifyInfoRecord.Add(InfoRecord);
                    var SelectUser = from user in db.T_User select user;
                    foreach (var insert in SelectUser)
                    {
                        //向通知信息表里面插入通知信息,这个实例化必须写在里面，不然只会保存最后一条信息
                        T_NotifyInfo notifyInfo = new T_NotifyInfo();
                        notifyInfo.N_ReplyUserName = ReplyUserName;
                        notifyInfo.N_ReplyObject = insert.U_UserName;
                        notifyInfo.W_Id = W_Id;
                        notifyInfo.N_WorkName = null;
                        notifyInfo.N_Time = Convert.ToString(DateTime.Now);
                        notifyInfo.N_SystemContent = content;
                        notifyInfo.N_ReadStatus = 0;
                        db.T_NotifyInfo.Add(notifyInfo);
                    }
                    db.SaveChanges();
                }
                return "通知成功";
            }
        }
        public void ModifyRole(string modify,FormCollection collection)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                //这是修改所有用户的功能权限
                var GetModify = from list in db.T_Function where list.F_SubId == "3F6" && list.F_Function == "ModifyRole" && list.F_FatherId == "2F3" select list;
                Int32 ModifyId = 0;
                foreach (var list in GetModify)
                {
                    ModifyId = list.F_Id;
                }
                //此时Find()里面的Id 不能加引号，不然会去找"Id"这个值不是Id了，而且页面不会报错只是不会往下面运行了
                var GetFindModify = db.T_Function.Find(ModifyId);
                if (modify == "启用")
                {
                    GetFindModify.F_IsOpen = 1;
                    GetManInfo("ModifyRole", null, "1");
                    NotifyInfo("修改角色功能被管理员启用,现在管理员可以在一定信息下修改用户角色", "系统公告：", 0, null, null);
                }
                if (modify == "禁止")
                {
                    GetFindModify.F_IsOpen = 0;
                    GetManInfo("ModifyRole", null, "0");
                    NotifyInfo("修改角色功能被管理员禁用,现在管理员禁止修改角色信息", "系统公告：", 0, null, null);
                }
                db.SaveChanges();
            }
        }
        public string ModifyRole_Select(string OptionRadio, FormCollection collection)
        {
            try
            {
                using (剧享网Entities db = new 剧享网Entities())
                {
                    //获取用户角色
                    string username = collection["UserName"];
                    string telnum = collection["TelNum"];
                    if (username == "" || telnum == "")
                    {
                        return "用户名、电话号码不能为空";
                    }
                    //通过对应用户的用户名和电话号码查询其角色编号
                    var SelectR_Id = from selectId in db.T_User where selectId.U_UserName == username && selectId.U_UserTelNum == telnum select selectId;
                    string R_Id = null;
                    foreach (var selectid in SelectR_Id)
                    {
                        R_Id = selectid.R_Id;
                    }
                    //通过其角色编号查询对应的角色
                    var SelectRole = from selectrole in db.T_Role where selectrole.R_Id == R_Id select selectrole;
                    string Role = null;
                    foreach (var selectrole in SelectRole)
                    {
                        Role = selectrole.R_role;
                    }
                    //返回角色
                    return Role;
                }
            }
            catch {
                return ("error");
            }
        }
        public string ModifyRole_updaterole(string OptionRadio, FormCollection collection)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                var GetModify = from list in db.T_Function where list.F_SubId == "3F6" && list.F_Function == "ModifyRole" && list.F_FatherId == "2F3" select list;
                Int32 ModifyId = 0;
                foreach (var list in GetModify)
                {
                    ModifyId = list.F_Id;
                }
                //此时Find()里面的Id 不能加引号，不然会去找"Id"这个值不是Id了，而且页面不会报错只是不会往下面运行了
                var GetFindModify = db.T_Function.Find(ModifyId);
                //先判断该功能是否启用
                if (GetFindModify.F_IsOpen == 0)
                {
                    return "功能被禁用";
                }
                //获取用户角色
                string username = collection["UserName"];
                string telnum = collection["TelNum"];
                //通过该用户用户名和电话查询出其用户编号
                var SelectU_Id = from id in db.T_User where id.U_UserName == username && id.U_UserTelNum == telnum select id;
                int U_Id = 0;
                foreach (var list in SelectU_Id)
                {
                    U_Id = list.U_Id;
                }
                //通过用户编号获取该用户的所有信息列表
                var UpdateU_Id = db.T_User.Find(U_Id);
                if (OptionRadio == "Normal")
                {
                    UpdateU_Id.R_Id = "R1";
                    UpdateU_Id.U_SendSpace = 1024 * 1024 * 50;
                    GetManInfo("CheckFunction", username, "R1");
                }
                if (OptionRadio == "VIP")
                {
                    UpdateU_Id.R_Id = "R2";
                    UpdateU_Id.U_SendSpace = 1024 * 1024 * 100;
                    GetManInfo("CheckFunction", username, "R2");
                }
                if (OptionRadio == "Manager")
                {
                    UpdateU_Id.R_Id = "R3";
                    UpdateU_Id.U_SendSpace = 1024 * 1024 * 200;
                    GetManInfo("CheckFunction", username, "R3");
                }
                db.SaveChanges();
                return "修改成功";
            }
        }
        //获取统计查询的结果
        public JsonResult GetStatisticalResult()
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                //一个组合查询出来的结果的字符串
                string Combination = null;
                //查询统计视图里面的信息，其实就只有1条，多个字段
                var statistical = from result in db.V_Statistical select result;
                foreach (var info in statistical)
                {
                    Combination = info.UserCount.ToString() + "," + info.IndexCount.ToString() + "," + info.AccessRate;
                }
                return Json(Combination);
            }
        }
    }
}
