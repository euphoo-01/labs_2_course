#include <iostream>
#include <iomanip>
#include <ctime>
#include "Combi.h"
#include "Salesman.h"
#include "Auxil.h"

// #define TEST

#ifdef TEST
int main(int argc, char* argv[])
{
    char AA[][2] = { "A", "B", "C", "D", "E" };
    int N = sizeof(AA) / sizeof(AA[0]);
    int M = 3;

    std::cout << std::endl << "Подмножества:";
    std::cout << std::endl << "Исходное множество: { ";
    for (int i = 0; i < N; i++) std::cout << AA[i] << ((i < N - 1) ? ", " : " ");
    std::cout << "}";

    combi::subset s1(N);
    short n_sub = s1.getfirst();
    while (n_sub >= 0)
    {
        std::cout << std::endl << std::setw(2) << s1.mask << ": { ";
        for (int i = 0; i < n_sub; i++)
            std::cout << AA[s1.ntx(i)] << ((i < n_sub - 1) ? ", " : " ");
        std::cout << "}";
        n_sub = s1.getnext();
    };
    std::cout << std::endl << "Всего подмножеств: " << s1.count() << std::endl;


    std::cout << std::endl << "Перестановки:";
    std::cout << std::endl << "Исходное множество: { ";
    for (int i = 0; i < N; i++)
        std::cout << AA[i] << ((i < N - 1) ? ", " : " ");
    std::cout << "}";

    std::cout << std::endl << "Перестановки из " << N << " элементов:";
    combi::permutation p(N);
    long long n = p.getfirst();

    while (n >= 0)
    {
        std::cout << std::endl << std::setw(4) << p.np << ": { ";
        for (int i = 0; i < p.n; i++)
            std::cout << AA[p.ntx(i)] << ((i < p.n - 1) ? ", " : " ");
        std::cout << "}";
        n = p.getnext();
    };

    std::cout << std::endl << "всего: " << p.count() << std::endl;

    std::cout << std::endl << std::endl << "Сочетания: ";
    std::cout << std::endl << "Сочетания из " << N << " по " << M << ":";

    combi::combination с(N, M);
    short k = с.getfirst();

    while (k >= 0)
    {
        std::cout << std::endl << std::setw(4) << с.nc << ": { ";
        for (int i = 0; i < k; i++)
            std::cout << AA[с.ntx(i)] << ((i < k - 1) ? ", " : " ");
        std::cout << "}";
        k = с.getnext();
    }
    std::cout << std::endl << "всего: " << с.count() << std::endl;

    // ==========================================
    // 3. ТЕСТ РАЗМЕЩЕНИЙ
    // ==========================================
    std::cout << std::endl << std::endl << "Размещения:";
    std::cout << std::endl << "Размещения из " << N << " по " << M << ":";

    combi::accomodation acc(N, M);
    short j = acc.getfirst();

    while (j >= 0)
    {
        std::cout << std::endl << std::setw(4) << acc.na << ": { ";
        for (int i = 0; i < j; i++)
            std::cout << AA[acc.ntx(i)] << ((i < j - 1) ? ", " : " ");
        std::cout << "}";
        j = acc.getnext();
    }
    std::cout << std::endl << "всего: " << acc.count() << std::endl;

    return 0;
}

#else
#define SPACE(n) std::setw(n)<<" "

int main(int argc, char* argv[])
{
    auxil::start();

    int N5 = 10;
    int* d5 = new int[N5 * N5];
    int* r5 = new int[N5];

    for (int i = 0; i < N5 * N5; i++) {
        if (i % (N5 + 1) == 0) d5[i] = INF; 
        else d5[i] = auxil::iget(10, 300);
    }

    int inf_count = 0;
    while (inf_count < 3)
    {
        int idx = auxil::iget(0, N5 * N5 - 1);
        if (d5[idx] != INF)
        {
            d5[idx] = INF;
            inf_count++;
        }
    }

    std::cout << std::endl << "Коммивояжер:";
    std::cout << std::endl << "количество городов: " << N5;
    std::cout << std::endl << "матрица расстояний: ";
    
    for(int i = 0; i < N5; i++)
    {
        std::cout << std::endl;
        for (int j = 0; j < N5; j++)
            if (d5[i*N5 + j] != INF) std::cout << std::setw(4) << d5[i*N5 + j] << " ";
            else std::cout << std::setw(4) << "INF" << " ";
    }

    int dist = salesman(N5, d5, r5);

    std::cout << std::endl << "оптимальный маршрут: ";
    for (int i = 0; i < N5; i++) std::cout << r5[i] << "-->";
    std::cout << 0;
    std::cout << std::endl << "длина маршрута: " << dist << std::endl;

    delete[] d5;
    delete[] r5;

    std::cout << std::endl << "Продолжительность: ";
    std::cout << std::endl << "-- количество ------ продолжительность -- ";
    std::cout << std::endl << "   городов            вычисления (тиков) ";

    clock_t t1, t2;
    int N_MAX = 12;
    int* d6 = new int[N_MAX * N_MAX];
    int* r6 = new int[N_MAX];

    for (int i = 6; i <= 12; i++)
    {
        for (int k = 0; k < i * i; k++) {
             if (k % (i + 1) == 0) d6[k] = INF;
             else d6[k] = auxil::iget(10, 300);
        }

        t1 = clock();
        salesman(i, d6, r6);
        t2 = clock();

        std::cout << std::endl << SPACE(7) << std::setw(2) << i
            << SPACE(15) << std::setw(5) << (t2 - t1);
    }

    delete[] d6;
    delete[] r6;

    std::cout << std::endl << std::endl;
    return 0;
}
#endif