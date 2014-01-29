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
-- Author:		T. Brooks (Quarks & Bits Software LLC)
-- Create date: 9/17/2012
-- Description: Check if username exists in the database for active users.
-- =============================================
Alter PROCEDURE dbo.spCheckActiveUsersForUsername
@Username NVARCHAR(30)

AS
BEGIN

	SELECT 1 AS DoesExist
	WHERE EXISTS(SELECT null FROM Users
					WHERE Username = @Username 
						AND IsActive = 1)
END
GO
