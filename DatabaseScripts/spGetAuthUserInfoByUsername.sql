USE [BitsBlog]
GO

/****** Object:  StoredProcedure [dbo].[spGetAuthUserInfoByUsername]    Script Date: 09/07/2012 00:29:00 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spGetAuthUserInfoByUsername]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spGetAuthUserInfoByUsername]
GO

USE [BitsBlog]
GO

/****** Object:  StoredProcedure [dbo].[spGetAuthUserInfoByUsername]    Script Date: 09/07/2012 00:29:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		T. Brooks (Quarks & Bits Software LLC)
-- Create date: 9/06/2012
-- Description:	Returns a limited amount user data in order to validate the user.
-- =============================================
CREATE PROCEDURE [dbo].[spGetAuthUserInfoByUsername]
@username NVARCHAR(30)

AS
BEGIN
	SELECT u.Id,
	u.salt,
	r.RoleName AS RoleName,
	u.Password AS [hash]
	FROM Users u
	INNER JOIN Roles r ON r.Id = u.PrimaryRoleId
	WHERE Username = @username 
		AND IsActive = 1
END

GO


