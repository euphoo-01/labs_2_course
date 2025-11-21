#include "../Headers/Lexer.h"
#include "../Headers/Error.h"
#include "../Headers/LT.h"
#include "../Headers/IT.h"
#include "../Headers/FST.h"
#include <cstring>
#include <cctype>
#include <cstdio>

namespace Lexer {
    
    // FSTs for keywords, as requested by user
    bool isUnsigned(const char *str) { return FST::execute(FST::FST((char *) str, 9, FST::NODE(1, FST::RELATION('u', 1)), FST::NODE(1, FST::RELATION('n', 2)), FST::NODE(1, FST::RELATION('s', 3)), FST::NODE(1, FST::RELATION('i', 4)), FST::NODE(1, FST::RELATION('g', 5)), FST::NODE(1, FST::RELATION('n', 6)), FST::NODE(1, FST::RELATION('e', 7)), FST::NODE(1, FST::RELATION('d', 8)), FST::NODE()));}
    bool isInteger(const char* str) { return FST::execute(FST::FST((char*)str, 8, FST::NODE(1, FST::RELATION('i', 1)), FST::NODE(1, FST::RELATION('n', 2)), FST::NODE(1, FST::RELATION('t', 3)), FST::NODE(1, FST::RELATION('e', 4)), FST::NODE(1, FST::RELATION('g', 5)), FST::NODE(1, FST::RELATION('e', 6)), FST::NODE(1, FST::RELATION('r', 7)), FST::NODE())); }
    bool isChar(const char *str) { return FST::execute(FST::FST((char *) str, 5, FST::NODE(1, FST::RELATION('c', 1)), FST::NODE(1, FST::RELATION('h', 2)), FST::NODE(1, FST::RELATION('a', 3)), FST::NODE(1, FST::RELATION('r', 4)), FST::NODE())); }
    bool isLogic(const char* str) { return FST::execute(FST::FST((char*)str, 6, FST::NODE(1, FST::RELATION('l', 1)), FST::NODE(1, FST::RELATION('o', 2)), FST::NODE(1, FST::RELATION('g', 3)), FST::NODE(1, FST::RELATION('i', 4)), FST::NODE(1, FST::RELATION('c', 5)), FST::NODE())); }
    bool isFunc(const char *str) { return FST::execute(FST::FST((char *) str, 5, FST::NODE(1, FST::RELATION('f', 1)), FST::NODE(1, FST::RELATION('u', 2)), FST::NODE(1, FST::RELATION('n', 3)), FST::NODE(1, FST::RELATION('c', 4)), FST::NODE())); }
    bool isSend(const char *str) { return FST::execute(FST::FST((char *) str, 5, FST::NODE(1, FST::RELATION('s', 1)), FST::NODE(1, FST::RELATION('e', 2)), FST::NODE(1, FST::RELATION('n', 3)), FST::NODE(1, FST::RELATION('d', 4)), FST::NODE())); }
    bool isMain(const char *str) { return FST::execute(FST::FST((char *) str, 5, FST::NODE(1, FST::RELATION('m', 1)), FST::NODE(1, FST::RELATION('a', 2)), FST::NODE(1, FST::RELATION('i', 3)), FST::NODE(1, FST::RELATION('n', 4)), FST::NODE())); }
    bool isIf(const char* str) { return FST::execute(FST::FST((char*)str, 3, FST::NODE(1, FST::RELATION('i', 1)), FST::NODE(1, FST::RELATION('f', 2)), FST::NODE())); }
    bool isDiffer(const char* str) { return FST::execute(FST::FST((char*)str, 7, FST::NODE(1, FST::RELATION('d', 1)), FST::NODE(1, FST::RELATION('i', 2)), FST::NODE(1, FST::RELATION('f', 3)), FST::NODE(1, FST::RELATION('f', 4)), FST::NODE(1, FST::RELATION('e', 5)), FST::NODE(1, FST::RELATION('r', 6)), FST::NODE())); }
    bool isBecause(const char* str) { return FST::execute(FST::FST((char*)str, 8, FST::NODE(1, FST::RELATION('b', 1)), FST::NODE(1, FST::RELATION('e', 2)), FST::NODE(1, FST::RELATION('c', 3)), FST::NODE(1, FST::RELATION('a', 4)), FST::NODE(1, FST::RELATION('u', 5)), FST::NODE(1, FST::RELATION('s', 6)), FST::NODE(1, FST::RELATION('e', 7)), FST::NODE())); }

