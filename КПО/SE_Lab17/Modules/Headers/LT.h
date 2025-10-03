#pragma once
#define LEXEMA_FIXSIZE 1
#define LT_MAXSIZE 4096
#define LT_TI_NULLIDX 0xffffffff
#define LEX_INTEGER 't'
#define LEX_STRING 't'
#define LEX_ID 'i'
#define LEX_LITERAL 'l'
#define LEX_FUNCTION 'f'
#define LEX_DECLARE 'd'
#define LEX_RETURN 'r'
#define LEX_PRINT 'p'
#define LEX_SEMICOLON ';'
#define LEX_COMMA ','
#define LEX_LEFTBRACE '{'
#define LEX_BRACELET '}'
#define LEX_LEFTHESIS '('
#define LEX_RIGHTHESIS ')'
#define LEX_PLUS 'v'
#define LEX_MINUS 'v'
#define LEX_STAR 'v'
#define LEX_DIRSLASH 'v'

namespace LT {
    struct Entry {
        char lexema[LEXEMA_FIXSIZE];
        int sn; // Номер строки в исходном тексте
        int idxTI; // индекс в таблице идентификаторов
    };

    struct LexTable {
        int maxsize; // емкость таблицы лексем < LT_MAXSIZE
        int size; // текущий размер таблицы лексем < maxsize
        Entry *table;
    };

    LexTable Create(
        int size // емкость таблицы лексем < LT_MAXSIZE
    );

    void Add(
        LexTable &lextable, // экземпляр таблицы лексем
        Entry entry // строка таблицы лексем
    );

    Entry GetEntry(
        LexTable &lextable,
        int n // номер получаемой строки
    );

    void Delete(LexTable &lextable);
}
