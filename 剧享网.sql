create database ������
--��ɫ��
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

--���ܱ�
create table T_Function
(
F_Id int identity(1,1) primary key,
F_SubId char(3),
F_Function char(19),
F_FatherId char(3),
F_IsOpen int not null--1:���ã�0������
)
insert into T_Function
values('1F1','UserManage','0',1);--���������ܣ��û�������
insert into T_Function
values('1F2','ManagerManage','0',1);--���������ܣ�����Ա������
insert into T_Function
values('2F1','NormalFunction','1F1',1)--�μ������ܣ���ͨ�û�����
insert into T_Function
values('2F2','VipFunction','1F1',1)--�μ������ܣ�VIP�û�����
insert into T_Function
values('2F3','ManagerFunction','1F2',1)--�μ������ܣ�����Ա����
insert into T_Function
values('3F1','SendFunction','2F1',1)--�ӹ��ܣ�NormalUser�������µȵĹ���
insert into T_Function
values('3F3','ClickFunction','2F1',1)--�ӹ��ܣ�NormalUser��������޵ȵĹ���
insert into T_Function
values('3F1','SendFunction','2F2',1)---�ӹ��ܣ�VipUser�������µȵĹ���
insert into T_Function
values('3F3','ClickFunction','2F2',1)--�ӹ��ܣ�VipUser��������޵ȵĹ���
insert into T_Function
values('3F4','DeleteFunction','2F2',1)--�ӹ��ܣ�VipUserɾ��NormalUser���۵ȵĹ���
insert into T_Function
values('3F1','SendFunction','2F3',1);--�ӹ��ܣ�Manager�������µȵĹ���
insert into T_Function
values('3F2','CheckFunction','2F3',1);--�ӹ��ܣ�Manager������¹淶�ȵĹ���
insert into T_Function
values('3F3','ClickFunction','2F3',1);--�ӹ��ܣ�Manager���޵ȵĹ���
insert into T_Function
values('3F4','DeleteFunction','2F3',1);--�ӹ��ܣ�Managerɾ��NormalUser���۵ȵĹ���
insert into T_Function
values('3F5','Notify','2F3',1);--�ӹ��ܣ�Manager֪ͨ����
insert into T_Function
values('3F6','ModifyRole','2F3',1);--�ӹ��ܣ�Manager�����û���ɫ����
select * from T_Function;

--�û���
create table T_User
(
U_Id int identity(1,1) primary key,
R_Id char(2) ,
U_HeadImgPath varchar(300),
U_UserName varchar(255),
U_UserPassWord varchar(255),
U_Email varchar(255),
U_UserTelNum char(11),
U_RoleIsOpen int not null, --0:��ֹ��½��1�������½
U_SendSpace int not null,--�û��ķ����ռ��С,R1:50M,R2:100M,R3:200M
U_RegisterTime datetime,--�û���ע���˺�ʱ��
)
select * from T_User;
select * from T_User 
where U_UserName like '%i%'

--Ȩ�ޱ�(��ɫ���ܱ�)
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

--��ע��
create table T_FollowUsers
(
F_Id int identity(1,1) primary key,
F_FollowerId int not null ,--��ע��ID(��֤Ψһȷ���û�)
F_BeFollowerId int not null ,--����ע��ID(��֤Ψһȷ���û�)
F_FollowTime varchar(30),--��עʱ��
)
select * from T_FollowUsers;
delete from T_FollowUsers

--���������
create table T_ManOperLog
(
M_Id int identity(1,1) primary key,
U_UserName varchar(255),--����Ա����
M_OperFunc char(19),--��������
M_OperObject varchar(255),--�������û�����
M_OperContent char(2),--����������
M_EnterTime datetime,--����Ա������������ʱ��
M_QuitTime datetime--����Ա�˳����������ʱ��
)
select * from T_ManOperLog;
delete from T_ManOperLog;

--֪ͨ��Ϣ��¼��
create table T_NotifyInfoRecord
(
N_Id int identity(1,1) primary key,
U_UserName varchar(255),--����Ա����
N_Content varchar(255),--֪ͨ�����ݣ�255�ֽ�
N_Time datetime--֪ͨ��ʱ��
)
select * from T_NotifyInfoRecord;
delete from T_NotifyInfoRecord;

--֪ͨ��Ϣ��
create table T_NotifyInfo
(
N_Id int identity(1,1) primary key,
N_ReplyUserName varchar(255),--�ظ����û���(ϵͳ���û�)
N_ReplyObject char(10),--���ظ��Ķ�����Ϊ�ǶԾ�������޸ģ������޷���ȫ��֪ͨ
W_Id int not null, --�����۵���ƷId
N_WorkName varchar(255),--�����¡���Ƶ������
N_Time varchar(30),--�ظ�ʱ��ʱ��
N_SystemContent varchar(255),--ϵͳ��֪ͨ������
N_ReadStatus int not null --0��δ����1���Ѷ�
)
select * from T_NotifyInfo;
update T_NotifyInfo set N_ReadStatus =0
delete from T_NotifyInfo
--��Ʒ���ͱ�
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

