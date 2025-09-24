#include "../Headers/Log.h"
#include <cstring>

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

    void Close(LOG log) {
        if (log.stream->is_open() && log.stream->good()) {
            log.stream->close();
        }
        delete log.stream;
        log.stream = nullptr;
    }
}
