USE [CallCenter]
GO

/****** Object:  View [dbo].[ViewMonthlySales]    Script Date: 1/3/2017 8:40:06 PM ******/
DROP VIEW [dbo].[ViewMonthlySales]
GO

/****** Object:  View [dbo].[ViewMonthlySales]    Script Date: 1/3/2017 8:40:06 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[ViewMonthlySales]
AS
SELECT
	ISNULL((OrderYear * 100 + OrderMonth), -1) AS [Key]
	, ISNULL(OrderYear, 1900) AS OrderYear
	, ISNULL(OrderMonth, 0) AS OrderMonth
	, COUNT(1) AS SalesPerMonth
	, SUM(Total) AS TotalPerMonth
	, Franchise
	, FranchiseStoreId
	, FranchiseStore
FROM(
	SELECT 
		YEAR(Ots.StartDatetime) AS OrderYear
		, MONTH(Ots.StartDatetime) AS OrderMonth
		, ISNULL(Po.Total, 0) AS Total
		, F.Name AS Franchise
		, Fs.FranchiseStoreId
		, Fs.Name AS FranchiseStore
	FROM 
		OrderToStore Ots
	INNER JOIN PosOrder Po ON Ots.PosOrderId = Po.PosOrderId
	INNER JOIN FranchiseStore Fs ON Fs.FranchiseStoreId = Ots.FranchiseStoreId
	INNER JOIN Franchise F ON F.FranchiseId = Fs.FranchiseId
	WHERE 
		Ots.LastStatus IN (SELECT Ose.OrderStatusEnd FROM OrderStatusEnd Ose)
		AND Ots.InputType = 0
) Tin
GROUP BY OrderYear, OrderMonth, FranchiseStoreId, FranchiseStore, Franchise





GO


