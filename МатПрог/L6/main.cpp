#include <iostream>
#include <algorithm>
#include <vector>
#include "Graph.h"
#include "BFS.h"
#include "DFS.h"

int main()
{
    setlocale(LC_ALL, "rus");

    int m[7][7] = {
        {0, 8, 1, 0, 0, 0, 0},
        {8, 0, 0, 4, 5, 0, 0},
        {1, 0, 0, 7, 0, 11, 0},
        {0, 4, 7, 0, 0, 6, 9},
        {0, 5, 0, 0, 0, 0, 10},
        {0, 0, 11, 6, 0, 0, 2},
        {0, 0, 0, 9, 10, 2, 0}
    };

    graph::AMatrix g1(7, (int*)m);
    std::cout << "-- матрица смежности " << std::endl;
    for (int i = 0; i < g1.n_vertex; i++) {
        for (int j = 0; j < g1.n_vertex; j++) std::cout << g1.get(i, j) << " ";
        std::cout << std::endl;
    }

    graph::AList g2(g1);
    std::cout << std::endl << "-- списки смежных вершин " << std::endl;
    for (int i = 0; i < g2.n_vertex; i++) {
        std::cout << i << ": ";
        for (int j = 0; j < g2.size(i); j++) std::cout << g2.get(i, j) << " ";
        std::cout << std::endl;
    }

    std::cout << std::endl << "-- поиск в ширину (от 0)" << std::endl;
    BFS b1(g2, 0);
    int k1;
    while ((k1 = b1.get()) != BFS::NIL) std::cout << k1 << " ";
    std::cout << std::endl;

    std::cout << std::endl << "-- поиск в глубину " << std::endl;
    DFS d1(g2);
    for (int i = 0; i < g2.n_vertex; i++) std::cout << d1.get(i) << " ";
    std::cout << std::endl;

    std::cout << std::endl << "-- топологическая сортировка " << std::endl;
    std::vector<int> ts = d1.topological_sort;
    std::reverse(ts.begin(), ts.end());
    for (int v : ts) std::cout << v << " ";
    std::cout << std::endl;

    return 0;
}