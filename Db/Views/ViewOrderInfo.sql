USE [CallCenter]
GO

/****** Object:  View [dbo].[ViewOrderInfo]    Script Date: 3/15/2017 11:28:46 AM ******/
DROP VIEW [dbo].[ViewOrderInfo]
GO

/****** Object:  View [dbo].[ViewOrderInfo]    Script Date: 3/15/2017 11:28:46 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[ViewOrderInfo]
AS
SELECT        
	Ots.OrderToStoreId
	, Fr.Name AS FranchiseName
	, Fs.Name AS FranchiseStoreName
	, Ots.StartDatetime
	, Ots.LastStatus
	, Cl.Phone
	, Ci.FirstName + ' ' + Ci.LastName AS FullName
	, ISNULL(Ad.RegionNameA, '') + ' ' + 
	ISNULL(Ad.RegionNameB, '') + ' ' +
	ISNULL(Ad.RegionNameC, '') + ' ' + 
	ISNULL(Ad.MainAddress, '') + ' ' + 
	ISNULL(Ad.ExtIntNumber, '') + ' ' + 
	ISNULL(Ad.Reference, '') AS [Address]
	, Po.Total
	, Apu.UserName
FROM  OrderToStore Ots 
	INNER JOIN Franchise Fr ON Ots.FranchiseId = Fr.FranchiseId
	INNER JOIN FranchiseStore Fs ON Ots.FranchiseStoreId = Fs.FranchiseStoreId 
	INNER JOIN ClientPhone Cl ON Ots.ClientPhoneId = Cl.ClientPhoneId
	INNER JOIN Client Ci ON Ots.ClientId = Ci.ClientId
	INNER JOIN Address Ad ON Ots.AddressId = Ad.AddressId
	INNER JOIN PosOrder Po ON Ots.PosOrderId = Po.PosOrderId 
	INNER JOIN AspNetUsers Apu ON Ots.UserInsId = Apu.Id



GO


