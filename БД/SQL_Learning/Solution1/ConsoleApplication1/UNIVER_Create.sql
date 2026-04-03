drop table PROGRESS
drop table STUDENT
drop table GROUPS
drop table SUBJECT
drop table TEACHER
drop table PULPIT
drop table PROFESSION
drop table FACULTY
drop table AUDITORIUM
drop table AUDITORIUM_TYPE

------------ Создание и заполнение таблицы AUDITORIUM_TYPE 
create table AUDITORIUM_TYPE
(    AUDITORIUM_TYPE  nchar(10) constraint AUDITORIUM_TYPE_PK  primary key,
     AUDITORIUM_TYPENAME  nvarchar(30)
)
    insert into AUDITORIUM_TYPE (AUDITORIUM_TYPE, AUDITORIUM_TYPENAME) values (N'ЛК', N'Лекционная');
insert into AUDITORIUM_TYPE (AUDITORIUM_TYPE, AUDITORIUM_TYPENAME) values (N'ЛБ-К', N'Компьютерный класс');
insert into AUDITORIUM_TYPE (AUDITORIUM_TYPE, AUDITORIUM_TYPENAME) values (N'ЛК-К', N'Лекционная с уст. проектором');
insert into AUDITORIUM_TYPE (AUDITORIUM_TYPE, AUDITORIUM_TYPENAME) values (N'ЛБ-X', N'Химическая лаборатория');
insert into AUDITORIUM_TYPE (AUDITORIUM_TYPE, AUDITORIUM_TYPENAME) values (N'ЛБ-СК', N'Спец. компьютерный класс');

------------- Создание и заполнение таблицы AUDITORIUM  
create table AUDITORIUM
(   AUDITORIUM   nchar(20)  constraint AUDITORIUM_PK  primary key,
    AUDITORIUM_TYPE     nchar(10) constraint  AUDITORIUM_AUDITORIUM_TYPE_FK foreign key
        references AUDITORIUM_TYPE(AUDITORIUM_TYPE),
    AUDITORIUM_CAPACITY  integer constraint  AUDITORIUM_CAPACITY_CHECK default 1  check (AUDITORIUM_CAPACITY between 1 and 300),
    AUDITORIUM_NAME      nvarchar(50)
)
    insert into  AUDITORIUM (AUDITORIUM, AUDITORIUM_NAME, AUDITORIUM_TYPE, AUDITORIUM_CAPACITY) values ('206-1', '206-1', N'ЛБ-К', 15);
insert into  AUDITORIUM (AUDITORIUM, AUDITORIUM_NAME, AUDITORIUM_TYPE, AUDITORIUM_CAPACITY) values ('301-1', '301-1', N'ЛБ-К', 15);
insert into  AUDITORIUM (AUDITORIUM, AUDITORIUM_NAME, AUDITORIUM_TYPE, AUDITORIUM_CAPACITY) values ('236-1', '236-1', N'ЛК', 60);
insert into  AUDITORIUM (AUDITORIUM, AUDITORIUM_NAME, AUDITORIUM_TYPE, AUDITORIUM_CAPACITY) values ('313-1', '313-1', N'ЛК-К', 60);
insert into  AUDITORIUM (AUDITORIUM, AUDITORIUM_NAME, AUDITORIUM_TYPE, AUDITORIUM_CAPACITY) values ('324-1', '324-1', N'ЛК-К', 50);
insert into  AUDITORIUM (AUDITORIUM, AUDITORIUM_NAME, AUDITORIUM_TYPE, AUDITORIUM_CAPACITY) values ('413-1', '413-1', N'ЛБ-К', 15);
insert into  AUDITORIUM (AUDITORIUM, AUDITORIUM_NAME, AUDITORIUM_TYPE, AUDITORIUM_CAPACITY) values ('423-1', '423-1', N'ЛБ-К', 90);
insert into  AUDITORIUM (AUDITORIUM, AUDITORIUM_NAME, AUDITORIUM_TYPE, AUDITORIUM_CAPACITY) values ('408-2', '408-2', N'ЛК', 90);

