ALTER TABLE [Projects] ADD [ProjectType] nvarchar(64) NOT NULL DEFAULT N'';

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200201161019_AddProjectType', N'2.2.6-servicing-10079');

GO


