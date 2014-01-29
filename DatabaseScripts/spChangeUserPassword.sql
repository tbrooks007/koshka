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
-- Create date: 10/14/2012
-- Description:	Update salt and password for user.
-- =============================================
CREATE PROCEDURE dbo.spChangeUserPassword
@UserId INT,
@Password varchar(256),
@Salt varchar(32)
AS
BEGIN
	
	DECLARE @CurrentTimestamp DATETIME
    SET @CurrentTimestamp = GETUTCDATE()
    
    -- update user
	UPDATE Users
	SET Password = @Password,
		Salt = @Salt,
		DateUpdated = @CurrentTimestamp
	WHERE Id = @UserId
END
GO
