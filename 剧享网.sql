create database 剧享网
--角色表
create table T_Role
(
R_Id char(2) primary key,
R_role char(10)
)
insert into T_Role
values('R1','NormalUser');
insert into T_Role
values('R2','VipUser');
insert into T_Role
values('R3','Manager');
select * from T_Role;

--功能表
create table T_Function
(
F_Id int identity(1,1) primary key,
F_SubId char(3),
F_Function char(19),
F_FatherId char(3),
F_IsOpen int not null--1:启用；0：禁用
)
insert into T_Function
values('1F1','UserManage','0',1);--顶级父功能：用户管理功能
insert into T_Function
values('1F2','ManagerManage','0',1);--顶级父功能：管理员管理功能
insert into T_Function
values('2F1','NormalFunction','1F1',1)--次级父功能：普通用户功能
insert into T_Function
values('2F2','VipFunction','1F1',1)--次级父功能：VIP用户功能
insert into T_Function
values('2F3','ManagerFunction','1F2',1)--次级父功能：管理员功能
insert into T_Function
values('3F1','SendFunction','2F1',1)--子功能：NormalUser发送文章等的功能
insert into T_Function
values('3F3','ClickFunction','2F1',1)--子功能：NormalUser点击评论赞等的功能
insert into T_Function
values('3F1','SendFunction','2F2',1)---子功能：VipUser发送文章等的功能
insert into T_Function
values('3F3','ClickFunction','2F2',1)--子功能：VipUser点击评论赞等的功能
insert into T_Function
values('3F4','DeleteFunction','2F2',1)--子功能：VipUser删除NormalUser评论等的功能
insert into T_Function
values('3F1','SendFunction','2F3',1);--子功能：Manager发送文章等的功能
insert into T_Function
values('3F2','CheckFunction','2F3',1);--子功能：Manager检查文章规范等的功能
insert into T_Function
values('3F3','ClickFunction','2F3',1);--子功能：Manager点赞等的功能
insert into T_Function
values('3F4','DeleteFunction','2F3',1);--子功能：Manager删除NormalUser评论等的功能
insert into T_Function
values('3F5','Notify','2F3',1);--子功能：Manager通知功能
insert into T_Function
values('3F6','ModifyRole','2F3',1);--子功能：Manager调整用户角色功能
select * from T_Function;

--用户表
create table T_User
(
U_Id int identity(1,1) primary key,
R_Id char(2) ,
U_HeadImgPath varchar(300),
U_UserName varchar(255),
U_UserPassWord varchar(255),
U_Email varchar(255),
U_UserTelNum char(11),
U_RoleIsOpen int not null, --0:禁止登陆；1：允许登陆
U_SendSpace int not null,--用户的发布空间大小,R1:50M,R2:100M,R3:200M
U_RegisterTime datetime,--用户的注册账号时间
)
select * from T_User;
select * from T_User 
where U_UserName like '%i%'

--权限表(角色功能表)
create table T_Permission
(
P_Id int identity(1,1) primary key,
R_Id char(2),
F_SubId char(3)
)
insert into T_Permission
values('R1','3F1');
insert into T_Permission
values('R1','3F3');
insert into T_Permission
values('R2','3F1');
insert into T_Permission
values('R2','3F3');
insert into T_Permission
values('R2','3F4');
insert into T_Permission
values('R3','3F1');
insert into T_Permission
values('R3','3F2');
insert into T_Permission
values('R3','3F3');
insert into T_Permission
values('R3','3F4');
insert into T_Permission
values('R3','3F5');
select * from T_Permission;

--关注表
create table T_FollowUsers
(
F_Id int identity(1,1) primary key,
F_FollowerId int not null ,--关注人ID(保证唯一确认用户)
F_BeFollowerId int not null ,--被关注人ID(保证唯一确认用户)
F_FollowTime varchar(30),--关注时间
)
select * from T_FollowUsers;
delete from T_FollowUsers

--管理操作表
create table T_ManOperLog
(
M_Id int identity(1,1) primary key,
U_UserName varchar(255),--管理员名字
M_OperFunc char(19),--操作功能
M_OperObject varchar(255),--操作的用户对象
M_OperContent char(2),--操作的内容
M_EnterTime datetime,--管理员进入操作界面的时间
M_QuitTime datetime--管理员退出操作界面的时间
)
select * from T_ManOperLog;
delete from T_ManOperLog;

