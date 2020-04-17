USE [dbSupermarket]
GO

INSERT INTO [dbo].[Category]
           ([name])
     VALUES
           ('Fruit')

INSERT INTO [dbo].[Category]
           ([name])
     VALUES
           ('Vegetable')

INSERT INTO [dbo].[Category]
           ([name])
     VALUES
           ('Bakery')
GO


INSERT INTO [dbo].[Product]
           ([name]
           ,[quantity_in_package]
           ,[unit_measurement_id]
           ,[category_id])
     VALUES
           ('Apple'
           ,50
           ,1  -- Unity
           ,1)
GO


INSERT INTO [dbo].[Product]
           ([name]
           ,[quantity_in_package]
           ,[unit_measurement_id]
           ,[category_id])
     VALUES
           ('Lettuce'
           ,10
           ,4  -- Kilogram
           ,2)
GO


INSERT INTO [dbo].[Product]
           ([name]
           ,[quantity_in_package]
           ,[unit_measurement_id]
           ,[category_id])
     VALUES
           ('Bread'
           ,20
           ,4  -- Kilogram
           ,3)
GO