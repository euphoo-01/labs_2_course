#include "../Headers/LT.h"
#include "../Headers/Error.h"

namespace LT {
    LexTable Create(int size) {
        if (size <= 0 || size > LT_MAXSIZE)
            throw ERROR_THROW(2);

        LexTable lt;
        lt.maxsize = size;
        lt.size = 0;
        lt.table = new Entry[size];
        return lt;
    }

    void Add(LexTable &lextable, Entry entry) {
        if (lextable.size >= lextable.maxsize)
            throw ERROR_THROW_IN(2, entry.sn, -1);

        lextable.table[lextable.size++] = entry;
    }

    Entry GetEntry(LexTable &lextable, int n) {
        if (n < 0 || n >= lextable.size)
            throw ERROR_THROW(102);
        return lextable.table[n];
    }

    // --- Удаление таблицы ---
    void Delete(LexTable &lextable) {
        if (lextable.table != nullptr) {
            delete[] lextable.table;
            lextable.table = nullptr;
        }
        lextable.size = 0;
        lextable.maxsize = 0;
    }
}
