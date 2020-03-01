Use master

DROP Database IF EXISTS ClubBaistSystem
CREATE Database ClubBaistSystem

USE "aspnet-ClubBaistSystem-53bc9b9d-9d6a-45d4-8429-2a2761773502"

/* TABLE CREATION */

GO
DROP TABLE IF EXISTS MembershipApplication
GO
CREATE TABLE MembershipApplication
(
	MembershipApplicationId INT IDENTITY(1,1) PRIMARY KEY,
	LastName				NVARCHAR(25),
	FirstName				NVARCHAR(25),
	[Address]				NVARCHAR(25),
	PostalCode				NVARCHAR(7),
	City					NVARCHAR(25),
	DateOfBirth				DATE,
	Shareholder1			NVARCHAR(50),
	Shareholder2			NVARCHAR(50),
	MembershipType			NVARCHAR(15),
	Occupation				NVARCHAR(25),
	CompanyName				NVARCHAR(25),
	CompanyAddress			NVARCHAR(25),
	CompanyPostalCode		NVARCHAR(7),
	CompanyCity				NVARCHAR(25),
	Email					NVARCHAR(25),
	Phone					NVARCHAR(10), 
	AlternatePhone			NVARCHAR(10) CONSTRAINT DF_MemApp_AlternatePhone DEFAULT '0000000000',
	ApplicationStatus		NVARCHAR(15) CONSTRAINT DF_MemApp_ApplicationStatus DEFAULT 'Onhold'
)

GO
DROP TABLE IF EXISTS TeeTime
GO
CREATE TABLE TeeTime
(
	[Date]				DATE,
	[Time]				TIME,
	Golfer1Id			NVARCHAR(450) NULL,
	Golfer2Id			NVARCHAR(450) NULL,
	Golfer3Id			NVARCHAR(450) NULL,
	Golfer4Id			NVARCHAR(450) NULL,
	BookerId			NVARCHAR(450) NULL,
	Golfer1CheckedIn	Bit NOT NULL,
	Golfer2CheckedIn	Bit NOT NULL,
	Golfer3CheckedIn	Bit NOT NULL,
	Golfer4CheckedIn	Bit NOT NULL,
						PRIMARY KEY	([Date],[Time]),
						FOREIGN KEY (Golfer1Id) REFERENCES AspNetUsers(Id),
						FOREIGN KEY (Golfer2Id) REFERENCES AspNetUsers(Id),
						FOREIGN KEY (Golfer3Id) REFERENCES AspNetUsers(Id),
						FOREIGN KEY (Golfer4Id) REFERENCES AspNetUsers(Id)
)

GO
DROP TABLE IF EXISTS StandingTeeTimeRequest
GO
CREATE TABLE StandingTeeTimeRequest
(
	StartDate			DATE,
	EndDate				DATE,
	[Time]				TIME,
	[DayOfWeek]			VARCHAR(9),
	Shareholder1Id		NVARCHAR(450) NULL,
	Shareholder2Id		NVARCHAR(450) NULL,
	Shareholder3Id		NVARCHAR(450) NULL,
	Shareholder4Id		NVARCHAR(450) NULL,
	BookerId			NVARCHAR(450) NULL,
						PRIMARY KEY (StartDate,EndDate,[Time],[DayOfWeek]),
						FOREIGN KEY (Shareholder1Id) REFERENCES AspNetUsers(Id),
						FOREIGN KEY (Shareholder2Id) REFERENCES AspNetUsers(Id),
						FOREIGN KEY (Shareholder3Id) REFERENCES AspNetUsers(Id),
						FOREIGN KEY (Shareholder4Id) REFERENCES AspNetUsers(Id)
)

GO
DROP TABLE IF EXISTS AccountEntry
GO
CREATE TABLE AccountEntry
(
	MemberId		   NVARCHAR(450),
	WhenCharged		   DATETIME,
	WhenMade		   DATETIME,
	Amount			   MONEY,
	EntryDescription   NVARCHAR(100)
					   PRIMARY KEY (MemberId,WhenCharged),	
					   FOREIGN KEY (MemberId) REFERENCES AspNetUsers(Id),
)

