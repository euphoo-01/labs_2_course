#include "Modules/Headers/Error.h"

namespace Error {
    // 0-99 - Системные ошибки
    // 100-109 - Ошибки параметров
    // 110-119 - Ошибка открытия и чтения файлов
    ERROR errors[ERROR_MAX_ENTRY] =
    {
        ERROR_ENTRY(0, "Недопустимый код ошибки"),
        ERROR_ENTRY(1, "Системный сбой"),
        ERROR_ENTRY(2, "Превышена длина таблицы лексем"),
        ERROR_ENTRY(3, "Неверный идентификатор в таблице лексем"),
        ERROR_ENTRY(4, "Превышен размер таблицы идентификаторов"),
        ERROR_ENTRY(5, "Неверный идентификатор таблицы идентификаторов"),
        ERROR_ENTRY(6, "Недопустимая лексема"), ERROR_ENTRY_NODEF(7), ERROR_ENTRY_NODEF(8), ERROR_ENTRY_NODEF(9),
        ERROR_ENTRY_NODEF10(10), ERROR_ENTRY_NODEF10(20), ERROR_ENTRY_NODEF10(30),
        ERROR_ENTRY_NODEF10(40), ERROR_ENTRY_NODEF10(50), ERROR_ENTRY_NODEF10(60),
        ERROR_ENTRY_NODEF10(70), ERROR_ENTRY_NODEF10(80), ERROR_ENTRY_NODEF10(90),
        ERROR_ENTRY(100, "Параметр -in должен быть задан"),
        ERROR_ENTRY(101, "Недопустимый входной параметр"),
        ERROR_ENTRY(102, "Пустой входной параметр"),
        ERROR_ENTRY_NODEF(103),
        ERROR_ENTRY(104, "Превышена длина входного параметра"),
        ERROR_ENTRY_NODEF(105), ERROR_ENTRY_NODEF(106), ERROR_ENTRY_NODEF(107),
        ERROR_ENTRY_NODEF(108), ERROR_ENTRY_NODEF(109),
        ERROR_ENTRY(110, "Ошибка при открытии файла с исходным кодом (-in)"),
        ERROR_ENTRY(111, "Недопустимый символ"),
        ERROR_ENTRY(112, "Ошибка создания потока логирования"),
        ERROR_ENTRY(113, "Ошибка обработки протокола"),
        ERROR_ENTRY(114, "Ошибка создания потока вывода"),
        ERROR_ENTRY(115, "Превышена длина входного файла")
    };

    ERROR geterror(int id) {
        // TODO Исключение
        return errors[id];
    }

    ERROR geterrorin(int id, int line = -1, int col = -1) {
        // TODO Исключение
        ERROR temp = errors[id];
        temp.inext.col = col;
        temp.inext.line = line;
        return temp;
    }
}
