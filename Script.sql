--------------------------------------------------------------------------------
-- АТК-Форум: схема базы данных TransportCompany.
--
-- Скрипт идемпотентный: создаёт базу и таблицы, только если их ещё нет.
-- Существующие данные НЕ удаляются. Для полного сброса базы разработчика
-- используйте отдельный явный скрипт (см. комментарий в конце файла).
--------------------------------------------------------------------------------

USE [master]
GO

IF NOT EXISTS (SELECT * FROM sys.databases WHERE [name] = 'TransportCompany')
BEGIN
    CREATE DATABASE [TransportCompany];
END
GO

USE [TransportCompany]
GO

--------------------------------------------------------------------------------
-- Реестр рейсов
--------------------------------------------------------------------------------
IF OBJECT_ID('[dbo].[TransportRegistry]', 'U') IS NULL
BEGIN
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
    );

    -- Индексы под основные запросы приложения
    CREATE INDEX IX_TransportRegistry_Date ON [dbo].[TransportRegistry] ([Date]);
    CREATE INDEX IX_TransportRegistry_FIO ON [dbo].[TransportRegistry] ([FIO]);
    CREATE INDEX IX_TransportRegistry_GosNumber ON [dbo].[TransportRegistry] ([GosNumber]);
    CREATE INDEX IX_TransportRegistry_Registry ON [dbo].[TransportRegistry] ([Registry]);
END
GO

--------------------------------------------------------------------------------
-- Журнал импорта (одна запись на файл)
--------------------------------------------------------------------------------
IF OBJECT_ID('[dbo].[ImportLog]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ImportLog] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [RegistryNumber] INT NULL,
        [FilePath] NVARCHAR(500) NULL,
        [ImportDate] DATETIME NOT NULL DEFAULT GETDATE(),
        [RecordCount] INT NULL
    );
END
GO

--------------------------------------------------------------------------------
-- Техобслуживание
-- Имена столбцов соответствуют контракту, который использует приложение.
--------------------------------------------------------------------------------
IF OBJECT_ID('[dbo].[Техобслуживание]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Техобслуживание] (
        [id] INT IDENTITY(1,1) PRIMARY KEY,
        [Номер машины] NVARCHAR(20) NOT NULL,
        [Дата последнего ТО] DATE NULL,
        [Пробег (км)] INT NULL,
        [Комментарий] NVARCHAR(500) NULL
    );
END
GO

--------------------------------------------------------------------------------
-- Справочники ТС и водителей
--------------------------------------------------------------------------------
IF OBJECT_ID('[dbo].[Vehicles]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Vehicles] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [RegistrationNumber] NVARCHAR(20) NOT NULL UNIQUE
    );
END
GO

IF OBJECT_ID('[dbo].[Drivers]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[Drivers] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [FullName] NVARCHAR(200) NOT NULL
    );
END
GO

--------------------------------------------------------------------------------
-- Полисы ОСАГО
-- Первичный ключ называется OSAGOId — так его ожидает приложение.
--------------------------------------------------------------------------------
IF OBJECT_ID('[dbo].[OSAGO]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[OSAGO] (
        [OSAGOId] INT IDENTITY(1,1) PRIMARY KEY,
        [VehicleRegistrationNumber] NVARCHAR(20) NOT NULL,
        [PolicyNumber] NVARCHAR(50) NULL,
        [StartDate] DATE NULL,
        [EndDate] DATE NULL,
        CONSTRAINT CK_OSAGO_Dates CHECK ([EndDate] IS NULL OR [StartDate] IS NULL OR [EndDate] >= [StartDate])
    );
END
GO

--------------------------------------------------------------------------------
-- Водительские удостоверения
-- Первичный ключ называется LicenseId — так его ожидает приложение.
--------------------------------------------------------------------------------
IF OBJECT_ID('[dbo].[DriverLicenses]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[DriverLicenses] (
        [LicenseId] INT IDENTITY(1,1) PRIMARY KEY,
        [DriverFullName] NVARCHAR(200) NOT NULL,
        [LicenseNumber] NVARCHAR(50) NULL,
        [IssueDate] DATE NULL,
        [ExpiryDate] DATE NULL,
        CONSTRAINT CK_DriverLicenses_Dates CHECK ([ExpiryDate] IS NULL OR [IssueDate] IS NULL OR [ExpiryDate] >= [IssueDate])
    );