GO
DROP TABLE IF EXISTS Rounds
GO
CREATE TABLE Rounds
(
	GolferId			NVARCHAR(450),
	CourseName			NVARCHAR(25),
	[Date]				DATE,
	Hole				INT,
	Score				INT,
	Rating				DECIMAL,
	Slope				DECIMAL,
						FOREIGN KEY (GolferId) REFERENCES AspNetUsers(Id),
						PRIMARY KEY (GolferId, CourseName, [Date], Hole),
)

/* STORED PROCEDURES */

GO 
DROP PROCEDURE IF EXISTS GetTeeTimesByDate
GO 
CREATE PROCEDURE GetTeeTimesByDate(@Date		DATE = NULL)
AS 
	 � DECLARE		@ReturnCode INT
	 � SET			@ReturnCode = 1

	 � SELECT * FROM TeeTime
	 � WHERE [Date] = @Date

	 � IF @@ERROR = 0
			SET @ReturnCode = 0
	 � ELSE 
		 � �RAISERROR('GetTeeTmesByDate - SELECT error from TeeTime Table',16,1)

	 � Return @ReturnCode

	 /*Approved*/

GO 
DROP PROCEDURE IF EXISTS CheckInGolfer
GO 
CREATE PROCEDURE CheckInGolfer	  (@Date			 DATE,
								   @Time			 TIME,
								   @golfer1CheckedIn BIT,
								   @golfer2CheckedIn BIT,
								   @golfer3CheckedIn BIT,
								   @golfer4CheckedIn BIT)
AS 
	 � DECLARE		@ReturnCode INT
	 � SET			@ReturnCode = 1

	  UPDATE TeeTime
	   SET	  Golfer1CheckedIn = @golfer1CheckedIn,
			  Golfer2CheckedIn = @golfer2CheckedIn,
			  Golfer3CheckedIn = @golfer3CheckedIn,
			  Golfer4CheckedIn = @golfer4CheckedIn
	   WHERE  [Date]  = @Date
	   AND	  [Time]  = @Time	

	 � IF @@ERROR = 0
			SET @ReturnCode = 0
	 � ELSE 
		 � �RAISERROR('CheckInGolfer - INSERT error from TeeTime Table',16,1)

	 � Return @ReturnCode 


GO 
DROP PROCEDURE IF EXISTS EditTeeTime
GO 
CREATE PROCEDURE EditTeeTime(	   @Date		DATE,
								   @Time		TIME,
								   @golfer1		VARCHAR(25) = NULL,
								   @golfer2		VARCHAR(25) = NULL,
								   @golfer3		VARCHAR(25) = NULL,
								   @golfer4		VARCHAR(25) = NULL,
								   @bookerId	NVARCHAR(450) = NULL)
AS 
	 � DECLARE		@ReturnCode INT
	 � SET			@ReturnCode = 1

	 � UPDATE TeeTime
	   SET	  Golfer1Id = (SELECT Id FROM AspNetUsers WHERE FullName = @golfer1),
			  Golfer2Id = (SELECT Id FROM AspNetUsers WHERE FullName = @golfer2),
			  Golfer3Id = (SELECT Id FROM AspNetUsers WHERE FullName = @golfer3),
			  Golfer4Id = (SELECT Id FROM AspNetUsers WHERE FullName = @golfer4),
			  BookerId =  @bookerId
	   WHERE  [Date]  = @Date 
	   AND	  [Time]  = @Time	

	 � IF @@ERROR = 0
			SET @ReturnCode = 0
	 � ELSE 
		 � �RAISERROR('EditTeeTime - INSERT error from TeeTime Table',16,1)

	 � Return @ReturnCode 
	 /*Approved*/
