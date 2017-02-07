SELECT 
	COUNT(*) 
FROM Client
GO
SELECT 
	COUNT(*) 
FROM ClientPhone
GO

DELETE FROM Recurrence
GO
DELETE FROM OrderToStoreLog
GO
DELETE FROM OrderToStore
GO
DELETE FROM RelClientPhoneClient
GO
DELETE FROM RelClientPhoneAddress
GO
DELETE FROM FranchiseCoverageLog
GO
DELETE FROM FranchiseCoverage
GO
DELETE FROM FranchiseDataFile
GO
DELETE FROM FranchiseDataVersion
GO
DELETE FROM FranchiseData
GO
DELETE FROM PosOrderItem
GO
DELETE FROM PosOrder
GO
DELETE FROM Recurrence
GO
DELETE FROM StoreAddressDistribution
GO
DELETE FROM StoreMessageDate
GO
DELETE FROM StoreMessage
GO
DELETE FROM Company
GO
DELETE FROM FranchiseStore
GO
DELETE FROM FranchiseStoreGeoMap
GO
DELETE FROM FranchiseStoreOffLine
GO
DELETE FROM InfoCallCenter
GO
DELETE FROM InfoClientTerminalVersion
GO
DELETE FROM InfoClientTerminalFranchise
GO
DELETE FROM InfoClientTerminal
GO
DELETE FROM InfoServer
GO
DELETE FROM FranchiseButton
GO
DELETE FROM Franchise
GO
DELETE FROM Address
GO
DELETE FROM ClientPhone
GO
DELETE FROM Client
GO
DELETE FROM AspNetUserRoles WHERE UserId IN (SELECT Id FROM AspNetUsers WHERE UserName NOT IN ('manager'))
GO
DELETE FROM UserDetail WHERE Id IN (SELECT Id FROM AspNetUsers Anu WHERE UserName NOT IN ('manager'))
GO
DELETE FROM AspNetUsers WHERE UserName NOT IN ('manager')
GO
