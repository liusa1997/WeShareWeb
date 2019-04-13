using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using 剧享网.Models;

namespace 剧享网.Comment_cs
{
    public class AgreeDisCount
    {
        public List<T_AgreeDisagree> CountInfo(int U_Id,int W_Id, int C_InfoId)
        {
            using (剧享网Entities db = new 剧享网Entities())
            {
                List<T_AgreeDisagree> Count = new List<T_AgreeDisagree>();
                var GetCountInfo = from countinfo in db.T_AgreeDisagree where countinfo.A_InfoId == C_InfoId && countinfo.U_Id == U_Id && countinfo.W_Id == W_Id select countinfo;
                int GetCountInfoCount = (from countinfocount in db.T_AgreeDisagree where countinfocount.A_InfoId == C_InfoId && countinfocount.U_Id == U_Id && countinfocount.W_Id == W_Id select countinfocount).Count();
                if (GetCountInfoCount == 0)
                {
                    T_AgreeDisagree agreeDisagree = new T_AgreeDisagree();
                    agreeDisagree.A_AgreeCount = 0;
                    agreeDisagree.A_DisAgreeCount = 0;
                    Count.Add(agreeDisagree);
                    return Count;
                }

                foreach (var tem in GetCountInfo)
                {
                    T_AgreeDisagree agreeDisagree = new T_AgreeDisagree();
                    agreeDisagree.A_AgreeCount = tem.A_AgreeCount;
                    agreeDisagree.A_DisAgreeCount = tem.A_DisAgreeCount;
                    Count.Add(agreeDisagree);
                }
                return Count;
            }
        }
    }
}