GO 
DROP PROCEDURE IF EXISTS GenerateDailyTeeSheet
GO 
CREATE PROCEDURE GenerateDailyTeeSheet (@NumberOfDay INT)
AS 
	 � DECLARE		@ReturnCode INT
	 � SET			@ReturnCode = 1

	 � DECLARE @start DATETIME, @end DATETIME, @counter int = 0

		/*The start and end of a golf session*/ 
		SET @start = '7:00AM';
		SET @end   = '7:01PM';    

		WHILE @start < @end
		BEGIN
			INSERT INTO TeeTime
			VALUES 
			(
			(SELECT CONVERT(VARCHAR(10), getdate() + @NumberOfDay, 111)),
			@start,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			0,
			0,
			0,
			0
			)

			SET @counter = @counter + 1	
			/*Add 7 minutes for first increment and 8 minutes for the next*/
			IF @counter % 2 = 0
				SET @start = DATEADD(MINUTE, 8, @start)
			ELSE
				SET @start = DATEADD(MINUTE, 7, @start)
		END
		SET @counter = 0
		/* Input all the submitted standing tee time requests */
		WHILE @counter < (SELECT count(Shareholder1Id) FROM StandingTeeTimeRequest WHERE Shareholder1Id IS NOT NULL AND GETDATE() BETWEEN StartDate AND EndDate AND [DayOfWeek] = FORMAT(GETDATE(), 'dddd'))
			BEGIN
				SET @counter = @counter + 1;
		
				WITH GrabGolfer AS
				(
					SELECT *,(ROW_NUMBER() OVER(ORDER BY Shareholder1Id)) AS RowNumber FROM StandingTeeTimeRequest WHERE Shareholder1Id IS NOT NULL AND GETDATE() BETWEEN StartDate AND EndDate AND [DayOfWeek] = FORMAT(GETDATE(), 'dddd')
				)

				UPDATE TeeTime
					SET		Golfer1Id = (SELECT Golfer1Id FROM GrabGolfer WHERE RowNumber = @counter),
							Golfer2Id = (SELECT Golfer2Id FROM GrabGolfer WHERE RowNumber = @counter),
							Golfer3Id = (SELECT Golfer3Id FROM GrabGolfer WHERE RowNumber = @counter),
							Golfer4Id = (SELECT Golfer4Id FROM GrabGolfer WHERE RowNumber = @counter)	
					WHERE 
							[Date] = (SELECT CONVERT(VARCHAR(10), getdate(), 111))
					AND		[Time] = (SELECT [Time] FROM GrabGolfer WHERE RowNumber = @counter)
			END
			
	 � IF @@ERROR = 0
			SET @ReturnCode = 0
	 � ELSE 
		 � �RAISERROR('GenerateDailyTeeSheet - INSERT error from TeeTime Table',16,1)

	 � Return @ReturnCode

GO 
DROP PROCEDURE IF EXISTS RetireDailyTeeSheet
GO 
CREATE PROCEDURE RetireDailyTeeSheet
AS 
	 � DECLARE		@ReturnCode INT
	 � SET			@ReturnCode = 1

	   DELETE FROM TeeTime 
       WHERE [Date] = (SELECT CONVERT(VARCHAR(11), dateadd(day,datediff(day,1,GETDATE()),0)))

	 � IF @@ERROR = 0
			SET @ReturnCode = 0
	 � ELSE 
		 � �RAISERROR('RetireDailyTeeSheet - DELETE error from TeeTime Table',16,1)

	 � Return @ReturnCode

GO 
DROP PROCEDURE IF EXISTS GetAvailableStandingTeeTimeRequestsByDay
GO 
CREATE PROCEDURE GetAvailableStandingTeeTimeRequestsByDay(@DayOfWeek		VARCHAR(10) = NULL)
AS 
	 � DECLARE		@ReturnCode INT
	 � SET			@ReturnCode = 1

	   SELECT	[Time],
				[DayOfWeek],
				(SELECT FullName FROM AspNetUsers WHERE Id = Shareholder1Id),
				(SELECT FullName FROM AspNetUsers WHERE Id = Shareholder2Id),
				(SELECT FullName FROM AspNetUsers WHERE Id = Shareholder3Id),
				(SELECT FullName FROM AspNetUsers WHERE Id = Shareholder4Id)
	   FROM		StandingTeeTimeRequest
       WHERE	[DayOfWeek] = @DayOfWeek

	 � IF @@ERROR = 0
			SET @ReturnCode = 0
	 � ELSE 
		 � �RAISERROR('GetAvailableStandingTeeTimeRequestsByDay - SELECT error from StandingTeeTimeRequest Table',16,1)

	 � Return @ReturnCode 
	 
