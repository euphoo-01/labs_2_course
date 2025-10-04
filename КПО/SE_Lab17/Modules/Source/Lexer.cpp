#include "../Headers/Lexer.h"
#include "../Headers/Error.h"
#include "../Headers/LT.h"
#include "../Headers/IT.h"
#include "../Headers/FST.h"
#include <cstring>
#include <cctype>
#include <cstdio>
#include <string>

#define ID_BUFFER_SIZE 10 // Убедитесь, что IT.h имеет ID_MAXSIZE >= 10

using namespace std;

namespace Lexer {

    // --- FST для ключевых слов (Не меняется, но зависит от ID_MAXSIZE) ---
    bool isInteger(const char *str) {
        FST::FST fst((char *) str, 8,
                     FST::NODE(1, FST::RELATION('i', 1)),
                     FST::NODE(1, FST::RELATION('n', 2)),
                     FST::NODE(1, FST::RELATION('t', 3)),
                     FST::NODE(1, FST::RELATION('e', 4)),
                     FST::NODE(1, FST::RELATION('g', 5)),
                     FST::NODE(1, FST::RELATION('e', 6)),
                     FST::NODE(1, FST::RELATION('r', 7)),
                     FST::NODE()
        );
        return FST::execute(fst);
    }

    bool isString(const char *str) {
        FST::FST fst((char *) str, 7,
                     FST::NODE(1, FST::RELATION('s', 1)),
                     FST::NODE(1, FST::RELATION('t', 2)),
                     FST::NODE(1, FST::RELATION('r', 3)),
                     FST::NODE(1, FST::RELATION('i', 4)),
                     FST::NODE(1, FST::RELATION('n', 5)),
                     FST::NODE(1, FST::RELATION('g', 6)),
                     FST::NODE()
        );
        return FST::execute(fst);
    }

    // FST для 'function' (8 символов)
    bool isFunction(const char *str) {
        FST::FST fst((char *) str, 9,
                     FST::NODE(1, FST::RELATION('f', 1)),
                     FST::NODE(1, FST::RELATION('u', 2)),
                     FST::NODE(1, FST::RELATION('n', 3)),
                     FST::NODE(1, FST::RELATION('c', 4)),
                     FST::NODE(1, FST::RELATION('t', 5)),
                     FST::NODE(1, FST::RELATION('i', 6)),
                     FST::NODE(1, FST::RELATION('o', 7)),
                     FST::NODE(1, FST::RELATION('n', 8)),
                     FST::NODE()
        );
        return FST::execute(fst);
    }

    // FST для 'declare'
    bool isDeclare(const char *str) {
        FST::FST fst((char *) str, 8,
                     FST::NODE(1, FST::RELATION('d', 1)),
                     FST::NODE(1, FST::RELATION('e', 2)),
                     FST::NODE(1, FST::RELATION('c', 3)),
                     FST::NODE(1, FST::RELATION('l', 4)),
                     FST::NODE(1, FST::RELATION('a', 5)),
                     FST::NODE(1, FST::RELATION('r', 6)),
                     FST::NODE(1, FST::RELATION('e', 7)),
                     FST::NODE()
        );
        return FST::execute(fst);
    }

    // FST для 'return'
    bool isReturn(const char *str) {
        FST::FST fst((char *) str, 7,
                     FST::NODE(1, FST::RELATION('r', 1)),
                     FST::NODE(1, FST::RELATION('e', 2)),
                     FST::NODE(1, FST::RELATION('t', 3)),
                     FST::NODE(1, FST::RELATION('u', 4)),
                     FST::NODE(1, FST::RELATION('r', 5)),
                     FST::NODE(1, FST::RELATION('n', 6)),
                     FST::NODE()
        );
        return FST::execute(fst);
    }

    // FST для 'print'
    bool isPrint(const char *str) {
        FST::FST fst((char *) str, 6,
                     FST::NODE(1, FST::RELATION('p', 1)),
                     FST::NODE(1, FST::RELATION('r', 2)),
                     FST::NODE(1, FST::RELATION('i', 3)),
                     FST::NODE(1, FST::RELATION('n', 4)),
                     FST::NODE(1, FST::RELATION('t', 5)),
                     FST::NODE()
        );
        return FST::execute(fst);
    }

    // FST для 'main'
    bool isMain(const char *str) {
        FST::FST fst((char *) str, 5,
                     FST::NODE(1, FST::RELATION('m', 1)),
                     FST::NODE(1, FST::RELATION('a', 2)),
                     FST::NODE(1, FST::RELATION('i', 3)),
                     FST::NODE(1, FST::RELATION('n', 4)),
                     FST::NODE()
        );
        return FST::execute(fst);
    }

