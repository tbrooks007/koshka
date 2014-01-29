/****** Object:  Login [BitsDev]    Script Date: 06/11/2012 21:39:56 ******/
IF  EXISTS (SELECT * FROM sys.server_principals WHERE name = N'BitsDev')
DROP LOGIN [BitsDev]
GO

/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [BitsDev]    Script Date: 06/11/2012 21:39:56 ******/
CREATE LOGIN [BitsDev] WITH PASSWORD=N'W©®¾%®¯*^
Ý¹[<YÅ£¯²ÝðL', DEFAULT_DATABASE=[BitsBlog], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO

ALTER LOGIN [BitsDev] DISABLE
GO