GO 
DROP PROCEDURE IF EXISTS EditStandingTeeTimeRequest
GO 
CREATE PROCEDURE EditStandingTeeTimeRequest(@StartDate		DATE,
											@EndDate		DATE,
											@DayOfWeek		VARCHAR(10), 
											@Time			TIME,
											@shareholder1	VARCHAR(25) = NULL,
											@shareholder2	VARCHAR(25) = NULL,
											@shareholder3	VARCHAR(25) = NULL,
											@shareholder4	VARCHAR(25) = NULL,
											@bookerId		NVARCHAR(450))
AS 
	 DECLARE		@ReturnCode INT
	 SET			@ReturnCode = 1

	 UPDATE StandingTeeTimeRequest
	   SET	  Shareholder1Id = (SELECT Id FROM AspNetUsers WHERE FullName = @shareholder1),
			  Shareholder2Id = (SELECT Id FROM AspNetUsers WHERE FullName = @shareholder2),
			  Shareholder3Id = (SELECT Id FROM AspNetUsers WHERE FullName = @shareholder3),
			  Shareholder4Id = (SELECT Id FROM AspNetUsers WHERE FullName = @shareholder4),
			  StartDate    = @StartDate,
			  EndDate      = @EndDate,
			  BookerId	   = @bookerId 
	   WHERE  [Time]	   = @Time	
	   AND	  [DayOfWeek]  = @DayOfWeek

	 IF @@ERROR = 0
			SET @ReturnCode = 0
	 ELSE 
			RAISERROR('EditStandingTeeTimeRequest - INSERT error from StandingTeeTimeRequest Table',16,1)

	 Return @ReturnCode 

GO 
DROP PROCEDURE IF EXISTS GenerateStandingTeeTimeRequestsCalender
GO 
CREATE PROCEDURE GenerateStandingTeeTimeRequestsCalender (@day VARCHAR(10))
AS 
	 DECLARE		@ReturnCode INT
	 SET			@ReturnCode = 1

	 DECLARE @start DATETIME, @end DATETIME, @counter INT = 0

		SET @start = '7:00AM';
		SET @end   = '7:01PM';    

		WHILE @start < @end
		BEGIN
			INSERT INTO StandingTeeTimeRequest
			VALUES 
			(
			'01-JAN-2020',
			'31-DEC-2020',
			@start,
			@day,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL
			)

			SET @counter = @counter + 1	

			IF @counter % 2 = 0
				SET @start = DATEADD(MINUTE, 8, @start)
			ELSE
				SET @start = DATEADD(MINUTE, 7, @start)
		END

	 IF @@ERROR = 0
			SET @ReturnCode = 0
	 ELSE 
		 RAISERROR('GenerateStandingTeeTimeRequestsCalender - INSERT error from StandingTeeTimeRequest Table',16,1)

	 Return @ReturnCode

GO 
DROP PROCEDURE IF EXISTS GetUserNameFromId
GO 
CREATE PROCEDURE GetUserNameFromId(@userId NVARCHAR(450))
AS 
	 � DECLARE		@ReturnCode INT
	 � SET			@ReturnCode = 1

	 � SELECT 
			UserName 
	   FROM AspNetUsers
       WHERE Id = @userId

	 � IF @@ERROR = 0
			SET @ReturnCode = 0
	 � ELSE 
		 � �RAISERROR('GetUserNameFromId - SELECT error from TeeTime Table',16,1)

	 � Return @ReturnCode 

