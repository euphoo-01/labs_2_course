#include "../Headers/LT.h"
#include "../Headers/Error.h"

namespace LT {
    LexTable Create(int size) {
        if (size > LT_MAXSIZE) {
            throw ERROR_THROW(117);
        }

        LexTable lextable;
        lextable.maxsize = size;
        lextable.size = 0;
        lextable.table = new Entry[size];
        return lextable;
    }

    void Add(LexTable &lextable, Entry entry) {
        if (lextable.size + 1 > lextable.maxsize) {
            throw ERROR_THROW_IN(117, entry.sn, -1);
        }
        lextable.table[lextable.size] = entry;
        lextable.size += 1;
    }

    Entry GetEntry(LexTable &lextable, int n) {
        if (n >= lextable.size || n < 0) {
            throw ERROR_THROW(126);
        }
        return lextable.table[n];
    }

    void Delete(LexTable &lextable) {
        delete[] lextable.table;
        lextable.table = nullptr;
        lextable.maxsize = 0;
        lextable.size = 0;
    }
}
