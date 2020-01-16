EXEC sp_rename N'[Tutorials].[TutorialType]', N'TutorialTopic', N'COLUMN';

GO

ALTER TABLE [Tutorials] ADD [TutorialCategory] nvarchar(64) NOT NULL DEFAULT N'';

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Accounts]') AND [c].[name] = N'Email');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Accounts] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Accounts] ALTER COLUMN [Email] nvarchar(512) NOT NULL;

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Accounts]') AND [c].[name] = N'AcceptOffers');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Accounts] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Accounts] ALTER COLUMN [AcceptOffers] bit NULL;

GO

CREATE TABLE [Questions] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [Title] nvarchar(256) NULL,
    [QuestionTopic] nvarchar(64) NOT NULL,
    [Description] nvarchar(2056) NULL,
    CONSTRAINT [PK_Questions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Questions_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [TutorialComment] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateModified] datetime2 NOT NULL,
    [Notes] nvarchar(1028) NULL,
    [Status] int NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [Title] nvarchar(256) NULL,
    [Body] nvarchar(1028) NULL,
    [TargetId] uniqueidentifier NULL,
    CONSTRAINT [PK_TutorialComment] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TutorialComment_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TutorialComment_Tutorials_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Tutorials] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [TutorialRating] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateModified] datetime2 NOT NULL,
    [Notes] nvarchar(1028) NULL,
    [Status] int NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [Score] int NOT NULL,
    [TargetId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_TutorialRating] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TutorialRating_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TutorialRating_Tutorials_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Tutorials] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Answer] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateModified] datetime2 NOT NULL,
    [Notes] nvarchar(1028) NULL,
    [Status] int NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [TargetId] uniqueidentifier NOT NULL,
    [Title] nvarchar(256) NULL,
    [Body] nvarchar(1028) NULL,
    CONSTRAINT [PK_Answer] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Answer_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Answer_Questions_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Questions] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [QuestionComment] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateModified] datetime2 NOT NULL,
    [Notes] nvarchar(1028) NULL,
    [Status] int NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [TargetId] uniqueidentifier NOT NULL,
    [Title] nvarchar(256) NULL,
    [Body] nvarchar(1028) NULL,
    CONSTRAINT [PK_QuestionComment] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_QuestionComment_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_QuestionComment_Questions_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Questions] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [QuestionRating] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateModified] datetime2 NOT NULL,
    [Notes] nvarchar(1028) NULL,
    [Status] int NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [TargetId] uniqueidentifier NOT NULL,
    [Score] int NOT NULL,
    CONSTRAINT [PK_QuestionRating] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_QuestionRating_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_QuestionRating_Questions_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Questions] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [TutorialCommentRating] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateModified] datetime2 NOT NULL,
    [Notes] nvarchar(1028) NULL,
    [Status] int NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [Score] int NOT NULL,
    [TargetId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_TutorialCommentRating] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TutorialCommentRating_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TutorialCommentRating_TutorialComment_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [TutorialComment] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AnswerComment] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateModified] datetime2 NOT NULL,
    [Notes] nvarchar(1028) NULL,
    [Status] int NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [TargetId] uniqueidentifier NOT NULL,
    [Title] nvarchar(256) NULL,
    [Body] nvarchar(1028) NULL,
    CONSTRAINT [PK_AnswerComment] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AnswerComment_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AnswerComment_Answer_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Answer] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AnswerRating] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateModified] datetime2 NOT NULL,
    [Notes] nvarchar(1028) NULL,
    [Status] int NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [TargetId] uniqueidentifier NOT NULL,
    [Score] int NOT NULL,
    CONSTRAINT [PK_AnswerRating] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AnswerRating_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AnswerRating_Answer_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Answer] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [QuestionCommentRating] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateModified] datetime2 NOT NULL,
    [Notes] nvarchar(1028) NULL,
    [Status] int NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [TargetId] uniqueidentifier NOT NULL,
    [Score] int NOT NULL,
    CONSTRAINT [PK_QuestionCommentRating] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_QuestionCommentRating_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_QuestionCommentRating_QuestionComment_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [QuestionComment] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AnswerCommentRating] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL,
    [DateModified] datetime2 NOT NULL,
    [Notes] nvarchar(1028) NULL,
    [Status] int NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [TargetId] uniqueidentifier NOT NULL,
    [Score] int NOT NULL,
    CONSTRAINT [PK_AnswerCommentRating] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AnswerCommentRating_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AnswerCommentRating_AnswerComment_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [AnswerComment] ([Id]) ON DELETE CASCADE
);

