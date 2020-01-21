DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Tutorials]') AND [c].[name] = N'DurationMS');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Tutorials] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Tutorials] DROP COLUMN [DurationMS];

GO

EXEC sp_rename N'[Tutorials].[TutorialType]', N'ProgrammingTopic', N'COLUMN';

GO

ALTER TABLE [Tutorials] ADD [ProjectId] uniqueidentifier NULL;

GO

ALTER TABLE [Tutorials] ADD [ThumbnailId] uniqueidentifier NULL;

GO

ALTER TABLE [Tutorials] ADD [VideoId] uniqueidentifier NULL;

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Accounts]') AND [c].[name] = N'Email');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [Accounts] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [Accounts] ALTER COLUMN [Email] nvarchar(512) NOT NULL;

GO

DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Accounts]') AND [c].[name] = N'AcceptOffers');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [Accounts] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [Accounts] ALTER COLUMN [AcceptOffers] bit NULL;

GO

CREATE TABLE [Projects] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [DurationMS] decimal(20,0) NOT NULL,
    CONSTRAINT [PK_Projects] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Projects_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION
);

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
    [ProgrammingTopic] nvarchar(64) NOT NULL,
    [Description] nvarchar(2056) NULL,
    CONSTRAINT [PK_Questions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Questions_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Thumbnails] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    CONSTRAINT [PK_Thumbnails] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Thumbnails_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION
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
    [TargetId] uniqueidentifier NOT NULL,
    [Title] nvarchar(256) NULL,
    [Body] nvarchar(1028) NULL,
    CONSTRAINT [PK_TutorialComments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TutorialComments_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TutorialComments_Tutorials_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Tutorials] ([Id]) ON DELETE CASCADE
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
    [TargetId] uniqueidentifier NOT NULL,
    [Score] int NOT NULL,
    CONSTRAINT [PK_TutorialRatings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_TutorialRatings_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_TutorialRatings_Tutorials_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Tutorials] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [Videos] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [DurationMS] decimal(20,0) NOT NULL,
    CONSTRAINT [PK_Videos] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Videos_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION
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

CREATE TABLE [Examples] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [Title] nvarchar(64) NULL,
    [ProgrammingTopic] nvarchar(64) NOT NULL,
    [Description] nvarchar(1028) NULL,
    [ProjectId] uniqueidentifier NULL,
    [ThumbnailId] uniqueidentifier NULL,
    CONSTRAINT [PK_Examples] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Examples_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Examples_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Projects] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Examples_Thumbnails_ThumbnailId] FOREIGN KEY ([ThumbnailId]) REFERENCES [Thumbnails] ([Id]) ON DELETE NO ACTION
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
    [TargetId] uniqueidentifier NOT NULL,
    [Score] int NOT NULL,
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

CREATE TABLE [ExampleComments] (
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
    CONSTRAINT [PK_ExampleComments] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ExampleComments_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ExampleComments_Examples_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Examples] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [ExampleRatings] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [TargetId] uniqueidentifier NOT NULL,
    [Score] int NOT NULL,
    CONSTRAINT [PK_ExampleRatings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ExampleRatings_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ExampleRatings_Examples_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [Examples] ([Id]) ON DELETE CASCADE
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

CREATE TABLE [ExampleCommentRatings] (
    [Id] uniqueidentifier NOT NULL,
    [DateCreated] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [DateModified] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    [Notes] nvarchar(1028) NULL,
    [Status] nvarchar(64) NOT NULL,
    [Owner] nvarchar(1028) NULL,
    [OwnerAccountId] uniqueidentifier NULL,
    [TargetId] uniqueidentifier NOT NULL,
    [Score] int NOT NULL,
    CONSTRAINT [PK_ExampleCommentRatings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_ExampleCommentRatings_Accounts_OwnerAccountId] FOREIGN KEY ([OwnerAccountId]) REFERENCES [Accounts] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_ExampleCommentRatings_ExampleComments_TargetId] FOREIGN KEY ([TargetId]) REFERENCES [ExampleComments] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_Tutorials_ProjectId] ON [Tutorials] ([ProjectId]);

GO

CREATE INDEX [IX_Tutorials_ThumbnailId] ON [Tutorials] ([ThumbnailId]);

GO

CREATE NONCLUSTERED INDEX [IX_Tutorials_Title] ON [Tutorials] ([Title]);

GO

CREATE INDEX [IX_Tutorials_VideoId] ON [Tutorials] ([VideoId]);

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

CREATE INDEX [IX_ExampleCommentRatings_OwnerAccountId] ON [ExampleCommentRatings] ([OwnerAccountId]);

GO

CREATE INDEX [IX_ExampleCommentRatings_TargetId] ON [ExampleCommentRatings] ([TargetId]);

GO

CREATE INDEX [IX_ExampleComments_OwnerAccountId] ON [ExampleComments] ([OwnerAccountId]);

GO

CREATE INDEX [IX_ExampleComments_TargetId] ON [ExampleComments] ([TargetId]);

GO

CREATE INDEX [IX_ExampleRatings_OwnerAccountId] ON [ExampleRatings] ([OwnerAccountId]);

GO

CREATE INDEX [IX_ExampleRatings_TargetId] ON [ExampleRatings] ([TargetId]);

GO

CREATE INDEX [IX_Examples_OwnerAccountId] ON [Examples] ([OwnerAccountId]);

GO

CREATE INDEX [IX_Examples_ProjectId] ON [Examples] ([ProjectId]);

GO

CREATE INDEX [IX_Examples_ThumbnailId] ON [Examples] ([ThumbnailId]);

GO

CREATE NONCLUSTERED INDEX [IX_Examples_Title] ON [Examples] ([Title]);

GO

CREATE INDEX [IX_Projects_OwnerAccountId] ON [Projects] ([OwnerAccountId]);

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

CREATE INDEX [IX_Thumbnails_OwnerAccountId] ON [Thumbnails] ([OwnerAccountId]);

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

CREATE INDEX [IX_Videos_OwnerAccountId] ON [Videos] ([OwnerAccountId]);

GO

ALTER TABLE [Tutorials] ADD CONSTRAINT [FK_Tutorials_Projects_ProjectId] FOREIGN KEY ([ProjectId]) REFERENCES [Projects] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [Tutorials] ADD CONSTRAINT [FK_Tutorials_Thumbnails_ThumbnailId] FOREIGN KEY ([ThumbnailId]) REFERENCES [Thumbnails] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [Tutorials] ADD CONSTRAINT [FK_Tutorials_Videos_VideoId] FOREIGN KEY ([VideoId]) REFERENCES [Videos] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200121143940_QuestionExampleCommentsRatings', N'2.2.6-servicing-10079');

GO


