-- Created by GitHub Copilot in SSMS - review carefully before executing
INSERT INTO [dbo].[MyOffice_ACPD] (
    [ACPD_SID], 
    [ACPD_Cname], 
    [ACPD_Ename], 
    [ACPD_Email], 
    [ACPD_LoginID],
    [ACPD_LoginPWD],
    [ACPD_Status]
) 
VALUES 
('SID00000000000000001', N'王大明', N'Da-Ming Wang', N'daming@example.com', N'daming_login', N'pwd123', 0),
('SID00000000000000002', N'李小華', N'Xiao-Hua Li', N'xiaohua@example.com', N'xiaohua_login', N'pwd456', 0),
('SID00000000000000003', N'張建國', N'Jian-Guo Zhang', N'jianguo@example.com', N'jianguo_login', N'pwd789', 0);
GO