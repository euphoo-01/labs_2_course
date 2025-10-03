#include "../Headers/Lexer.h"
#include "../Headers/Error.h"
#include <cstring>
#include <cctype>
#include <string>

using namespace std;

namespace Lexer
{

    bool isInteger(const char* str) {
        FST::FST fst((char*)str, 8,
            FST::NODE(1, FST::RELATION('i',1)),
            FST::NODE(1, FST::RELATION('n',2)),
            FST::NODE(1, FST::RELATION('t',3)),
            FST::NODE(1, FST::RELATION('e',4)),
            FST::NODE(1, FST::RELATION('g',5)),
            FST::NODE(1, FST::RELATION('e',6)),
            FST::NODE(1, FST::RELATION('r',7)),
            FST::NODE()
        );
        return FST::execute(fst);
    }

    bool isString(const char* str) {
        FST::FST fst((char*)str, 7,
            FST::NODE(1, FST::RELATION('s',1)),
            FST::NODE(1, FST::RELATION('t',2)),
            FST::NODE(1, FST::RELATION('r',3)),
            FST::NODE(1, FST::RELATION('i',4)),
            FST::NODE(1, FST::RELATION('n',5)),
            FST::NODE(1, FST::RELATION('g',6)),
            FST::NODE()
        );
        return FST::execute(fst);
    }

    bool isFunction(const char* str) {
        FST::FST fst((char*)str, 9,
            FST::NODE(1, FST::RELATION('f',1)),
            FST::NODE(1, FST::RELATION('u',2)),
            FST::NODE(1, FST::RELATION('n',3)),
            FST::NODE(1, FST::RELATION('c',4)),
            FST::NODE(1, FST::RELATION('t',5)),
            FST::NODE(1, FST::RELATION('i',6)),
            FST::NODE(1, FST::RELATION('o',7)),
            FST::NODE(1, FST::RELATION('n',8)),
            FST::NODE()
        );
        return FST::execute(fst);
    }

    bool isDeclare(const char* str) {
        FST::FST fst((char*)str, 8,
            FST::NODE(1, FST::RELATION('d',1)),
            FST::NODE(1, FST::RELATION('e',2)),
            FST::NODE(1, FST::RELATION('c',3)),
            FST::NODE(1, FST::RELATION('l',4)),
            FST::NODE(1, FST::RELATION('a',5)),
            FST::NODE(1, FST::RELATION('r',6)),
            FST::NODE(1, FST::RELATION('e',7)),
            FST::NODE()
        );
        return FST::execute(fst);
    }

    bool isReturn(const char* str) {
        FST::FST fst((char*)str, 7,
            FST::NODE(1, FST::RELATION('r',1)),
            FST::NODE(1, FST::RELATION('e',2)),
            FST::NODE(1, FST::RELATION('t',3)),
            FST::NODE(1, FST::RELATION('u',4)),
            FST::NODE(1, FST::RELATION('r',5)),
            FST::NODE(1, FST::RELATION('n',6)),
            FST::NODE()
        );
        return FST::execute(fst);
    }

    bool isPrint(const char* str) {
        FST::FST fst((char*)str, 6,
            FST::NODE(1, FST::RELATION('p',1)),
            FST::NODE(1, FST::RELATION('r',2)),
            FST::NODE(1, FST::RELATION('i',3)),
            FST::NODE(1, FST::RELATION('n',4)),
            FST::NODE(1, FST::RELATION('t',5)),
            FST::NODE()
        );
        return FST::execute(fst);
    }

    bool isMain(const char* str) {
        FST::FST fst((char*)str, 5,
            FST::NODE(1, FST::RELATION('m',1)),
            FST::NODE(1, FST::RELATION('a',2)),
            FST::NODE(1, FST::RELATION('i',3)),
            FST::NODE(1, FST::RELATION('n',4)),
            FST::NODE()
        );
        return FST::execute(fst);
    }


    bool isIdentifier(const char* str) {
        if (!isalpha(str[0])) return false;
        if (strlen(str) > 5) return false;
        return true;
    }

    bool isIntegerLiteral(const char* str) {
        for (int i = 0; str[i]; i++)
            if (!isdigit(str[i])) return false;
        return true;
    }

    bool isStringLiteral(const char* str) {
        int len = strlen(str);
        return len >= 2 && str[0] == '\'' && str[len-1] == '\'';
    }

    Tables Analyze(In::IN& in) {
        Tables tables;
        tables.lexTable = LT::Create(LT_MAXSIZE);
        tables.idTable  = IT::Create(TI_MAXSIZE);

        char* token = strtok((char*)in.text, " \n\t");
        int line = 1;

        while (token != nullptr) {
            if (isInteger(token)) {
                LT::Add(tables.lexTable, {LEX_INTEGER,line,LT_TI_NULLIDX});
            }
            else if (isString(token)) {
                LT::Add(tables.lexTable, {LEX_STRING,line,LT_TI_NULLIDX});
            }
            else if (isFunction(token)) {
                LT::Add(tables.lexTable, {LEX_FUNCTION,line,LT_TI_NULLIDX});
            }
            else if (isDeclare(token)) {
                LT::Add(tables.lexTable, {LEX_DECLARE,line,LT_TI_NULLIDX});
            }
            else if (isReturn(token)) {
                LT::Add(tables.lexTable, {LEX_RETURN,line,LT_TI_NULLIDX});
            }
            else if (isPrint(token)) {
                LT::Add(tables.lexTable, {LEX_PRINT,line,LT_TI_NULLIDX});
            }
            else if (isMain(token)) {
                LT::Add(tables.lexTable, {LEX_MAIN,line,LT_TI_NULLIDX});
            }
            else if (isIntegerLiteral(token)) {
                int idx = IT::AddLiteralInteger(tables.idTable, atoi(token));
                LT::Add(tables.lexTable, {LEX_LITERAL,line,idx});
            }
            else if (isStringLiteral(token)) {
                int idx = IT::AddLiteralString(tables.idTable, token);
                LT::Add(tables.lexTable, {LEX_LITERAL,line,idx});
            }
            else if (isIdentifier(token)) {
                int idx = IT::AddIdentifier(tables.idTable, token, IT::UNDEF);
                LT::Add(tables.lexTable, {LEX_ID,line,idx});
            }
            else if (strchr(";,{}()+-*/", token[0]) && strlen(token)==1) {
                char c = token[0];
                switch(c) {
                    case ';': LT::Add(tables.lexTable,{LEX_SEMICOLON,line,LT_TI_NULLIDX}); break;
                    case ',': LT::Add(tables.lexTable,{LEX_COMMA,line,LT_TI_NULLIDX}); break;
                    case '{': LT::Add(tables.lexTable,{LEX_LEFTBRACE,line,LT_TI_NULLIDX}); break;
                    case '}': LT::Add(tables.lexTable,{LEX_BRACELET,line,LT_TI_NULLIDX}); break;
                    case '(': LT::Add(tables.lexTable,{LEX_LEFTHESIS,line,LT_TI_NULLIDX}); break;
                    case ')': LT::Add(tables.lexTable,{LEX_RIGHTHESIS,line,LT_TI_NULLIDX}); break;
                    case '+': case '-': case '*': case '/':
                        LT::Add(tables.lexTable,{LEX_PLUS,line,LT_TI_NULLIDX}); break;
                }
            }
            else {
                throw ERROR_THROW_IN(200,line,0);
            }

            token = strtok(nullptr," \n\t");
        }

        return tables;
    }
}
