USE [dbSupermarket]
GO

CREATE TABLE [dbo].[Category](
	[category_id] [identifier] IDENTITY(1,1) NOT NULL,
	[name] [names] NOT NULL,
		
	PRIMARY KEY ([category_id]),
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Product](
	[product_id] [int] Identity(1,1) NOT NULL,
	[name] [varchar](144) NOT NULL,
	[quantity_in_package] [smallint] NOT NULL,
	[unit_measurement_id] [int] NOT NULL,
	[category_id] int not null,

	PRIMARY KEY ([product_id]),
    FOREIGN KEY ([category_id]) REFERENCES Category([category_id])
) ON [PRIMARY]
GO


