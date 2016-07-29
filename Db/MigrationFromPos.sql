SET NOCOUNT ON;  

DECLARE @PhoneNumber NVARCHAR(15)
	, @FirstName NVARCHAR(30)
	, @LastName NVARCHAR(30)
	, @EmailAddress NVARCHAR(50)
	, @ClientPhoneId INT
	, @ClientId INT

DECLARE GuestCursor CURSOR FOR
SELECT DISTINCT 
	PhoneNumber
	, FirstName
	, LastName
	, EmailAddress
FROM(
	SELECT
		CASE WHEN Gu.PrimaryPhoneNumber IS NULL OR LTRIM(RTRIM(Gu.PrimaryPhoneNumber)) = '' 
			THEN Pn.AreaCode + Pn.PhoneNumber
		ELSE
			'33' + Gu.PrimaryPhoneNumber 
		END AS PhoneNumber
		, Gu.FirstName
		, ISNULL(Gu.LastName, '') AS LastName
		, Gu.EmailAddress
	FROM [AlohaToGo].dbo.Guest Gu
	LEFT JOIN [AlohaToGo].dbo.PhoneNumber Pn ON Pn.GuestID = Gu.GuestID
	WHERE (AreaCode IS NULL OR AreaCode <> '')
) InQy
WHERE FirstName IS NOT NULL AND FirstName <> '.' AND FirstName <> 'XXX' AND PhoneNumber IS NOT NULL AND LEN(PhoneNumber) = 10

OPEN GuestCursor
FETCH NEXT FROM GuestCursor INTO @PhoneNumber, @FirstName, @LastName, @EmailAddress

WHILE @@FETCH_STATUS = 0  
BEGIN
	SET @ClientPhoneId = NULL
	SET @ClientId = NULL

	BEGIN TRANSACTION
	
		SELECT @ClientPhoneId = Cp.ClientPhoneId
		FROM [CallCenter].dbo.ClientPhone Cp
		WHERE Cp.Phone = @PhoneNumber
	
		IF @ClientPhoneId IS NULL
		BEGIN
			INSERT INTO [CallCenter].dbo.ClientPhone (Phone, UserIdIns, DatetimeIns)
			VALUES(@PhoneNumber, 'cfac5071-eab4-4817-acab-97ee1506626a', GETDATE())
			SET @ClientPhoneId = SCOPE_IDENTITY()

			PRINT '***********>>>>'
			PRINT @ClientPhoneId
			PRINT '<<<<***********'
		END

		IF NOT EXISTS( 
			SELECT * FROM Client C 
				INNER JOIN RelClientPhoneClient RcPc ON RcPc.ClientId = C.ClientId
				INNER JOIN ClientPhone Cp ON Cp.ClientPhoneId = RcPc.ClientPhoneId
			WHERE C.FirstName = @FirstName AND C.LastName = @LastName AND Cp.Phone = @PhoneNumber)
		BEGIN
			INSERT INTO [CallCenter].dbo.Client (FirstName, LastName, Email, DatetimeIns)
			VALUES(@FirstName, @LastName, @EmailAddress, GETDATE())
			SET @ClientId = SCOPE_IDENTITY()

			INSERT INTO [CallCenter].dbo.RelClientPhoneClient(ClientId, ClientPhoneId)
			VALUES(@ClientId, @ClientPhoneId)

			PRINT '----------->>>>'
			PRINT @ClientId
			PRINT @ClientPhoneId
			PRINT '<<<<-----------'

		END

	COMMIT TRANSACTION
	FETCH NEXT FROM GuestCursor INTO @PhoneNumber, @FirstName, @LastName, @EmailAddress
END

CLOSE GuestCursor
DEALLOCATE GuestCursor
GO