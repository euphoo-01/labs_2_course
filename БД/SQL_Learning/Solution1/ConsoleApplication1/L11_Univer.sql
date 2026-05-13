PRINT N'--- ЗАДАНИЕ 1 ---';
DECLARE @subj_name nchar(10);
DECLARE @result_list nvarchar(max) = N'';

-- Объявляем курсор
DECLARE SubjectCursor CURSOR FOR
SELECT SUBJECT FROM SUBJECT WHERE PULPIT = N'ИСиТ';

OPEN SubjectCursor;
FETCH SubjectCursor INTO @subj_name;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @result_list = @result_list + RTRIM(@subj_name) + N', ';
FETCH SubjectCursor INTO @subj_name;
END;

CLOSE SubjectCursor;
DEALLOCATE SubjectCursor;

IF LEN(@result_list) > 0
    SET @result_list = LEFT(@result_list, LEN(@result_list) - 2);

PRINT N'Дисциплины кафедры ИСиТ: ' + @result_list;
PRINT N'';
GO


PRINT N'--- ЗАДАНИЕ 2 ---';
DECLARE LocalCursor CURSOR LOCAL FOR SELECT FACULTY_NAME FROM FACULTY;
OPEN LocalCursor;
PRINT N'Локальный курсор создан и открыт.';
GO

DECLARE GlobalCursor CURSOR GLOBAL FOR SELECT FACULTY_NAME FROM FACULTY;
OPEN GlobalCursor;
PRINT N'Глобальный курсор создан и открыт.';
GO

DECLARE @fac_name nvarchar(50);
FETCH NEXT FROM GlobalCursor INTO @fac_name;
PRINT N'Чтение из глобального курсора в другом пакете: ' + @fac_name;

CLOSE GlobalCursor;
DEALLOCATE GlobalCursor;
PRINT N'';
GO


PRINT N'--- ЗАДАНИЕ 3 ---';
DECLARE @aud_name nvarchar(50);

DECLARE StaticCursor CURSOR LOCAL STATIC FOR SELECT AUDITORIUM_NAME FROM AUDITORIUM WHERE AUDITORIUM_TYPE = N'ЛК';
DECLARE DynamicCursor CURSOR LOCAL DYNAMIC FOR SELECT AUDITORIUM_NAME FROM AUDITORIUM WHERE AUDITORIUM_TYPE = N'ЛК';

OPEN StaticCursor;
OPEN DynamicCursor;

UPDATE AUDITORIUM SET AUDITORIUM_NAME = N'Изменено_ЛК' WHERE AUDITORIUM = '236-1';

PRINT N'[Статический курсор] (не должен видеть изменения):';
FETCH NEXT FROM StaticCursor INTO @aud_name;
WHILE @@FETCH_STATUS = 0 BEGIN PRINT @aud_name; FETCH NEXT FROM StaticCursor INTO @aud_name; END;

PRINT N'[Динамический курсор] (увидит измененное название):';
FETCH NEXT FROM DynamicCursor INTO @aud_name;
WHILE @@FETCH_STATUS = 0 BEGIN PRINT @aud_name; FETCH NEXT FROM DynamicCursor INTO @aud_name; END;

CLOSE StaticCursor; DEALLOCATE StaticCursor;
CLOSE DynamicCursor; DEALLOCATE DynamicCursor;

UPDATE AUDITORIUM SET AUDITORIUM_NAME = '236-1' WHERE AUDITORIUM = '236-1';
PRINT N'';
GO


PRINT N'--- ЗАДАНИЕ 4 ---';
DECLARE @prof_name nvarchar(100), @row_num int;

DECLARE ScrollCursor CURSOR LOCAL SCROLL FOR
SELECT ROW_NUMBER() OVER(ORDER BY PROFESSION_NAME), PROFESSION_NAME
FROM PROFESSION WHERE FACULTY = N'ИТ';

OPEN ScrollCursor;

FETCH FIRST FROM ScrollCursor INTO @row_num, @prof_name;
PRINT N'ПЕРВАЯ строка (FIRST): ' + CAST(@row_num as varchar) + N' - ' + @prof_name;

FETCH LAST FROM ScrollCursor INTO @row_num, @prof_name;
PRINT N'ПОСЛЕДНЯЯ строка (LAST): ' + CAST(@row_num as varchar) + N' - ' + @prof_name;

FETCH PRIOR FROM ScrollCursor INTO @row_num, @prof_name;
PRINT N'ПРЕДЫДУЩАЯ строка (PRIOR): ' + CAST(@row_num as varchar) + N' - ' + @prof_name;

FETCH ABSOLUTE 2 FROM ScrollCursor INTO @row_num, @prof_name;
PRINT N'АБСОЛЮТНАЯ 2-я строка от начала (ABSOLUTE 2): ' + CAST(@row_num as varchar) + N' - ' + @prof_name;

FETCH RELATIVE 1 FROM ScrollCursor INTO @row_num, @prof_name;
PRINT N'СМЕЩЕНИЕ на 1 вперед от текущей (RELATIVE 1): ' + CAST(@row_num as varchar) + N' - ' + @prof_name;

CLOSE ScrollCursor;
DEALLOCATE ScrollCursor;
PRINT N'';
GO


PRINT N'--- ЗАДАНИЕ 5 ---';
INSERT INTO AUDITORIUM (AUDITORIUM, AUDITORIUM_NAME, AUDITORIUM_TYPE, AUDITORIUM_CAPACITY)
VALUES ('999-TEST', 'TestAud', N'ЛК', 10);

DECLARE @aud nchar(20), @cap int;
DECLARE UpdateCursor CURSOR LOCAL DYNAMIC FOR
SELECT AUDITORIUM, AUDITORIUM_CAPACITY FROM AUDITORIUM WHERE AUDITORIUM = '999-TEST' FOR UPDATE;

OPEN UpdateCursor;
FETCH NEXT FROM UpdateCursor INTO @aud, @cap;

IF @@FETCH_STATUS = 0
BEGIN
UPDATE AUDITORIUM SET AUDITORIUM_CAPACITY = 100 WHERE CURRENT OF UpdateCursor;
PRINT N'Вместимость аудитории обновлена через CURRENT OF.';
    
DELETE FROM AUDITORIUM WHERE CURRENT OF UpdateCursor;
PRINT N'Тестовая запись удалена через CURRENT OF.';
END

CLOSE UpdateCursor;
DEALLOCATE UpdateCursor;
PRINT N'';
GO


PRINT N'--- ЗАДАНИЕ 6 ---';

INSERT INTO PROGRESS (SUBJECT, IDSTUDENT, PDATE, NOTE) VALUES (N'ОАиП', 1001, GETDATE(), 3);

DELETE P
FROM PROGRESS P
INNER JOIN STUDENT S ON P.IDSTUDENT = S.IDSTUDENT
INNER JOIN GROUPS G ON S.IDGROUP = G.IDGROUP
WHERE P.NOTE < 4;
PRINT N'Оценки ниже 4 успешно удалены.';

UPDATE PROGRESS
SET NOTE = NOTE + 1
WHERE IDSTUDENT = 1001 AND NOTE < 10;
PRINT N'Оценка студента 1001 увеличена на 1.';
GO