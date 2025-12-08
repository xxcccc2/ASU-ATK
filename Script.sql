use [master]

IF exists(select * from sys.databases where [name] ='TransportCompany')
begin
alter database [TransportCompany] set single_user with rollback IMMEDIATE;
drop database  [TransportCompany]
end
go
-- создаем БД
create database [TransportCompany]
go


use [TransportCompany]

----------------------------------------------------------
 -- создаем таблицу Перевозки
----------------------------------------------------------
IF OBJECT_ID('[dbo].[Перевозки]', 'U') IS NOT NULL DROP TABLE [dbo].[Перевозки]

CREATE TABLE [dbo].[Перевозки] (
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Дата рейса] [DateTime2]  NULL,
    [ФИО водителя]  [varchar] (50) NULL,
    [Номер машины]  [varchar] (20) NULL,
    [Груз (кг.)] [int] NULL,
    [Расход топлива (литр.)] [int] NULL,
    [Прибыль (руб.)] [int] NULL)
----------------------------------------------------------
-- наполнение данными
----------------------------------------------------------

INSERT INTO [dbo].[Перевозки]
           ([Дата рейса]
           ,[ФИО водителя], 
            [Номер машины]
           ,[Груз (кг.)]
	       ,[Расход топлива (литр.)]
           ,[Прибыль (руб.)]
           )
     VALUES
           (
           '22.01.2024', 'Иванов Иван Иванович' ,'н111кр150', 11000, 130, 65000
		   )
----------------------------------------------------------

INSERT INTO [dbo].[Перевозки]
           ([Дата рейса]
           ,[ФИО водителя], 
            [Номер машины]
           ,[Груз (кг.)]
	       ,[Расход топлива (литр.)]
           ,[Прибыль (руб.)]
           )
     VALUES
           (
           '23.01.2024', 'Петров Петр Петрович' ,'н222кр150', 10000, 160, 75000
		   )

---------------------------------------------------------
INSERT INTO [dbo].[Перевозки]
           ([Дата рейса]
           ,[ФИО водителя], 
            [Номер машины]
           ,[Груз (кг.)]
	       ,[Расход топлива (литр.)]
           ,[Прибыль (руб.)]
           )
     VALUES
           (
           '24.01.2024', 'Федоров Федор Федорович' ,'н333кр150', 15000, 180, 95000
		   )


INSERT INTO [dbo].[Перевозки]
           ([Дата рейса]
           ,[ФИО водителя], 
            [Номер машины]
           ,[Груз (кг.)]
	       ,[Расход топлива (литр.)]
           ,[Прибыль (руб.)]
           )
     VALUES
           (
           '28.01.2024', 'Федоров Федор Федорович' ,'н333кр150', 11500, 110, 115000
		   )


INSERT INTO [dbo].[Перевозки]
           ([Дата рейса]
           ,[ФИО водителя], 
            [Номер машины]
           ,[Груз (кг.)]
	       ,[Расход топлива (литр.)]
           ,[Прибыль (руб.)]
           )
     VALUES
           (
           '18.02.2024', 'Федоров Федор Федорович' ,'н333кр150', 8500, 90, 65000
		   )

INSERT INTO [dbo].[Перевозки]
           ([Дата рейса]
           ,[ФИО водителя], 
            [Номер машины]
           ,[Груз (кг.)]
	       ,[Расход топлива (литр.)]
           ,[Прибыль (руб.)]
           )
     VALUES
           (
           '23.02.2024', 'Петров Петр Петрович' ,'н222кр150', 11000, 150, 85000
		   )

INSERT INTO [dbo].[Перевозки]
           ([Дата рейса]
           ,[ФИО водителя], 
            [Номер машины]
           ,[Груз (кг.)]
	       ,[Расход топлива (литр.)]
           ,[Прибыль (руб.)]
           )
     VALUES
           (
           '17.02.2024', 'Петров Петр Петрович' ,'н222кр150', 16000, 120, 85000
		   )


INSERT INTO [dbo].[Перевозки]
           ([Дата рейса]
           ,[ФИО водителя], 
            [Номер машины]
           ,[Груз (кг.)]
	       ,[Расход топлива (литр.)]
           ,[Прибыль (руб.)]
           )
     VALUES
           (
           '22.02.2024', 'Иванов Иван Иванович' ,'н111кр150', 13500, 130, 75000
		   )
