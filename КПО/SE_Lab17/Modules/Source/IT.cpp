#include "../Headers/IT.h"
#include "../Headers/Error.h"
#include <cstring>

namespace IT {
    IdTable Create(int size) {
        if (size <= 0 || size > TI_MAXSIZE)
            throw ERROR_THROW(4);

        IdTable idt;
        idt.maxsize = size;
        idt.size = 0;
        idt.table = new Entry[size];
        return idt;
    }

    void Add(IdTable &idtable, Entry entry) {
        if (idtable.size >= idtable.maxsize)
            throw ERROR_THROW(4);

        idtable.table[idtable.size++] = entry;
    }

    Entry GetEntry(IdTable &idtable, int n) {
        if (n < 0 || n >= idtable.size)
            throw ERROR_THROW(5);
        return idtable.table[n];
    }

    int IsId(IdTable &idtable, char id[ID_MAXSIZE]) {
        for (int i = 0; i < idtable.size; i++) {
            if (std::strcmp(idtable.table[i].id, id) == 0) {
                return i;
            }
        }
        return TI_NULLIDX;
    }

    void Delete(IdTable &idtable) {
        if (idtable.table != nullptr) {
            delete[] idtable.table;
            idtable.table = nullptr;
        }
        idtable.size = 0;
        idtable.maxsize = 0;
    }
}