------ Создание и заполнение таблицы FACULTY
create table FACULTY
(    FACULTY      nchar(10)   constraint  FACULTY_PK primary key,
     FACULTY_NAME  nvarchar(50) default N'???'
);
insert into FACULTY (FACULTY, FACULTY_NAME) values (N'ХТиТ', N'Химическая технология и техника');
insert into FACULTY (FACULTY, FACULTY_NAME) values (N'ЛХФ', N'Лесохозяйственный факультет');
insert into FACULTY (FACULTY, FACULTY_NAME) values (N'ИЭФ', N'Инженерно-экономический факультет');
insert into FACULTY (FACULTY, FACULTY_NAME) values (N'ТТЛП', N'Технология и техника лесной промышленности');
insert into FACULTY (FACULTY, FACULTY_NAME) values (N'ТОВ', N'Технология органических веществ');
insert into FACULTY (FACULTY, FACULTY_NAME) values (N'ИТ', N'Факультет информационных технологий');

------ Создание и заполнение таблицы PROFESSION
create table PROFESSION
(   PROFESSION   nchar(20) constraint PROFESSION_PK  primary key,
    FACULTY    nchar(10) constraint PROFESSION_FACULTY_FK foreign key
        references FACULTY(FACULTY),
    PROFESSION_NAME nvarchar(100),    QUALIFICATION   nvarchar(50)
);
insert into PROFESSION(FACULTY, PROFESSION, PROFESSION_NAME, QUALIFICATION) values (N'ИТ', '1-40 01 02', N'Информационные системы и технологии', N'инженер-программист-системотехник');
insert into PROFESSION(FACULTY, PROFESSION, PROFESSION_NAME, QUALIFICATION) values (N'ИТ', '1-47 01 01', N'Издательское дело', N'редактор-технолог');
insert into PROFESSION(FACULTY, PROFESSION, PROFESSION_NAME, QUALIFICATION) values (N'ИТ', '1-36 06 01', N'Полиграфическое оборудование и системы обработки информации', N'инженер-электромеханик');
insert into PROFESSION(FACULTY, PROFESSION, PROFESSION_NAME, QUALIFICATION) values (N'ХТиТ', '1-36 01 08', N'Конструирование и производство изделий из композиционных материалов', N'инженер-механик');
insert into PROFESSION(FACULTY, PROFESSION, PROFESSION_NAME, QUALIFICATION) values (N'ХТиТ', '1-36 07 01', N'Машины и аппараты химических производств и предприятий строительных материалов', N'инженер-механик');
insert into PROFESSION(FACULTY, PROFESSION, PROFESSION_NAME, QUALIFICATION) values (N'ЛХФ', '1-75 01 01', N'Лесное хозяйство', N'инженер лесного хозяйства');
insert into PROFESSION(FACULTY, PROFESSION, PROFESSION_NAME, QUALIFICATION) values (N'ЛХФ', '1-75 02 01', N'Садово-парковое строительство', N'инженер садово-паркового строительства');
insert into PROFESSION(FACULTY, PROFESSION, PROFESSION_NAME, QUALIFICATION) values (N'ЛХФ', '1-89 02 02', N'Туризм и природопользование', N'специалист в сфере туризма');
insert into PROFESSION(FACULTY, PROFESSION, PROFESSION_NAME, QUALIFICATION) values (N'ИЭФ', '1-25 01 07', N'Экономика и управление на предприятии', N'экономист-менеджер');
insert into PROFESSION(FACULTY, PROFESSION, PROFESSION_NAME, QUALIFICATION) values (N'ИЭФ', '1-25 01 08', N'Бухгалтерский учет, анализ и аудит', N'экономист');
insert into PROFESSION(FACULTY, PROFESSION, PROFESSION_NAME, QUALIFICATION) values (N'ТТЛП', '1-36 05 01', N'Машины и оборудование лесного комплекса', N'инженер-механик');
insert into PROFESSION(FACULTY, PROFESSION, PROFESSION_NAME, QUALIFICATION) values (N'ТТЛП', '1-46 01 01', N'Лесоинженерное дело', N'инженер-технолог');
insert into PROFESSION(FACULTY, PROFESSION, PROFESSION_NAME, QUALIFICATION) values (N'ТОВ', '1-48 01 02', N'Химическая технология органических веществ, материалов и изделий', N'инженер-химик-технолог');
insert into PROFESSION(FACULTY, PROFESSION, PROFESSION_NAME, QUALIFICATION) values (N'ТОВ', '1-48 01 05', N'Химическая технология переработки древесины', N'инженер-химик-технолог');
insert into PROFESSION(FACULTY, PROFESSION, PROFESSION_NAME, QUALIFICATION) values (N'ТОВ', '1-54 01 03', N'Физико-химические методы и приборы контроля качества продукции', N'инженер по сертификации');