----------------------------------------------------------
INSERT INTO [dbo].[Перевозки]
           ([Дата рейса]
           ,[ФИО водителя], 
            [Номер машины]
           ,[Груз (кг.)]
	       ,[Расход топлива (литр.)]
           ,[Прибыль (руб.)]
           )
     VALUES
           (
           '27.02.2024', 'Иванов Иван Иванович' ,'н111кр150', 15500, 120, 85000
		   )
----------------------------------------------------------
INSERT INTO [dbo].[Перевозки]
           ([Дата рейса]
           ,[ФИО водителя], 
            [Номер машины]
           ,[Груз (кг.)]
	       ,[Расход топлива (литр.)]
           ,[Прибыль (руб.)]
           )
     VALUES
           (
           '29.02.2024', 'Иванов Иван Иванович' ,'н111кр150', 14500, 230, 125000
		   )
----------------------------------------------------------

go

----------------------------------------------------------
-- Таблица TransportRegistry (реестр перевозок)
----------------------------------------------------------
IF OBJECT_ID('[dbo].[TransportRegistry]', 'U') IS NOT NULL DROP TABLE [dbo].[TransportRegistry]

CREATE TABLE [dbo].[TransportRegistry] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [NPP] INT NULL,
    [Date] DATE NULL,
    [FIO] NVARCHAR(200) NULL,
    [NSL] NVARCHAR(50) NULL,
    [GosNumber] NVARCHAR(20) NULL,
    [Tonnage] DECIMAL(18,2) NULL,
    [VehicleType] NVARCHAR(100) NULL,
    [TransportNumber] NVARCHAR(50) NULL,
    [RCLoad] NVARCHAR(100) NULL,
    [Branch] NVARCHAR(100) NULL,
    [DeliveryRegion] NVARCHAR(200) NULL,
    [TripCost] DECIMAL(18,2) NULL,
    [OrderNumber] NVARCHAR(50) NULL,
    [UnloadPoints] INT NULL,
    [LoadPoints] INT NULL,
    [Zone] INT NULL,
    [ExtraStores] DECIMAL(18,2) NULL,
    [ExtraLoad] DECIMAL(18,2) NULL,
    [Supply] DECIMAL(18,2) NULL,
    [NQNumber] NVARCHAR(50) NULL,
    [SumTTK] DECIMAL(18,2) NULL,
    [KmCost] DECIMAL(18,2) NULL,
    [Discount] DECIMAL(18,2) NULL,
    [TotalWithoutVAT] DECIMAL(18,2) NULL,
    [TotalWithVAT] DECIMAL(18,2) NULL,
    [TransportNumber2] NVARCHAR(50) NULL,
    [Registry] NVARCHAR(200) NULL
)
GO

----------------------------------------------------------
-- Таблица Техобслуживание (ТО)
----------------------------------------------------------
IF OBJECT_ID('[dbo].[Техобслуживание]', 'U') IS NOT NULL DROP TABLE [dbo].[Техобслуживание]

CREATE TABLE [dbo].[Техобслуживание] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [Номер машины] NVARCHAR(20) NOT NULL,
    [Дата последнего ТО] DATE NULL,
    [Пробег] INT NULL,
    [Примечание] NVARCHAR(500) NULL
)

INSERT INTO [dbo].[Техобслуживание] ([Номер машины], [Дата последнего ТО], [Пробег])
VALUES ('н111кр150', '2024-09-15', 150000),
       ('н222кр150', '2024-08-20', 120000),
       ('н333кр150', '2024-07-10', 180000)
GO

----------------------------------------------------------
-- Таблица OSAGO (полисы ОСАГО)
-- Столбцы: VehicleRegistrationNumber, PolicyNumber, EndDate
----------------------------------------------------------
IF OBJECT_ID('[dbo].[OSAGO]', 'U') IS NOT NULL DROP TABLE [dbo].[OSAGO]

CREATE TABLE [dbo].[OSAGO] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [VehicleRegistrationNumber] NVARCHAR(20) NOT NULL,
    [PolicyNumber] NVARCHAR(50) NULL,
    [InsuranceCompany] NVARCHAR(200) NULL,
    [StartDate] DATE NULL,
    [EndDate] DATE NULL,
    [Cost] DECIMAL(18,2) NULL
)

