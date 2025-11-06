#include <iostream>
#include <vector>
#include <algorithm>

int main() {
    std::vector<int> input;
    std::cout << "Введите число N: ";
    int N;
    std::cin >> N;
    int in;
    std::cout << "Введите последовательность (через пробел): " << std::endl;
    for (int m = 0; m < N; m++) {
        std::cin >> in;
        input.push_back(in);
    }

    if (input.empty()) {
        std::cout << "0" << std::endl;
        return 0;
    }
    std::vector<int> aux;
    std::vector<int> res;

    aux.push_back(1);
    int max_length = 1;

    for (int i = 1; i < input.size(); i++) {
        int cur_length = 1;
        for (int j = 0; j < i; j++) {
            if (input[j] < input[i] && aux[j] + 1 > cur_length) {
                cur_length = aux[j] + 1;
            }
        }
        aux.push_back(cur_length);
        if (cur_length > max_length) {
            max_length = cur_length;
        }
    }

    int cur_length = max_length;
    for (int i = input.size() - 1; i >= 0 && cur_length > 0; i--) {
        if (aux[i] == cur_length) {
            res.push_back(input[i]);
            cur_length--;
        }
    }

    std::reverse(res.begin(), res.end());

    std::cout << max_length << std::endl;
    for (int k = 0; k < res.size(); k++) {
        std::cout << res[k];
        if (k < res.size() - 1) {
            std::cout << ", ";
        }
    }
    std::cout << std::endl;

    return 0;
}