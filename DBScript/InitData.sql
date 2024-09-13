/* Generate Admin role and Employee Role */
INSERT INTO [DB_ORDER_MANAGEMENT_SQL].[Role]
           ([Name]
           ,[Key])
     VALUES
           ('Admin'
           ,'admin')
GO

INSERT INTO [DB_ORDER_MANAGEMENT_SQL].[Role]
           ([Name]
           ,[Key])
     VALUES
           ('Employee'
           ,'employee')
GO

/* Add Admin info data */
USE [DB_ORDER_MANAGEMENT_SQL]
GO

INSERT INTO [DB_ORDER_MANAGEMENT_SQL].[Employee]
           ([Name]
           ,[PhoneNumber])
     VALUES
           ('Administrator'
           ,'0123456789')
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
           ('Admin'
           ,'c1c224b03cd9bc7b6a86d77f5dace40191766c485cd55dc48caf9ac873335d6f'
           ,(SELECT [Id]
			 FROM [DB_ORDER_MANAGEMENT_SQL].[Role]
			 WHERE [Key]='admin')
           ,(SELECT [Id]
			 FROM [DB_ORDER_MANAGEMENT_SQL].[Employee]
			 WHERE [Name]='Administrator'))
GO