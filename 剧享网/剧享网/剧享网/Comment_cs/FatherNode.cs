using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using 剧享网.Models;

namespace 剧享网.Comment_cs
{
    public class FatherNode
    {
        public List<T_Comment> FatherInfo(string AuthorName, int W_Id, string WorkName, string WorkTime)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                //
                var fatherinfo = from info in db.T_Comment where info.C_Response == 0 && info.C_UserName == AuthorName && info.W_Id == W_Id && info.C_WorkName == WorkName && info.C_WorkTime == WorkTime select info;
                //
                List<T_Comment> FatherList = new List<T_Comment>();
                foreach (var info in fatherinfo)
                {
                    T_Comment comment = new T_Comment();
                    comment.U_Id = info.U_Id;
                    comment.C_Id = info.C_Id;
                    comment.C_InfoId = info.C_InfoId;
                    comment.U_UserName = info.U_UserName;
                    comment.C_Info = info.C_Info;
                    comment.C_CommentTime = info.C_CommentTime;
                    FatherList.Add(comment);
                }
                return FatherList;
            }
        }
    }
}