------ Создание и заполнение таблицы PULPIT
create table  PULPIT
(   PULPIT   nchar(20)  constraint PULPIT_PK  primary key,
    PULPIT_NAME  nvarchar(100),
    FACULTY   nchar(10)   constraint PULPIT_FACULTY_FK foreign key
        references FACULTY(FACULTY)
);
-- Базовые кафедры
insert into PULPIT (PULPIT, PULPIT_NAME, FACULTY) values (N'ИСиТ', N'Информационных систем и технологий', N'ИТ');
insert into PULPIT (PULPIT, PULPIT_NAME, FACULTY) values (N'ЛВ', N'Лесоводства', N'ЛХФ');
insert into PULPIT (N'ЛУ', N'Лесоустройства', N'ЛХФ');
insert into PULPIT (N'ЛЗиДВ', N'Лесозащиты и древесиноведения', N'ЛХФ');
insert into PULPIT (N'ЛКиП', N'Лесных культур и почвоведения', N'ЛХФ');
insert into PULPIT (N'ТиП', N'Туризма и природопользования', N'ЛХФ');
insert into PULPIT (N'ЛПиСПС', N'Ландшафтного проектирования и садово-паркового строительства', N'ЛХФ');
insert into PULPIT (N'ТЛ', N'Транспорта леса', N'ТТЛП');
insert into PULPIT (N'ЛМиЛЗ', N'Лесных машин и технологии лесозаготовок', N'ТТЛП');
insert into PULPIT (N'ТДП', N'Технологий деревообрабатывающих производств', N'ТТЛП');
insert into PULPIT (N'ТиДИД', N'Технологии и дизайна изделий из древесины', N'ТТЛП');
insert into PULPIT (N'ОХ', N'Органической химии', N'ТОВ');
insert into PULPIT (N'ХПД', N'Химической переработки древесины', N'ТОВ');
insert into PULPIT (N'ТНВиОХТ', N'Технологии неорганических веществ и общей химической технологии', N'ХТиТ');
insert into PULPIT (N'ПиАХП', N'Процессов и аппаратов химических производств', N'ХТиТ');
insert into PULPIT (N'ЭТиМ', N'Экономической теории и маркетинга', N'ИЭФ');
insert into PULPIT (N'МиЭП', N'Менеджмента и экономики природопользования', N'ИЭФ');
insert into PULPIT (N'СБУАиА', N'Статистики, бухгалтерского учета, анализа и аудита', N'ИЭФ');

-- ДОБАВЛЯЕМ НЕДОСТАЮЩИЕ КАФЕДРЫ (чтобы не было ошибок Foreign Key)
insert into PULPIT (PULPIT, PULPIT_NAME, FACULTY) values (N'ИВД', N'Инженерно-вычислительной деятельности', N'ИТ');
insert into PULPIT (PULPIT, PULPIT_NAME, FACULTY) values (N'ПОиСОИ', N'Программного обеспечения информ. систем', N'ИТ');
insert into PULPIT (PULPIT, PULPIT_NAME, FACULTY) values (N'ОВ', N'Охотоведения', N'ЛХФ');
insert into PULPIT (PULPIT, PULPIT_NAME, FACULTY) values (N'ТНХСиППМ', N'Технологии нефтехимического синтеза', N'ТОВ');
insert into PULPIT (PULPIT, PULPIT_NAME, FACULTY) values (N'ХТЭПиМЭЕ', N'Химии и технологии электрохим. производств', N'ХТиТ');

