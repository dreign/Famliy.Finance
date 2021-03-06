/****** Script for SelectTopNRows command from SSMS  ******/
SELECT TOP 1000 * FROM [familybank].[dbo].[bank_family]
SELECT TOP 1000 * FROM [familybank].[dbo].[sys_user]
SELECT TOP 1000 * FROM [familybank].[dbo].[sys_subject]
SELECT TOP 1000 * FROM [familybank].[dbo].bank_account
SELECT TOP 1000 * FROM [familybank].[dbo].bank_operate_log

--delete from [familybank].[dbo].[sys_user] where family_id not in(1,2)
--delete from [familybank].[dbo].[bank_family] where family_id not in(1,3)
--update [familybank].[dbo].[sys_user] set bank_family_family_id=1



--家庭-用户关系
use [familybank]
drop table report_family_user;
select [bank_family_family_id],b.[family_name],[user_name]
into report_family_user 
from dbo.sys_user a inner join dbo.bank_family b on a.bank_family_family_id=b.family_id
group by [bank_family_family_id],[user_name],b.[family_name]

select * from report_family_user
--家庭帐号历史汇总
use [familybank]
drop table report_family
select [bank_family_family_id],[family_name],subject_id,subject_name,sum(total) as total 
into report_family
from report_user a inner join report_family_user b on a.[user_name]=b.[user_name]
group by [bank_family_family_id],[family_name],subject_id,subject_name

select * from report_family

--家庭帐号每日金额汇总
use [familybank]
drop table report_family_day
select [bank_family_family_id],[family_name],sum(total) as total,dt
into report_family_day
 from report_user_day a inner join report_family_user b on a.[user_name]=b.[user_name]
 group by [bank_family_family_id],[family_name],dt

 select * from report_family_day
 
--家庭帐号每日分类汇总
use [familybank]
drop table report_family_subject_day
select [bank_family_family_id],[family_name],sum(total) as total,subject_id,subject_name,dt
into report_family_subject_day
 from report_user_subject_day a inner join report_family_user b on a.[user_name]=b.[user_name]
 group by [bank_family_family_id],[family_name],subject_id,subject_name,dt

 select * from report_family_subject_day




--个人帐号历史汇总
use [familybank]
drop table report_user;
select a.*,b.[subject_name]
into report_user
from(
SELECT [user_name] ,[subject_id],sum([money]) as total 
 FROM [familybank].[dbo].bank_operate_log 
 group by [user_name],[subject_id]
 ) a inner join [familybank].[dbo].[sys_subject] b on a.subject_id=b.subject_id
order by a.[user_name],a.[subject_id]

select *  FROM [familybank].[dbo].[report_user]


 --个人帐号每日金额汇总
use [familybank]
drop table report_user_day;
SELECT [user_name],sum([money]) as total ,convert(varchar(10),[create_date],20) as dt
into report_user_day
 FROM [familybank].[dbo].bank_operate_log
 group by [user_name],convert(varchar(10),[create_date],20)
 order by [user_name],dt

select * from report_user_day

 --个人帐号每日分类汇总
use [familybank]
drop table report_user_subject_day;
select a.*,b.[subject_name]
into report_user_subject_day
from(
SELECT [user_name] ,[subject_id],sum([money]) as total ,convert(varchar(10),[create_date],20) as dt
 FROM [familybank].[dbo].bank_operate_log
 group by [user_name],[subject_id],convert(varchar(10),[create_date],20)
  ) a inner join [familybank].[dbo].[sys_subject] b on a.subject_id=b.subject_id
order by [user_name],[subject_id],dt

select * from report_user_subject_day