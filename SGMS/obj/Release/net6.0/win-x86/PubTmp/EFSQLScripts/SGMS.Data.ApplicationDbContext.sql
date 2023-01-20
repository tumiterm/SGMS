IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221201134000_Add-migration')
BEGIN
    CREATE TABLE [Attachments] (
        [AttachmentId] uniqueidentifier NOT NULL,
        [AssociativeKey] uniqueidentifier NOT NULL,
        [Type] int NOT NULL,
        [File] nvarchar(max) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [CreatedOn] nvarchar(max) NULL,
        [ModifiedOn] nvarchar(max) NULL,
        [ModifiedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_Attachments] PRIMARY KEY ([AttachmentId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221201134000_Add-migration')
BEGIN
    CREATE TABLE [Contacts] (
        [ContactId] uniqueidentifier NOT NULL,
        [Email] nvarchar(max) NULL,
        [Phone] nvarchar(10) NOT NULL,
        [Alternative] nvarchar(10) NULL,
        CONSTRAINT [PK_Contacts] PRIMARY KEY ([ContactId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221201134000_Add-migration')
BEGIN
    CREATE TABLE [Districts] (
        [DistrictId] uniqueidentifier NOT NULL,
        [DistrictName] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Districts] PRIMARY KEY ([DistrictId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221201134000_Add-migration')
BEGIN
    CREATE TABLE [EmergencyUsers] (
        [Id] uniqueidentifier NOT NULL,
        [Role] int NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [Phone] nvarchar(10) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [CreatedOn] nvarchar(max) NULL,
        [ModifiedOn] nvarchar(max) NULL,
        [ModifiedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_EmergencyUsers] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221201134000_Add-migration')
BEGIN
    CREATE TABLE [Matches] (
        [MatchId] uniqueidentifier NOT NULL,
        [FirstTeamId] uniqueidentifier NOT NULL,
        [SecondTeamId] uniqueidentifier NOT NULL,
        [RefereeId] uniqueidentifier NOT NULL,
        [PlayerId] uniqueidentifier NULL,
        [Score1] int NULL,
        [Score2] int NULL,
        [Status] int NULL,
        [TournamentId] uniqueidentifier NOT NULL,
        [GameDay] datetime2 NOT NULL,
        [Venue] nvarchar(max) NULL,
        [AddPostGameInfo] bit NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [CreatedOn] nvarchar(max) NULL,
        [ModifiedOn] nvarchar(max) NULL,
        [ModifiedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_Matches] PRIMARY KEY ([MatchId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221201134000_Add-migration')
BEGIN
    CREATE TABLE [Municipalities] (
        [MunicipalityId] uniqueidentifier NOT NULL,
        [DistrictId] uniqueidentifier NOT NULL,
        [MunicipalityName] nvarchar(max) NOT NULL,
        CONSTRAINT [PK_Municipalities] PRIMARY KEY ([MunicipalityId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221201134000_Add-migration')
BEGIN
    CREATE TABLE [Players] (
        [PlayerId] uniqueidentifier NOT NULL,
        [TeamId] uniqueidentifier NOT NULL,
        [PlayerName] nvarchar(max) NOT NULL,
        [JerseyNumber] int NOT NULL,
        [PlayerLastName] nvarchar(max) NOT NULL,
        [IDNumber] nvarchar(13) NOT NULL,
        [RSAIDCopy] nvarchar(max) NULL,
        [DOB] nvarchar(max) NOT NULL,
        [Photo] nvarchar(max) NULL,
        [Position] int NOT NULL,
        [CardNumber] nvarchar(max) NULL,
        [Phone] nvarchar(10) NOT NULL,
        [AlternativePhone] nvarchar(10) NULL,
        [Email] nvarchar(max) NULL,
        [StreetName] nvarchar(max) NULL,
        [City] nvarchar(max) NULL,
        [Province] int NOT NULL,
        [Gender] int NOT NULL,
        [PostalCode] nvarchar(4) NULL,
        [IsActive] bit NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [CreatedOn] nvarchar(max) NULL,
        [ModifiedOn] nvarchar(max) NULL,
        [ModifiedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_Players] PRIMARY KEY ([PlayerId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221201134000_Add-migration')
BEGIN
    CREATE TABLE [Selections] (
        [Id] uniqueidentifier NOT NULL,
        [MatchId] uniqueidentifier NOT NULL,
        [TeamId] uniqueidentifier NOT NULL,
        [PLayerId] uniqueidentifier NOT NULL,
        [Position] int NOT NULL,
        [Number] int NOT NULL,
        [IsSubstitute] int NOT NULL,
        [FirstTeamId] uniqueidentifier NOT NULL,
        [SecondTeamId] uniqueidentifier NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [CreatedOn] nvarchar(max) NULL,
        [ModifiedOn] nvarchar(max) NULL,
        [ModifiedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_Selections] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221201134000_Add-migration')
BEGIN
    CREATE TABLE [Teams] (
        [TeamId] uniqueidentifier NOT NULL,
        [TeamName] nvarchar(max) NOT NULL,
        [TeamLogo] nvarchar(max) NULL,
        [Description] nvarchar(max) NULL,
        [DistrictId] int NOT NULL,
        [MunicipalityId] int NOT NULL,
        [TeamUser] nvarchar(max) NULL,
        [Role] int NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [CreatedOn] nvarchar(max) NULL,
        [ModifiedOn] nvarchar(max) NULL,
        [ModifiedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_Teams] PRIMARY KEY ([TeamId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221201134000_Add-migration')
BEGIN
    CREATE TABLE [Tournaments] (
        [TournamentId] uniqueidentifier NOT NULL,
        [TournamentName] nvarchar(max) NOT NULL,
        [From] datetime2 NOT NULL,
        [Till] datetime2 NOT NULL,
        [CutOffDate] datetime2 NULL,
        [TournamentStatus] int NOT NULL,
        [Cycle] int NULL,
        [Type] int NULL,
        [HasSponsor] bit NOT NULL,
        [SponsorType] int NOT NULL,
        [HasAffiliationFee] bit NOT NULL,
        [AffiliationFee] decimal(18,2) NULL,
        [HasPrice] bit NOT NULL,
        [Price1] decimal(18,2) NULL,
        [Price2] decimal(18,2) NULL,
        [IsActive] bit NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [CreatedOn] nvarchar(max) NULL,
        [ModifiedOn] nvarchar(max) NULL,
        [ModifiedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_Tournaments] PRIMARY KEY ([TournamentId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221201134000_Add-migration')
BEGIN
    CREATE TABLE [Users] (
        [Id] uniqueidentifier NOT NULL,
        [ConfirmPassword] nvarchar(max) NOT NULL,
        [Name] nvarchar(25) NOT NULL,
        [LastName] nvarchar(25) NOT NULL,
        [Username] nvarchar(max) NOT NULL,
        [Password] nvarchar(max) NOT NULL,
        [IsEmailVerified] bit NOT NULL,
        [ActivationCode] uniqueidentifier NULL,
        [ResetPasswordCode] nvarchar(max) NULL,
        [LastLoginDate] nvarchar(max) NULL,
        [Role] int NULL,
        [IsActive] bit NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [CreatedOn] nvarchar(max) NULL,
        [ModifiedOn] nvarchar(max) NULL,
        [ModifiedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221201134000_Add-migration')
BEGIN
    CREATE TABLE [Coaches] (
        [CoachId] uniqueidentifier NOT NULL,
        [Title] int NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [IDNumber] nvarchar(13) NOT NULL,
        [RSAIDCopy] nvarchar(max) NULL,
        [ContactId] uniqueidentifier NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [CreatedOn] nvarchar(max) NULL,
        [ModifiedOn] nvarchar(max) NULL,
        [ModifiedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_Coaches] PRIMARY KEY ([CoachId]),
        CONSTRAINT [FK_Coaches_Contacts_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221201134000_Add-migration')
BEGIN
    CREATE TABLE [Referees] (
        [RefereeId] uniqueidentifier NOT NULL,
        [Title] int NOT NULL,
        [Gender] int NOT NULL,
        [Name] nvarchar(15) NOT NULL,
        [LastName] nvarchar(15) NOT NULL,
        [IDNumber] nvarchar(13) NOT NULL,
        [ContactId] uniqueidentifier NULL,
        [Photo] nvarchar(max) NULL,
        [RefereeType] int NOT NULL,
        [RefereeLevel] int NOT NULL,
        [HasJoinedLFA] int NOT NULL,
        [Experience] int NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedBy] nvarchar(max) NULL,
        [CreatedOn] nvarchar(max) NULL,
        [ModifiedOn] nvarchar(max) NULL,
        [ModifiedBy] nvarchar(max) NULL,
        CONSTRAINT [PK_Referees] PRIMARY KEY ([RefereeId]),
        CONSTRAINT [FK_Referees_Contacts_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [Contacts] ([ContactId])
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221201134000_Add-migration')
BEGIN
    CREATE TABLE [Sponsors] (
        [SponsorId] uniqueidentifier NOT NULL,
        [Title] int NOT NULL,
        [TournamentId] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [LastName] nvarchar(max) NOT NULL,
        [Email] nvarchar(max) NULL,
        [Phone] nvarchar(10) NOT NULL,
        [OrganizationName] nvarchar(max) NULL,
        CONSTRAINT [PK_Sponsors] PRIMARY KEY ([SponsorId]),
        CONSTRAINT [FK_Sponsors_Tournaments_TournamentId] FOREIGN KEY ([TournamentId]) REFERENCES [Tournaments] ([TournamentId]) ON DELETE CASCADE
    );
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221201134000_Add-migration')
BEGIN
    CREATE INDEX [IX_Coaches_ContactId] ON [Coaches] ([ContactId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221201134000_Add-migration')
BEGIN
    CREATE INDEX [IX_Referees_ContactId] ON [Referees] ([ContactId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221201134000_Add-migration')
BEGIN
    CREATE UNIQUE INDEX [IX_Sponsors_TournamentId] ON [Sponsors] ([TournamentId]);
END;
GO

IF NOT EXISTS(SELECT * FROM [__EFMigrationsHistory] WHERE [MigrationId] = N'20221201134000_Add-migration')
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20221201134000_Add-migration', N'6.0.10');
END;
GO

COMMIT;
GO