------ Создание и заполнение таблицы TEACHER
create table TEACHER
(   TEACHER    nchar(10)  constraint TEACHER_PK  primary key,
    TEACHER_NAME  nvarchar(100),
    GENDER     nchar(1) CHECK (GENDER in (N'м', N'ж')),
    PULPIT   nchar(20) constraint TEACHER_PULPIT_FK foreign key
        references PULPIT(PULPIT)
);
insert into  TEACHER (TEACHER, TEACHER_NAME, GENDER, PULPIT) values (N'СМЛВ', N'Смелов Владимир Владиславович', N'м', N'ИСиТ');
insert into  TEACHER (TEACHER, TEACHER_NAME, GENDER, PULPIT) values (N'ДТК', N'Дятко Александр Аркадьевич', N'м', N'ИВД');
insert into  TEACHER (TEACHER, TEACHER_NAME, GENDER, PULPIT) values (N'УРБ', N'Урбанович Павел Павлович', N'м', N'ИСиТ');
insert into  TEACHER (TEACHER, TEACHER_NAME, GENDER, PULPIT) values (N'ГРН', N'Гурин Николай Иванович', N'м', N'ИСиТ');
insert into  TEACHER (TEACHER, TEACHER_NAME, GENDER, PULPIT) values (N'ЖЛК', N'Жиляк Надежда Александровна', N'ж', N'ИСиТ');
insert into  TEACHER (TEACHER, TEACHER_NAME, GENDER, PULPIT) values (N'МРЗ', N'Мороз Елена Станиславовна', N'ж', N'ИСиТ');
insert into  TEACHER (TEACHER, TEACHER_NAME, GENDER, PULPIT) values (N'БРТШВЧ', N'Барташевич Святослав Александрович', N'м', N'ПОиСОИ');
insert into  TEACHER (TEACHER, TEACHER_NAME, GENDER, PULPIT) values (N'АРС', N'Арсентьев Виталий Арсентьевич', N'м', N'ПОиСОИ');
insert into  TEACHER (TEACHER, TEACHER_NAME, GENDER, PULPIT) values (N'НВРВ', N'Неверов Александр Васильевич', N'м', N'МиЭП');
insert into  TEACHER (TEACHER, TEACHER_NAME, GENDER, PULPIT) values (N'РВКЧ', N'Ровкач Андрей Иванович', N'м', N'ЛВ');
insert into  TEACHER (TEACHER, TEACHER_NAME, GENDER, PULPIT) values (N'ДМДК', N'Демидко Марина Николаевна', N'ж', N'ЛПиСПС');
insert into  TEACHER (TEACHER, TEACHER_NAME, GENDER, PULPIT) values (N'БРГ', N'Бурганская Татьяна Минаевна', N'ж', N'ЛПиСПС');
insert into  TEACHER (TEACHER, TEACHER_NAME, GENDER, PULPIT) values (N'РЖК', N'Рожков Леонид Николаевич', N'м', N'ЛВ');
insert into  TEACHER (TEACHER, TEACHER_NAME, GENDER, PULPIT) values (N'ЗВГЦВ', N'Звягинцев Вячеслав Борисович', N'м', N'ЛЗиДВ');
insert into  TEACHER (TEACHER, TEACHER_NAME, GENDER, PULPIT) values (N'БЗБРДВ', N'Безбородов Владимир Степанович', N'м', N'ОХ');
insert into  TEACHER (TEACHER, TEACHER_NAME, GENDER, PULPIT) values (N'НСКВЦ', N'Насковец Михаил Трофимович', N'м', N'ТЛ');

