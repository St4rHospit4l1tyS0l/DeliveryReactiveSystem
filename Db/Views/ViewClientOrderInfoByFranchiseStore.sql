--SELECT
--	Fs.FranchiseStoreId
--	, Fs.Name
--	, Cp.ClientId
--	, Cp.FirstName
--	, Cp.LastName
--	, MIN(Ots.StartDatetime) AS FirstDatetime
--	, Ots.LastStatus
--FROM OrderToStore Ots
--INNER JOIN Client Cp ON Ots.ClientId = Cp.ClientId
--INNER JOIN FranchiseStore Fs ON Fs.FranchiseStoreId = Ots.FranchiseStoreId
----WHERE Ots.LastStatus = 'Closed'
--GROUP BY 
--	Fs.FranchiseStoreId
--	, Fs.Name
--	, Cp.ClientId
--	, Cp.FirstName
--	, Cp.LastName
--	, Ots.LastStatus
--ORDER BY FirstName





--Filtros, sucursal, intervalo de tiempo

--Nombre
--Terlefono.
--Sucursal
--Rango de fechas.
--Producto o pedido.
--Al menos tres niveles.
--Total en pesos.
