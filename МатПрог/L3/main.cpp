#include <iostream>
#include <vector>
#include <climits>
#include <cstring>
#include "Combi.h"

using namespace std;

const int INF = 999999;
const int N = 5;

int distMatrix[N][N] = {
    {INF, 8, 27, 11, 14},
    {9, INF, 20, 32, 60},
    {15, 16, INF, 72, 45},
    {26, 47, 24, INF, 16},
    {77, 40, 45, 20, INF}
};

namespace little {
    int minCost = INF;
    int bestPath[N + 1];

    void copyToFinal(int curr_path[]) {
        for (int i = 0; i < N; i++)
            bestPath[i] = curr_path[i];
        bestPath[N] = curr_path[0];
    }

    int reduceMatrix(int matrix[N][N]) {
        int cost = 0;

        for (int i = 0; i < N; i++) {
            int min_val = INF;
            for (int j = 0; j < N; j++) {
                if (matrix[i][j] < min_val) {
                    min_val = matrix[i][j];
                }
            }
            if (min_val != 0 && min_val != INF) {
                cost += min_val;
                for (int j = 0; j < N; j++) {
                    if (matrix[i][j] != INF) {
                        matrix[i][j] -= min_val;
                    }
                }
            }
        }

        for (int j = 0; j < N; j++) {
            int min_val = INF;
            for (int i = 0; i < N; i++) {
                if (matrix[i][j] < min_val) {
                    min_val = matrix[i][j];
                }
            }
            if (min_val != 0 && min_val != INF) {
                cost += min_val;
                for (int i = 0; i < N; i++) {
                    if (matrix[i][j] != INF) {
                        matrix[i][j] -= min_val;
                    }
                }
            }
        }
        return cost;
    }

    void solveRecursive(int parentMatrix[N][N], int curr_bound, int curr_weight, int level, int curr_path[]) {
        if (level == N) {
            if (distMatrix[curr_path[level - 1]][curr_path[0]] != INF) {
                int curr_res = curr_weight + distMatrix[curr_path[level - 1]][curr_path[0]];
                if (curr_res < minCost) {
                    minCost = curr_res;
                    copyToFinal(curr_path);
                }
            }
            return;
        }

        for (int i = 0; i < N; i++) {
            bool is_visited = false;
            for (int j = 0; j < level; ++j) {
                if (curr_path[j] == i) {
                    is_visited = true;
                    break;
                }
            }

            if (!is_visited && parentMatrix[curr_path[level - 1]][i] != INF) {
                int tempMatrix[N][N];
                memcpy(tempMatrix, parentMatrix, N * N * sizeof(int));

                int new_weight = curr_weight + distMatrix[curr_path[level - 1]][i];

                int prev_city = curr_path[level - 1];

                for (int k = 0; k < N; k++) {
                    tempMatrix[prev_city][k] = INF;
                    tempMatrix[k][i] = INF;
                }
                tempMatrix[i][0] = INF;

                int reduction_cost = reduceMatrix(tempMatrix);
                int new_bound = curr_bound + parentMatrix[prev_city][i] + reduction_cost;

                if (new_bound < minCost) {
                    curr_path[level] = i;
                    solveRecursive(tempMatrix, new_bound, new_weight, level + 1, curr_path);
                }
            }
        }
    }

    void solve() {
        int curr_path[N + 1];
        memset(curr_path, -1, sizeof(curr_path));

        int initialMatrix[N][N];
        memcpy(initialMatrix, distMatrix, N * N * sizeof(int));

        int initial_bound = reduceMatrix(initialMatrix);

        curr_path[0] = 0;

        solveRecursive(initialMatrix, initial_bound, 0, 1, curr_path);

        cout << "Метод ветвей и границ:" << endl;
        cout << "Оптимальная стоимость: " << minCost << "\nОптимальный путь: ";
        for (int i = 0; i <= N; i++) {
            cout << (bestPath[i] + 1) << (i == N ? "" : " -> ");
        }
        cout << "\n\n";
    }
}


namespace bruteforce {
    void solve() {
        combi::permutation p(N);
        long long rc = p.getfirst();

        int min_cost = INF;
        vector<short> best_path;

        while (rc >= 0) {
            int current_cost = 0;
            bool valid = true;

            for (int i = 0; i < N - 1; i++) {
                int from = p.ntx(i);
                int to = p.ntx(i + 1);
                if (distMatrix[from][to] == INF) {
                    valid = false;
                    break;
                }
                current_cost += distMatrix[from][to];
            }

            int last = p.ntx(N - 1);
            int first = p.ntx(0);
            if (distMatrix[last][first] == INF) valid = false;
            else current_cost += distMatrix[last][first];

            if (valid && current_cost < min_cost) {
                min_cost = current_cost;
                best_path.clear();
                for (int i = 0; i < N; i++) best_path.push_back(p.ntx(i) + 1);
            }

            rc = p.getnext();
        }

        cout << "Перестановки:" << endl;
        cout << "Оптимальная стоимость: " << min_cost << "\nОптимальный путь: ";
        if (!best_path.empty()) {
            for (int city: best_path) cout << city << " -> ";
            cout << best_path[0] << endl;
        }
    }
}

int main() {
    setlocale(LC_ALL, "Russian");
    little::solve();
    bruteforce::solve();
    return 0;
}