------ Создание и заполнение таблицы SUBJECT
create table SUBJECT
(     SUBJECT  nchar(10) constraint SUBJECT_PK  primary key,
      SUBJECT_NAME nvarchar(100) unique,
      PULPIT  nchar(20) constraint SUBJECT_PULPIT_FK foreign key
          references PULPIT(PULPIT)
)
    insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'СУБД', N'Системы управления базами данных', N'ИСиТ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'БД', N'Базы данных', N'ИСиТ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ИНФ', N'Информационные технологии', N'ИСиТ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ОАиП', N'Основы алгоритмизации и программирования', N'ИСиТ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ПЗ', N'Представление знаний в компьютерных системах', N'ИСиТ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ПСП', N'Программирование сетевых приложений', N'ИСиТ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'МСОИ', N'Моделирование систем обработки информации', N'ИСиТ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ПИС', N'Проектирование информационных систем', N'ИСиТ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'КГ', N'Компьютерная геометрия', N'ИСиТ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ПМАПЛ', N'Полиграф. машины, автоматы и поточные линии', N'ПОиСОИ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'КМС', N'Компьютерные мультимедийные системы', N'ИСиТ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ОПП', N'Организация полиграф. производства', N'ПОиСОИ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ДМ', N'Дискретная математика', N'ИСиТ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'МП', N'Математическое программирование', N'ИСиТ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ЛЭВМ', N'Логические основы ЭВМ', N'ИСиТ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ООП', N'Объектно-ориентированное программирование', N'ИСиТ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ЭП', N'Экономика природопользования', N'МиЭП');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ЭТ', N'Экономическая теория', N'ЭТиМ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'БЛЗиПсOO', N'Биология лесных зверей и птиц с осн. охотов.', N'ОВ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ОСПиЛПХ', N'Основы садово-паркового и лесопаркового хозяйства', N'ЛПиСПС');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ИГ', N'Инженерная геодезия', N'ЛУ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ЛВ', N'Лесоводство', N'ЛЗиДВ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ОХ', N'Органическая химия', N'ОХ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ТРИ', N'Технология резиновых изделий', N'ТНХСиППМ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ВТЛ', N'Водный транспорт леса', N'ТЛ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ТиОЛ', N'Технология и оборудование лесозаготовок', N'ЛМиЛЗ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ТОПИ', N'Технология обогащения полезных ископаемых', N'ТНВиОХТ');
insert into SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) values (N'ПЭХ', N'Прикладная электрохимия', N'ХТЭПиМЭЕ');

------ Создание и заполнение таблицы GROUPS
create table GROUPS
(   IDGROUP  integer  identity(1,1) constraint GROUP_PK  primary key,
    FACULTY   nchar(10) constraint  GROUPS_FACULTY_FK foreign key
        references FACULTY(FACULTY),
    PROFESSION  nchar(20) constraint  GROUPS_PROFESSION_FK foreign key
        references PROFESSION(PROFESSION),
    YEAR_FIRST  smallint  check (YEAR_FIRST<=YEAR(GETDATE())),
)
    insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ИТ','1-40 01 02', 2013);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ИТ','1-40 01 02', 2012);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ИТ','1-40 01 02', 2011);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ИТ','1-40 01 02', 2010);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ИТ','1-47 01 01', 2013);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ИТ','1-47 01 01', 2012);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ИТ','1-47 01 01', 2011);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ИТ','1-36 06 01', 2010);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ИТ','1-36 06 01', 2013);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ИТ','1-36 06 01', 2012);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ИТ','1-36 06 01', 2011);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ХТиТ','1-36 01 08', 2013);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ХТиТ','1-36 01 08', 2012);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ХТиТ','1-36 07 01', 2011);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ХТиТ','1-36 07 01', 2010);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ТОВ','1-48 01 02', 2012);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ТОВ','1-48 01 02', 2011);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ТОВ','1-48 01 05', 2013);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ТОВ','1-54 01 03', 2012);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ЛХФ','1-75 01 01', 2013);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ЛХФ','1-75 02 01', 2012);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ЛХФ','1-75 02 01', 2011);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ЛХФ','1-89 02 02', 2012);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ЛХФ','1-89 02 02', 2011);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ТТЛП','1-36 05 01', 2013);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ТТЛП','1-36 05 01', 2012);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ТТЛП','1-46 01 01', 2012);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ИЭФ','1-25 01 07', 2013);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ИЭФ','1-25 01 07', 2012);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ИЭФ','1-25 01 07', 2010);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ИЭФ','1-25 01 08', 2013);
insert into GROUPS (FACULTY, PROFESSION, YEAR_FIRST) values (N'ИЭФ','1-25 01 08', 2012);