--通知信息记录表
create table T_NotifyInfoRecord
(
N_Id int identity(1,1) primary key,
U_UserName varchar(255),--管理员名字
N_Content varchar(255),--通知的内容，255字节
N_Time datetime--通知的时间
)
select * from T_NotifyInfoRecord;
delete from T_NotifyInfoRecord;

--通知信息表
create table T_NotifyInfo
(
N_Id int identity(1,1) primary key,
N_ReplyUserName varchar(255),--回复的用户名(系统、用户)
N_ReplyObject char(10),--被回复的对象，因为是对具体对象修改，所以无法用全体通知
W_Id int not null, --被评论的作品Id
N_WorkName varchar(255),--有文章、视频、评论
N_Time varchar(30),--回复时的时间
N_SystemContent varchar(255),--系统所通知的内容
N_ReadStatus int not null --0：未读，1：已读
)
select * from T_NotifyInfo;
update T_NotifyInfo set N_ReadStatus =0
delete from T_NotifyInfo
--作品类型表
create table T_WorkType
(
W_Id int primary key,
W_Type char(10)
)
insert into T_WorkType
values(1,'article');
insert into T_WorkType
values(2,'video');
insert into T_WorkType
values(3,'comment');
select * from T_WorkType;

--发送作品表（还需要添加内容路径等字段，暂定如此）
create table T_SendWork
(
U_UserName varchar(255),--发送者的用户名
U_Id int not null,--发送者的Id
W_Id int not null,--作品Id
S_InfoId int identity(1,1) primary key,--该ID是作品的自增
S_WorkName varchar(255),--只有文章和视频
S_WorkPath varchar(255),--存放文章的路径
S_WorkSize int not null,--作品大小
S_WorkViewCount int not null, --作品的浏览数,衡量热度的因素之一,另一个是点赞与反对的差值数量,二者之和为热度衡量
S_SendTime varchar(30),--发送作品的时间，来确保唯一检索
)
insert into T_SendWork
values('官方',0,1,'马男波杰克',null,0,0,'2018/12/16 22:26:50');--官方的id是0
select * from T_SendWork;

--评论表（这个就是3）
create table T_Comment
(
U_UserName varchar(255),--评论者的用户名
U_Id int not null,--评论者的Id
C_Id int not null,--评论表的Id,其值一直为3
C_UserName varchar(255),--被评论者的用户名
W_Id int not null, --被评论的作品Id
C_WorkName varchar(255),--被评论的作品名
C_WorkTime char(30),--被评论的作品发送时间
C_InfoId int identity(1,1) primary key,--该ID是评论的自增
C_Info varchar(255),--发送评论的内容
C_Response int not null,--回复的对象，先取出C_InfoId然后赋给这个值，如果对象是父对象则插入0
C_CommentTime char(30)--来区分同一用户同一内容的区别(精确)
)
select * from T_Comment;
select * from T_User;
select * from T_SendWork;
delete from T_Comment

--赞同反对表
create table T_AgreeDisagree
(
A_Id int identity(1,1) primary key,
--被点赞对象的用户Id，为评论的点赞通过T_Comment里的U_UserName来获取该Id
--为视频或者文章的点赞通过T_SendWork里的U_UserName来获取该Id
U_Id int not null,
--被点赞对象的用户的作品类型Id
W_Id int not null,
--被点赞对象的用户的作品名字Id（其为自增的）
--为评论的点赞通过T_Comment里的U_UserName、U_Id、C_Info来获取该Id
--为视频或者文章的点赞通过T_SendWork里的U_UserName、U_Id、W_Id、S_WorkName来获取该Id
A_InfoId int not null ,
--通过以上三个信息将下面两个count分别取出如果为Agree则A_AgreeCount++，相反则A_DisAgreeCount++
A_AgreeCount int not null ,
A_DisAgreeCount int not null
)
select * from T_AgreeDisagree;
delete from T_AgreeDisagree

