#include "TransportTask.h"
#include <algorithm>
#include <numeric>

TransportSolver::TransportSolver(std::vector<std::vector<double>> costs,
                                 std::vector<double> supply,
                                 std::vector<double> demand)
    : C(costs), A(supply), B(demand) {
    balanceTask();
    rows = A.size();
    cols = B.size();
    X.assign(rows, std::vector<double>(cols, 0.0));
    isBasic.assign(rows, std::vector<bool>(cols, false));
}

void TransportSolver::balanceTask() {
    double sumA = std::accumulate(A.begin(), A.end(), 0.0);
    double sumB = std::accumulate(B.begin(), B.end(), 0.0);
    if (sumA < sumB) {
        A.push_back(sumB - sumA);
        C.push_back(std::vector<double>(B.size(), 0.0));
    } else if (sumA > sumB) {
        B.push_back(sumA - sumB);
        for (auto& row : C) row.push_back(0.0);
    }
}

void TransportSolver::initialPlan() {
    std::vector<double> tempA = A;
    std::vector<double> tempB = B;

    while (true) {
        int r = -1, c = -1;
        double minC = 1e18;

        for(int i=0; i<rows; i++) {
            if (tempA[i] <= 0) continue;
            for(int j=0; j<cols; j++) {
                if (tempB[j] <= 0) continue;
                if (C[i][j] < minC) {
                    minC = C[i][j]; r = i; c = j;
                }
            }
        }

        if (r == -1) break;

        double val = std::min(tempA[r], tempB[c]);
        X[r][c] = val;
        isBasic[r][c] = true;
        tempA[r] -= val;
        tempB[c] -= val;
    }
}

void TransportSolver::calculatePotentials(std::vector<double>& u, std::vector<double>& v) {
    std::vector<bool> u_set(rows, false), v_set(cols, false);
    u.assign(rows, 0.0); v.assign(cols, 0.0);
    u_set[0] = true;

    for (int k = 0; k < rows + cols; ++k) {
        for (int i = 0; i < rows; ++i) {
            for (int j = 0; j < cols; ++j) {
                if (isBasic[i][j]) {
                    if (u_set[i] && !v_set[j]) {
                        v[j] = C[i][j] - u[i]; v_set[j] = true;
                    } else if (!u_set[i] && v_set[j]) {
                        u[i] = C[i][j] - v[j]; u_set[i] = true;
                    }
                }
            }
        }
    }
}

bool findPath(Cell current, Cell target, int rows, int cols,
              const std::vector<std::vector<bool>>& isBasic,
              std::vector<Cell>& path, bool lookInRow) {

    if (path.size() > 3 && current.r == target.r && current.c == target.c) return true;

    if (lookInRow) {
        for (int j = 0; j < cols; j++) {
            if (j != current.c && isBasic[current.r][j]) {
                path.push_back({current.r, j});
                if (findPath({current.r, j}, target, rows, cols, isBasic, path, false)) return true;
                path.pop_back();
            }
        }
    } else {
        for (int i = 0; i < rows; i++) {
            if (i != current.r && isBasic[i][current.c]) {
                path.push_back({i, current.c});
                if (findPath({i, current.c}, target, rows, cols, isBasic, path, true)) return true;
                path.pop_back();
            }
        }
    }
    return false;
}

void TransportSolver::solve() {
    initialPlan();

    while (true) {
        std::vector<double> u, v;
        calculatePotentials(u, v);

        double maxDelta = -1e-9;
        Cell entryCell = {-1, -1};

        for (int i = 0; i < rows; i++) {
            for (int j = 0; j < cols; j++) {
                if (!isBasic[i][j]) {
                    double delta = u[i] + v[j] - C[i][j];
                    if (delta > maxDelta) {
                        maxDelta = delta; entryCell = {i, j};
                    }
                }
            }
        }

        if (entryCell.r == -1 || maxDelta <= 0) break;

        std::vector<Cell> cycle;
        cycle.push_back(entryCell);
        if (!findPath(entryCell, entryCell, rows, cols, isBasic, cycle, true)) {
            break;
        }

        double theta = 1e18;
        for (size_t k = 1; k < cycle.size(); k += 2) {
            theta = std::min(theta, X[cycle[k].r][cycle[k].c]);
        }

        for (size_t k = 0; k < cycle.size(); k++) {
            if (k % 2 == 0) {
                X[cycle[k].r][cycle[k].c] += theta;
                isBasic[cycle[k].r][cycle[k].c] = true;
            } else {
                X[cycle[k].r][cycle[k].c] -= theta;
            }
        }

        bool removed = false;
        for (size_t k = 1; k < cycle.size(); k += 2) {
            if (X[cycle[k].r][cycle[k].c] < 1e-9 && !removed) {
                isBasic[cycle[k].r][cycle[k].c] = false;
                removed = true;
            }
        }
        std::cout << "Текущая стоимость Z = " << getTotalCost() << std::endl;
    }
}

double TransportSolver::getTotalCost() {
    double total = 0;
    for (int i = 0; i < rows; i++)
        for (int j = 0; j < cols; j++)
            total += X[i][j] * C[i][j];
    return total;
}

void TransportSolver::printResult() {
    std::cout << "\nМатрица цен:\n";
    for (int i = 0; i < rows; i++) {
        for (int j = 0; j < cols; j++) std::cout << std::setw(8) << C[i][j] << " ";
        std::cout << "\n";
    }
    std::cout << "\nОптимальный план:\n";
    for (int i = 0; i < rows; i++) {
        for (int j = 0; j < cols; j++) std::cout << std::setw(8) << X[i][j] << " ";
        std::cout << "\n";
    }
    std::cout << "\nМинимальные затраты Z = " << getTotalCost() << std::endl;
}
