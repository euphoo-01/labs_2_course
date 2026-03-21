CREATE TABLE IF NOT EXISTS CoursePriceAudit (
    Id SERIAL PRIMARY KEY,
    CourseId INT NOT NULL,
    OldPrice NUMERIC(10, 2),
    NewPrice NUMERIC(10, 2),
    ChangedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE OR REPLACE FUNCTION log_course_price_change()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.Price <> OLD.Price THEN
        INSERT INTO CoursePriceAudit(CourseId, OldPrice, NewPrice)
        VALUES (OLD.Id, OLD.Price, NEW.Price);
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_course_price_change ON Courses;
CREATE TRIGGER trg_course_price_change
AFTER UPDATE ON Courses
FOR EACH ROW
EXECUTE FUNCTION log_course_price_change();

CREATE OR REPLACE FUNCTION prevent_category_deletion()
RETURNS TRIGGER AS $$
BEGIN
    IF EXISTS (SELECT 1 FROM Courses WHERE CategoryId = OLD.Id) THEN
        RAISE EXCEPTION 'Cannot delete category "%" because it contains courses.', OLD.Name;
    END IF;
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_prevent_category_deletion ON Categories;
CREATE TRIGGER trg_prevent_category_deletion
BEFORE DELETE ON Categories
FOR EACH ROW
EXECUTE FUNCTION prevent_category_deletion();

CREATE OR REPLACE FUNCTION update_course_availability()
RETURNS TRIGGER AS $$
BEGIN
    IF NEW.Quantity > 0 THEN
        NEW.IsAvailable := TRUE;
    ELSE
        NEW.IsAvailable := FALSE;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

DROP TRIGGER IF EXISTS trg_update_course_availability ON Courses;
CREATE TRIGGER trg_update_course_availability
BEFORE INSERT OR UPDATE ON Courses
FOR EACH ROW
EXECUTE FUNCTION update_course_availability();
