//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace 剧享网.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class T_ManOperLog
    {
        public int M_Id { get; set; }
        public string U_UserName { get; set; }
        public string M_OperFunc { get; set; }
        public string M_OperObject { get; set; }
        public string M_OperContent { get; set; }
        public Nullable<System.DateTime> M_EnterTime { get; set; }
        public Nullable<System.DateTime> M_QuitTime { get; set; }
    }
}