------ Создание и заполнение таблицы STUDENT
create table STUDENT
(    IDSTUDENT   integer  identity(1000,1) constraint STUDENT_PK  primary key,
     IDGROUP   integer  constraint STUDENT_GROUP_FK foreign key
         references GROUPS(IDGROUP),
     NAME   nvarchar(100),
     BDAY   date,
     STAMP  timestamp,
     INFO     xml,
     FOTO     varbinary
)
    insert into STUDENT (IDGROUP, NAME, BDAY)
values (2, N'Силюк Валерия Ивановна', '1994-07-12'),
       (2, N'Сергель Виолетта Николаевна', '1994-03-06'),
       (2, N'Добродей Ольга Анатольевна', '1994-11-09'),
       (2, N'Подоляк Мария Сергеевна', '1994-10-04'),
       (2, N'Никитенко Екатерина Дмитриевна', '1994-01-08'),
       (3, N'Яцкевич Галина Иосифовна', '1993-08-02'),
       (3, N'Осадчая Эла Васильевна', '1993-12-07'),
       (3, N'Акулова Елена Геннадьевна', '1993-12-02'),
       (4, N'Плешкун Милана Анатольевна', '1992-03-08'),
       (4, N'Буянова Мария Александровна', '1992-06-02'),
       (4, N'Харченко Елена Геннадьевна', '1992-12-11'),
       (4, N'Крученок Евгений Александрович', '1992-05-11'),
       (4, N'Бороховский Виталий Петрович', '1992-11-09'),
       (4, N'Мацкевич Надежда Валерьевна', '1992-11-01'),
       (5, N'Логинова Мария Вячеславовна', '1995-07-08'),
       (5, N'Белько Наталья Николаевна', '1995-11-02'),
       (5, N'Селило Екатерина Геннадьевна', '1995-05-07'),
       (5, N'Дрозд Анастасия Андреевна', '1995-08-04'),
       (6, N'Козловская Елена Евгеньевна', '1994-11-08'),
       (6, N'Потапнин Кирилл Олегович', '1994-03-02'),
       (6, N'Равковская Ольга Николаевна', '1994-06-04'),
       (6, N'Ходоронок Александра Вадимовна', '1994-11-09'),
       (6, N'Рамук Владислав Юрьевич', '1994-07-04'),
       (7, N'Неруганенок Мария Владимировна', '1993-01-03'),
       (7, N'Цыганок Анна Петровна', '1993-09-12'),
       (7, N'Масилевич Оксана Игоревна', '1993-06-12'),
       (7, N'Алексиевич Елизавета Викторовна', '1993-02-09'),
       (7, N'Ватолин Максим Андреевич', '1993-07-04'),
       (8, N'Синица Валерия Андреевна', '1992-01-08'),
       (8, N'Кудряшова Алина Николаевна', '1992-05-12'),
       (8, N'Мигулина Елена Леонидовна', '1992-11-08'),
       (8, N'Шпиленя Алексей Сергеевич', '1992-03-12'),
       (9, N'Астафьев Игорь Александрович', '1995-08-10'),
       (9, N'Гайтюкевич Андрей Игоревич', '1995-05-02'),
       (9, N'Рученя Наталья Александровна', '1995-01-08'),
       (9, N'Тарасевич Анастасия Ивановна', '1995-09-11'),
       (10, N'Жоглин Николай Владимирович', '1994-01-08'),
       (10, N'Санько Андрей Дмитриевич', '1994-09-11'),
       (10, N'Пещур Анна Александровна', '1994-04-06'),
       (10, N'Бучалис Никита Леонидович', '1994-08-12');

