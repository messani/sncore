IF NOT EXISTS ( SELECT * FROM sys.database_principals where name = 'NT AUTHORITY\NETWORK SERVICE' )
BEGIN
 CREATE USER [NT AUTHORITY\NETWORK SERVICE] FOR LOGIN [NT AUTHORITY\NETWORK SERVICE] WITH DEFAULT_SCHEMA=[dbo]
 END
GO
ALTER AUTHORIZATION ON SCHEMA::[db_owner] TO [NT AUTHORITY\NETWORK SERVICE]
GO
EXEC sp_addrolemember N'db_owner', N'NT AUTHORITY\NETWORK SERVICE'
GO
