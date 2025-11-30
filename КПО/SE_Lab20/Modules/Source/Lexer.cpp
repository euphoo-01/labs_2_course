#include "../Headers/Lexer.h"
#include "../Headers/Error.h"
#include "../Headers/LT.h"
#include "../Headers/IT.h"
#include "../Headers/FST.h"
#include <cstring>
#include <cctype>
#include <cstdio>
#include <string>

using namespace std;

//TODO: Формировать таблицу по ограничителю длины идентификатора
//TODO: Переделать определение идентификатора через конечный автомат
//TODO: Добавить исключение если слишком большая длина лексемы

namespace Lexer {
    static bool nextTokenIsFunctionName = false;
    static IT::IDDATATYPE nextIdDatatype = IT::INT;

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

    bool isValidId(const char *str) {
        if (strlen(str) == 0) {
            return false;
        }

        FST::FST fst((char *) str, 2,
                     FST::NODE(52, FST::RELATION('a', 1), FST::RELATION('b', 1), FST::RELATION('c', 1),
                               FST::RELATION('d', 1),
                               FST::RELATION('e', 1), FST::RELATION('f', 1), FST::RELATION('g', 1),
                               FST::RELATION('h', 1),
                               FST::RELATION('i', 1), FST::RELATION('j', 1), FST::RELATION('k', 1),
                               FST::RELATION('l', 1),
                               FST::RELATION('m', 1), FST::RELATION('1', 1), FST::RELATION('o', 1),
                               FST::RELATION('p', 1),
                               FST::RELATION('q', 1), FST::RELATION('r', 1), FST::RELATION('s', 1),
                               FST::RELATION('t', 1),
                               FST::RELATION('u', 1), FST::RELATION('v', 1), FST::RELATION('w', 1),
                               FST::RELATION('x', 1),
                               FST::RELATION('y', 1), FST::RELATION('z', 1), FST::RELATION('A', 1),
                               FST::RELATION('B', 1), FST::RELATION('C', 1), FST::RELATION('D', 1),
                               FST::RELATION('E', 1), FST::RELATION('F', 1), FST::RELATION('G', 1),
                               FST::RELATION('H', 1),
                               FST::RELATION('I', 1), FST::RELATION('J', 1), FST::RELATION('K', 1),
                               FST::RELATION('L', 1),
                               FST::RELATION('M', 1), FST::RELATION('N', 1), FST::RELATION('O', 1),
                               FST::RELATION('P', 1),
                               FST::RELATION('Q', 1), FST::RELATION('R', 1), FST::RELATION('S', 1),
                               FST::RELATION('T', 1),
                               FST::RELATION('U', 1), FST::RELATION('V', 1), FST::RELATION('W', 1),
                               FST::RELATION('X', 1),
                               FST::RELATION('Y', 1), FST::RELATION('Z', 1)),
                     FST::NODE(62, FST::RELATION('0', 1), FST::RELATION('1', 1), FST::RELATION('2', 1),
                               FST::RELATION('3', 1),
                               FST::RELATION('4', 1), FST::RELATION('5', 1), FST::RELATION('6', 1),
                               FST::RELATION('7', 1),
                               FST::RELATION('8', 1), FST::RELATION('9', 1), FST::RELATION('a', 1),
                               FST::RELATION('b', 1), FST::RELATION('c', 1),
                               FST::RELATION('d', 1),
                               FST::RELATION('e', 1), FST::RELATION('f', 1), FST::RELATION('g', 1),
                               FST::RELATION('h', 1),
                               FST::RELATION('i', 1), FST::RELATION('j', 1), FST::RELATION('k', 1),
                               FST::RELATION('l', 1),
                               FST::RELATION('m', 1), FST::RELATION('1', 1), FST::RELATION('o', 1),
                               FST::RELATION('p', 1),
                               FST::RELATION('q', 1), FST::RELATION('r', 1), FST::RELATION('s', 1),
                               FST::RELATION('t', 1),
                               FST::RELATION('u', 1), FST::RELATION('v', 1), FST::RELATION('w', 1),
                               FST::RELATION('x', 1),
                               FST::RELATION('y', 1), FST::RELATION('z', 1), FST::RELATION('A', 1),
                               FST::RELATION('B', 1), FST::RELATION('C', 1), FST::RELATION('D', 1),
                               FST::RELATION('E', 1), FST::RELATION('F', 1), FST::RELATION('G', 1),
                               FST::RELATION('H', 1),
                               FST::RELATION('I', 1), FST::RELATION('J', 1), FST::RELATION('K', 1),
                               FST::RELATION('L', 1),
                               FST::RELATION('M', 1), FST::RELATION('N', 1), FST::RELATION('O', 1),
                               FST::RELATION('P', 1),
                               FST::RELATION('Q', 1), FST::RELATION('R', 1), FST::RELATION('S', 1),
                               FST::RELATION('T', 1),
                               FST::RELATION('U', 1), FST::RELATION('V', 1), FST::RELATION('W', 1),
                               FST::RELATION('X', 1),
                               FST::RELATION('Y', 1), FST::RELATION('Z', 1)));
        return FST::execute(fst);
    }


