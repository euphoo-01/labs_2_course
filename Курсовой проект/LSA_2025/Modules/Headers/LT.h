#pragma once
#define LT_MAXSIZE 4096
#define LT_TI_NULLIDX -1

// Keywords and types
#define LEX_UNSIGNED 'u'
#define LEX_INTEGER 't'
#define LEX_CHAR 'c'
#define LEX_LOGIC 'o'
#define LEX_FUNC 'f'
#define LEX_MAIN 'm'
#define LEX_IF 'j'
#define LEX_DIFFER 'k'
#define LEX_BECAUSE 'b'
#define LEX_SEND 's'

// Other
#define LEX_ID 'i'
#define LEX_LITERAL 'l'
#define LEX_SEMICOLON ';'
#define LEX_COMMA ','
#define LEX_LEFTBRACE '{'
#define LEX_BRACELET '}'
#define LEX_LEFTHESIS '('
#define LEX_RIGHTHESIS ')'

// Single-character operators
#define LEX_PLUS '+'
#define LEX_MINUS '-'
#define LEX_STAR '*'
#define LEX_COLON ':'
#define LEX_EQUAL '='
#define LEX_LESS '<'
#define LEX_GREATER '>'
#define LEX_TILDE '~'

// Psuedo-chars for multi-character operators
#define LEX_INCREMENT 'P'
#define LEX_DECREMENT 'M'
#define LEX_EQUAL_EQUAL 'E'
#define LEX_NOT_EQUAL 'N'
#define LEX_LESS_EQUAL 'L'
#define LEX_GREATER_EQUAL 'G'

namespace LT {
    struct Entry {
        char lexema;
        int sn; // Номер строки в исходном тексте
        int idxTI; // индекс в таблице идентификаторов
    };

    struct LexTable {
        int maxsize;
        int size;
        Entry *table;
    };

    LexTable Create(int size);
    void Add(LexTable &lextable, Entry entry);
    Entry GetEntry(LexTable &lextable, int n);
    void Delete(LexTable &lextable);
}