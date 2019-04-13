﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class 剧享网Entities : DbContext
    {
        public 剧享网Entities()
            : base("name=剧享网Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<T_Permission> T_Permission { get; set; }
        public virtual DbSet<T_Role> T_Role { get; set; }
        public virtual DbSet<T_Function> T_Function { get; set; }
        public virtual DbSet<T_ManOperLog> T_ManOperLog { get; set; }
        public virtual DbSet<T_NotifyInfoRecord> T_NotifyInfoRecord { get; set; }
        public virtual DbSet<T_AgreeDisagree> T_AgreeDisagree { get; set; }
        public virtual DbSet<T_IpReport> T_IpReport { get; set; }
        public virtual DbSet<V_Statistical> V_Statistical { get; set; }
        public virtual DbSet<T_Comment> T_Comment { get; set; }
        public virtual DbSet<T_User> T_User { get; set; }
        public virtual DbSet<T_SendWork> T_SendWork { get; set; }
        public virtual DbSet<T_FollowUsers> T_FollowUsers { get; set; }
        public virtual DbSet<T_NotifyInfo> T_NotifyInfo { get; set; }
    
        public virtual ObjectResult<string> P_FindUserRole(string userName)
        {
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("P_FindUserRole", userNameParameter);
        }
    
        public virtual int P_GetUserName(string userName)
        {
            var userNameParameter = userName != null ?
                new ObjectParameter("UserName", userName) :
                new ObjectParameter("UserName", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("P_GetUserName", userNameParameter);
        }
    }
}
