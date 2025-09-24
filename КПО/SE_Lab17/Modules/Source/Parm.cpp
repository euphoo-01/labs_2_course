#include "../Headers/Parm.h"
#include "../Headers/Error.h"
#include <cwchar>

namespace Parm {
    PARM getparm(int argc, wchar_t *argv[]) {
        PARM parameters = {L"\0", L"\0", L"\0"};
        if (argc <= 1) {
            throw ERROR_THROW(100);
        }
        bool hasInParm = false,
                hasOutParm = false,
                hasLogParm = false;
        for (int i = 1; i < argc; i++) {
            // i = 1, т.к. 0 параметр - название исполнительного файла
            if (wcslen(argv[i]) > PARM_MAXSIZE) {
                throw ERROR_THROW(104);
            }

            if (argv[i][0] == L'-') {
                if (wcsstr(argv[i],PARM_IN) == argv[i] && wcslen(argv[i]) >= wcslen(PARM_IN)) {
                    wchar_t *value = argv[i] + wcslen(PARM_IN); // Сдвиг указателя на значение параметра
                    if (wcslen(value) > PARM_MAXSIZE) {
                        throw ERROR_THROW(104);
                    }
                    if (wcslen(value) == 0) {
                        throw ERROR_THROW(102);
                    }
                    wcscpy(parameters.in, value);
                    hasInParm = true;
                } else if (wcsstr(argv[i],PARM_OUT) == argv[i] && wcslen(argv[i]) >= wcslen(PARM_OUT)) {
                    wchar_t *value = argv[i] + wcslen(PARM_OUT); // Сдвиг указателя на значение параметра
                    if (wcslen(value) > PARM_MAXSIZE) {
                        throw ERROR_THROW(104);
                    }
                    if (wcslen(value) == 0) {
                        continue;
                    }
                    wcscpy(parameters.out, value);
                    hasOutParm = true;
                } else if (wcsstr(argv[i],PARM_LOG) == argv[i] && wcslen(argv[i]) >= wcslen(PARM_LOG)) {
                    wchar_t *value = argv[i] + wcslen(PARM_LOG); // Сдвиг указателя на значение параметра
                    if (wcslen(value) > PARM_MAXSIZE) {
                        throw ERROR_THROW(104);
                    }
                    if (wcslen(value) == 0) {
                        continue;
                    }
                    wcscpy(parameters.log, value);
                    hasLogParm = true;
                } else {
                    throw ERROR_THROW(101);
                }
            } else {
                throw ERROR_THROW(101);
            }
        }

        if (!hasInParm) {
            throw ERROR_THROW(100);
        }


        // Параметры по умолчанию
        if (!hasOutParm) {
            wchar_t temp[PARM_MAXSIZE] = {0};
            wcscpy(temp, parameters.in);
            if (wcslen(temp) + wcslen(PARM_OUT_DEFAULT_EXT) < PARM_MAXSIZE) {
                wcscat(temp, PARM_OUT_DEFAULT_EXT);
                wcscpy(parameters.out, temp);
            } else {
                throw ERROR_THROW(104);
            }
        }
        if (!hasLogParm) {
            wchar_t temp[PARM_MAXSIZE] = {0};
            wcscpy(temp, parameters.in);
            if (wcslen(temp) + wcslen(PARM_LOG_DEFAULT_EXT) < PARM_MAXSIZE) {
                wcscat(temp, PARM_LOG_DEFAULT_EXT);
                wcscpy(parameters.log, temp);
            } else {
                throw ERROR_THROW(104);
            }
        }

        return parameters;
    }
}