GO 
DROP PROCEDURE IF EXISTS GetUserFromUserName
GO 
CREATE PROCEDURE GetUserFromUserName(@UserName NVARCHAR(450))
AS 
	 � DECLARE		@ReturnCode INT
	 � SET			@ReturnCode = 1

	 �SELECT 
			UserId, 
			[FullName],
			AspNetRoles.Name
	   FROM AspNetUsers
	   INNER JOIN AspNetUserRoles
	   ON AspNetUsers.Id = AspNetUserRoles.UserId
	   INNER JOIN AspNetRoles
	   ON AspNetRoles.Id = AspNetUserRoles.RoleId
       WHERE AspNetUsers.UserName = @UserName

	 � IF @@ERROR = 0
			SET @ReturnCode = 0
	 � ELSE 
		 � �RAISERROR('GetUserFromUserName - SELECT error from TeeTime Table',16,1)

	 � Return @ReturnCode 

GO
DROP PROCEDURE IF EXISTS GetTeeTimeByDateAndTime
GO
CREATE PROCEDURE GetTeeTimeByDateAndTime (@Date Date, 
							              @Time Time)
AS 
	 � DECLARE		@ReturnCode INT
	 � SET			@ReturnCode = 1

	   SELECT [Date],
			  [Time], 
			  Golfer1Id, 
			  Golfer2Id,
			  Golfer3Id,
			  Golfer4Id,
			  BookerId,
			  Golfer1CheckedIn,
			  Golfer2CheckedIn,
			  Golfer3CheckedIn,
			  Golfer4CheckedIn
	   FROM   TeeTime
	   WHERE  [Date] = @Date
		 AND  [Time] = @Time
	
	 � IF @@ERROR = 0
			SET @ReturnCode = 0
	 � ELSE 
		 � �RAISERROR('GetTeeTimeByDateAndTime - SELECT error from TeeTime Table',16,1)

	 � Return @ReturnCode 


GO
DROP PROCEDURE IF EXISTS RecordMembershipApplication
GO
CREATE PROCEDURE RecordMembershipApplication (@lastName NVARCHAR(25),@firstName NVARCHAR(25),@address NVARCHAR(25),@postalCode NVARCHAR(7),
											  @city NVARCHAR(25),@dateOfBirth DATE,@shareholder1 NVARCHAR(50),@shareholder2 NVARCHAR(50),
										      @membershipType NVARCHAR(25),@occupation NVARCHAR(25),@companyName NVARCHAR(25),@companyAddress NVARCHAR(25),
										      @companyPostalCode NVARCHAR(7),@companyCity NVARCHAR(25),@email NVARCHAR(25),@phone NVARCHAR(10),
											  @alternatePhone NVARCHAR(10) NULL)

AS 
	 � DECLARE		@ReturnCode INT
	 � SET			@ReturnCode = 1
	 
	   INSERT 
		INTO MembershipApplication
			(LastName,FirstName,[Address],PostalCode,City,DateOfBirth,Shareholder1,Shareholder2,MembershipType,Occupation,
			CompanyName,CompanyAddress,CompanyPostalCode,CompanyCity,Email,Phone,AlternatePhone)
			VALUES
			(@lastName,@firstName,@address,@postalCode,@city,@dateOfBirth,@shareholder1,@shareholder2,@membershipType,@occupation,
			@companyName,@companyAddress,@companyPostalCode,@companyCity,@email,@phone,@alternatePhone)
�	   
       IF @@ERROR = 0
			SET @ReturnCode = 0
	 �ELSE 
		� �	RAISERROR('RecordMembershipApplication - INSERT error from MembershipApplication Table',16,1)

	 � Return @ReturnCode 

GO
DROP PROCEDURE IF EXISTS GetAllOnholdMembershipApplications
GO
CREATE PROCEDURE GetAllOnholdMembershipApplications
AS 
	 � DECLARE		@ReturnCode INT
	 � SET			@ReturnCode = 1

	   
	   SELECT MembershipApplicationId,LastName,FirstName,[Address],PostalCode,
			  City,DateOfBirth,Shareholder1,Shareholder2,MembershipType,Occupation,
			  CompanyName,CompanyAddress,CompanyPostalCode,CompanyCity,Email,
			  Phone,AlternatePhone
	   FROM   MembershipApplication
	   WHERE  ApplicationStatus = 'Onhold'
	
       IF @@ERROR = 0
			SET @ReturnCode = 0
	 � ELSE 
		� �	RAISERROR('GetAllOnholdMembershipApplications - SELECT error from MembershipApplication Table',16,1)

	 � Return @ReturnCode 

