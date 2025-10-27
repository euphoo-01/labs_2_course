#include "GRB.h"
#include "cstring"
#include "Rule.h"
#include <cstdarg>
#include <iostream>


namespace GRB {
    Rule::Chain::Chain(short psize, GRBALPHABET s, ...) {
        nt = new GRBALPHABET[size = psize];
        if (nt && psize > 0) {
            va_list args;
            va_start(args, s);

            nt[0] = s;

            for (short i = 1; i < psize; i++) {
                nt[i] = (GRBALPHABET) va_arg(args, int); // Используем int!
            }

            va_end(args);
        }
    }

    Rule::Rule(GRBALPHABET pnn, int piderror, short psize, Chain c, ...) {
        nn = pnn;
        iderror = piderror;
        size = psize;

        if (psize > 0) {
            chains = new Chain[psize];

            if (chains) {
                // Первая цепочка уже передана как параметр c
                chains[0] = c;

                // Обрабатываем остальные цепочки через va_arg
                if (psize > 1) {
                    va_list args;
                    va_start(args, c);

                    // Пропускаем первый аргумент (c), так как мы его уже обработали
                    for (int i = 1; i < psize; i++) {
                        // Получаем следующий аргумент как Chain
                        Chain next_chain = va_arg(args, Chain);
                        chains[i] = next_chain;
                    }

                    va_end(args);
                }
            }
        } else {
            chains = nullptr;
        }
    }

    Greibach::Greibach(GRBALPHABET pstartN, GRBALPHABET pstbottom, short psize, Rule r, ...) {
        startN = pstartN;
        stbottomT = pstbottom;
        rules = new Rule[size = psize];

        // Первый переданный named-аргумент
        if (size > 0) rules[0] = r;

        if (size > 1) {
            va_list args;
            va_start(args, r);
            for (int i = 1; i < size; ++i) {
                Rule next = va_arg(args, Rule);
                rules[i] = next;
            }
            va_end(args);
        }
    };

    Greibach getGreibach() {
        return greibach;
    };

    short Greibach::getRule(GRBALPHABET pnn, Rule &prule) {
        short rc = -1;
        short k = 0;
        while (k < size && rules[k].nn != pnn) k++;
        if (k < size) prule = rules[rc = k];
        return rc;
    };

    Rule Greibach::getRule(short n) {
        Rule rc;
        if (n < size) rc = rules[n];
        return rc;
    };

    char *Rule::getCRule(char *b, short nchain) {
        char bchain[200];
        b[0] = Chain::alphabet_to_char(nn);
        b[1] = '-';
        b[2] = '>';
        b[3] = 0x00;
        chains[nchain].getCChain(bchain);
        std::strcat(b, bchain);
        return b;
    };

    short Rule::getNextChain(GRBALPHABET t, Rule::Chain &pchain, short j) {
        short rc = -1;
        while (j < size && chains[j].nt[0] != t) ++j;
        rc = (j < size ? j : -1);
        if (rc >= 0) pchain = chains[rc];
        return rc;
    };

    char *Rule::Chain::getCChain(char *b) {
        for (int i = 0; i < size; i++) b[i] = Chain::alphabet_to_char(nt[i]);
        b[size] = 0x00;
        return b;
    };
}
