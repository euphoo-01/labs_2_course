#include <iostream>
#include <cwchar>
#include <cstring>
#include <cstdlib>

#include "Modules/Headers/Error.h"
#include "Modules/Headers/In.h"
#include "Modules/Headers/Log.h"
#include "Modules/Headers/Parm.h"
#include "Modules/Headers/Out.h"
#include "Modules/Headers/Lexer.h"

int main(int argc, char* argv[]) {
    wchar_t* wargv[argc];
    for (int i = 0; i < argc; ++i) {
        size_t len = strlen(argv[i]) + 1; // +1 для \0
        wargv[i] = new wchar_t[len];;
        mbstowcs(wargv[i], argv[i], len);
    }

    Log::LOG log = Log::INITLOG;
    Out::OUT out = Out::INITOUT;
    LT::LexTable lextable = LT::Create(LT_MAXSIZE);
    IT::IdTable idtable = IT::Create(TI_MAXSIZE);
    try {
        Parm::PARM parameters = Parm::getparm(argc, wargv);
        log = Log::getlog(parameters.log);
        out = Out::getout(parameters.out);
        Log::WriteLog(log);
        Log::WriteParm(log, parameters);
        In::IN input = In::getin(parameters.in);
        Log::WriteIn(log, input);

        Lexer::Analyze(input, lextable, idtable);

        Log::WriteLT(log, lextable);
        Log::WriteIT(log, idtable);


        Out::Write(out, input.text);

        std::cout << "Файл обработан успешно!\n";
    }
    catch (Error::ERROR e) {
        std::cerr << "Ошибка " << e.id << " на строке " << e.inext.line << " символ " << e.inext.col
        << ". " << e.message << std::endl;
        Log::WriteError(log, e);
        Out::WriteError(out, e);
    }

    for (int i = 0; i < argc; i++) {
        delete wargv[i];
    }
    Log::Close(log);
    Out::Close(out);
    return 0;
}