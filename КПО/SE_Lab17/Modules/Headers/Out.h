#pragma once;
#include <fstream>;
#include "Parm.h";
#include "Error.h"

namespace Out {
    struct OUT {
        wchar_t outfile[PARM_MAXSIZE];
        std::ofstream* stream;
    };

    static const OUT INITOUT = {L"",NULL};
    OUT getout(wchar_t outfile[PARM_MAXSIZE]);
    void Write (OUT out, unsigned char* text);
    void WriteError(OUT out, Error::ERROR error);
    void Close (OUT out);
}