--分类列表
create table T_Classificationlist
(
C_Id int identity(1,1) primary key,
C_TypeId int not null,
C_TypeName char(8)
)
insert into T_Classificationlist
values(1,'Index');
select * from T_Classificationlist;

--统计报表：统计Ip表
create table T_IpReport
(
I_Id int identity(1,1) primary key,
I_Ipv4 char(15),
C_TypeId int not null,--分类列表的
I_EnTime datetime,
I_QuitTime datetime--二者相减获取用户在当前列表停留时间
)
select * from T_IpReport;
delete from T_IpReport;

--视图：统计报表获取网页个列表访问量
--该视图会随着列表的数量增加而修改的
create view V_Statistical
as--必须添加主键，不然EF找不到
--确保该视图中有一列值是唯一的且是not null的
select ISNULL(NEWID(),'d1e57ca7-6eee-495a-be13-73d5e7d51f36') as V_Id ,COUNT(U_UserName) as 'UserCount',
		 COUNT(I_Ipv4) as 'IndexCount',
		 dbo.F_GetAccessRate(COUNT(U_UserName),COUNT(I_Ipv4)) as 'AccessRate'
from T_User full join T_IpReport --此处需要全连接才不影响查询的数量
on T_User.U_Id = T_IpReport.I_Id and C_TypeId = 1 

select * from V_Statistical
go

--视图：用户查找自己的评论


		
--存储过程：获取对应用户当前角色
create procedure P_FindUserRole
@UserName varchar(255)
as 
select R_role
from T_Role
where R_Id = (
				select R_Id 
				from T_User
				where U_UserName = @UserName)
				
execute P_FindUserRole 'liu'
go

--触发器：当删除用户时，删除T_RoleUser里面对应的信息
create trigger DeleteUserAndRole
on T_User
for delete 
as
begin
declare @UserName varchar(255)
set @UserName = (select U_UserName from ##Temp)
delete from T_RoleUser
where U_UserName = @UserName
end
--存储过程：获取用户名然后为了之后生成的触发器获得值（注意#和##区别）
create procedure P_GetUserName
@UserName varchar(255)
as 
begin
	if (OBJECT_ID('tempdb..##Temp') is not null)--删除全局临时表需要tempdb..
		drop table ##Temp
	create table ##Temp(U_UserName varchar(255))
	insert into ##Temp values(@UserName)
end

exec P_GetUserName 'liu'
drop table ##Temp
select * from ##Temp
insert into T_User
values(1,'liu');
select * from T_User;
delete from T_User where U_UserName = 'liu'


select *
from T_Function,T_User,T_Permission
where T_User.R_Id = T_Permission.R_Id and 
	T_Permission.F_SubId = T_Function.F_SubId and
	T_User.U_UserName = 'liu'  and 
	T_Function.F_Function = 'SendFunction' and T_Function.F_FatherId='2F3' and
	T_Function.F_IsOpen = 1 
go	

--函数：求访问率（用户数/访问数）
create function F_GetAccessRate(@UserCount float,@IndexCount float)
returns varchar(255)--2008不支持double
as
begin
declare @accessrate varchar(255)
set @accessrate = cast(round((@UserCount/@IndexCount)*100,3) as varchar)+'%'
return @accessrate
end
select dbo.F_GetAccessRate(2,3)
drop function F_GetAccessRate


--自定义的函数语法
create function hello(@inputStr varchar(255))--就是函数的参数
returns varchar(255)
as
begin
declare @result varchar(255)--函数内部变量
select @result = @inputStr--调用
return @result
end

select * from T_SendWork;
select * from T_AgreeDisagree;
select *
from T_SendWork left join T_AgreeDisagree
on T_SendWork.U_Id=T_AgreeDisagree.U_Id 
and T_SendWork.W_Id=T_AgreeDisagree.W_Id 
and T_SendWork.S_InfoId=T_AgreeDisagree.A_InfoId
and T_SendWork.U_Id!=0
 
 

select S_WorkViewCount,ISNULL(A_AgreeCount,0)as '赞同数',ISNULL(A_DisAgreeCount,0)as '反对数'
from T_SendWork full join T_AgreeDisagree 
on T_SendWork.U_Id=1
group by S_WorkViewCount
 