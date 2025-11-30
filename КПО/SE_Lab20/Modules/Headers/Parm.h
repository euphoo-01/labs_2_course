#pragma once
#define PARM_IN L"-in:"
#define PARM_OUT L"-out:"
#define PARM_LOG L"-log:"
#define PARM_MAXSIZE 300
#define PARM_OUT_DEFAULT_EXT L".out"
#define PARM_LOG_DEFAULT_EXT L".log"

namespace Parm {
    struct PARM {
        wchar_t in[PARM_MAXSIZE];
        wchar_t out[PARM_MAXSIZE];
        wchar_t log[PARM_MAXSIZE];
    };

    PARM getparm(int argc, wchar_t *argv[]);
}
