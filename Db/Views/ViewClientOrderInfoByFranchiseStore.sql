USE [CallCenter]
GO

/****** Object:  View [dbo].[ViewClientOrderInfoByFranchiseStore]    Script Date: 2/14/2017 9:14:38 AM ******/
DROP VIEW [dbo].[ViewClientOrderInfoByFranchiseStore]
GO

/****** Object:  View [dbo].[ViewClientOrderInfoByFranchiseStore]    Script Date: 2/14/2017 9:14:38 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE VIEW [dbo].[ViewClientOrderInfoByFranchiseStore]
AS
SELECT
	Fs.FranchiseId
	, F.Name AS FranchiseName
	, Fs.FranchiseStoreId
	, Fs.Name AS FranchiseStoreName
	, C.ClientId
	, Cp.Phone
	, C.FirstName
	, C.LastName
	, Ots.OrderToStoreId
	, Ots.OrderAtoId
	, Ots.StartDatetime AS FirstDatetime
	, Ots.LastStatus
	, Po.Total
FROM OrderToStore Ots
	INNER JOIN PosOrder Po ON Po.PosOrderId = Ots.PosOrderId
	INNER JOIN Client C ON Ots.ClientId = C.ClientId
	INNER JOIN ClientPhone Cp ON Ots.ClientPhoneId = Cp.ClientPhoneId
	INNER JOIN FranchiseStore Fs ON Fs.FranchiseStoreId = Ots.FranchiseStoreId
	INNER JOIN Franchise F ON F.FranchiseId = Fs.FranchiseId
WHERE 
	Ots.OrderAtoId IS NOT NULL


GO


