
/****** Object:  StoredProcedure [dbo].[spGetUserById]    Script Date: 06/05/2012 22:28:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spGetUserById]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spGetUserById]
GO

/****** Object:  StoredProcedure [dbo].[spGetUserById]    Script Date: 06/05/2012 22:28:06 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		T. Brooks (Quarks & Bits Software LLC)
-- Create date: 6/05/2012
-- Description:	Get user by userId
-- =============================================
CREATE PROCEDURE [dbo].[spGetUserById]
@userId INT
AS
BEGIN
	
	-- decrypt our the key so that we can use it to encrypte key columns
	OPEN SYMMETRIC KEY BitsBlogTableKey DECRYPTION
	BY CERTIFICATE BitsBlogEncryptCert
	
	SELECT u.FirstName,
	u.LastName,
	u.MiddleName,
	u.DisplayName,
	u.Email,
	u.Username,
	u.Salt,
	r.RoleName AS RoleName,
	CONVERT(nvarchar(256),DECRYPTBYKEY(uc.AccessKey)) as AccessKey,
	CONVERT(nvarchar(256),DECRYPTBYKEY(uc.SecretKey)) as SecretKey
	FROM Users u
	INNER JOIN Roles r ON r.Id = u.PrimaryRoleId
	INNER JOIN UserAPIAccessCredentials uc ON uc.UserId = u.Id  
	WHERE u.Id = @userId 
		AND IsActive = 1
	
END

GO

