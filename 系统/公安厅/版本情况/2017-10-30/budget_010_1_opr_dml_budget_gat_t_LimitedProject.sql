<<<<<<< HEAD
--增加项目库可选       
alter table budget_gat_t_LimitedProject add ProjectQuota nvarchar2(1) default 0 ;
--如程序失败后进行回滚删除项目库可选，切记失败后执行，不是立马执行
=======
--增加项目库可选       
alter table budget_gat_t_LimitedProject add ProjectQuota nvarchar2(1) default 0 ;
--如程序失败后进行回滚删除项目库可选，切记失败后执行，不是立马执行
>>>>>>> c103756b5f3f9d91c2c34515b335fd3f88abaa8b
ALTER TABLE budget_gat_t_LimitedProject DROP COLUMN ProjectQuota    