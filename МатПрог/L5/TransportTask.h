#pragma once
#include <vector>
#include <iostream>
#include <iomanip>

struct Cell {
    int r, c;
};

class TransportSolver {
public:
    TransportSolver(std::vector<std::vector<double>> costs,
                    std::vector<double> supply,
                    std::vector<double> demand);
    void solve();
    void printResult();

private:
    int rows, cols;
    std::vector<std::vector<double>> C; // стоимости перевозки
    std::vector<std::vector<double>> X; // таблица плана
    std::vector<std::vector<bool>> isBasic;
    std::vector<double> A; // склады
    std::vector<double> B; // магазины

    void balanceTask();
    void initialPlan();
    void calculatePotentials(std::vector<double>& u, std::vector<double>& v);
    double getTotalCost();
};
