using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 剧享网.Models;

namespace 剧享网.Controllers
{
    public class SearchInfoController : Controller
    {
        // GET: FollowUsers
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult FuzzyFollow(string SearchUserName)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                //存放搜索的信息
                List<string> SearchInfo = new List<string>();
                //模糊查询信息
                var selectinfo = from info in db.T_User where info.U_UserName.Contains(SearchUserName) select info;
                foreach (var info in selectinfo)
                {
                    SearchInfo.Add(info.U_HeadImgPath);
                    SearchInfo.Add(info.U_UserName);
                    SearchInfo.Add(info.R_Id);
                    SearchInfo.Add(info.U_Email);
                }
                //前端无法接收后台的数组所以先把它json序列化，然后前端再去json.parse()即可
                string ReturnValue = JsonConvert.SerializeObject(SearchInfo);
                return Json(ReturnValue);
            }
        }

        [HttpPost]
        public string FollowOKUsers(string SearchUserName,string Email)
        {
            //string UserName = "liu";
            //string PassWord = "123456";
            //string SelfEmail = "1984713348@qq.com";
            string SelfEmail = Session["Email"].ToString();
            string PassWord = Session["PassWord"].ToString();
            string UserName = Session["UserName"].ToString();
            using (剧享网Entities db = new 剧享网Entities())
            {
                //查找被关注人的Id
                var selectBeFollowerUser = from info in db.T_User where info.U_UserName == SearchUserName && info.U_Email == Email select info;
                int BeFollowerU_Id = 0;
                foreach (var info in selectBeFollowerUser)
                {
                    BeFollowerU_Id = info.U_Id;
                }
                //获取当前用户的Id
                var selectCurrentUser = from info in db.T_User where info.U_UserName == UserName && info.U_Email == SelfEmail && info.U_UserPassWord == PassWord select info;
                int FollowerU_Id = 0;
                foreach (var info in selectCurrentUser)
                {
                    FollowerU_Id = info.U_Id;
                }
                //关注自己判断
                if (FollowerU_Id == BeFollowerU_Id)
                {
                    return "Laugh";
                }
                //关注的时间
                string FollowTime = Convert.ToString(DateTime.Now);
                //先找关注表是否有对应关注信息了
                int FollowCount = (from info in db.T_FollowUsers where info.F_FollowerId == FollowerU_Id && info.F_BeFollowerId == BeFollowerU_Id select info).Count();
                if (FollowCount > 0)
                {
                    return "Followed";
                }
                //保存数据
                T_FollowUsers followUsers = new T_FollowUsers();
                followUsers.F_FollowerId = FollowerU_Id;
                followUsers.F_BeFollowerId = BeFollowerU_Id;
                followUsers.F_FollowTime = FollowTime;
                db.T_FollowUsers.Add(followUsers);
                db.SaveChanges();
                //通知被关注者关注信息,W_Id依旧是0，类似系统通知，作品名是null
                ManagerController manager = new ManagerController();
                manager.NotifyInfo("关注了你", SearchUserName, 0, UserName,null);
                return "Success";
            }
        }

        [HttpPost]
        public string FollowOverUsers(string SearchUserName, string Email)
        {
            //string UserName = "liu";//session["UserName"]
            //string SelfEmail = "1984713348@qq.com";//session["Email"]
            //string PassWord = "123456";//session["PassWord"]
            string SelfEmail = Session["Email"].ToString();
            string PassWord = Session["PassWord"].ToString();
            string UserName = Session["UserName"].ToString();
            using (剧享网Entities db = new 剧享网Entities())
            {
                //查找被关注人的Id
                var selectBeFollowerUser = from info in db.T_User where info.U_UserName == SearchUserName && info.U_Email == Email select info;
                int BeFollowerU_Id = 0;
                foreach (var info in selectBeFollowerUser)
                {
                    BeFollowerU_Id = info.U_Id;
                }
                //获取当前用户的Id
                var selectCurrentUser = from info in db.T_User where info.U_UserName == UserName && info.U_Email == SelfEmail && info.U_UserPassWord == PassWord select info;
                int FollowerU_Id = 0;
                foreach (var info in selectCurrentUser)
                {
                    FollowerU_Id = info.U_Id;
                }
                //先找关注表是否有对应关注信息了
                int FollowCount = (from info in db.T_FollowUsers where info.F_FollowerId == FollowerU_Id && info.F_BeFollowerId == BeFollowerU_Id select info).Count();
                if (FollowCount <= 0)
                {
                    return "NoneFollow";
                }
                //获取对应关注信息的自增ID
                var GetId = from info in db.T_FollowUsers where info.F_FollowerId == FollowerU_Id && info.F_BeFollowerId == BeFollowerU_Id select info;
                //通过ID，删除的对应数据
                int DeleteId = 0;
                foreach (var info in GetId)
                {
                    DeleteId = info.F_Id;
                }
                //先实例化表的对象
                T_FollowUsers followUsers = new T_FollowUsers();
                //接着绑定该ID
                followUsers = db.T_FollowUsers.Find(DeleteId);
                //然后删除该ID对应的信息
                db.T_FollowUsers.Remove(followUsers);
                //最后保存操作即可 
                db.SaveChanges();
                return "Success";
            }
        }
    }
}
