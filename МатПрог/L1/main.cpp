#include "pch.h"

#define TEST

long long fibonacci(int n) {
    if (n <= 1) return n;
    return fibonacci(n - 1) + fibonacci(n - 2);
}

#ifdef TEST
void test_graphs() {
    std::cout << "\nРандомные числа" << std::endl;
    std::cout << "Циклов\tВремя(тик)" << std::endl;
    for (int cycles = 1000000; cycles <= 10000000; cycles += 1000000) {
        double av1 = 0, av2 = 0;
        clock_t t1 = clock();
        for (int i = 0; i < cycles; i++) {
            av1 += (double)auxil::iget(-100, 100);
            av2 += auxil::dget(-100, 100);
        }
        clock_t t2 = clock();
        std::cout << cycles << "\t"
                  << ((double)(t2 - t1)) << std::endl;
    }

    std::cout << "\nФибоначчи:" << std::endl;
    std::cout << "N\tВремя(тик)" << std::endl;
    for (int n = 25; n <= 40; n++) {
        clock_t t1 = clock();
        long long res = fibonacci(n);
        clock_t t2 = clock();
        std::cout << n << "\t"
                  << ((double)(t2 - t1))  << std::endl;
    }
}
#endif

int main() {
    setlocale(LC_ALL, "ru_RU.UTF-8");
    auxil::start();

#ifdef TEST
    test_graphs();
#else
    const int CYCLE = 1000000;
    double av1 = 0, av2 = 0;
    clock_t t1 = clock();
    for (int i = 0; i < CYCLE; i++) {
        av1 += (double)auxil::iget(-100, 100);
        av2 += auxil::dget(-100, 100);
    }
    clock_t t2 = clock();

    std::cout << "Задание 2" << std::endl;
    std::cout << "Количество циклов:         " << CYCLE << std::endl;
    std::cout << "Среднее значение (int):    " << av1 / CYCLE << std::endl;
    std::cout << "Среднее значение (double): " << av2 / CYCLE << std::endl;
    std::cout << "Продолжительность (у.е):   " << (t2 - t1) << std::endl;
    std::cout << "                  (сек):   " << ((double)(t2 - t1)) / CLOCKS_PER_SEC << std::endl;

    std::cout << "\nЗадание 3" << std::endl;
    int n = 30;
    t1 = clock();
    long long res = fibonacci(n);
    t2 = clock();
    std::cout << "Fib(" << n << ") = " << res << ", Время: " << (t2-t1) << " у.е." << std::endl;
#endif

    return 0;
}
