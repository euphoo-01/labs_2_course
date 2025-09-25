#include <iostream>
#include <vector>
#include <iomanip>
#include <limits>
using namespace std;

const int INF = numeric_limits<int>::max();

vector<int> restorePath(int u, int v, const vector<vector<int> > &S) {
    if (S[u][v] == -1) return {};
    vector<int> path;
    path.push_back(u);
    while (u != v) {
        u = S[u][v];
        path.push_back(u);
    }
    return path;
}

int main() {
    vector<vector<int> > C = {
        {0, 28, 21, 59, 12, 27},
        {7, 0, 24, INF, 21, 9},
        {9, 32, 0, 13, 11, INF},
        {8, INF, 5, 0, 16, INF},
        {14, 13, 15, 10, 0, 22},
        {15, 18, INF, INF, 6, 0}
    };

    int n = C.size();

    vector<vector<int> > D = C;

    vector<vector<int> > S(n, vector<int>(n, -1));
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            if (C[i][j] != INF && i != j) {
                S[i][j] = j;
            }
        }
    }


    for (int m = 0; m < n; m++) {
        for (int i = 0; i < n; i++) {
            for (int j = 0; j < n; j++) {
                if (D[i][m] < INF && D[m][j] < INF && D[i][m] + D[m][j] < D[i][j]) {
                    D[i][j] = D[i][m] + D[m][j];
                    S[i][j] = S[i][m];
                }
            }
        }
    }

    cout << "\nМатрица D:\n";
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            if (D[i][j] == INF)
                cout << setw(7) << "INF";
            else
                cout << setw(7) << D[i][j];
        }
        cout << "\n";
    }

    cout << "\nМатрица S:\n";
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            cout << setw(7) << S[i][j] + 1;
        }
        cout << "\n";
    }

    cout << "\nКратчайшие пути между всеми парами вершин:\n";
    for (int i = 0; i < n; i++) {
        for (int j = 0; j < n; j++) {
            if (i == j) continue;
            auto path = restorePath(i, j, S);
            if (path.empty()) {
                cout << "Путь " << i << " -> " << j << ": нет пути\n";
            } else {
                cout << "Путь " << i << " -> " << j << " (длина " << D[i][j] << "): ";
                for (int k = 0; k < path.size(); k++) {
                    cout << path[k];
                    if (k + 1 < path.size()) cout << " -> ";
                }
                cout << "\n";
            }
        }
    }

    return 0;
}
