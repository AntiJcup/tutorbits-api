EXEC sp_rename N'[Tutorials].[TutorialType]', N'TutorialLanguage', N'COLUMN';

GO

ALTER TABLE [Tutorials] ADD [TutorialCategory] nvarchar(64) NOT NULL DEFAULT N'Tutorial';

GO

CREATE TABLE [Comments] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateModified] datetime2 NOT NULL,
    [Notes] nvarchar(1028) NULL,
    [Status] int NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [Title] int NOT NULL,
    [Body] int NOT NULL,
    [CommentType] int NOT NULL,
    [TargetId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Comments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Comments_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Comments_Tutorials_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Tutorials] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Ratings] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateModified] datetime2 NOT NULL,
    [Notes] nvarchar(1028) NULL,
    [Status] int NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [Score] int NOT NULL,
    [TargetId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_Ratings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Ratings_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Ratings_Tutorials_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Tutorials] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Comments_OwnerAccountId] ON [Comments] ([OwnerAccountId]);

GO

CREATE INDEX [IX_Comments_TargetId] ON [Comments] ([TargetId]);

GO

CREATE INDEX [IX_Ratings_OwnerAccountId] ON [Ratings] ([OwnerAccountId]);

GO

CREATE INDEX [IX_Ratings_TargetId] ON [Ratings] ([TargetId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200109170957_AddCommentsAndRatings', N'2.2.6-servicing-10079');

GO


