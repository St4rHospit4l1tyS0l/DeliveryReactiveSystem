ALTER TABLE FranchiseStore ADD StoreEmail NVARCHAR(500) 
GO
ALTER TABLE FranchiseStore ADD HasSendEmailWhenNewOrder BIT 
GO
UPDATE FranchiseStore SET StoreEmail = ''
GO
UPDATE FranchiseStore SET HasSendEmailWhenNewOrder = 0
GO
ALTER TABLE FranchiseStore ALTER COLUMN StoreEmail NVARCHAR(500) NOT NULL 
GO
ALTER TABLE FranchiseStore ALTER COLUMN HasSendEmailWhenNewOrder BIT NOT NULL
GO


/****** Object:  Table [dbo].[OrderToStoreEmail]    Script Date: 10/2/2017 11:48:34 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderToStoreEmail](
	[OrderToStoreEmailId] [bigint] IDENTITY(1,1) NOT NULL,
	[OrderToStoreId] [bigint] NOT NULL,
	[TriesToSend] [int] NOT NULL,
	[IsSent] [bit] NOT NULL,
	[SendTimestamp]	[datetime2] NULL
 CONSTRAINT [PK_OrderToStoreEmail] PRIMARY KEY CLUSTERED 
(
	[OrderToStoreEmailId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[OrderToStoreEmail]  WITH CHECK ADD  CONSTRAINT [FK_OrderToStoreEmail_OrderToStore] FOREIGN KEY([OrderToStoreId])
REFERENCES [dbo].[OrderToStore] ([OrderToStoreId])
GO
ALTER TABLE [dbo].[OrderToStoreEmail] CHECK CONSTRAINT [FK_OrderToStoreEmail_OrderToStore]
GO


IF NOT EXISTS (SELECT * FROM Setting WHERE GroupName = 'STORE' AND KeyName = 'TIME_SEND_ORDER_EMAIL')
BEGIN
	INSERT INTO Setting(GroupName, KeyName, Value, Description)
		VALUES('STORE', 'TIME_SEND_ORDER_EMAIL', '30', 'Intervalo para enviar correos de pedidos enviados')
END
GO

IF NOT EXISTS (SELECT * FROM Setting WHERE GroupName = 'STORE' AND KeyName = 'MAX_TRIES_SEND_ORDER_EMAIL')
BEGIN
	INSERT INTO Setting(GroupName, KeyName, Value, Description)
		VALUES('STORE', 'MAX_TRIES_SEND_ORDER_EMAIL', '20', 'Número máximo de intentos para enviar el correo electrónico de nuevos pedidos')
END
GO

IF NOT EXISTS (SELECT * FROM Setting WHERE GroupName = 'STORE' AND KeyName = 'EMAIL_SETTINGS')
BEGIN
	INSERT INTO Setting(GroupName, KeyName, Value, Description)
		VALUES('STORE', 'EMAIL_SETTINGS', 
		'{Title: "Pedido #{0} del Call Center", TemplatePath: "C:\\Projects\\SHS\\DeliverySystem\\Virtual\\DeliveryReactiveSystem\\Deploy\\TemplateEmail.html", Port: 587, Host: "smtp.office365.com", EnableSsl: true, Username: "callcenter@mmmx.mx", Password: "nk", Sender: "callcenter@mmmx.mx"}', 
		'Configuración para enviar correos, incluye nombre completo y ruta de la plantilla a emplear para enviar correos')
END
GO

IF NOT EXISTS (SELECT * FROM Setting WHERE GroupName = 'STORE' AND KeyName = 'ENABLE_SEND_EMAIL')
BEGIN
	INSERT INTO Setting(GroupName, KeyName, Value, Description)
		VALUES('STORE', 'ENABLE_SEND_EMAIL', 
		'false', 
		'Habilita o deshabilita el envío de correos')
END
GO