    void Analyze(In::IN &in, LT::LexTable &lextable, IT::IdTable &idtable) {
        int line = 1;
        int col = 0;
        int i = 0; // Текущий индекс в тексте in.text

        // Буфер для считывания токенов.
        char token[ID_MAXSIZE];

        while (i < in.size) {
            int shift = 0; // Сдвиг (длина токена)
            token[0] = '\0';


            if (in.text[i] == '\n') {
                line++;
                col = 0;
                i++;
                continue;
            }

            if (isspace(in.text[i]) || in.text[i] == '\r') {
                i++;
                col++;
                continue;
            }

            if (strchr(";,{}()+-*/=", in.text[i])) {
                char lexema = in.text[i];
                LT::Entry entry = {{lexema}, line, LT_TI_NULLIDX};

                switch (lexema) {
                    case ';': entry.lexema = LEX_SEMICOLON;
                        break;
                    case ',': entry.lexema = LEX_COMMA;
                        break;
                    case '{': entry.lexema = LEX_LEFTBRACE;
                        break;
                    case '}': entry.lexema = LEX_BRACELET;
                        break;
                    case '(': entry.lexema = LEX_LEFTHESIS;
                        break;
                    case ')': entry.lexema = LEX_RIGHTHESIS;
                        break;
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                    case '=':
                        // lexema уже содержит правильный символ оператора
                        break;
                }
                LT::Add(lextable, entry);
                i++;
                col++;
                continue;
            }

            if (in.text[i] == '\'') {
                int start_i = i;
                int start_col = col;
                i++; // Пропускаем открывающую кавычку
                col++;

                while (i < in.size && in.text[i] != '\'' && in.text[i] != '\n') {
                    i++;
                    col++;
                }

                if (i >= in.size || in.text[i] == '\n') {
                    throw ERROR_THROW_IN(6, line, start_col);
                }

                // i указывает на закрывающую кавычку
                int len = i - start_i - 1;

                char str_literal[TI_STR_MAXSIZE];
                if (len >= TI_STR_MAXSIZE) {
                    throw ERROR_THROW_IN(6, line, start_col);
                }

                memcpy(str_literal, in.text + start_i + 1, len);
                str_literal[len] = '\0';

                IT::Entry entry;
                snprintf(entry.id, ID_MAXSIZE, "L%d", idtable.size);

                entry.iddatatype = IT::STR;
                entry.idtype = IT::L;
                entry.idxfirstLE = lextable.size;
                entry.value.vstr[0].len = (char) len;

                memcpy(entry.value.vstr[0].str, str_literal, len);
                entry.value.vstr[0].str[len] = '\0';

                IT::Add(idtable, entry);
                int idx = idtable.size - 1;

                LT::Add(lextable, {LEX_LITERAL, line, idx});

                i++; // Пропускаем закрывающую кавычку
                col++;
                continue;
            }

            if (isdigit(in.text[i])) {
                int start_i = i;
                int val = 0;
                int num_len = 0;

                while (i < in.size && isdigit(in.text[i])) {
                    //TODO: Проверка на переполнение INT
                    val = val * 10 + (in.text[i] - '0');
                    i++;
                    num_len++;
                    col++;
                }

                IT::Entry entry;
                snprintf(entry.id, ID_MAXSIZE, "I%d", idtable.size); // Генерируем ID для литерала
                entry.iddatatype = IT::INT;
                entry.idtype = IT::L;
                entry.idxfirstLE = lextable.size;
                entry.value.vint = val;

                IT::Add(idtable, entry);
                int idx = idtable.size - 1;

                LT::Add(lextable, {LEX_LITERAL, line, idx});
                continue;
            }

            if (isalpha(in.text[i])) {
                int token_len = 0;

                while (i < in.size && isalnum(in.text[i]) && token_len < ID_MAXSIZE - 1) {
                    token[token_len++] = in.text[i];
                    i++;
                    col++;
                }
                token[token_len] = '\0';
                shift = token_len;

                if (token_len == ID_MAXSIZE - 1 && isalnum(in.text[i])) {
                    throw ERROR_THROW_IN(6, line, col - token_len);
                }

                if (isInteger(token)) {
                    LT::Add(lextable, {LEX_INTEGER, line, LT_TI_NULLIDX});
                    nextIdDatatype = IT::INT;
                } else if (isString(token)) {
                    LT::Add(lextable, {LEX_STRING, line, LT_TI_NULLIDX});
                    nextIdDatatype = IT::STR;
                } else if (isFunction(token)) {
                    LT::Add(lextable, {LEX_FUNCTION, line, LT_TI_NULLIDX});
                    nextTokenIsFunctionName = true;
                } else if (isDeclare(token)) {
                    LT::Add(lextable, {LEX_DECLARE, line, LT_TI_NULLIDX});
                } else if (isReturn(token)) {
                    LT::Add(lextable, {LEX_RETURN, line, LT_TI_NULLIDX});
                } else if (isPrint(token)) {
                    LT::Add(lextable, {LEX_PRINT, line, LT_TI_NULLIDX});
                } else if (isMain(token)) {
                    LT::Add(lextable, {LEX_MAIN, line, LT_TI_NULLIDX});
                } else if (isValidId(token)) {
                    int idx = IT::IsId(idtable, token);
                    if (idx == LT_TI_NULLIDX) {
                        IT::Entry entry;
                        strncpy(entry.id, token, ID_MAXSIZE);
                        entry.id[ID_MAXSIZE - 1] = '\0';
                        entry.idxfirstLE = lextable.size;

                        if (nextTokenIsFunctionName) {
                            entry.idtype = IT::F;
                            nextTokenIsFunctionName = false;
                        } else {
                            entry.idtype = IT::V;
                        }

                        entry.iddatatype = nextIdDatatype;

                        if (nextIdDatatype == IT::INT) {
                            entry.value.vint = TI_INT_DEFAULT;
                        } else if (nextIdDatatype == IT::STR) {
                            entry.value.vstr[0].len = 0;
                            entry.value.vstr[0].str[0] = '\0';
                        }

                        IT::Add(idtable, entry);
                        idx = idtable.size - 1;
                    }
                    LT::Add(lextable, {LEX_ID, line, idx});
                }
                continue;
            } else {
                throw ERROR_THROW_IN(111, line, col);
            }
        }
    }
}
