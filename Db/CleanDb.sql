SELECT 
	COUNT(*) 
FROM Client

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
DELETE FROM Address
WHERE AddressId NOT IN (SELECT AddressId FROM FranchiseStore)
GO
DELETE FROM ClientPhone
GO
DELETE FROM Client
GO
