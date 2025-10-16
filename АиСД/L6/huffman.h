#pragma once
#include <string>

namespace HCompression {
    typedef struct _nodeTree {
        wchar_t val;
        int priority;
        _nodeTree* left;
        _nodeTree* right;

        bool operator<(const _nodeTree& other) const {
            return priority < other.priority;
        }
    } Node;

    typedef struct _tree {
        int size;
        _nodeTree* root;
    } Tree;

    typedef struct _nodeTable {
        wchar_t val;
        wchar_t* code;
        _nodeTable* next;
    } NodeTable;

    typedef struct _table {
        int size;
        _nodeTable* root;
    } Table;

    void encode(const std::wstring& text);
}
