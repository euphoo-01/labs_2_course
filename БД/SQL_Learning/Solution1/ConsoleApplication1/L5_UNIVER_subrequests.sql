-- 1. Кафедры факультетов со специальностями "технология/технологии" (IN + некоррелированный подзапрос)
SELECT PULPIT_NAME
FROM PULPIT
WHERE FACULTY IN (
    SELECT FACULTY
    FROM PROFESSION
    WHERE PROFESSION_NAME LIKE N'%технологи%'
);

-- 2. Тот же запрос, но подзапрос в секции FROM (Derived Table)
SELECT P.PULPIT_NAME
FROM PULPIT P
         INNER JOIN (
    SELECT DISTINCT FACULTY
    FROM PROFESSION
    WHERE PROFESSION_NAME LIKE N'%технологи%'
) AS FilteredFaculties ON P.FACULTY = FilteredFaculties.FACULTY;

-- 3. Тот же запрос без подзапросов (INNER JOIN трех таблиц)
SELECT DISTINCT P.PULPIT_NAME
FROM PULPIT P
         INNER JOIN FACULTY F ON P.FACULTY = F.FACULTY
         INNER JOIN PROFESSION PR ON F.FACULTY = PR.FACULTY
WHERE PR.PROFESSION_NAME LIKE N'%технологи%';

-- 4. Аудитории максимальной вместимости для каждого типа (Коррелированный подзапрос + TOP)
SELECT A1.AUDITORIUM, A1.AUDITORIUM_TYPE, A1.AUDITORIUM_CAPACITY
FROM AUDITORIUM A1
WHERE A1.AUDITORIUM_CAPACITY = (
    SELECT TOP 1 A2.AUDITORIUM_CAPACITY
    FROM AUDITORIUM A2
    WHERE A2.AUDITORIUM_TYPE = A1.AUDITORIUM_TYPE
    ORDER BY A2.AUDITORIUM_CAPACITY DESC
)
ORDER BY A1.AUDITORIUM_CAPACITY DESC;

-- 5. Факультеты без кафедр (EXISTS + коррелированный подзапрос)
SELECT F.FACULTY_NAME
FROM FACULTY F
WHERE NOT EXISTS (
    SELECT *
    FROM PULPIT P
    WHERE P.FACULTY = F.FACULTY
);

-- 6. Средние значения оценок по дисциплинам ОАиП, БД, СУБД (Три подзапроса в SELECT)
SELECT
    (SELECT AVG(CAST(NOTE AS FLOAT)) FROM PROGRESS WHERE SUBJECT = N'ОАиП') AS [Среднее ОАиП],
    (SELECT AVG(CAST(NOTE AS FLOAT)) FROM PROGRESS WHERE SUBJECT = N'БД') AS [Среднее БД],
    (SELECT AVG(CAST(NOTE AS FLOAT)) FROM PROGRESS WHERE SUBJECT = N'СУБД') AS [Среднее СУБД];

-- 7. Применение ALL (Предметы, где все оценки выше любой оценки по КГ)
SELECT DISTINCT SUBJECT
FROM PROGRESS
WHERE NOTE > ALL (SELECT NOTE FROM PROGRESS WHERE SUBJECT = N'КГ');

-- 8. Применение ANY (Студенты с оценкой выше хотя бы одной оценки по СУБД)
SELECT DISTINCT IDSTUDENT, SUBJECT, NOTE
FROM PROGRESS
WHERE NOTE > ANY (SELECT NOTE FROM PROGRESS WHERE SUBJECT = N'СУБД');