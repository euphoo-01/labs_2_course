#include "TransportTask.h"

int main() {
    std::vector<double> supply = {177, 122, 159, 168, 109};
    std::vector<double> demand = {152, 116, 140, 202, 104, 172};
    std::vector<std::vector<double>> costs = {
        {21, 11, 15, 12, 20, 10},
        {19, 9, 17, 14, 16, 22},
        {10, 14, 20, 17, 11, 20},
        {13, 19, 19, 12, 22, 11},
        {12, 20, 18, 9, 19, 13}
    };

    TransportSolver solver(costs, supply, demand);
    solver.solve();
    solver.printResult();

    return 0;
}