INSERT INTO [dbo].[OSAGO] ([VehicleRegistrationNumber], [PolicyNumber], [InsuranceCompany], [StartDate], [EndDate], [Cost])
VALUES ('н111кр150', 'ХХХ-0001234567', 'Росгосстрах', '2024-01-01', '2025-01-01', 15000),
       ('н222кр150', 'ХХХ-0002345678', 'СОГАЗ', '2024-03-15', '2025-03-15', 14500),
       ('н333кр150', 'ХХХ-0003456789', 'Ингосстрах', '2024-06-01', '2025-06-01', 16000)
GO

----------------------------------------------------------
-- Таблица DriverLicenses (водительские удостоверения)
-- Столбцы: DriverFullName, LicenseNumber, ExpiryDate
----------------------------------------------------------
IF OBJECT_ID('[dbo].[DriverLicenses]', 'U') IS NOT NULL DROP TABLE [dbo].[DriverLicenses]

CREATE TABLE [dbo].[DriverLicenses] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [DriverFullName] NVARCHAR(200) NOT NULL,
    [LicenseNumber] NVARCHAR(20) NULL,
    [Category] NVARCHAR(20) NULL,
    [IssueDate] DATE NULL,
    [ExpiryDate] DATE NULL,
    [IssuedBy] NVARCHAR(200) NULL
)

INSERT INTO [dbo].[DriverLicenses] ([DriverFullName], [LicenseNumber], [Category], [IssueDate], [ExpiryDate])
VALUES ('Иванов Иван Иванович', '77 01 123456', 'B, C, CE', '2020-05-15', '2030-05-15'),
       ('Петров Петр Петрович', '77 02 234567', 'B, C', '2019-08-20', '2029-08-20'),
       ('Федоров Федор Федорович', '77 03 345678', 'B, C, CE', '2021-03-10', '2031-03-10')
GO

----------------------------------------------------------
-- Таблица настроек стоимости зон
----------------------------------------------------------
IF OBJECT_ID('[dbo].[ZoneSettings]', 'U') IS NOT NULL DROP TABLE [dbo].[ZoneSettings]

CREATE TABLE [dbo].[ZoneSettings] (
    [ZoneId] INT PRIMARY KEY,
    [Cost] DECIMAL(18,2) NOT NULL DEFAULT 0,
    [UpdatedDate] DATETIME DEFAULT GETDATE()
)

INSERT INTO [dbo].[ZoneSettings] ([ZoneId], [Cost]) VALUES (0, 0.00)
INSERT INTO [dbo].[ZoneSettings] ([ZoneId], [Cost]) VALUES (1, 100.00)
INSERT INTO [dbo].[ZoneSettings] ([ZoneId], [Cost]) VALUES (2, 200.00)
INSERT INTO [dbo].[ZoneSettings] ([ZoneId], [Cost]) VALUES (3, 300.00)
INSERT INTO [dbo].[ZoneSettings] ([ZoneId], [Cost]) VALUES (4, 400.00)
INSERT INTO [dbo].[ZoneSettings] ([ZoneId], [Cost]) VALUES (5, 500.00)
INSERT INTO [dbo].[ZoneSettings] ([ZoneId], [Cost]) VALUES (6, 600.00)
INSERT INTO [dbo].[ZoneSettings] ([ZoneId], [Cost]) VALUES (7, 700.00)
INSERT INTO [dbo].[ZoneSettings] ([ZoneId], [Cost]) VALUES (8, 800.00)
INSERT INTO [dbo].[ZoneSettings] ([ZoneId], [Cost]) VALUES (9, 900.00)
INSERT INTO [dbo].[ZoneSettings] ([ZoneId], [Cost]) VALUES (10, 1000.00)

----------------------------------------------------------
-- Таблица истории изменений стоимости зон
----------------------------------------------------------
IF OBJECT_ID('[dbo].[ZoneCostHistory]', 'U') IS NOT NULL DROP TABLE [dbo].[ZoneCostHistory]

CREATE TABLE [dbo].[ZoneCostHistory] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [ZoneId] INT NOT NULL,
    [OldCost] DECIMAL(18,2) NULL,
    [NewCost] DECIMAL(18,2) NOT NULL,
    [ChangeDate] DATETIME DEFAULT GETDATE(),
    [ChangedBy] NVARCHAR(100) NULL
)

GO