GO

CREATE NONCLUSTERED INDEX [IX_Tutorials_Title] ON [Tutorials] ([Title]);

GO

CREATE NONCLUSTERED INDEX [IX_Accounts_Email] ON [Accounts] ([Email]);

GO

CREATE NONCLUSTERED INDEX [IX_Accounts_NickName] ON [Accounts] ([NickName]);

GO

CREATE INDEX [IX_Answer_OwnerAccountId] ON [Answer] ([OwnerAccountId]);

GO

CREATE INDEX [IX_Answer_TargetId] ON [Answer] ([TargetId]);

GO

CREATE INDEX [IX_AnswerComment_OwnerAccountId] ON [AnswerComment] ([OwnerAccountId]);

GO

CREATE INDEX [IX_AnswerComment_TargetId] ON [AnswerComment] ([TargetId]);

GO

CREATE INDEX [IX_AnswerCommentRating_OwnerAccountId] ON [AnswerCommentRating] ([OwnerAccountId]);

GO

CREATE INDEX [IX_AnswerCommentRating_TargetId] ON [AnswerCommentRating] ([TargetId]);

GO

CREATE INDEX [IX_AnswerRating_OwnerAccountId] ON [AnswerRating] ([OwnerAccountId]);

GO

CREATE INDEX [IX_AnswerRating_TargetId] ON [AnswerRating] ([TargetId]);

GO

CREATE INDEX [IX_QuestionComment_OwnerAccountId] ON [QuestionComment] ([OwnerAccountId]);

GO

CREATE INDEX [IX_QuestionComment_TargetId] ON [QuestionComment] ([TargetId]);

GO

CREATE INDEX [IX_QuestionCommentRating_OwnerAccountId] ON [QuestionCommentRating] ([OwnerAccountId]);

GO

CREATE INDEX [IX_QuestionCommentRating_TargetId] ON [QuestionCommentRating] ([TargetId]);

GO

CREATE INDEX [IX_QuestionRating_OwnerAccountId] ON [QuestionRating] ([OwnerAccountId]);

GO

CREATE INDEX [IX_QuestionRating_TargetId] ON [QuestionRating] ([TargetId]);

GO

CREATE INDEX [IX_Questions_OwnerAccountId] ON [Questions] ([OwnerAccountId]);

GO

CREATE NONCLUSTERED INDEX [IX_Questions_Title] ON [Questions] ([Title]);

GO

CREATE INDEX [IX_TutorialComment_OwnerAccountId] ON [TutorialComment] ([OwnerAccountId]);

GO

CREATE INDEX [IX_TutorialComment_TargetId] ON [TutorialComment] ([TargetId]);

GO

CREATE INDEX [IX_TutorialCommentRating_OwnerAccountId] ON [TutorialCommentRating] ([OwnerAccountId]);

GO

CREATE INDEX [IX_TutorialCommentRating_TargetId] ON [TutorialCommentRating] ([TargetId]);

GO

CREATE INDEX [IX_TutorialRating_OwnerAccountId] ON [TutorialRating] ([OwnerAccountId]);

GO

CREATE INDEX [IX_TutorialRating_TargetId] ON [TutorialRating] ([TargetId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200116152446_Questions', N'2.2.6-servicing-10079');

GO


