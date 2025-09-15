#include <iostream>
#include <queue>
#include <vector>
using namespace std;

const int V = 10;

void BFS(vector<vector<int> > &adj_list, int start) {
    vector<bool> visited(V + 1, false);
    queue<int> q;

    visited[start] = true;
    q.push(start);

    cout << "BFS: ";
    while (!q.empty()) {
        int u = q.front();
        q.pop();
        cout << u << " ";

        for (int w: adj_list[u]) {
            if (!visited[w]) {
                visited[w] = true;
                q.push(w);
            }
        }
    }
}

void DFS(vector<vector<int> > &adj_list, int v, vector<bool> &visited) {
    visited[v] = true;
    cout << v << ' ';

    for (int w: adj_list[v]) {
        if (!visited[w]) {
            DFS(adj_list, w, visited);
        }
    }
}


int main() {
    vector<pair<int, int> > edges = {
        {1, 2}, {1, 5}, {2, 7}, {2, 8}, {3, 8},
        {4, 6}, {4, 9}, {5, 6}, {6, 9}, {7, 8}, {9, 10}
    };
    cout << "Список ребер: " << endl;
    for (pair<int, int> e: edges) {
        cout << '(' << e.first << ',' << e.second << ')' << endl;
    }

    vector<vector<int> > adj_list(V + 1);
    for (pair<int, int> e: edges) {
        adj_list[e.first].push_back(e.second);
        adj_list[e.second].push_back(e.first);
    }

    cout << "Список смежности: " << endl;
    for (int i = 1; i <= V; i++) {
        cout << i << " : ";
        for (int j: adj_list[i]) {
            cout << j << ' ';
        }
        cout << endl;
    }
    cout << endl;
    vector<vector<int> > adj_matrix(V + 1, vector<int>(V + 1, 0));
    for (pair<int, int> e: edges) {
        adj_matrix[e.first][e.second] = 1;
        adj_matrix[e.second][e.first] = 1;
    }

    cout << "Матрица смежности:\n";
    for (int i = 1; i <= V; i++) {
        for (int j = 1; j <= V; j++) {
            cout << adj_matrix[i][j] << " ";
        }
        cout << endl;
    }

    BFS(adj_list, 1);

    cout << endl << "DFS: ";
    vector<bool> visited(V + 1, false);
    DFS(adj_list, 1, visited);
    cout << endl;

    return 0;
}
