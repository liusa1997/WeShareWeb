using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using 剧享网.Controllers;
using 剧享网.Models;

namespace 剧享网.Controllers
{
    public class SharedController : Controller
    {
        // GET: Shared
        public ActionResult AnZiXiang_MainPage()
        {
            return View();
        }
        public ActionResult AnZiXiang_HeadPage()
        {
            return View();
        }
        public ActionResult AnZiXiang_Dark()
        {
            return View();
        }
        public ActionResult AnZiXiang_Light()
        {
            return View();
        }
        public ActionResult LiuBoWen_Movies()
        {
            return View();
        }
        public ActionResult LiuBoWen_Iframe()
        {
            return View();
        }
        public ActionResult LiuBoWen_dragonball()
        {
            return View();
        }
        public ActionResult LiuBoWen_CharacterBrief()
        {
            return View();
        }
        public ActionResult YangWanJing_bojeck(string AuthorName, string W_Id, string WorkName,string WorkTime)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                int w_id = Convert.ToInt32(W_Id);
                剧享网.Comment_cs.FatherNode father = new Comment_cs.FatherNode();
                List<T_Comment> FatherNode = new List<T_Comment>();
                FatherNode = father.FatherInfo(AuthorName, w_id, WorkName, WorkTime);
                ViewBag.FatherNode = FatherNode;

                TestController test = new TestController();
                List<int> list = new List<int>();
                list = test.WebInitGetAgreeCount(AuthorName, w_id, WorkName, WorkTime);
                ViewBag.List = list;
                return View();
            }
        }
        public ActionResult YangWanJing_rickyandmorty()
        {
            return View();
        }
        public ActionResult YangWanJing_BVS()
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}