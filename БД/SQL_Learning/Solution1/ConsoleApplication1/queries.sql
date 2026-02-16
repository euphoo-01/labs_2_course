SELECT Наименование_товара
FROM ЗАКАЗЫ
WHERE Дата_поставки > '2026-02-20';

SELECT Наименование, Цена
FROM ТОВАРЫ
WHERE Цена BETWEEN 200 AND 250;

SELECT Заказчик
FROM ЗАКАЗЫ
WHERE Наименование_товара = N'Монитор';

SELECT *
FROM ЗАКАЗЫ
WHERE Заказчик = N'ООО "Тлижу"'
ORDER BY Дата_поставки;