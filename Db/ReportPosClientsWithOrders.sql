SELECT --TOP 1000
		G.GuestID,
		G.FirstName,
		ISNULL(G.LastName, '') AS LastName,
		G.EmailAddress,
		CASE WHEN LEN(G.PrimaryPhoneNumber) = 8 AND (G.PrimaryPhoneAreaCode <> '33' OR G.PrimaryPhoneAreaCode IS NULL)
			THEN 
				'33' + G.PrimaryPhoneNumber
			ELSE 
				G.PrimaryPhoneAreaCode + G.PrimaryPhoneNumber 
		END AS PhoneNumber,
		ISNULL(G.AddressLine1, 'NE') AS AddressLine1,
		ISNULL(G.AddressLine2, 'NE') AS AddressLine2,
		G.LastActivity,
		COUNT(Ho.OrderID) AS Orders
	FROM [AlohaToGo].[dbo].Guest G
	INNER JOIN [AlohaToGo].[dbo].[HistoricalOrderHeader] Ho ON Ho.GuestID = G.GuestID
	WHERE (LEN(G.PrimaryPhoneAreaCode + G.PrimaryPhoneNumber)  = 10
		OR LEN(G.PrimaryPhoneNumber) = 8 ) 
		AND G.FirstName IS NOT NULL AND G.FirstName <> '.' AND G.FirstName <> 'XXX'
	GROUP BY 
		G.GuestID, G.FirstName, G.LastName, G.EmailAddress, G.PrimaryPhoneNumber, G.PrimaryPhoneAreaCode,
		G.AddressLine1, G.AddressLine2, G.LastActivity
	ORDER BY G.LastActivity