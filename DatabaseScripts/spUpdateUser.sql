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
-- Description:	Update user information.
-- =============================================
alter PROCEDURE dbo.spUpdateUser
@UserId INT,
@Email nvarchar(128),
@FirstName nvarchar(50),
@LastName nvarchar(50),
@MiddleName nvarchar(50) = NULL,
@DisplayName nvarchar(256) = NULL,
@RoleName nvarchar(256),
@IsActive bit 

AS
BEGIN
    -- get roleId by role name
    DECLARE @RoleId INT
    DECLARE @CurrentTimestamp DATETIME
    SET @CurrentTimestamp = GETUTCDATE()	
	SELECT @RoleId = (SELECT Id 
						FROM Roles 
						WHERE RoleName = @RoleName)	
					
	-- update user
	UPDATE Users
	SET Email = @Email,
		FirstName = @FirstName,
		LastName = @LastName,
		MiddleName = @MiddleName,
		DisplayName = @DisplayName,
		PrimaryRoleId = @RoleId,
		IsActive = @IsActive,
		DateUpdated = @CurrentTimestamp
	WHERE Id = @UserId
END
GO
