
EXEC [dbo].[ClientPhoneAddressAndOrderPosMigration] 
	11  --Identificador de la franquicia 
	, 7  --Identificador de la sucursal
	, '33' --Prefijo del tel�fono
	, 8  --Tama�o num�rico del tel�fono sin prefijo
	, 'Closed' --�ltimo estado en el que quedan las �rdenes
	, 'cfac5071-eab4-4817-acab-97ee1506626a' --Identificador del usuario para insertar
  , 1 --1 Migrar �rdenes, 0 no migrar
  , 1 --Identificador del pago (1 - Efectivo)
  , 1 --1 Ejecutar (Commit), cualquier otro valor Rollback