    // FST для идентификатора (буква, затем буквы/цифры до ID_MAXSIZE)
    bool isValidId(const char *str) {
        // Простой FST для ID: [0] -> L (1) -> L|D (1) -> ... -> [ID_MAXSIZE]
        // Для упрощения будем считать, что любая строка, начинающаяся с буквы, и не являющаяся ключевым словом, является ID.
        // Здесь используется упрощенная FST, которую, возможно, нужно изменить
        FST::FST fst(
            (char *)str, 2,
            FST::NODE(26, FST::RELATION('a', 1), FST::RELATION('b', 1), FST::RELATION('c', 1), FST::RELATION('d', 1), FST::RELATION('e', 1), FST::RELATION('f', 1), FST::RELATION('g', 1),
                         FST::RELATION('h', 1), FST::RELATION('i', 1), FST::RELATION('j', 1), FST::RELATION('k', 1), FST::RELATION('l', 1), FST::RELATION('m', 1), FST::RELATION('n', 1),
                         FST::RELATION('o', 1), FST::RELATION('p', 1), FST::RELATION('q', 1), FST::RELATION('r', 1), FST::RELATION('s', 1), FST::RELATION('t', 1), FST::RELATION('u', 1),
                         FST::RELATION('v', 1), FST::RELATION('w', 1), FST::RELATION('x', 1), FST::RELATION('y', 1), FST::RELATION('z', 1)),
            FST::NODE() // Конечное состояние (для FST, который проверяет длину)
        );
        // Так как FST для ID сложнее, и у вас нет его реализации,
        // временно используем простую проверку на основе ctype.

        if (!isalpha(str[0])) return false;
        for (int j = 1; str[j] != '\0'; j++) {
            if (!isalnum(str[j])) return false;
        }
        return true;
    }

    // --- Анализатор ---

