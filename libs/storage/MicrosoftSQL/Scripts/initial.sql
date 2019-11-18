IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Tutorials] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [Title] nvarchar(64) NULL,
    [Language] nvarchar(64) NULL,
    [Description] nvarchar(1028) NULL,
    CONSTRAINT [PK_Tutorials] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191118141119_Initial', N'2.2.6-servicing-10079');

GO


