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
           ('Admin'
           ,'Admin'
           ,'21CF6E2B-E041-4EAA-A37B-4364E7261942'
           ,'EA2B955F-73F9-45E6-B343-E69AEE1D0EE5')
GO

select * from [DB_ORDER_MANAGEMENT_SQL].Role;
select * from [DB_ORDER_MANAGEMENT_SQL].Employee;

select * from [DB_ORDER_MANAGEMENT_SQL].Account;

delete [DB_ORDER_MANAGEMENT_SQL].Account where Id = '90DC1D4A-A825-416B-AE00-0744EB0ADF54'