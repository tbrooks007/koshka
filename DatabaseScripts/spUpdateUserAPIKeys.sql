-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		T.Brooks
-- Create date: 10/16/2012
-- Description:	Updates users API keys.
-- =============================================
CREATE PROCEDURE dbo.spUpdateUserAPIKeys
@UserId INT,
@AccessKey nvarchar(256),
@SecretKey nvarchar(256)
AS
BEGIN

	DECLARE @CurrentTimestamp DATETIME
    SET @CurrentTimestamp = GETUTCDATE()
    
	-- decrypt our the key so that we can use it to encrypte key columns
	OPEN SYMMETRIC KEY BitsBlogTableKey DECRYPTION
	BY CERTIFICATE BitsBlogEncryptCert

	DECLARE @AccessBinary VARBINARY(256)
	SET @AccessBinary = ENCRYPTBYKEY(KEY_GUID('BitsBlogTableKey'),@AccessKey)
	
	DECLARE @KeyBinary VARBINARY(256)
	SET @KeyBinary = ENCRYPTBYKEY(KEY_GUID('BitsBlogTableKey'),@SecretKey)
	
	--update
	UPDATE UserAPIAccessCredentials
	SET AccessKey = @AccessBinary,
		SecretKey = @KeyBinary,
		DateUpdated = @CurrentTimestamp
	WHERE userid = @UserId
	
END
GO
