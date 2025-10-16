#include <iostream>

#include "huffman.h"
#include <locale>

int main() {
    std::locale::global(std::locale(""));
    std::wcout << L"Введите исходную строку: " << std::endl;
    std::wstring text;
    std::getline(std::wcin, text);
    HCompression::encode(text);
    return 0;
}