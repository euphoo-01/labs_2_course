ALTER TABLE [Предприятия] ADD [Email] nvarchar(50);
GO

ALTER TABLE [Значения показателей] ADD CONSTRAINT CHK_Znak CHECK ([Значение] >= 0);
GO

ALTER TABLE [Предприятия] DROP COLUMN [Email];
GO