GO
DROP PROCEDURE IF EXISTS GetAllWaitlistedMembershipApplications
GO
CREATE PROCEDURE GetAllWaitlistedMembershipApplications
AS 
	 � DECLARE		@ReturnCode INT
	 � SET			@ReturnCode = 1

	   
	   SELECT MembershipApplicationId,LastName,FirstName,[Address],PostalCode,
			  City,DateOfBirth,Shareholder1,Shareholder2,MembershipType,Occupation,
			  CompanyName,CompanyAddress,CompanyPostalCode,CompanyCity,Email,
			  Phone,AlternatePhone
	   FROM   MembershipApplication
	   WHERE  ApplicationStatus = 'Waitlist'
	
       IF @@ERROR = 0
			SET @ReturnCode = 0
	 � ELSE 
		� �	RAISERROR('GetAllWaitlistedMembershipApplications - SELECT error from MembershipApplication Table',16,1)

	 � Return @ReturnCode 
GO
DROP PROCEDURE IF EXISTS GetMembershipApplication
GO
CREATE PROCEDURE GetMembershipApplication(@membershipApplicationId INT)
AS 
	 � DECLARE		@ReturnCode INT
	 � SET			@ReturnCode = 1

	   SELECT LastName,FirstName,[Address],PostalCode,City,DateOfBirth,
			  Shareholder1,Shareholder2,MembershipType,Occupation,
			  CompanyName,CompanyAddress,CompanyPostalCode,CompanyCity,Email,
			  Phone,AlternatePhone,ApplicationStatus
	   FROM   MembershipApplication
	   WHERE  MembershipApplicationId = @membershipApplicationId	
	
       IF @@ERROR = 0
			SET @ReturnCode = 0
	 � ELSE 
		� �	RAISERROR('GetMembershipApplication - SELECT error from MembershipApplication Table',16,1)

	 � Return @ReturnCode 


GO
DROP PROCEDURE IF EXISTS CancelMembershipApplication
GO
CREATE PROCEDURE CancelMembershipApplication(@membershipApplicationId INT)
AS 
	 � DECLARE		@ReturnCode INT
	 � SET			@ReturnCode = 1

	   UPDATE MembershipApplication
	   SET	  ApplicationStatus = 'Camcel'
	   WHERE  MembershipApplicationId = @membershipApplicationId	
	
       IF @@ERROR = 0
			SET @ReturnCode = 0
	 � ELSE 
		� �	RAISERROR('CancelMembershipApplication - UPDATE error from MembershipApplication Table',16,1)

	 � Return @ReturnCode 


GO
DROP PROCEDURE IF EXISTS WaitlistMembershipApplication
GO
CREATE PROCEDURE WaitlistMembershipApplication(@membershipApplicationId INT)
AS 
	 DECLARE		@ReturnCode INT
	 SET			@ReturnCode = 1

	   UPDATE MembershipApplication
	   SET	  ApplicationStatus = 'Waitlist'
	   WHERE  MembershipApplicationId = @membershipApplicationId	
	
       IF @@ERROR = 0
			SET @ReturnCode = 0
	 ELSE 
		RAISERROR('WaitlistMembershipApplication - UPDATE error from MembershipApplication Table',16,1)

	 Return @ReturnCode 

GO
DROP PROCEDURE IF EXISTS GetMemberAccount
GO
CREATE PROCEDURE GetMemberAccount(@memberId NVARCHAR(450))
AS 
	 DECLARE		@ReturnCode INT
	 SET			@ReturnCode = 1
	 
	   SELECT	WhenCharged, 
				WhenMade,
				Amount,
				EntryDescription,
				(SELECT SUM(Amount) FROM AccountEntry 
				Where MemberId = @memberId) AS Balance
	   FROM		AccountEntry 
	   WHERE	MemberId = @memberId

       IF @@ERROR = 0
			SET @ReturnCode = 0
	 ELSE 
		RAISERROR('GetMemberAccount - SELECT error from AccountEntry Table',16,1)

	 Return @ReturnCode