insert into STUDENT (IDGROUP, NAME, BDAY)
values (11, N'Лавренчук Владислав Николаевич', '1993-11-07'),
       (11, N'Власик Евгения Викторовна', '1993-06-04'),
       (11, N'Абрамов Денис Дмитриевич', '1993-12-10'),
       (11, N'Оленчик Сергей Николаевич', '1993-07-04'),
       (11, N'Савинко Павел Андреевич', '1993-01-08'),
       (11, N'Бакун Алексей Викторович', '1993-09-02'),
       (12, N'Бань Сергей Анатольевич', '1995-12-11'),
       (12, N'Сечейко Илья Александрович', '1995-06-10'),
       (12, N'Кузмичева Анна Андреевна', '1995-08-09'),
       (12, N'Бурко Диана Францевна', '1995-07-04'),
       (12, N'Даниленко Максим Васильевич', '1995-03-08'),
       (12, N'Зизюк Ольга Олеговна', '1995-09-12'),
       (13, N'Шарапо Мария Владимировна', '1994-10-08'),
       (13, N'Касперович Вадим Викторович', '1994-02-10'),
       (13, N'Чупрыгин Арсений Александрович', '1994-11-11'),
       (13, N'Воеводская Ольга Леонидовна', '1994-02-10'),
       (13, N'Метушевский Денис Игоревич', '1994-01-12'),
       (14, N'Ловецкая Валерия Александровна', '1993-09-11'),
       (14, N'Дворак Антонина Николаевна', '1993-12-01'),
       (14, N'Щука Татьяна Николаевна', '1993-06-09'),
       (14, N'Коблинец Александра Евгеньевна', '1993-01-05'),
       (14, N'Фомичевская Елена Эрнестовна', '1993-07-01'),
       (15, N'Бесараб Маргарита Вадимовна', '1992-04-07'),
       (15, N'Бадуро Виктория Сергеевна', '1992-12-10'),
       (15, N'Тарасенко Ольга Викторовна', '1992-05-05'),
       (15, N'Афанасенко Ольга Владимировна', '1992-01-11'),
       (15, N'Чуйкевич Ирина Дмитриевна', '1992-06-04'),
       (16, N'Брель Алеся Алексеевна', '1994-01-08'),
       (16, N'Кузнецова Анастасия Андреевна', '1994-02-07'),
       (16, N'Томина Карина Геннадьевна', '1994-06-12'),
       (16, N'Дуброва Павел Игоревич', '1994-07-03'),
       (16, N'Шпаков Виктор Андреевич', '1994-07-04'),
       (17, N'Шнейдер Анастасия Дмитриевна', '1993-11-08'),
       (17, N'Шыгина Елена Викторовна', '1993-04-02'),
       (17, N'Клюева Анна Ивановна', '1993-06-03'),
       (17, N'Доморад Марина Андреевна', '1993-11-05'),
       (17, N'Линчук Михаил Александрович', '1993-07-03'),
       (18, N'Васильева Дарья Олеговна', '1995-01-08'),
       (18, N'Щигельская Екатерина Андреевна', '1995-09-06'),
       (18, N'Сазонова Екатерина Дмитриевна', '1995-03-08'),
       (18, N'Бакунович Алина Олеговна', '1995-08-07');

------ Создание и заполнение таблицы PROGRESS
create table PROGRESS
(  SUBJECT   nchar(10) constraint PROGRESS_SUBJECT_FK foreign key
    references SUBJECT(SUBJECT),
   IDSTUDENT integer  constraint PROGRESS_IDSTUDENT_FK foreign key
       references STUDENT(IDSTUDENT),
   PDATE    date,
   NOTE     integer check (NOTE between 1 and 10)
)
    insert into PROGRESS (SUBJECT, IDSTUDENT, PDATE, NOTE)
values  (N'ОАиП', 1001, '2013-10-01', 8),
        (N'ОАиП', 1002, '2013-10-01', 7),
        (N'ОАиП', 1003, '2013-10-01', 5),
        (N'ОАиП', 1005, '2013-10-01', 4);
insert into PROGRESS (SUBJECT, IDSTUDENT, PDATE, NOTE)
values   (N'СУБД', 1014, '2013-12-01', 5),
         (N'СУБД', 1015, '2013-12-01', 9),
         (N'СУБД', 1016, '2013-12-01', 5),
         (N'СУБД', 1017, '2013-12-01', 4);
insert into PROGRESS (SUBJECT, IDSTUDENT, PDATE, NOTE)
values (N'КГ', 1018, '2013-05-06', 4),
       (N'КГ', 1019, '2013-05-06', 7),
       (N'КГ', 1020, '2013-05-06', 7),
       (N'КГ', 1021, '2013-05-06', 9),
       (N'КГ', 1022, '2013-05-06', 5),
       (N'КГ', 1023, '2013-05-06', 6);