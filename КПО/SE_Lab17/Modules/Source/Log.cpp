#include "../Headers/Log.h"
#include <cstring>
#include <iomanip>

namespace Log {
    LOG getlog(wchar_t logfile[]) {
        LOG log = Log::INITLOG;
        wcscpy(log.logfile, logfile);

        char clogfile[PARM_MAXSIZE];
        wcstombs(clogfile, logfile, PARM_MAXSIZE + 1);
        log.stream = new std::ofstream();
        log.stream->open(clogfile, std::ios::app);
        if (!log.stream->is_open() || !log.stream->good()) {
            delete log.stream;
            log.stream = nullptr;
            throw ERROR_THROW(112);
        }
        return log;
    }

    void WriteLine(LOG log, char *c, ...) {
        if (log.stream->good() || log.stream->is_open()) {
            char **current = &c;
            while (current) {
                if (strncmp(*current, "", strlen(*current))) {
                    (*log.stream) << *current << " ";
                }
                current++;
            }
            (*log.stream) << std::endl;
        }
        else {
            throw ERROR_THROW(112);
        }
    }

    void WriteLine(LOG log, wchar_t *c, ...) {
        if (log.stream->good() || log.stream->is_open()) {
            wchar_t **current = &c;
            while (current) {
                if (wcscmp(*current, L"")) {
                    const int len = wcslen(*current);
                    char c_current[len];
                    wcstombs(c_current, *current, len + 1);
                    (*log.stream) << c_current << " ";
                }
                current++;
            }
            (*log.stream) << std::endl;
        } else {
            throw ERROR_THROW(113);
        }
    }

    void WriteLog(LOG log) {
        if (log.stream->good() || log.stream->is_open()) {
            time_t rawtime;
            time(&rawtime);
            struct tm timeinfo;
            localtime_r(&rawtime, &timeinfo);
            char buffer[80];
            strftime(buffer, sizeof(buffer), "%d.%m.%Y %H:%M:%S", &timeinfo);
            (*log.stream) << "---- Протокол ----- " << buffer << " ----" << std::endl;
        } else {
            throw ERROR_THROW(113);
        }
    }

    void WriteParm(LOG log, Parm::PARM parm) {
        if (log.stream->good() || log.stream->is_open()) {
            const int in_len = wcslen(parm.in) + 1, out_len = wcslen(parm.out) + 1, log_len = wcslen(parm.log) + 1;
            char c_in[in_len], c_out[out_len], c_log[log_len];
            wcstombs(c_in, parm.in, in_len);
            wcstombs(c_out, parm.out, out_len);
            wcstombs(c_log, parm.log, log_len);
            (*log.stream) << "---- Входные параметры: ----\n" << "-in: " << c_in << std::endl << "-out: " << c_out <<
                    std::endl
                    << "-log: " << c_log << std::endl;
        } else {
            throw ERROR_THROW(113);
        }
    }

    void WriteIn(LOG log, In::IN in) {
        if (log.stream->good() || log.stream->is_open()) {
            (*log.stream) << "---- Исходные данные: ----\n" << "Количество символов: " << in.size << std::endl <<
                    "Проигнорировано: " << in.ignor << std::endl << "Количество строк: " << in.lines << std::endl;
        } else {
            throw ERROR_THROW(113);
        }
    }

    void WriteError(LOG log, Error::ERROR error) {
        if (log.stream->good() || log.stream->is_open()) {
            (*log.stream) << "Ошибка " << error.id << ". " << error.message << ", строка: " << error.inext.line <<
                    ", символ: " << error.inext.col << std::endl;
        }
    }

   void WriteLT(LOG log, LT::LexTable lextable) {
        if (!(log.stream->good() || log.stream->is_open())) {
            throw ERROR_THROW(113);
        }

        (*log.stream) << "---- Таблица лексем ----" << std::endl;
        (*log.stream) << std::left
                      << std::setw(6)  << "Idx"
                      << std::setw(8)  << "Lex"
                      << std::setw(10) << "IdxTI"
                      << std::setw(8)  << "Line"
                      << std::endl;
        (*log.stream) << std::string(34, '-') << std::endl;

        for (int i = 0; i < lextable.size; ++i) {
            LT::Entry &entry = lextable.table[i];

            char lexch = entry.lexema[0];

            (*log.stream) << std::left
                          << std::setw(6)  << i
                          << std::setw(8);

            if (lexch >= 32 && lexch <= 126) {
                // печатный ASCII
                (*log.stream) << lexch;
            } else {
                // непечатный — выводим числовой код
                (*log.stream) << ("\\x" + std::to_string(static_cast<unsigned char>(lexch)));
            }

            (*log.stream) << std::setw(10);
            if (entry.idxTI == LT_TI_NULLIDX) {
                (*log.stream) << "-";
            } else {
                (*log.stream) << entry.idxTI;
            }

            (*log.stream) << std::setw(8) << entry.sn << std::endl;
        }

        (*log.stream) << std::endl;
    }

    void WriteIT(LOG log, IT::IdTable idtable) {
        if (!(log.stream->good() || log.stream->is_open())) {
            throw ERROR_THROW(113);
        }

        (*log.stream) << "---- Таблица идентификаторов ----" << std::endl;
        (*log.stream) << std::left
                      << std::setw(6)  << "Idx"
                      << std::setw(8)  << "Name"
                      << std::setw(10) << "FirstLE"
                      << std::setw(10) << "DType"
                      << std::setw(12) << "IType"
                      << std::setw(18) << "Value"
                      << std::endl;
        (*log.stream) << std::string(64, '-') << std::endl;

        for (int i = 0; i < idtable.size; ++i) {
            IT::Entry &entry = idtable.table[i];

            int idlen = 0;
            for (; idlen < ID_MAXSIZE && entry.id[idlen] != '\0'; ++idlen);
            std::string idstr(entry.id, idlen);

            const char* dtype = "unknown";
            if (entry.iddatatype == IT::INT) dtype = "integer";
            else if (entry.iddatatype == IT::STR) dtype = "string";

            const char* itype = "unknown";
            switch (entry.idtype) {
                case IT::V: itype = "var"; break;
                case IT::F: itype = "func"; break;
                case IT::P: itype = "param"; break;
                default: itype = "other"; break;
            }

            std::string valstr;
            if (entry.iddatatype == IT::INT) {
                valstr = std::to_string(entry.value.vint);
            } else if (entry.iddatatype == IT::STR) {
                unsigned char slen = static_cast<unsigned char>(entry.value.vstr[0].len);
                if (slen > 0 && slen < TI_STR_MAXSIZE) {
                    valstr.assign(entry.value.vstr[0].str, slen);
                } else {
                    int sllen = 0;
                    while (sllen < (TI_STR_MAXSIZE - 1) && entry.value.vstr[0].str[sllen] != '\0') ++sllen;
                    valstr.assign(entry.value.vstr[0].str, sllen);
                }
            } else {
                valstr = "-";
            }

            (*log.stream) << std::left
                          << std::setw(6)  << i
                          << std::setw(8)  << idstr
                          << std::setw(10) << entry.idxfirstLE
                          << std::setw(10) << dtype
                          << std::setw(12) << itype
                          << std::setw(18) << valstr
                          << std::endl;
        }

        (*log.stream) << std::endl;
    }
    void Close(LOG log) {
        if (log.stream->is_open() && log.stream->good()) {
            log.stream->close();
        }
        delete log.stream;
        log.stream = nullptr;
    }
}
