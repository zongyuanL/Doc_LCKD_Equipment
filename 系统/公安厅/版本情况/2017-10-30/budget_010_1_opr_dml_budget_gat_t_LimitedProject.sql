--������Ŀ���ѡ       
alter table budget_gat_t_LimitedProject add ProjectQuota nvarchar2(1) default 0 ;
--�����ʧ�ܺ���лع�ɾ����Ŀ���ѡ���м�ʧ�ܺ�ִ�У���������ִ��
ALTER TABLE budget_gat_t_LimitedProject DROP COLUMN ProjectQuota    