/* SELECT STATEMENTS Dry Template */

GO
GenerateDailyTeeSheet 0
GO
GenerateDailyTeeSheet 1
GO
GenerateDailyTeeSheet 2
GO
GenerateDailyTeeSheet 3
GO
GenerateDailyTeeSheet 4
GO
GenerateDailyTeeSheet 5
GO
GenerateDailyTeeSheet 6

GO
GenerateStandingTeeTimeRequestsCalender 'Monday'
GO
GenerateStandingTeeTimeRequestsCalender 'Tuesday'
GO
GenerateStandingTeeTimeRequestsCalender 'Wednesday'
GO
GenerateStandingTeeTimeRequestsCalender 'Thursday'
GO
GenerateStandingTeeTimeRequestsCalender 'Friday'
GO
GenerateStandingTeeTimeRequestsCalender 'Saturday'
GO
GenerateStandingTeeTimeRequestsCalender 'Sunday'
GO

GO
DROP PROCEDURE IF EXISTS RecordGolferScore
GO
CREATE PROCEDURE RecordGolferScore (@golferId NVARCHAR(450), @courseName NVARCHAR(25), @date DATE, 
									@hole INT, @score INT, @rating DECIMAL, @slope DECIMAL)
AS 
	 DECLARE		@ReturnCode INT
	 SET			@ReturnCode = 1

	   INSERT 
	   INTO Rounds
	   VALUES 
	   (
			@golferId,
			@courseName,
			@date,
			@hole,
			@score,
			@rating,
			@slope
	   )

	IF @@ERROR = 0
			SET @ReturnCode = 0
	 ELSE 
		RAISERROR('RecordGolferScore - INSERT error from Rounds Table',16,1)

	 Return @ReturnCode 

	 RecordGolferScore '879c1fa7-8992-452f-ade2-7ae1f41a3e95', 'Club BAIST Golf Course', '21-FEB-2020', 4, 6, 70.6, 128

	 SELECT * FROM Rounds


	 SELECT TOP 20 * FROM Rounds  
	 WHERE GolferId = '879c1fa7-8992-452f-ade2-7ae1f41a3e95'
		   AND Score != 0
	 ORDER BY [Date] DESC, Hole DESC

	 SELECT * FROM AspNetUsers




	 INSERT INTO AspNetUsers (Id, UserName, NormalizedUserName, Email, EmailConfirmed, NormalizedEmail, PasswordHash, FullName, PhoneNumberConfirmed, TwoFactorEnabled, LockoutEnabled,AccessFailedCount)
	 VALUES ('879c1fa7-8992-452f-ade2-7ae1f41a3e99','bobdoe@gmail.com', 'BOBDOE@GMAIL.COM', 'bobdoe@gmail.com', '1','BOBDOE@GMAIL.COM', 'AQAAAAEAACcQAAAAELuCNBt0tV2D6rRxGbZOQjBTeU73TsOsu1HuKw31KMg3JsQGUo1VqXEyMQgBrk2cKQ==','Bob Doe',1,0,0,0)

		UPDATE AspNetUsers
		Set SecurityStamp = 'TRFXGFMGJDP2FZZ7GONDKIOKS5GTYPWO',
			ConcurrencyStamp = '91a1f1d1-7a65-4fe7-9275-7c8077777984'
		WHERE Id = 'c0ff790c-ea82-4378-acb3-0cb8e787ab09'

		SELECT AspNetUsers.Id, AspNetUsers.FullName, AspNetRoles.Name
		FROM AspNetUsers
		INNER JOIN AspNetUserRoles
		ON AspNetUsers.Id = AspNetUserRoles.UserId
		INNER JOIN AspNetRoles
		ON AspNetRoles.Id = AspNetUserRoles.RoleId
		
		INSERT 
		INTO AspNetUserRoles
		(UserId,RoleId)
		VALUES
		(@userId, 
		(SELECT TOP 1 AspNetRoles.Id
		FROM AspNetUserRoles
		INNER JOIN AspNetRoles
		ON AspNetRoles.Id = AspNetUserRoles.RoleId
		Where Name = 'Shareholder')
		)

		SELECT AspNetUsers.Id
		FROM AspNetUsers
		INNER JOIN AspNetUserRoles
		ON AspNetUsers.Id = AspNetUserRoles.UserId
		Where AspNetUsers.Id = 'c0ff790c-ea82-4378-acb3-0cb8e787ab09'


