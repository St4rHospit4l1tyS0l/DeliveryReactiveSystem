USE [CallCenter]
GO

/****** Object:  View [dbo].[ViewPosOrderInfo]    Script Date: 2/1/2017 1:19:20 PM ******/
DROP VIEW [dbo].[ViewPosOrderInfo]
GO

/****** Object:  View [dbo].[ViewPosOrderInfo]    Script Date: 2/1/2017 1:19:20 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[ViewPosOrderInfo]
AS
SELECT
	Fs.FranchiseId
	, Fs.FranchiseStoreId
	, Ots.OrderToStoreId
	, Ots.StartDatetime AS FirstDatetime
	, Poi.ItemId
	, Poi.Name
	, Poi.Price
FROM OrderToStore Ots
	INNER JOIN FranchiseStore Fs ON Fs.FranchiseStoreId = Ots.FranchiseStoreId
	INNER JOIN PosOrder Po ON Po.PosOrderId = Ots.PosOrderId
	INNER JOIN PosOrderItem Poi ON Poi.PosOrderId = Ots.PosOrderId
WHERE 
	Ots.OrderAtoId IS NOT NULL



GO