END
GO

--------------------------------------------------------------------------------
-- Тарифы зон — единый источник для всех расчётов приложения.
-- Начальные значения соответствуют действующим тарифам расчёта зарплаты.
--------------------------------------------------------------------------------
IF OBJECT_ID('[dbo].[ZoneSettings]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ZoneSettings] (
        [ZoneId] INT PRIMARY KEY,
        [Cost] DECIMAL(18,2) NOT NULL DEFAULT 0,
        [UpdatedDate] DATETIME DEFAULT GETDATE()
    );

    INSERT INTO [dbo].[ZoneSettings] ([ZoneId], [Cost]) VALUES
        (0, 2700.00),
        (1, 2700.00),
        (2, 3200.00),
        (3, 3600.00),
        (4, 4000.00),
        (5, 5000.00),
        (6, 6000.00),
        (7, 7000.00),
        (8, 8000.00),
        (9, 9000.00),
        (10, 10000.00);
END
GO

--------------------------------------------------------------------------------
-- История изменений тарифов зон
--------------------------------------------------------------------------------
IF OBJECT_ID('[dbo].[ZoneCostHistory]', 'U') IS NULL
BEGIN
    CREATE TABLE [dbo].[ZoneCostHistory] (
        [Id] INT IDENTITY(1,1) PRIMARY KEY,
        [ZoneId] INT NOT NULL,
        [OldCost] DECIMAL(18,2) NULL,
        [NewCost] DECIMAL(18,2) NOT NULL,
        [ChangeDate] DATETIME DEFAULT GETDATE(),
        [ChangedBy] NVARCHAR(100) NULL
    );
END
GO

--------------------------------------------------------------------------------
-- Миграция существующих баз, созданных прежней версией скрипта:
--  - Техобслуживание: [Пробег] -> [Пробег (км)], [Примечание] -> [Комментарий]
--  - OSAGO: [Id] -> [OSAGOId]
--  - DriverLicenses: [Id] -> [LicenseId]
--------------------------------------------------------------------------------
IF COL_LENGTH('[dbo].[Техобслуживание]', 'Пробег') IS NOT NULL
   AND COL_LENGTH('[dbo].[Техобслуживание]', 'Пробег (км)') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Техобслуживание.Пробег', 'Пробег (км)', 'COLUMN';
END
GO

IF COL_LENGTH('[dbo].[Техобслуживание]', 'Примечание') IS NOT NULL
   AND COL_LENGTH('[dbo].[Техобслуживание]', 'Комментарий') IS NULL
BEGIN
    EXEC sp_rename 'dbo.Техобслуживание.Примечание', 'Комментарий', 'COLUMN';
END
GO

IF COL_LENGTH('[dbo].[OSAGO]', 'Id') IS NOT NULL
   AND COL_LENGTH('[dbo].[OSAGO]', 'OSAGOId') IS NULL
BEGIN
    EXEC sp_rename 'dbo.OSAGO.Id', 'OSAGOId', 'COLUMN';
END
GO

IF COL_LENGTH('[dbo].[DriverLicenses]', 'Id') IS NOT NULL
   AND COL_LENGTH('[dbo].[DriverLicenses]', 'LicenseId') IS NULL
BEGIN
    EXEC sp_rename 'dbo.DriverLicenses.Id', 'LicenseId', 'COLUMN';
END
GO

--------------------------------------------------------------------------------
-- Полный сброс базы данных разработчика (НЕ запускать на рабочей базе!):
--
-- USE [master];
-- ALTER DATABASE [TransportCompany] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
-- DROP DATABASE [TransportCompany];
-- ... затем выполнить этот скрипт заново.
--------------------------------------------------------------------------------
