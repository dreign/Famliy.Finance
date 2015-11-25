USE [msdb]
GO

/****** Object:  Job [bankreport]    Script Date: 2015/10/22 1:34:47 ******/
BEGIN TRANSACTION
DECLARE @ReturnCode INT
SELECT @ReturnCode = 0
/****** Object:  JobCategory [[Uncategorized (Local)]]]    Script Date: 2015/10/22 1:34:47 ******/
IF NOT EXISTS (SELECT name FROM msdb.dbo.syscategories WHERE name=N'[Uncategorized (Local)]' AND category_class=1)
BEGIN
EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=N'[Uncategorized (Local)]'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback

END

DECLARE @jobId BINARY(16)
EXEC @ReturnCode =  msdb.dbo.sp_add_job @job_name=N'bankreport', 
		@enabled=1, 
		@notify_level_eventlog=0, 
		@notify_level_email=0, 
		@notify_level_netsend=0, 
		@notify_level_page=0, 
		@delete_level=0, 
		@description=N'个人帐号历史汇总，个人帐号每日汇总', 
		@category_name=N'[Uncategorized (Local)]', 
		@owner_login_name=N'sa', @job_id = @jobId OUTPUT
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [个人帐号历史汇总]    Script Date: 2015/10/22 1:34:47 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'个人帐号历史汇总', 
		@step_id=1, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'use [familybank]
drop table report_user;
select identity(int,1,1) as id,a.*,b.[subject_name]
into report_user
from(
SELECT [user_name] ,[subject_id],sum([money]) as total 
 FROM [familybank].[dbo].bank_operate_log 
 group by [user_name],[subject_id]
 ) a inner join [familybank].[dbo].[sys_subject] b on a.subject_id=b.subject_id
order by a.[user_name],a.[subject_id]', 
		@database_name=N'familybank', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [个人帐号每日金额汇总]    Script Date: 2015/10/22 1:34:47 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'个人帐号每日金额汇总', 
		@step_id=2, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'use [familybank]
drop table report_user_day;
SELECT identity(int,1,1) as id,[user_name],sum([money]) as total ,convert(varchar(10),[create_date],20) as dt
into report_user_day
 FROM [familybank].[dbo].bank_operate_log
 group by [user_name],convert(varchar(10),[create_date],20)
 order by [user_name],dt', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [个人帐号每日分类汇总]    Script Date: 2015/10/22 1:34:47 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'个人帐号每日分类汇总', 
		@step_id=3, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'use [familybank]
drop table report_user_subject_day;
select identity(int,1,1) as id,a.*,b.[subject_name]
into report_user_subject_day
from(
SELECT [user_name] ,[subject_id],sum([money]) as total ,convert(varchar(10),[create_date],20) as dt
 FROM [familybank].[dbo].bank_operate_log
 group by [user_name],[subject_id],convert(varchar(10),[create_date],20)
  ) a inner join [familybank].[dbo].[sys_subject] b on a.subject_id=b.subject_id
order by [user_name],[subject_id],dt', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [家庭-用户关系]    Script Date: 2015/10/22 1:34:47 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'家庭-用户关系', 
		@step_id=4, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'use [familybank]
drop table report_family_user;
select identity(int,1,1) as id,[bank_family_family_id],b.[family_name],[user_name]
into report_family_user 
from dbo.sys_user a inner join dbo.bank_family b on a.bank_family_family_id=b.family_id
group by [bank_family_family_id],[user_name],b.[family_name]', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [家庭帐号历史汇总]    Script Date: 2015/10/22 1:34:47 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'家庭帐号历史汇总', 
		@step_id=5, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'use [familybank]
drop table report_family
select identity(int,1,1) as id,[bank_family_family_id],[family_name],subject_id,subject_name,sum(total) as total 
into report_family
from report_user a inner join report_family_user b on a.[user_name]=b.[user_name]
group by [bank_family_family_id],[family_name],subject_id,subject_name', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [家庭帐号每日金额汇总]    Script Date: 2015/10/22 1:34:47 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'家庭帐号每日金额汇总', 
		@step_id=6, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'use [familybank]
drop table report_family_day
select identity(int,1,1) as id,[bank_family_family_id],[family_name],sum(total) as total,dt
into report_family_day
 from report_user_day a inner join report_family_user b on a.[user_name]=b.[user_name]
 group by [bank_family_family_id],[family_name],dt', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [家庭帐号每日分类汇总]    Script Date: 2015/10/22 1:34:47 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'家庭帐号每日分类汇总', 
		@step_id=7, 
		@cmdexec_success_code=0, 
		@on_success_action=3, 
		@on_success_step_id=0, 
		@on_fail_action=3, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'use [familybank]
drop table report_family_subject_day
select identity(int,1,1) as id,[bank_family_family_id],[family_name],sum(total) as total,subject_id,subject_name,dt
into report_family_subject_day
 from report_user_subject_day a inner join report_family_user b on a.[user_name]=b.[user_name]
 group by [bank_family_family_id],[family_name],subject_id,subject_name,dt', 
		@database_name=N'master', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
/****** Object:  Step [家庭账户余额更新]    Script Date: 2015/10/22 1:34:47 ******/
EXEC @ReturnCode = msdb.dbo.sp_add_jobstep @job_id=@jobId, @step_name=N'家庭账户余额更新', 
		@step_id=8, 
		@cmdexec_success_code=0, 
		@on_success_action=1, 
		@on_success_step_id=0, 
		@on_fail_action=2, 
		@on_fail_step_id=0, 
		@retry_attempts=0, 
		@retry_interval=0, 
		@os_run_priority=0, @subsystem=N'TSQL', 
		@command=N'use [familybank]
update a set a.assets_total = b.total,a.assets_debt=b.debt,a.assets_net = b.net
from [bank_family] a left join  (
select a.[bank_family_family_id],
sum(case when [money]>0 then [money] else 0 end) as [total],
sum(case when [money]<0 then [money] else 0 end) as [debt],
sum([money]) as net
FROM report_family_user a inner join [dbo].bank_operate_log b on a.[user_name]=b.[user_name]
group by a.[bank_family_family_id]
) b on a.family_id=b.bank_family_family_id', 
		@database_name=N'familybank', 
		@flags=0
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_update_job @job_id = @jobId, @start_step_id = 1
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobschedule @job_id=@jobId, @name=N'allday', 
		@enabled=1, 
		@freq_type=4, 
		@freq_interval=1, 
		@freq_subday_type=4, 
		@freq_subday_interval=1, 
		@freq_relative_interval=0, 
		@freq_recurrence_factor=0, 
		@active_start_date=20151021, 
		@active_end_date=99991231, 
		@active_start_time=0, 
		@active_end_time=235959, 
		@schedule_uid=N'634ef485-b972-4996-9d08-731f735d76dc'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
EXEC @ReturnCode = msdb.dbo.sp_add_jobserver @job_id = @jobId, @server_name = N'(local)'
IF (@@ERROR <> 0 OR @ReturnCode <> 0) GOTO QuitWithRollback
COMMIT TRANSACTION
GOTO EndSave
QuitWithRollback:
    IF (@@TRANCOUNT > 0) ROLLBACK TRANSACTION
EndSave:

GO


