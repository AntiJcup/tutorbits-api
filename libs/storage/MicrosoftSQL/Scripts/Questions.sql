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

CREATE TABLE [TutorialComments] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [Title] nvarchar(256) NULL,
    [Body] nvarchar(1028) NULL,
    [TargetId] uniqueidentifier NULL,
    CONSTRAINT [PK_TutorialComments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TutorialComments_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TutorialComments_Tutorials_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Tutorials] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [TutorialRatings] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [Score] int NOT NULL,
    [TargetId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_TutorialRatings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TutorialRatings_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TutorialRatings_Tutorials_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Tutorials] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Answers] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [TargetId] uniqueidentifier NOT NULL,
    [Title] nvarchar(256) NULL,
    [Body] nvarchar(1028) NULL,
    CONSTRAINT [PK_Answers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Answers_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Answers_Questions_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Questions] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [QuestionComments] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [TargetId] uniqueidentifier NOT NULL,
    [Title] nvarchar(256) NULL,
    [Body] nvarchar(1028) NULL,
    CONSTRAINT [PK_QuestionComments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_QuestionComments_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_QuestionComments_Questions_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Questions] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [QuestionRatings] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [TargetId] uniqueidentifier NOT NULL,
    [Score] int NOT NULL,
    CONSTRAINT [PK_QuestionRatings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_QuestionRatings_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_QuestionRatings_Questions_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Questions] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [TutorialCommentRatings] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [Score] int NOT NULL,
    [TargetId] uniqueidentifier NOT NULL,
    CONSTRAINT [PK_TutorialCommentRatings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TutorialCommentRatings_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TutorialCommentRatings_TutorialComments_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [TutorialComments] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AnswerComments] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [TargetId] uniqueidentifier NOT NULL,
    [Title] nvarchar(256) NULL,
    [Body] nvarchar(1028) NULL,
    CONSTRAINT [PK_AnswerComments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AnswerComments_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AnswerComments_Answers_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Answers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AnswerRatings] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [TargetId] uniqueidentifier NOT NULL,
    [Score] int NOT NULL,
    CONSTRAINT [PK_AnswerRatings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AnswerRatings_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AnswerRatings_Answers_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Answers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [QuestionCommentRatings] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [TargetId] uniqueidentifier NOT NULL,
    [Score] int NOT NULL,
    CONSTRAINT [PK_QuestionCommentRatings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_QuestionCommentRatings_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_QuestionCommentRatings_QuestionComments_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [QuestionComments] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AnswerCommentRatings] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [TargetId] uniqueidentifier NOT NULL,
    [Score] int NOT NULL,
    CONSTRAINT [PK_AnswerCommentRatings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AnswerCommentRatings_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AnswerCommentRatings_AnswerComments_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [AnswerComments] ([Id]) ON DELETE CASCADE
);

GO

CREATE NONCLUSTERED INDEX [IX_Tutorials_Title] ON [Tutorials] ([Title]);

GO

CREATE NONCLUSTERED INDEX [IX_Accounts_Email] ON [Accounts] ([Email]);

GO

CREATE NONCLUSTERED INDEX [IX_Accounts_NickName] ON [Accounts] ([NickName]);

GO

CREATE INDEX [IX_AnswerCommentRatings_OwnerAccountId] ON [AnswerCommentRatings] ([OwnerAccountId]);

GO

CREATE INDEX [IX_AnswerCommentRatings_TargetId] ON [AnswerCommentRatings] ([TargetId]);

GO

CREATE INDEX [IX_AnswerComments_OwnerAccountId] ON [AnswerComments] ([OwnerAccountId]);

GO

CREATE INDEX [IX_AnswerComments_TargetId] ON [AnswerComments] ([TargetId]);

GO

CREATE INDEX [IX_AnswerRatings_OwnerAccountId] ON [AnswerRatings] ([OwnerAccountId]);

GO

CREATE INDEX [IX_AnswerRatings_TargetId] ON [AnswerRatings] ([TargetId]);

GO

CREATE INDEX [IX_Answers_OwnerAccountId] ON [Answers] ([OwnerAccountId]);

GO

CREATE INDEX [IX_Answers_TargetId] ON [Answers] ([TargetId]);

GO

CREATE INDEX [IX_QuestionCommentRatings_OwnerAccountId] ON [QuestionCommentRatings] ([OwnerAccountId]);

GO

CREATE INDEX [IX_QuestionCommentRatings_TargetId] ON [QuestionCommentRatings] ([TargetId]);

GO

CREATE INDEX [IX_QuestionComments_OwnerAccountId] ON [QuestionComments] ([OwnerAccountId]);

GO

CREATE INDEX [IX_QuestionComments_TargetId] ON [QuestionComments] ([TargetId]);

GO

CREATE INDEX [IX_QuestionRatings_OwnerAccountId] ON [QuestionRatings] ([OwnerAccountId]);

GO

CREATE INDEX [IX_QuestionRatings_TargetId] ON [QuestionRatings] ([TargetId]);

GO

CREATE INDEX [IX_Questions_OwnerAccountId] ON [Questions] ([OwnerAccountId]);

GO

CREATE NONCLUSTERED INDEX [IX_Questions_Title] ON [Questions] ([Title]);

GO

CREATE INDEX [IX_TutorialCommentRatings_OwnerAccountId] ON [TutorialCommentRatings] ([OwnerAccountId]);

GO

CREATE INDEX [IX_TutorialCommentRatings_TargetId] ON [TutorialCommentRatings] ([TargetId]);

GO

CREATE INDEX [IX_TutorialComments_OwnerAccountId] ON [TutorialComments] ([OwnerAccountId]);

GO

CREATE INDEX [IX_TutorialComments_TargetId] ON [TutorialComments] ([TargetId]);

GO

CREATE INDEX [IX_TutorialRatings_OwnerAccountId] ON [TutorialRatings] ([OwnerAccountId]);

GO

CREATE INDEX [IX_TutorialRatings_TargetId] ON [TutorialRatings] ([TargetId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200117173046_Questions', N'2.2.6-servicing-10079');

GO


