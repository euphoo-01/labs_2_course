IF DB_ID(N'Лавшук_MyBase') IS NOT NULL
    DROP DATABASE Лавшук_MyBase;
GO

CREATE DATABASE Лавшук_MyBase 
ON PRIMARY 
( 
    NAME = N'Лавшук_MyBase_mdf', 
    FILENAME = N'/var/opt/mssql/data/Лавшук_MyBase_mdf.mdf', 
    SIZE = 10MB, MAXSIZE = UNLIMITED, FILEGROWTH = 5MB
),
FILEGROUP FG1
( 
    NAME = N'Лавшук_MyBase_fg1_1', 
    FILENAME = N'/var/opt/mssql/data/Лавшук_MyBase_fg1_1.ndf', 
    SIZE = 10MB, MAXSIZE = 1GB, FILEGROWTH = 25%
)
LOG ON 
( 
    NAME = N'Лавшук_MyBase_log', 
    FILENAME = N'/var/opt/mssql/data/Лавшук_MyBase_log.ldf', 
    SIZE = 5MB, MAXSIZE = 2GB, FILEGROWTH = 10%
);
GO