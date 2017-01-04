DECLARE 
	@GuestId INT, 
	@ClientId INT, 
	@PhoneId INT,
	@AddressId INT,
	@FirstName NVARCHAR(100),
	@LastName NVARCHAR(200),
	@Email NVARCHAR(500),
	@PhoneNumber VARCHAR(30),
	@MainAddress NVARCHAR(500),
	@Reference NVARCHAR(500),
	@CountPhoneIns INT = 0,
	@CountClientIns INT = 0,
	@CountRelClPhIns INT = 0,
	@CountAddressIns INT = 0,
	@CountRelAdPhIns INT = 0,
	@CountDone INT = 1

DECLARE MIGRATION_CURSOR CURSOR 
  LOCAL STATIC READ_ONLY FORWARD_ONLY
FOR 
SELECT
		G.GuestID,
		G.FirstName,
		ISNULL(G.LastName, ''),
		G.EmailAddress,
		CASE WHEN LEN(G.PrimaryPhoneNumber) = 8 AND (G.PrimaryPhoneAreaCode <> '33' OR G.PrimaryPhoneAreaCode IS NULL)
			THEN 
				'33' + G.PrimaryPhoneNumber
			ELSE 
				G.PrimaryPhoneAreaCode + G.PrimaryPhoneNumber 
		END AS PhoneNumber,
		G.AddressLine1,
		G.AddressLine2
	FROM [AlohaToGo].[dbo].Guest G
WHERE (LEN(G.PrimaryPhoneAreaCode + G.PrimaryPhoneNumber)  = 10
	OR LEN(G.PrimaryPhoneNumber) = 8 ) AND G.FirstName IS NOT NULL AND G.FirstName <> '.' AND G.FirstName <> 'XXX'

OPEN MIGRATION_CURSOR
FETCH NEXT FROM MIGRATION_CURSOR INTO @GuestId, @FirstName, @LastName, @Email, @PhoneNumber, @MainAddress, @Reference

SET NOCOUNT ON
SET XACT_ABORT ON

BEGIN TRANSACTION
WHILE @@FETCH_STATUS = 0
BEGIN
	SELECT @ClientId = NULL, @PhoneId = NULL, @AddressId = NULL

	SELECT @PhoneId = ClientPhoneId FROM [CallCenter].[dbo].ClientPhone WHERE Phone = @PhoneNumber

	IF @PhoneId IS NULL
	BEGIN
		INSERT INTO [CallCenter].[dbo].ClientPhone(Phone, UserIdIns, DatetimeIns)
		VALUES(@PhoneNumber, 'cfac5071-eab4-4817-acab-97ee1506626a', GETDATE())
		SET @PhoneId = SCOPE_IDENTITY()
		SET @CountPhoneIns = @CountPhoneIns + 1
	END
	
	SELECT @ClientId = C.ClientId FROM [CallCenter].[dbo].Client C 
		INNER JOIN [CallCenter].[dbo].RelClientPhoneClient Rpc ON Rpc.ClientId = C.ClientId
		INNER JOIN [CallCenter].[dbo].ClientPhone Cp ON Cp.ClientPhoneId = Rpc.ClientPhoneId
	WHERE Phone = @PhoneNumber AND C.FirstName = @FirstName AND C.LastName = @LastName

	IF @ClientId IS NULL
	BEGIN
		INSERT INTO [CallCenter].[dbo].Client(FirstName, LastName, Email, DatetimeIns)
		VALUES(@FirstName, @LastName, @Email, GETDATE())
		SET @ClientId = SCOPE_IDENTITY()
		SET @CountClientIns = @CountClientIns + 1
	END

	IF NOT EXISTS (SELECT * FROM [CallCenter].[dbo].[RelClientPhoneClient] WHERE ClientPhoneId = @PhoneId AND ClientId = @ClientId)
	BEGIN
		INSERT INTO [CallCenter].[dbo].RelClientPhoneClient(ClientId, ClientPhoneId)
		VALUES(@ClientId, @PhoneId)
		SET @CountRelClPhIns = @CountRelClPhIns + 1
	END
	
	SELECT @AddressId = AddressId FROM [CallCenter].[dbo].Address WHERE MainAddress = @MainAddress

	IF @AddressId IS NULL
	BEGIN
		INSERT INTO [CallCenter].[dbo].Address(MainAddress, Reference, CountryName, IsMap)
		VALUES(@MainAddress, @Reference, 'México', 1)
		SET @AddressId = SCOPE_IDENTITY()
		SET @CountAddressIns = @CountAddressIns + 1
	END
	
	IF NOT EXISTS (SELECT * FROM [CallCenter].[dbo].[RelClientPhoneAddress] WHERE ClientPhoneId = @PhoneId AND AddressId = @AddressId)
	BEGIN
		INSERT INTO [CallCenter].[dbo].RelClientPhoneAddress(AddressId, ClientPhoneId)
		VALUES(@AddressId, @PhoneId)
		SET @CountRelAdPhIns = @CountRelAdPhIns + 1
	END

	PRINT 'Contador: ' + CAST(@CountDone AS VARCHAR)

	SELECT 
		'Closed',
		Ho.LastModifiedTimestamp,
		@PhoneId,
		@ClientId,
		@AddressId,
		11,

	FROM [AlohaToGo].[dbo].[HistoricalOrderHeader] Ho
		



	SET @CountDone = @CountDone + 1
	FETCH NEXT FROM MIGRATION_CURSOR INTO @GuestId, @FirstName, @LastName, @Email, @PhoneNumber, @MainAddress, @Reference

	PRINT 'Teléfonos insertados: ' + CAST(@CountPhoneIns AS VARCHAR)
	PRINT 'Clientes insertados: ' + CAST(@CountClientIns AS VARCHAR)
	PRINT 'Rel Tel-Cli insertadas: ' + CAST(@CountRelClPhIns AS VARCHAR)
	PRINT 'Direcciones insertadas: ' + CAST(@CountAddressIns AS VARCHAR)
	PRINT 'Rel Dir-Cli insertadas: ' + CAST(@CountRelAdPhIns AS VARCHAR)

END
COMMIT