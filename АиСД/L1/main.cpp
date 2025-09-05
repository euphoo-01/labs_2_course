#include <iostream>

void hanoi(int n, int from, int to, int helper) {
    if (n == 1) {
        std::cout << "Переместить диск 1 с " << from << " на " << to << " стержень" << std::endl;
        return;
    }
    hanoi(n - 1, from, helper, to);
    std::cout << "Переместить диск " << n << " с " << from << " на " << to << " стержень" << std::endl;
    hanoi(n - 1, helper, to, from);
}

int main() {
    int N, k;
    std::cout << "Введите количество дисков N: ";
    std::cin >> N;
    std::cout << "Введите номер целевого стержня k: ";
    std::cin >> k;
    if (k > 3) {
        std::cerr << "Максимум 3 стержня." << std::endl;
        return 1;
    }

    int from = 1;
    int helper = 6 - from - k;

    hanoi(N, from, k, helper);
    return 0;
}