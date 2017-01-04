USE [CallCenter]
GO

/****** Object:  StoredProcedure [dbo].[MigratePosOrdersByOrderId]    Script Date: 11/23/2016 1:06:50 PM ******/
DROP PROCEDURE [dbo].[MigratePosOrdersByOrderId]
GO

/****** Object:  StoredProcedure [dbo].[MigratePosOrdersByOrderId]    Script Date: 11/23/2016 1:06:50 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[MigratePosOrdersByOrderId]
	@OrderId AS INT	
	,@PosOrderId AS INT 
AS
BEGIN
	SET NOCOUNT ON;


END

GO


