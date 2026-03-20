-- Скрипт создания хранимых процедур (Требование 2 и 4c)

-- 1. Добавление нового курса
CREATE OR REPLACE PROCEDURE add_course_proc(
    p_name VARCHAR(200),
    p_full_name VARCHAR(300),
    p_description TEXT,
    p_price NUMERIC(10,2),
    p_quantity INT,
    p_discount DOUBLE PRECISION,
    p_rating NUMERIC(3,2),
    p_category_id INT,
    p_author_id INT,
    p_cover_image_path VARCHAR(500),
    p_image_paths TEXT,
    p_related_courses_ids TEXT
)
LANGUAGE plpgsql
AS $$
BEGIN
    INSERT INTO Courses (
        Name, FullName, Description, Price, Quantity, Discount, Rating,
        CategoryId, AuthorId, CoverImagePath, ImagePaths, RelatedCoursesIds
    )
    VALUES (
        p_name, p_full_name, p_description, p_price, p_quantity, p_discount, p_rating,
        p_category_id, p_author_id, p_cover_image_path, p_image_paths, p_related_courses_ids
    );
END;
$$;

-- 2. Получение списка курсов с информацией о категории и авторе
CREATE OR REPLACE FUNCTION get_all_courses_with_details()
RETURNS TABLE (
    Id INT,
    Name VARCHAR(200),
    FullName VARCHAR(300),
    Description TEXT,
    Price NUMERIC(10, 2),
    Quantity INT,
    Discount DOUBLE PRECISION,
    IsAvailable BOOLEAN,
    PurchasesCount INT,
    Rating NUMERIC(3, 2),
    CoverImagePath VARCHAR(500),
    ImagePaths TEXT,
    RelatedCoursesIds TEXT,
    CategoryId INT,
    CategoryName VARCHAR(100),
    AuthorId INT,
    AuthorName VARCHAR(150)
)
AS $$
BEGIN
    RETURN QUERY
    SELECT
        c.Id,
        c.Name,
        c.FullName,
        c.Description,
        c.Price,
        c.Quantity,
        c.Discount,
        c.IsAvailable,
        c.PurchasesCount,
        c.Rating,
        c.CoverImagePath,
        c.ImagePaths,
        c.RelatedCoursesIds,
        c.CategoryId,
        cat.Name AS CategoryName,
        c.AuthorId,
        a.FullName AS AuthorName
    FROM Courses c
    JOIN Categories cat ON c.CategoryId = cat.Id
    LEFT JOIN Authors a ON c.AuthorId = a.Id
    ORDER BY c.Name ASC;
END;
$$ LANGUAGE plpgsql;

-- 3. Обновление цены курса через хранимую процедуру с валидацией
CREATE OR REPLACE PROCEDURE update_course_price_proc(
    p_course_id INT,
    p_new_price NUMERIC(10, 2)
)
LANGUAGE plpgsql
AS $$
BEGIN
    IF p_new_price < 0 THEN
        RAISE EXCEPTION 'Price cannot be negative.';
    END IF;

    UPDATE Courses SET Price = p_new_price WHERE Id = p_course_id;
END;
$$;

-- 4. Удаление курса по ID (хранимая процедура)
CREATE OR REPLACE PROCEDURE delete_course_proc(
    p_course_id INT
)
LANGUAGE plpgsql
AS $$
BEGIN
    DELETE FROM Courses WHERE Id = p_course_id;
END;
$$;