--������Ʒ������Ҫ�������·�����ֶΣ��ݶ���ˣ�
create table T_SendWork
(
U_UserName varchar(255),--�����ߵ��û���
U_Id int not null,--�����ߵ�Id
W_Id int not null,--��ƷId
S_InfoId int identity(1,1) primary key,--��ID����Ʒ������
S_WorkName varchar(255),--ֻ�����º���Ƶ
S_WorkPath varchar(255),--������µ�·��
S_WorkSize int not null,--��Ʒ��С
S_WorkViewCount int not null, --��Ʒ�������,�����ȶȵ�����֮һ,��һ���ǵ����뷴�ԵĲ�ֵ����,����֮��Ϊ�ȶȺ���
S_SendTime varchar(30),--������Ʒ��ʱ�䣬��ȷ��Ψһ����
)
insert into T_SendWork
values('�ٷ�',0,1,'���в��ܿ�',null,0,0,'2018/12/16 22:26:50');--�ٷ���id��0
select * from T_SendWork;

--���۱��������3��
create table T_Comment
(
U_UserName varchar(255),--�����ߵ��û���
U_Id int not null,--�����ߵ�Id
C_Id int not null,--���۱��Id,��ֵһֱΪ3
C_UserName varchar(255),--�������ߵ��û���
W_Id int not null, --�����۵���ƷId
C_WorkName varchar(255),--�����۵���Ʒ��
C_WorkTime char(30),--�����۵���Ʒ����ʱ��
C_InfoId int identity(1,1) primary key,--��ID�����۵�����
C_Info varchar(255),--�������۵�����
C_Response int not null,--�ظ��Ķ�����ȡ��C_InfoIdȻ�󸳸����ֵ����������Ǹ����������0
C_CommentTime char(30)--������ͬһ�û�ͬһ���ݵ�����(��ȷ)
)
select * from T_Comment;
select * from T_User;
select * from T_SendWork;
delete from T_Comment

--��ͬ���Ա�
create table T_AgreeDisagree
(
A_Id int identity(1,1) primary key,
--�����޶�����û�Id��Ϊ���۵ĵ���ͨ��T_Comment���U_UserName����ȡ��Id
--Ϊ��Ƶ�������µĵ���ͨ��T_SendWork���U_UserName����ȡ��Id
U_Id int not null,
--�����޶�����û�����Ʒ����Id
W_Id int not null,
--�����޶�����û�����Ʒ����Id����Ϊ�����ģ�
--Ϊ���۵ĵ���ͨ��T_Comment���U_UserName��U_Id��C_Info����ȡ��Id
--Ϊ��Ƶ�������µĵ���ͨ��T_SendWork���U_UserName��U_Id��W_Id��S_WorkName����ȡ��Id
A_InfoId int not null ,
--ͨ������������Ϣ����������count�ֱ�ȡ�����ΪAgree��A_AgreeCount++���෴��A_DisAgreeCount++
A_AgreeCount int not null ,
A_DisAgreeCount int not null
)
select * from T_AgreeDisagree;
delete from T_AgreeDisagree

--�����б�
create table T_Classificationlist
(
C_Id int identity(1,1) primary key,
C_TypeId int not null,
C_TypeName char(8)
)
insert into T_Classificationlist
values(1,'Index');
select * from T_Classificationlist;

--ͳ�Ʊ���ͳ��Ip��
create table T_IpReport
(
I_Id int identity(1,1) primary key,
I_Ipv4 char(15),
C_TypeId int not null,--�����б��
I_EnTime datetime,
I_QuitTime datetime--���������ȡ�û��ڵ�ǰ�б�ͣ��ʱ��
)
select * from T_IpReport;
delete from T_IpReport;

--��ͼ��ͳ�Ʊ����ȡ��ҳ���б������
--����ͼ�������б���������Ӷ��޸ĵ�
create view V_Statistical
as--���������������ȻEF�Ҳ���
--ȷ������ͼ����һ��ֵ��Ψһ������not null��
select ISNULL(NEWID(),'d1e57ca7-6eee-495a-be13-73d5e7d51f36') as V_Id ,COUNT(U_UserName) as 'UserCount',
		 COUNT(I_Ipv4) as 'IndexCount',
		 dbo.F_GetAccessRate(COUNT(U_UserName),COUNT(I_Ipv4)) as 'AccessRate'
from T_User full join T_IpReport --�˴���Ҫȫ���ӲŲ�Ӱ���ѯ������
on T_User.U_Id = T_IpReport.I_Id and C_TypeId = 1 

select * from V_Statistical
go

--��ͼ���û������Լ�������


		
--�洢���̣���ȡ��Ӧ�û���ǰ��ɫ
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

--����������ɾ���û�ʱ��ɾ��T_RoleUser�����Ӧ����Ϣ
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
--�洢���̣���ȡ�û���Ȼ��Ϊ��֮�����ɵĴ��������ֵ��ע��#��##����
create procedure P_GetUserName
@UserName varchar(255)
as 
begin
	if (OBJECT_ID('tempdb..##Temp') is not null)--ɾ��ȫ����ʱ����Ҫtempdb..
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

--������������ʣ��û���/��������
create function F_GetAccessRate(@UserCount float,@IndexCount float)
returns varchar(255)--2008��֧��double
as
begin
declare @accessrate varchar(255)
set @accessrate = cast(round((@UserCount/@IndexCount)*100,3) as varchar)+'%'
return @accessrate
end
select dbo.F_GetAccessRate(2,3)
drop function F_GetAccessRate


--�Զ���ĺ����﷨
create function hello(@inputStr varchar(255))--���Ǻ����Ĳ���
returns varchar(255)
as
begin
declare @result varchar(255)--�����ڲ�����
select @result = @inputStr--����
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
 
 

select S_WorkViewCount,ISNULL(A_AgreeCount,0)as '��ͬ��',ISNULL(A_DisAgreeCount,0)as '������'
from T_SendWork full join T_AgreeDisagree 
on T_SendWork.U_Id=1
group by S_WorkViewCount
 