DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Accounts]') AND [c].[name] = N'AcceptOffers');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [Accounts] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [Accounts] ALTER COLUMN [AcceptOffers] bit NULL;

GO

CREATE NONCLUSTERED INDEX [IX_Tutorials_Title] ON [Tutorials] ([Title]);

GO

CREATE NONCLUSTERED INDEX [IX_Accounts_Email] ON [Accounts] ([Email]);

GO

CREATE NONCLUSTERED INDEX [IX_Accounts_NickName] ON [Accounts] ([NickName]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200114154535_AddIndicesToTutorialAndAccounts', N'2.2.6-servicing-10079');

GO