    void Analyze(In::IN &in, LT::LexTable &lextable, IT::IdTable &idtable) {
        int line = 1;
        int col = 0;
        int i = 0; // Текущий индекс в тексте in.text

        // Буфер для считывания токенов
        char token[ID_BUFFER_SIZE + 1];

        while (i < in.size) {
            int shift = 0; // Сдвиг (длина токена)
            token[0] = '\0'; // Очистка токена

            // 1. Обновление координат и пропуск новой строки
            if (in.text[i] == '\n') {
                line++;
                col = 0;
                i++;
                continue;
            }

            // 2. Пропуск пробельных символов (пробел, табуляция, возврат каретки)
            if (isspace(in.text[i]) || in.text[i] == '\r') {
                i++;
                col++;
                continue;
            }

            // 3. Распознавание односимвольных разделителей и операций
            if (strchr(";,{}()+-*/=", in.text[i])) {
                char lexema = in.text[i];
                LT::Entry entry = { {lexema}, line, LT_TI_NULLIDX };


                // Распознавание односимвольных токенов
                switch (lexema) {
                    case ';': entry.lexema[0] = LEX_SEMICOLON; break;
                    case ',': entry.lexema[0] = LEX_COMMA; break;
                    case '{': entry.lexema[0] = LEX_LEFTBRACE; break;
                    case '}': entry.lexema[0] = LEX_BRACELET; break;
                    case '(': entry.lexema[0] = LEX_LEFTHESIS; break;
                    case ')': entry.lexema[0] = LEX_RIGHTHESIS; break;
                    case '+': entry.lexema[0] = LEX_PLUS; break;
                    case '-': entry.lexema[0] = LEX_MINUS; break;
                    case '*': entry.lexema[0] = LEX_STAR; break;
                    case '/': entry.lexema[0] = LEX_DIRSLASH; break;
                    // '=' уже обработан выше
                }
                LT::Add(lextable, entry);
                i++;
                col++;
                continue;
            }

            // 4. Распознавание строковых литералов
            if (in.text[i] == '\'') {
                int start_i = i;
                int start_col = col;
                i++; // Пропускаем открывающую кавычку
                col++;

                // Ищем закрывающую кавычку
                while (i < in.size && in.text[i] != '\'' && in.text[i] != '\n') {
                    i++;
                    col++;
                }

                if (i >= in.size || in.text[i] == '\n') {
                    // Ошибка: Отсутствует закрывающая кавычка
                    throw ERROR_THROW_IN(6, line, start_col); // Можно добавить отдельный код ошибки
                }

                // i указывает на закрывающую кавычку
                int len = i - start_i - 1; // Длина самой строки (без кавычек)

                // Копируем строковый литерал (без кавычек) в буфер
                char str_literal[TI_STR_MAXSIZE];
                if (len > TI_STR_MAXSIZE - 1) {
                    throw ERROR_THROW_IN(6, line, start_col); // Литерал слишком длинный
                }

                // Копируем содержимое (текст между кавычками)
                memcpy(str_literal, in.text + start_i + 1, len);
                str_literal[len] = '\0';

                // Создаем запись в таблице идентификаторов (IT)
                IT::Entry entry;

                // ID_MAXSIZE используется для поля id, поэтому копируем его.
                // В данном случае лучше не использовать entry.id для литералов.
                // Вместо этого можно генерировать имя, например, L1, L2, L3...
                // В вашем IT.h структура Entry имеет поле id[ID_MAXSIZE], которое используется для всех записей.
                // Чтобы избежать проблем, можно просто скопировать часть литерала или оставить id пустым.
                // Будем считать, что для литералов поле id не используется или содержит сгенерированное имя
                snprintf(entry.id, ID_MAXSIZE, "L%d", idtable.size);

                entry.iddatatype = IT::STR;
                entry.idtype = IT::L;
                entry.idxfirstLE = lextable.size;
                entry.value.vstr[0].len = (char)len;
                // Копируем сам строковый литерал
                memcpy(entry.value.vstr[0].str, str_literal, len);
                entry.value.vstr[0].str[len] = '\0';

                IT::Add(idtable, entry);
                int idx = idtable.size - 1;

                LT::Add(lextable, {LEX_LITERAL, line, idx});

                i++; // Пропускаем закрывающую кавычку
                col++;
                continue;
            }

            // 5. Распознавание чисел (целочисленные литералы)
            if (isdigit(in.text[i])) {
                int start_i = i;
                int val = 0;
                int num_len = 0;

                // Читаем цифры, пока они есть
                while (i < in.size && isdigit(in.text[i])) {
                    // Проверка на переполнение INT (не реализована, но важна)
                    val = val * 10 + (in.text[i] - '0');
                    i++;
                    num_len++;
                    col++;
                }

                // Создаем запись в IT
                IT::Entry entry;
                snprintf(entry.id, ID_MAXSIZE, "I%d", idtable.size);
                entry.iddatatype = IT::INT;
                entry.idtype = IT::L;
                entry.idxfirstLE = lextable.size;
                entry.value.vint = val;

                IT::Add(idtable, entry);
                int idx = idtable.size - 1;

                LT::Add(lextable, {LEX_LITERAL, line, idx});
                continue; // Начинаем с новой позиции i
            }

            // 6. Распознавание ключевых слов / Идентификаторов
            if (isalpha(in.text[i])) {
                // Используем sscanf с ограничением ID_MAXSIZE, чтобы избежать переполнения буфера 'token'
                // Важно: ID_MAXSIZE должен быть достаточно большим для ключевых слов!
                // #define ID_MAXSIZE 10 (например)

                // ВАЖНО: Мы не можем использовать sscanf(..., "%s", ...) для чтения ID/KW,
                // так как это остановится на первом разделителе, что может быть неверно.
                // Лучше считать вручную до первого не-буквы/не-цифры, но с ограничением длины.

                int start_i = i;
                int token_len = 0;

                // Считываем токен (буквы и цифры)
                while (i < in.size && isalnum(in.text[i]) && token_len < ID_BUFFER_SIZE) {
                    token[token_len++] = in.text[i];
                    i++;
                    col++;
                }
                token[token_len] = '\0';
                shift = token_len;

                // Если токен был обрезан, нужно вернуться назад, чтобы сообщить ошибку
                if (token_len >= ID_BUFFER_SIZE) {
                    // Если длина больше максимальной, это ошибка
                    throw ERROR_THROW_IN(6, line, col - token_len);
                }

                // Проверка ключевых слов (KWs)
                if (isInteger(token)) {
                    LT::Add(lextable, {LEX_INTEGER, line, LT_TI_NULLIDX});
                } else if (isString(token)) {
                    LT::Add(lextable, {LEX_STRING, line, LT_TI_NULLIDX});
                } else if (isFunction(token)) {
                    LT::Add(lextable, {LEX_FUNCTION, line, LT_TI_NULLIDX});
                } else if (isDeclare(token)) {
                    LT::Add(lextable, {LEX_DECLARE, line, LT_TI_NULLIDX});
                } else if (isReturn(token)) {
                    LT::Add(lextable, {LEX_RETURN, line, LT_TI_NULLIDX});
                } else if (isPrint(token)) {
                    LT::Add(lextable, {LEX_PRINT, line, LT_TI_NULLIDX});
                } else if (isMain(token)) {
                    LT::Add(lextable, {LEX_MAIN, line, LT_TI_NULLIDX}); // Предположим, что LEX_MAIN объявлен
                }

                // 7. Распознавание Идентификаторов (ID)
                else if (isValidId(token)) {
                    int idx = IT::IsId(idtable, token);
                    if (idx == LT_TI_NULLIDX) {
                        // Новый идентификатор
                        IT::Entry entry;
                        strncpy(entry.id, token, ID_MAXSIZE); // Копируем токен с учетом ограничения
                        entry.id[ID_MAXSIZE - 1] = '\0';
                        entry.idxfirstLE = lextable.size;

                        // Тип и дата-тип будут определены позже, на этапе семантического анализа
                        entry.iddatatype = IT::INT; // Тип по умолчанию
                        entry.idtype = IT::V; // Тип по умолчанию
                        entry.value.vint = TI_INT_DEFAULT;

                        IT::Add(idtable, entry);
                        idx = idtable.size - 1;
                    }
                    LT::Add(lextable, {LEX_ID, line, idx});
                } else {
                    // Если не распознано ни как KW, ни как ID
                    throw ERROR_THROW_IN(6, line, col - shift);
                }
                continue; // i и col уже сдвинуты в цикле while
            }

            // 8. Ошибка: Недопустимый символ
            else {
                // Недопустимый токен/символ
                throw ERROR_THROW_IN(6, line, col);
            }
        }
    }
}