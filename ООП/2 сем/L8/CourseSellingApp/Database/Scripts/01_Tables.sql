-- Скрипт создания таблиц

CREATE TABLE IF NOT EXISTS Categories (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE IF NOT EXISTS Authors (
    Id SERIAL PRIMARY KEY,
    FullName VARCHAR(150) NOT NULL,
    Biography TEXT,
    Photo BYTEA -- Графическая информация (Требование 2)
);

CREATE TABLE IF NOT EXISTS Courses (
    Id SERIAL PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    FullName VARCHAR(300),
    Description TEXT,
    Price NUMERIC(10, 2) NOT NULL CHECK (Price >= 0),
    Quantity INT NOT NULL DEFAULT 0,
    Discount DOUBLE PRECISION DEFAULT 0,
    IsAvailable BOOLEAN DEFAULT TRUE,
    PurchasesCount INT DEFAULT 0,
    Rating NUMERIC(3, 2) DEFAULT 0 CHECK (Rating >= 0 AND Rating <= 5),
    CoverImage BYTEA, -- Графическая информация (Требование 2)
    CoverImagePath VARCHAR(500),
    ImagePaths TEXT, -- Будет хранить JSON массив строк
    RelatedCoursesIds TEXT, -- Будет хранить JSON массив ID
    CategoryId INT NOT NULL,
    AuthorId INT,
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (CategoryId) REFERENCES Categories(Id) ON DELETE RESTRICT,
    FOREIGN KEY (AuthorId) REFERENCES Authors(Id) ON DELETE SET NULL
);

-- Индексы для ускорения поиска
CREATE INDEX IF NOT EXISTS idx_courses_category_id ON Courses(CategoryId);
CREATE INDEX IF NOT EXISTS idx_courses_author_id ON Courses(AuthorId);
CREATE INDEX IF NOT EXISTS idx_courses_price ON Courses(Price);
