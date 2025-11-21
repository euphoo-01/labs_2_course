#include "Modules/Headers/Error.h"

//TODO: Исключения для: Размер литерала, длина идентификатора,

namespace Error {
    // 0-99   - Системные ошибки
    // 100-110 - Ошибки параметров
    // 111-125 - Ошибки лексического анализатора
    // 126-136 - Ошибки семантического анализа
    // 600-610 - Ошибки синтаксического анализа
    ERROR errors[ERROR_MAX_ENTRY] =
    {
        ERROR_ENTRY(0, "Недопустимый код ошибки"),
        ERROR_ENTRY(1, "Системный сбой"),
        ERROR_ENTRY_NODEF(2), ERROR_ENTRY_NODEF(3), ERROR_ENTRY_NODEF(4), ERROR_ENTRY_NODEF(5), ERROR_ENTRY_NODEF(6),
        ERROR_ENTRY(7, "Ошибка создания потока логирования"),
        ERROR_ENTRY(8, "Ошибка обработки протокола"),
        ERROR_ENTRY(9, "Ошибка создания потока вывода"),
        ERROR_ENTRY(10, "Превышена длина входного файла"),
        ERROR_ENTRY_NODEF(11), ERROR_ENTRY_NODEF(12), ERROR_ENTRY_NODEF(13), ERROR_ENTRY_NODEF(14),
        ERROR_ENTRY_NODEF(15), ERROR_ENTRY_NODEF(16), ERROR_ENTRY_NODEF(17), ERROR_ENTRY_NODEF(18), ERROR_ENTRY_NODEF(19),
        ERROR_ENTRY_NODEF10(20), ERROR_ENTRY_NODEF10(30), ERROR_ENTRY_NODEF10(40), ERROR_ENTRY_NODEF10(50),
        ERROR_ENTRY_NODEF10(60), ERROR_ENTRY_NODEF10(70), ERROR_ENTRY_NODEF10(80), ERROR_ENTRY_NODEF10(90),

        ERROR_ENTRY(100, "Параметр -in должен быть задан"),
        ERROR_ENTRY(101, "Недопустимый входной параметр"),
        ERROR_ENTRY(102, "Пустой входной параметр"),
        ERROR_ENTRY(103, "Ошибка при открытии файла с исходным кодом (-in)"),
        ERROR_ENTRY(104, "Превышена длина входного параметра"),
        ERROR_ENTRY_NODEF(105), ERROR_ENTRY_NODEF(106), ERROR_ENTRY_NODEF(107), ERROR_ENTRY_NODEF(108), ERROR_ENTRY_NODEF(109),
        ERROR_ENTRY(110, "Слишком много параметров"), // From Parm.cpp, was 101

        ERROR_ENTRY(111, "Недопустимый символ в исходном коде"),
        ERROR_ENTRY(112, "Незакрытый строковый/символьный литерал"),
        ERROR_ENTRY_NODEF(113), ERROR_ENTRY_NODEF(114), ERROR_ENTRY_NODEF(115),
        ERROR_ENTRY(116, "Длина идентификатора превышает 3 символа"),
        ERROR_ENTRY(117, "Превышена длина таблицы лексем"), // Was 2
        ERROR_ENTRY(118, "Превышен размер таблицы идентификаторов"), // Was 4
        ERROR_ENTRY_NODEF(119), ERROR_ENTRY_NODEF(120), ERROR_ENTRY_NODEF(121), ERROR_ENTRY_NODEF(122),
        ERROR_ENTRY_NODEF(123), ERROR_ENTRY_NODEF(124), ERROR_ENTRY_NODEF(125),

        ERROR_ENTRY(126, "Неверный идентификатор в таблице лексем"), // Was 3
        ERROR_ENTRY(127, "Неверный идентификатор таблицы идентификаторов"), // Was 5
        ERROR_ENTRY_NODEF(128), ERROR_ENTRY_NODEF(129), ERROR_ENTRY_NODEF(130), ERROR_ENTRY_NODEF(131),
        ERROR_ENTRY_NODEF(132), ERROR_ENTRY_NODEF(133), ERROR_ENTRY_NODEF(134), ERROR_ENTRY_NODEF(135), ERROR_ENTRY_NODEF(136),
        ERROR_ENTRY_NODEF(137), ERROR_ENTRY_NODEF(138), ERROR_ENTRY_NODEF(139), ERROR_ENTRY_NODEF(140),

        ERROR_ENTRY_NODEF10(140),ERROR_ENTRY_NODEF10(150), ERROR_ENTRY_NODEF10(160), ERROR_ENTRY_NODEF10(170),
        ERROR_ENTRY_NODEF10(180), ERROR_ENTRY_NODEF10(190),
        ERROR_ENTRY_NODEF100(200), ERROR_ENTRY_NODEF100(300), ERROR_ENTRY_NODEF100(400),
        ERROR_ENTRY_NODEF100(500),

        ERROR_ENTRY(600, "Неверная структура программы"),
        ERROR_ENTRY(601, "Ошибочный оператор"),
        ERROR_ENTRY(602, "Ошибка в выражении"),
        ERROR_ENTRY(603, "Ошибка в параметрах функции"),
        ERROR_ENTRY(604, "Ошибка в параметрах вызываемой функции"),
        ERROR_ENTRY_NODEF(605), ERROR_ENTRY_NODEF(606), ERROR_ENTRY_NODEF(607),
        ERROR_ENTRY_NODEF(608), ERROR_ENTRY_NODEF(609),
        ERROR_ENTRY_NODEF10(610), ERROR_ENTRY_NODEF10(620), ERROR_ENTRY_NODEF10(630),
        ERROR_ENTRY_NODEF10(640), ERROR_ENTRY_NODEF10(650), ERROR_ENTRY_NODEF10(660),
        ERROR_ENTRY_NODEF10(670), ERROR_ENTRY_NODEF10(680), ERROR_ENTRY_NODEF10(690),
        ERROR_ENTRY_NODEF100(700), ERROR_ENTRY_NODEF100(800), ERROR_ENTRY_NODEF100(900)
    };

    ERROR geterror(int id) {
        if (id < 0) {
            throw ERROR_THROW(0);
        }
        return errors[id];
    }

    ERROR geterrorin(int id, int line = -1, int col = -1) {
        if (id < 0) {
            throw ERROR_THROW(0);
        }
        ERROR temp = errors[id];
        temp.inext.col = col;
        temp.inext.line = line;
        return temp;
    }
}
