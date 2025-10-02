#include <iostream>
#include <vector>
#include <algorithm>
#include <limits>
using namespace std;

#define V 8

//Прима
int minKey(vector<int>& key, vector<bool>& mstSet) {
    int min = numeric_limits<int>::max(), min_index = -1;
    for (int v = 0; v < V; v++)
        if (!mstSet[v] && key[v] < min)
            min = key[v], min_index = v;
    return min_index;
}

vector<pair<pair<int,int>,int>> Prima(vector<vector<int>>& graph, int &totalWeight) {
    vector<int> parent(V);
    vector<int> key(V);
    vector<bool> mstSet(V);
    totalWeight = 0;

    for (int i = 0; i < V; i++)
        key[i] = numeric_limits<int>::max(), mstSet[i] = false;

    key[0] = 0;
    parent[0] = -1;

    for (int count = 0; count < V - 1; count++) {
        int u = minKey(key, mstSet);
        mstSet[u] = true;

        for (int v = 0; v < V; v++)
            if (graph[u][v] && !mstSet[v] && graph[u][v] < key[v])
                parent[v] = u, key[v] = graph[u][v];
    }

    vector<pair<pair<int,int>,int>> result;
    cout << "Рёбра минимального остовного дерева (Прим):\n";
    for (int i = 1; i < V; i++) {
        cout << "V" << parent[i] + 1 << " - V" << i + 1
             << "  вес: " << graph[i][parent[i]] << endl;
        totalWeight += graph[i][parent[i]];
        result.push_back({{parent[i]+1, i+1}, graph[i][parent[i]]});
    }
    cout << "Суммарный вес: " << totalWeight << endl << endl;
    return result;
}

//Краскал
struct Edge { int src, dest, weight; };
struct Subset { int parent, rank; };

int find(vector<Subset>& subsets, int i) {
    if (subsets[i].parent != i)
        subsets[i].parent = find(subsets, subsets[i].parent);
    return subsets[i].parent;
}
void Union(vector<Subset>& subsets, int x, int y) {
    int xroot = find(subsets, x);
    int yroot = find(subsets, y);
    if (subsets[xroot].rank < subsets[yroot].rank)
        subsets[xroot].parent = yroot;
    else if (subsets[xroot].rank > subsets[yroot].rank)
        subsets[yroot].parent = xroot;
    else {
        subsets[yroot].parent = xroot;
        subsets[xroot].rank++;
    }
}
vector<pair<pair<int,int>,int>> Kruskal(vector<vector<int>>& graph, int &totalWeight) {
    vector<Edge> edges;
    totalWeight = 0;

    for (int i = 0; i < V; i++) {
        for (int j = i+1; j < V; j++) {
            if (graph[i][j] != 0) {
                edges.push_back({i, j, graph[i][j]});
            }
        }
    }

    sort(edges.begin(), edges.end(), [](Edge a, Edge b) {
        return a.weight < b.weight;
    });

    vector<Subset> subsets(V);
    for (int v = 0; v < V; v++) {
        subsets[v].parent = v;
        subsets[v].rank = 0;
    }

    vector<pair<pair<int,int>,int>> result;
    cout << "Рёбра минимального остовного дерева (Краскал):\n";
    int e = 0, i = 0;

    while (e < V - 1 && i < edges.size()) {
        Edge next_edge = edges[i++];
        int x = find(subsets, next_edge.src);
        int y = find(subsets, next_edge.dest);

        if (x != y) {
            cout << "V" << next_edge.src + 1 << " - V" << next_edge.dest + 1
                 << "  вес: " << next_edge.weight << endl;
            totalWeight += next_edge.weight;
            result.push_back({{next_edge.src+1, next_edge.dest+1}, next_edge.weight});
            Union(subsets, x, y);
            e++;
        }
    }
    cout << "Суммарный вес: " << totalWeight << endl << endl;
    return result;
}
int main() {
    vector<vector<int>> graph = {
        {0, 2, 0, 8,10, 0, 0, 0}, // V1
        {2, 0, 3, 0, 5, 0, 0, 0}, // V2
        {0, 3, 0, 0,12, 0, 0, 7}, // V3
        {8, 0, 0, 0,14, 3,11, 0}, // V4
        {10,5,12,14,0, 0, 4, 8},  // V5
        {0, 0, 0, 3, 0, 0, 6, 0}, // V6
        {0, 0, 0,11, 4, 6, 0, 9}, // V7
        {0, 0, 7, 0, 8, 0, 9, 0}  // V8
    };

    int weightPrima, weightKruskal;
    auto mstPrima = Prima(graph, weightPrima);
    auto mstKruskal = Kruskal(graph, weightKruskal);

    cout << "Прима: ";
    for (auto &edge : mstPrima)
        cout << "(V" << edge.first.first << "-V" << edge.first.second << ":" << edge.second << ") ";
    cout << " | Вес = " << weightPrima << endl;

    cout << "Краскал: ";
    for (auto &edge : mstKruskal)
        cout << "(V" << edge.first.first << "-V" << edge.first.second << ":" << edge.second << ") ";
    cout << " | Вес = " << weightKruskal << endl;

    return 0;
}
