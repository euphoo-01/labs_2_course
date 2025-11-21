#include "../Headers/Out.h";
#include "../Headers/Error.h"

#include <complex>
#include <cstring>

namespace Out {
    OUT getout(wchar_t outfile[PARM_MAXSIZE]) {
        OUT out = Out::INITOUT;
        wcscpy(out.outfile,outfile);

        char coutfile[PARM_MAXSIZE];
        wcstombs(coutfile,outfile,PARM_MAXSIZE + 1);
        out.stream = new std::ofstream();
        out.stream->open(coutfile, std::ios::out);
        if (!out.stream->is_open() || !out.stream->good()) {
            delete out.stream;
            out.stream = nullptr;
            throw ERROR_THROW(9);
        }
        return out;
    }

    void Write(OUT out, unsigned char* text) {
        if (out.stream->good() || !out.stream->is_open()) {
            int idx = 0;
            while (text[idx] != '\0') {
                (*out.stream) << text[idx];
                idx++;
            }
        } else {
            throw ERROR_THROW(1);
        }

    }

    void WriteError(OUT out, Error::ERROR error) {
        if (out.stream->good() || out.stream->is_open()) {
            (*out.stream) << "Ошибка " << error.id << ". " << error.message << ", строка: " << error.inext.line <<
                    ", символ: " << error.inext.col << std::endl;
        }
    }

    void Close(OUT out) {
        if (out.stream->is_open() && out.stream->good()) {
            out.stream->close();
        }
        delete out.stream;
        out.stream = nullptr;
    }

}