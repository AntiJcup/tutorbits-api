IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Accounts] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [Email] nvarchar(1028) NOT NULL,
    [NickName] nvarchar(256) NOT NULL,
    [AcceptOffers] bit NOT NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Accounts_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Tutorials] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [Title] nvarchar(64) NULL,
    [TutorialType] nvarchar(64) NOT NULL,
    [Description] nvarchar(1028) NULL,
    [DurationMS] decimal(20,0) NOT NULL,
    CONSTRAINT [PK_Tutorials] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Tutorials_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_Accounts_OwnerAccountId] ON [Accounts] ([OwnerAccountId]);

GO

CREATE INDEX [IX_Tutorials_OwnerAccountId] ON [Tutorials] ([OwnerAccountId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191203165949_Initial', N'2.2.6-servicing-10079');

GO


