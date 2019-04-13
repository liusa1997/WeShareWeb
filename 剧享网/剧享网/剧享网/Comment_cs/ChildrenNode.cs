using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using 剧享网.Models;

namespace 剧享网.Comment_cs
{
    public class ChildrenNode
    {
        public List<T_Comment> ChildrenInfo(int JudgeObject)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                List<T_Comment> SubList = new List<T_Comment>();//存放C_Response!=0的列表
                var GetSubInfo = from subinfo in db.T_Comment where subinfo.C_Response == JudgeObject select subinfo;
                int GetSubInfoCount = (from subinfocount in db.T_Comment where subinfocount.C_Response == JudgeObject select subinfocount).Count();
                if (GetSubInfoCount == 0)
                {
                    return SubList;
                }
                //string str1 = "：";
                foreach (var subinfo in GetSubInfo)
                {
                    T_Comment comment = new T_Comment();
                    comment.U_UserName = subinfo.U_UserName;
                    comment.C_Info = subinfo.C_Info;
                    SubList.Add(comment);
                    //SubList.Add(subinfo.U_UserName);//1：子信息的用户名
                    //SubList.Add(str1);//2：冒号
                    //SubList.Add(subinfo.C_Info);//3：子信息的用户评论内容
                }
                return SubList;
            }
        }
    }
}