    // This FST is not updated for Cyrillic as it's very verbose.
    bool isValidId(const char *str) {
        if (!str || strlen(str) == 0) return false;
        if (!isalpha(str[0])) return false;
        for (int i = 1; str[i] != '\0'; i++) {
            if (!isalnum(str[i])) return false;
        }
        return true;
    }


    void Analyze(In::IN &in, LT::LexTable &lextable, IT::IdTable &idtable) {
        int line = 1;
        int col = 1;
        unsigned int i = 0;
        char token_buffer[ID_MAXSIZE];

        while (i < in.size) {
            // 1. Skip whitespace and count lines/cols
            if (isspace(in.text[i])) {
                if (in.text[i] == '\n') { line++; col = 1; } 
                else { col++; }
                i++;
                continue;
            }

            int start_col = col;

            // 2. Comments
            if (in.text[i] == '/' && i + 1 < in.size && in.text[i + 1] == '/') {
                while (i < in.size && in.text[i] != '\n') i++;
                continue;
            }

            // 3. Operators and Separators (with lookahead)
            char next_char = (i + 1 < in.size) ? in.text[i + 1] : '\0';
            char lexema = '\0';
            int shift = 1;

            if (in.text[i] == '+' && next_char == '+') { lexema = LEX_INCREMENT; shift = 2; }
            else if (in.text[i] == '-' && next_char == '-') { lexema = LEX_DECREMENT; shift = 2; }
            else if (in.text[i] == '=' && next_char == '=') { lexema = LEX_EQUAL_EQUAL; shift = 2; }
            else if (in.text[i] == '!' && next_char == '=') { lexema = LEX_NOT_EQUAL; shift = 2; }
            else if (in.text[i] == '<' && next_char == '=') { lexema = LEX_LESS_EQUAL; shift = 2; }
            else if (in.text[i] == '>' && next_char == '=') { lexema = LEX_GREATER_EQUAL; shift = 2; }
            else if (strchr(";,{}()+-*:~=<>", in.text[i])) { lexema = in.text[i]; }

            if (lexema != '\0') {
                LT::Add(lextable, LT::Entry{lexema, line, LT_TI_NULLIDX});
                i += shift;
                col += shift;
                continue;
            }

            // 4. Character Literals
            if (in.text[i] == '\'') {
                i++; col++;
                if (i >= in.size) throw ERROR_THROW_IN(112, line, start_col);

                char literal_val = in.text[i];
                if (literal_val == '\\') { // Escape seq
                    i++; col++;
                    if (i >= in.size) throw ERROR_THROW_IN(112, line, start_col);
                    if (in.text[i] == 'n') literal_val = '\n';
                    else if (in.text[i] == 't') literal_val = '\t';
                    else if (in.text[i] == '\'') literal_val = '\'';
                    else throw ERROR_THROW_IN(111, line, col);
                }
                
                i++; col++;
                if (i >= in.size || in.text[i] != '\'') throw ERROR_THROW_IN(112, line, start_col);

                IT::Entry entry;
                snprintf(entry.id, ID_MAXSIZE, "L%d", idtable.size);
                entry.iddatatype = IT::STR;
                entry.idtype = IT::L;
                entry.idxfirstLE = lextable.size;
                entry.value.vstr[0].len = 1;
                entry.value.vstr[0].str[0] = literal_val;
                entry.value.vstr[0].str[1] = '\0';
                IT::Add(idtable, entry);
                LT::Add(lextable, LT::Entry{LEX_LITERAL, line, idtable.size - 1});
                
                i++; col++;
                continue;
            }
            
            // 5. Identifiers and Keywords
            if (isalpha(in.text[i])) { // ToDo: Add Cyrillic support
                int token_len = 0;
                while (i < in.size && isalnum(in.text[i]) && token_len < ID_MAXSIZE - 1) {
                    token_buffer[token_len++] = in.text[i++];
                }
                token_buffer[token_len] = '\0';

                if (token_len > 0 && isalnum(in.text[i])) throw ERROR_THROW_IN(116, line, start_col);

                if (isUnsigned(token_buffer)) { LT::Add(lextable, LT::Entry{LEX_UNSIGNED, line, LT_TI_NULLIDX}); } 
                else if (isInteger(token_buffer)) { LT::Add(lextable, LT::Entry{LEX_INTEGER, line, LT_TI_NULLIDX}); } 
                else if (isChar(token_buffer)) { LT::Add(lextable, LT::Entry{LEX_CHAR, line, LT_TI_NULLIDX}); } 
                else if (isLogic(token_buffer)) { LT::Add(lextable, LT::Entry{LEX_LOGIC, line, LT_TI_NULLIDX}); } 
                else if (isFunc(token_buffer)) { LT::Add(lextable, LT::Entry{LEX_FUNC, line, LT_TI_NULLIDX}); } 
                else if (isSend(token_buffer)) { LT::Add(lextable, LT::Entry{LEX_SEND, line, LT_TI_NULLIDX}); } 
                else if (isMain(token_buffer)) { LT::Add(lextable, LT::Entry{LEX_MAIN, line, LT_TI_NULLIDX}); } 
                else if (isIf(token_buffer)) { LT::Add(lextable, LT::Entry{LEX_IF, line, LT_TI_NULLIDX}); } 
                else if (isDiffer(token_buffer)) { LT::Add(lextable, LT::Entry{LEX_DIFFER, line, LT_TI_NULLIDX}); } 
                else if (isBecause(token_buffer)) { LT::Add(lextable, LT::Entry{LEX_BECAUSE, line, LT_TI_NULLIDX}); } 
                else if (isValidId(token_buffer)) {
                    int idx = IT::IsId(idtable, token_buffer);
                    if (idx == LT_TI_NULLIDX) {
                        IT::Entry entry;
                        strncpy(entry.id, token_buffer, ID_MAXSIZE);
                        entry.id[ID_MAXSIZE - 1] = '\0';
                        entry.idtype = IT::V; 
                        entry.iddatatype = IT::INT;
                        entry.idxfirstLE = lextable.size;
                        IT::Add(idtable, entry);
                        idx = idtable.size - 1;
                    }
                    LT::Add(lextable, LT::Entry{LEX_ID, line, idx});
                } else {
                    throw ERROR_THROW_IN(111, line, start_col);
                }
                col += token_len;
                continue;
            }

            // 6. Integer Literals
            if (isdigit(in.text[i])) {
                int val = 0;
                if (in.text[i] == '0' && (in.text[i+1] == 'x' || in.text[i+1] == 'X')) {
                    i += 2; col += 2;
                    while(i < in.size && isxdigit(in.text[i])) {
                        char d = in.text[i];
                        val = val * 16 + (isdigit(d) ? d - '0' : tolower(d) - 'a' + 10);
                        i++; col++;
                    }
                } else {
                    while (i < in.size && isdigit(in.text[i])) {
                        val = val * 10 + (in.text[i] - '0');
                        i++; col++;
                    }
                }
                
                IT::Entry entry;
                snprintf(entry.id, ID_MAXSIZE, "L%d", idtable.size);
                entry.iddatatype = IT::INT;
                entry.idtype = IT::L;
                entry.idxfirstLE = lextable.size;
                entry.value.vint = val;
                IT::Add(idtable, entry);
                LT::Add(lextable, LT::Entry{LEX_LITERAL, line, idtable.size - 1});
                continue;
            }

            // 7. Unrecognized character
            throw ERROR_THROW_IN(111, line, col);
        }
    }
}