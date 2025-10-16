#include "huffman.h"

#include <algorithm>
#include <iostream>
#include <map>
#include <string>
#include <cwchar>
#include <queue>

namespace HCompression {

    static void buildTable(Node *root, std::wstring prefix, Table *table) {
        if (!root) return;

        if (!root->left && !root->right) {
            auto *node = new NodeTable();
            node->val = root->val;
            node->code = new wchar_t[prefix.size() + 1];
            std::wcscpy(node->code, prefix.c_str());
            node->next = table->root;
            table->root = node;
            table->size++;
            return;
        }

        buildTable(root->left, prefix + L"0", table);
        buildTable(root->right, prefix + L"1", table);
    }

    static const wchar_t *findCode(Table *table, wchar_t ch) {
        NodeTable *it = table->root;
        while (it) {
            if (it->val == ch) return it->code;
            it = it->next;
        }
        return nullptr;
    }

    static Node *buildTree(const std::wstring &text) {
        std::map<wchar_t, int> freq;
        for (wchar_t c : text)
            freq[c]++;

        struct Compare {
            bool operator()(const Node *a, const Node *b) const {
                return a->priority > b->priority;
            }
        };

        std::priority_queue<Node *, std::vector<Node *>, Compare> q;

        for (auto &p: freq) {
            Node *temp = new Node();
            temp->val = p.first;
            temp->priority = p.second;
            temp->left = nullptr;
            temp->right = nullptr;
            q.push(temp);
        }

        while (q.size() > 1) {
            Node *qn1 = q.top();
            q.pop();
            Node *qn2 = q.top();
            q.pop();

            Node *parent = new Node();
            parent->val = 0;
            parent->priority = qn1->priority + qn2->priority;
            parent->left = qn1;
            parent->right = qn2;

            q.push(parent);
        }

        return q.top();
    }

    void encode(const std::wstring &text) {
        if (text.empty()) {
            std::wcout << L"Пустая строка.\n";
            return;
        }

        Node *root = buildTree(text);

        Table table = {0, nullptr};
        buildTable(root, L"", &table);

        std::map<wchar_t, int> freq;
        for (wchar_t c : text)
            freq[c]++;

        std::wcout << L"Таблица частот символов:\n";
        for (auto &p: freq)
            std::wcout << L"'" << p.first << L"' : " << p.second << L"\n";

        std::wcout << L"\nТаблица кодов Хаффмана:\n";
        for (NodeTable *it = table.root; it; it = it->next)
            std::wcout << L"'" << it->val << L"' : " << it->code << L"\n";

        std::wstring encoded;
        for (wchar_t c: text) {
            const wchar_t *code = findCode(&table, c);
            if (code)
                encoded += code;
        }

        std::wcout << L"\nИсходная строка: " << text << L"\n";
        std::wcout << L"Закодированная строка: " << encoded << L"\n";

        int original_bits = text.size() * 16;
        int compressed_bits = encoded.size();

        std::wcout << L"Исходный размер:   " << original_bits << L" бит\n";
        std::wcout << L"Сжатый размер:     " << compressed_bits << L" бит\n";
    }
}
