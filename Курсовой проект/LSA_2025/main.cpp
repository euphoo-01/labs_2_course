#include <iostream>
#include <cwchar>
#include <cstring>
#include <cstdlib>
#include <iomanip>

#include "Modules/Headers/Error.h"
#include "Modules/Headers/In.h"
#include "Modules/Headers/Log.h"
#include "Modules/Headers/Parm.h"
#include "Modules/Headers/Out.h"
#include "Modules/Headers/Lexer.h"
#include "Modules/Headers/GRB.h"
#include "Modules/Headers/Mfst.h"


int main(int argc, char *argv[]) {
    std::setlocale(LC_ALL, "ru_RU.CP1251");
    // {
    //     auto table = LT::Create(30);
    //     LT::Add(table, {'t', 1});
    //     LT::Add(table, {'f', 1});
    //     LT::Add(table, {'i', 1});
    //
    //     LT::Add(table, {'(', 1});
    //     LT::Add(table, {'t', 1});
    //     LT::Add(table, {'i', 1});
    //
    //     LT::Add(table, {',', 1});
    //     LT::Add(table, {'t', 1});
    //     LT::Add(table, {'i', 1});
    //     LT::Add(table, {')', 1});
    //
    //     LT::Add(table, {'{', 1});
    //     LT::Add(table, {'d', 1});
    //     LT::Add(table, {'t', 1});
    //     LT::Add(table, {'i', 1});
    //     LT::Add(table, {';', 1});
    //
    //     LT::Add(table, {'i', 1});
    //     LT::Add(table, {'=', 1});
    //     LT::Add(table, {'i', 1});
    //     LT::Add(table, {'v', 1});
    //
    //
    //     LT::Add(table, {'(', 1});
    //     LT::Add(table, {'i', 1});
    //     LT::Add(table, {'v', 1});
    //     LT::Add(table, {'i', 1});
    //     LT::Add(table, {')', 1});
    //
    //     LT::Add(table, {';', 1});
    //     LT::Add(table, {'r', 1});
    //     LT::Add(table, {'i', 1});
    //     LT::Add(table, {';', 1});
    //
    //     LT::Add(table, {'}', 1});
    //     LT::Add(table, {';', 1});
    //
    //     std::ofstream mfst_log("MFST.log");
    //
    //     MFST_TRACE_START(mfst_log)
    //
    //     Lexer::LEX lex;
    //     lex.lextable = table;
    //     lex.idtable = IT::Create(30);
    //
    //     MFST::Mfst mfst(lex, GRB::getGreibach());
    //     mfst.start(mfst_log);
    // }


    {
        wchar_t *wargv[argc];
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
            Lexer::LEX lex = Lexer::LEX(lextable, idtable);

            Log::WriteLT(log, lextable);
            Log::WriteIT(log, idtable);

            std::ofstream mfst_log("MFST.log");
            MFST_TRACE_START(mfst_log)

            MFST::Mfst mfst(lex, GRB::getGreibach());
            if (!mfst.start(mfst_log)) {
                throw ERROR_THROW(600);
            }

            mfst.savededucation();

            mfst.printrules(mfst_log);


            Out::Write(out, input.text);

            std::cout << "Файл обработан успешно!\n";
        } catch (Error::ERROR e) {
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
    }

    return 0;
}
