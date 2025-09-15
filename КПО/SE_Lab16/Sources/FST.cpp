#include "../Headers/FST.h"

#include <cstring>
#include <cstdarg>

namespace FST {
    RELATION::RELATION(
        char c = 0x00, // символ перехода
        short n = NULL // новое состояние
    ) {
        this->symbol = c;
        this->nnode = n;
    };

    NODE::NODE() {
        this->n_relation = 0;
        this->relations = nullptr;
    }


    NODE::NODE(
        short n, // количество инцидентных ребер
        RELATION rel, ... // список ребер
    ) {
        this->n_relation = n;
        this->relations = new RELATION[n];
        va_list args;
        this->relations[0] = rel;
        va_start(args, n);
        for (int i = 1; i < n; i++) {
            this->relations[i] = va_arg(args, RELATION);
        }
        va_end(args);
    }

    FST::FST(
        char *s, // цепочка
        short ns, // количество состояний автомата
        NODE n, ... // список состояний (граф переходов)
    ) {
        this->string = s;
        this->position = 0;
        this->nstates = ns;
        this->nodes = new NODE[ns];
        this->nodes[0] = n;
        va_list args;
        va_start(args, ns);
        for (int i = 1; i < ns; i++) {
            this->nodes[i] = va_arg(args, NODE);
        }
        va_end(args);
        this->rstates = new short[ns];
        for (int i = 0; i < ns; i++) {
            this->rstates[i] = -1;
        }
    };

    bool execute(FST &fst) {
        for (int i = 0; i < fst.nstates; i++)
            fst.rstates[i] = -1;

        fst.rstates[0] = 0;

        for (int pos = 0; fst.string[pos] != '\0'; pos++) {
            short *temp = new short[fst.nstates];
            for (int i = 0; i < fst.nstates; i++)
                temp[i] = -1;

            for (int state = 0; state < fst.nstates; state++) {
                if (fst.rstates[state] != -1) {
                    NODE node = fst.nodes[state];
                    for (int j = 0; j < node.n_relation; j++) {
                        RELATION rel = node.relations[j];
                        if (rel.symbol == fst.string[pos]) {
                            temp[rel.nnode] = pos + 1;
                        }
                    }
                }
            }

            for (int i = 0; i < fst.nstates; i++) {
                fst.rstates[i] = temp[i];
            }
            delete[] temp;
        }

        return fst.rstates[fst.nstates - 1] == (short) strlen(fst.string);
    }
}
