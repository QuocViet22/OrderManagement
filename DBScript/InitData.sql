USE [DB_ORDER_MANAGEMENT_SQL]
GO

INSERT INTO [DB_ORDER_MANAGEMENT_SQL].[Employee]
           ([Name]
           ,[PhoneNumber])
     VALUES
           ('Administrator'
           ,'0123456789')
GO

USE [DB_ORDER_MANAGEMENT_SQL]
GO

INSERT INTO [DB_ORDER_MANAGEMENT_SQL].[Employee]
           ([Name]
           ,[PhoneNumber])
     VALUES
           ('Employee1'
           ,'0123456789')
GO

USE [DB_ORDER_MANAGEMENT_SQL]
GO

INSERT INTO [DB_ORDER_MANAGEMENT_SQL].[Employee]
           ([Name]
           ,[PhoneNumber])
     VALUES
           ('Employee2'
           ,'0123456789')
GO

USE [DB_ORDER_MANAGEMENT_SQL]
GO

/* Generate Admin role and Employee Role */
INSERT INTO [DB_ORDER_MANAGEMENT_SQL].[Role]
           ([Name]
           ,[Key])
     VALUES
           ('Admin'
           ,'Admin')
GO

INSERT INTO [DB_ORDER_MANAGEMENT_SQL].[Role]
           ([Name]
           ,[Key])
     VALUES
           ('Employee'
           ,'employee')
GO

/* Insert account */
USE [DB_ORDER_MANAGEMENT_SQL]
GO

INSERT INTO [DB_ORDER_MANAGEMENT_SQL].[Account]
           ([UserName]
           ,[Password]
           ,[RoleId]
           ,[EmployeeId])
     VALUES
           ('Employee2'
           ,'Employee2'
           ,'3455252F-5A82-409B-8323-13214B3727E2'
           ,'830EF51E-B640-4D78-827F-593A01BE051B')
GO