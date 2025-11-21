#include "../Headers/In.h"
#include "../Headers/Error.h"
#include <cstring>
#include <fstream>
#include <cwchar>
#include <iconv.h>

unsigned char *convertWindows1251ToUTF8(const unsigned char *input) {
    if (!input) {
        throw ERROR_THROW(1);
    }
    size_t inbytes = strlen(reinterpret_cast<const char *>(input));

    iconv_t cd = iconv_open("UTF-8", "CP1251");
    if (cd == (iconv_t) -1) {
        throw ERROR_THROW(1);
    }

    size_t outbytes = inbytes * 4; // Запас для UTF-8
    unsigned char *outbuf = new unsigned char[outbytes + 1]; // +1 для нулевого байта
    unsigned char *outptr = outbuf;
    const char *inbuf = reinterpret_cast<const char *>(input);

    size_t result = iconv(cd, const_cast<char **>(&inbuf), &inbytes, reinterpret_cast<char **>(&outptr), &outbytes);
    if (result == static_cast<size_t>(-1)) {
        delete[] outbuf;
        iconv_close(cd);
        throw ERROR_THROW(1);
    }
    *outptr = '\0';

    size_t converted_len = outptr - outbuf;
    unsigned char *result_buf = new unsigned char[converted_len + 1];
    memcpy(result_buf, outbuf, converted_len + 1); // Плюс нуль байт

    delete[] outbuf;
    iconv_close(cd);

    return result_buf;
}

namespace In {
    IN getin(wchar_t infile[]) {
        IN result = {0, 0, 0, {}, IN_CODE_TABLE};
        int filename_length = wcslen(infile) + 1;
        char filename[filename_length];
        wcstombs(filename, infile, filename_length);

        int cur_line = 1, cur_col = 0, cur_pos = 0,
            result_pos = 0, ignored = 0;

        result.text = new unsigned char[IN_MAX_LEN_TEXT];
        std::ifstream file(filename, std::ios::binary);
        if (file.fail())
        {
            throw ERROR_THROW(103);
        }
        bool inString = false; // флаг, находимся ли внутри строки
        bool wasSpace = false; // флаг, был ли предыдущий символ пробелом

        while (true) {
            const int ch = file.get();
            if (ch == EOF) break;

            cur_pos++;
        if (result.size == IN_MAX_LEN_TEXT)
        {
            throw ERROR_THROW(10);
        }

            if (inString) {
                result.text[result_pos++] = static_cast<char>(ch);
                if (ch == '\'') {
                    inString = false;
                }
                wasSpace = (ch == ' '); // обновляем флаг пробела
                continue;
            }

            if (ch == '\'') {
                inString = true;
                result.text[result_pos++] = static_cast<char>(ch);
                wasSpace = false; // строка сбрасывает флаг пробела
                continue;
            }

            if (ch == ' ') {
                if (wasSpace && !inString) {
                    ignored++;
                    continue;
                }
                wasSpace = true;
            } else {
                wasSpace = false;
            }

            switch (result.code[ch]) {
                case IN::T:
                    result.text[result_pos++] = static_cast<char>(ch);
                    cur_col++;
                    if (ch == IN_CODE_ENDL) {
                        cur_line++;
                        cur_col = 0;
                        wasSpace = false;
                    }
                    break;
                case IN::I:
                    ignored++;
                    if (ch == IN_CODE_ENDL) {
                        cur_line++;
                        cur_col = 0;
                        wasSpace = false;
                    }
                    break;
                case IN::F:
                    throw ERROR_THROW_IN(111, cur_line, cur_col);
                default:
                    result.text[result_pos++] = result.code[ch];
                    break;
            }
        }

        unsigned char *utf8text = convertWindows1251ToUTF8(result.text);
        result.text = utf8text;

        result.size = result_pos;
        result.lines = cur_line;
        result.ignor = ignored;
        return result;
    }
}