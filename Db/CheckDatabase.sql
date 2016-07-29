SELECT
	Gu.PrimaryPhoneNumber
	, SUM(1) Total
	--, Pn.*
	--, Pn.PhoneNumber 
FROM [AlohaToGo].dbo.Guest Gu
LEFT JOIN [AlohaToGo].dbo.PhoneNumber Pn ON Pn.GuestID = Gu.GuestID
WHERE (AreaCode IS NULL OR AreaCode <> '')
GROUP BY Gu.PrimaryPhoneNumber
ORDER BY Total DESC



/*SELECT 
	FullName
	, SUM(Cnt) Total
FROM(	
	SELECT 
		FirstName + '-' + ISNULL(LastName, '') + '-' + PrimaryPhoneNumber AS FullName 
		, 1 AS Cnt
	FROM Guest
	WHERE FirstName <> '.'
) InV
GROUP BY FullName
ORDER BY Total DESC
GO
*/
/*
SELECT
	PhoneNumber
	, FirstName
	, LastName
FROM(
SELECT
	CASE WHEN Gu.PrimaryPhoneNumber IS NULL OR LTRIM(RTRIM(Gu.PrimaryPhoneNumber)) = '' 
		THEN Pn.AreaCode + Pn.PhoneNumber
	ELSE
		'33' + Gu.PrimaryPhoneNumber 
	END AS PhoneNumber
	, Gu.FirstName
	, Gu.LastName
	-- , Pn.AreaCode
	-- Gu.PrimaryPhoneNumber
	--, SUM(1) Total
	--, Pn.*
	--, Pn.PhoneNumber 
FROM [AlohaToGo].dbo.Guest Gu
LEFT JOIN [AlohaToGo].dbo.PhoneNumber Pn ON Pn.GuestID = Gu.GuestID
WHERE (AreaCode IS NULL OR AreaCode <> '')
-- GROUP BY Gu.PrimaryPhoneNumber
-- ORDER BY Total DESC
) InQy
*/
