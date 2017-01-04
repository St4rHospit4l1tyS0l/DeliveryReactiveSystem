
EXEC [dbo].[ClientPhoneAddressAndOrderPosMigration] 
	11  --Identificador de la franquicia 
	, 7  --Identificador de la sucursal
	, '33' --Prefijo del teléfono
	, 8  --Tamaño numérico del teléfono sin prefijo
	, 'Closed' --Último estado en el que quedan las órdenes
	, 'cfac5071-eab4-4817-acab-97ee1506626a' --Identificador del usuario para insertar
  , 1 --1 Migrar órdenes, 0 no migrar
  , 1 --Identificador del pago (1 - Efectivo)
  , 1 --1 Ejecutar (Commit), cualquier otro valor Rollback

