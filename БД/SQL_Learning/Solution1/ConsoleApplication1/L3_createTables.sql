CREATE TABLE [Типы показателей] (
    [ID типа] uniqueidentifier PRIMARY KEY DEFAULT NEWID(),
    [Название показателя] nvarchar(15) NOT NULL,
    [Важность] nvarchar(15)
    ) ON [PRIMARY];
GO

CREATE TABLE [Предприятия] (
    [ID предприятия] uniqueidentifier PRIMARY KEY DEFAULT NEWID(),
    [Название предприятия] nvarchar(30) NOT NULL,[Расчетный счет] nvarchar(30),
    [БИК банка] nvarchar(30),
    [Телефон] nvarchar(30),
    [Контактное лицо] nvarchar(20)
    ) ON [PRIMARY];
GO

CREATE TABLE [Значения показателей] (
    [ID значения] uniqueidentifier PRIMARY KEY DEFAULT NEWID(),
    [ID предприятия] uniqueidentifier FOREIGN KEY REFERENCES[Предприятия]([ID предприятия]),
    [ID типа показателя] uniqueidentifier FOREIGN KEY REFERENCES[Типы показателей]([ID типа]),
    [Значение] decimal(18,2) NOT NULL,
    [Дата] date DEFAULT GETDATE()
    ) ON FG1;
GO