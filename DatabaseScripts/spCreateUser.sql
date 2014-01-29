USE [BitsBlog]
GO

/****** Object:  StoredProcedure [dbo].[spCreateUser]    Script Date: 05/11/2012 01:31:16 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spCreateUser]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spCreateUser]
GO

USE [BitsBlog]
GO

/****** Object:  StoredProcedure [dbo].[spCreateUser]    Script Date: 05/11/2012 01:31:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		T. Brooks (Quarks & Bits Software LLC)
-- Create date: 5/10/2012
-- Description:	Create new user.
-- =============================================
CREATE PROCEDURE [dbo].[spCreateUser]
@UUID uniqueidentifier,
@Username NVARCHAR(30),
@Password varchar(256),
@Salt varchar(32),
@Email nvarchar(128),
@FirstName nvarchar(50),
@LastName nvarchar(50),
@MiddleName nvarchar(50) = NULL,
@DisplayName nvarchar(256) = NULL,
@ApplicationName nvarchar(128),
@RoleName nvarchar(256),
@AccessKey nvarchar(256),
@SecretKey nvarchar(256),
@IsActive bit    
AS
BEGIN
	
	DECLARE @ApplicationId INT
	DECLARE @RoleId INT
	DECLARE @NewUserId INT
							
	-- get roleId by role name
	SELECT @RoleId = (SELECT Id 
						FROM Roles 
						WHERE RoleName = @RoleName)	
						
	-- insert new user
	INSERT INTO Users
	VALUES(@UUID,@Username,@Password,@Salt,@Email,@FirstName,@LastName,@MiddleName,@RoleId,GETUTCDATE(),GETUTCDATE(),GETUTCDATE(),@DisplayName,@ApplicationName,@IsActive)
	
	SELECT @NewUserId = SCOPE_IDENTITY()
	
	-- insert API keys
	
	-- decrypt our the key so that we can use it to encrypte key columns
	OPEN SYMMETRIC KEY BitsBlogTableKey DECRYPTION
	BY CERTIFICATE BitsBlogEncryptCert

	DECLARE @AccessBinary VARBINARY(256)
	SET @AccessBinary = ENCRYPTBYKEY(KEY_GUID('BitsBlogTableKey'),@AccessKey)
	
	DECLARE @KeyBinary VARBINARY(256)
	SET @KeyBinary = ENCRYPTBYKEY(KEY_GUID('BitsBlogTableKey'),@SecretKey)
	
	INSERT INTO UserAPIAccessCredentials
	VALUES(@NewUserId,@UUID,@AccessBinary,@KeyBinary)
	
	-- return userId
	SELECT @NewUserId as UserId
END

GO

