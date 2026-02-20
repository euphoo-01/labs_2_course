#pragma once

namespace combi
{
    struct permutation
    {
        const static bool L = true;  // левая стрелка
        const static bool R = false; // правая стрелка

        short n;        // количество элементов исходного множества
        short* sset;    // массив индексов текущей перестановки
        bool* dart;     // массив стрелок
        unsigned long long np; // номер перестановки

        permutation(short n = 1);
        ~permutation();

        void reset();
        long long getfirst();
        long long getnext();
        short ntx(short i);
        unsigned long long count() const;
    };
    struct combination
 {
        short n; // количество элементов исходного множества
        short m; // количество элементов в сочетаниях
        short* sset; // массив индексов текущего сочетания
        unsigned long long nc; // номер сочетания


        combination(short n = 1, short m = 1);
        ~combination();

        void reset(); // сбросить генератор, начать сначала
        short getfirst(); // сформировать первый массив индексов
        short getnext(); // сформировать следующий массив индексов
        short ntx(short i); // получить i-й элемент массива индексов
        unsigned long long count() const; // вычислить количество сочетаний
 };

    struct accomodation
    {
        short n; // количество элементов исходного множества
        short m; // количество элементов в размещении
        short* sset; // массив индексов текущего размещения
        combination* cgen; // указатель на генератор сочетаний
        permutation* pgen; // указатель на генератор перестановок
        unsigned long long na; // номер размещения

        accomodation(short n = 1, short m = 1);
        ~accomodation();

        void reset(); // сбросить генератор, начать сначала
        short getfirst(); // сформировать первый массив индексов
        short getnext(); // сформировать следующий массив индексов
        short ntx(short i); // получить i-й элемент массива индексов
        unsigned long long count() const; // общее количество размещений
    };
    struct subset
{
        short n; // количество элементов исходного множества < 64
        short sn; // количество элементов текущего подмножества
        short* sset; // массив индексов текущего подмножества
        unsigned long long mask; // битовая маска

        subset(short n = 1); // конструктор
        ~subset(); // деструктор

        void reset(); // сбросить генератор, начать сначала
        short getfirst(); // сформормировать массив индексов по битовой маске
        short getnext(); // ++маска и сформировать массив индексов
        short ntx(short i); // получить i-й элемент массива индексов
        unsigned long long count(); // вычислить общее количество подмножеств
};
};