GO
	DROP PROCEDURE IF EXISTS CreateNewAccount
GO
	CREATE PROCEDURE CreateNewAccount(@newMemberId NVARCHAR(450), @userName NVARCHAR(256), @normalizedUserName NVARCHAR(256),
									@passwordHash NVARCHAR(MAX), @fullName NVARCHAR(50), @userType NVARCHAR(15), @membershipApplicationId INT)
	AS 
	 DECLARE		@ReturnCode INT
	 SET			@ReturnCode = 1
	 DECLARE		@yearlyfees INT 
	 SET			@yearlyfees = 0

	IF @userType = 'Shareholder'
		SET @yearlyfees = 3500
	ELSE 
		SET @yearlyfees = 5000

	 INSERT INTO AspNetUsers 
	 (Id, UserName, 
	 NormalizedUserName, Email, 
	 EmailConfirmed, NormalizedEmail, 
	 PasswordHash, FullName, 
	 PhoneNumberConfirmed, TwoFactorEnabled, 
	 LockoutEnabled,AccessFailedCount, 
	 SecurityStamp, ConcurrencyStamp)
	 VALUES 
	 (@newMemberId, @userName, @normalizedUserName, 
	  @userName, 1 , @normalizedUserName, 
	  @passwordHash, @fullName, 1, 0, 0, 0,
	  'TRFXGFMGJDP2FZZ7GONDKIOKS5GTYPWO', 
	  '91a1f1d1-7a65-4fe7-9275-7c8077777984' )
	 
	 INSERT INTO AspNetUserRoles
	 (UserId,RoleId)
	 VALUES
	(@newMemberId, 
	(SELECT TOP 1 AspNetRoles.Id
		FROM AspNetUserRoles
		INNER JOIN AspNetRoles
		ON AspNetRoles.Id = AspNetUserRoles.RoleId
		Where Name = @userType))

	UPDATE MembershipApplication
	SET	  ApplicationStatus = 'Approve'
	WHERE  MembershipApplicationId = @membershipApplicationId	

	INSERT INTO AccountEntry
	VALUES
	(@newMemberId,
	 GETDATE(),
	 GETDATE(),
	 @yearlyfees,
	 'Yearly Membership Fees')
	 
	 IF @@ERROR = 0
			SET @ReturnCode = 0
	 ELSE 
		RAISERROR('CreateNewAccount - INSERT error from AspNetUser Table',16,1)

	 Return @ReturnCode 
	 GO
	 sp_help AspNetUsers
	 GO
	 sp_help MembershipApplication

	 GetMemberAccount '879c1fa7-8992-452f-ade2-7ae1f41a3e99'

	 SELECT * FROM AspNetUsers

	 exec sp_executesql N'SELECT TOP(1) [a].[Id], [a].[AccessFailedCount], [a].[ConcurrencyStamp], [a].[Email], [a].[EmailConfirmed], [a].[FullName], [a].[LockoutEnabled], [a].[LockoutEnd], [a].[NormalizedEmail], [a].[NormalizedUserName], [a].[PasswordHash], [a].[PhoneNumber], [a].[PhoneNumberConfirmed], [a].[SecurityStamp], [a].[TwoFactorEnabled], [a].[UserName]
FROM [AspNetUsers] AS [a]
WHERE (([a].[NormalizedUserName] = @__normalizedUserName_0) AND ([a].[NormalizedUserName] IS NOT NULL AND @__normalizedUserName_0 IS NOT NULL)) OR ([a].[NormalizedUserName] IS NULL AND @__normalizedUserName_0 IS NULL)',N'@__normalizedUserName_0 nvarchar(256)',@__normalizedUserName_0=N'REBECCADOE@GMAIL.COM'