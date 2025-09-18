#include <iostream>
#include <cmath>
#include <limits>
const int INF = std::numeric_limits<int>::max();
const int N = 9;

enum peaks {
    A = 0,
    B = 1,
    C = 2,
    D = 3,
    E = 4,
    F = 5,
    G = 6,
    H = 7,
    I = 8,
};

void init_graph(int **adj_matrix);
int* dijkstra(int **adj_matrix, int N, int start_peak);
int min_element_idx(int* dist, bool* visited, int N);
char peak_to_char(int idx);
int char_to_peak(char peak);

int main() {
    int** adj_matrix = new int *[N];
    for (int i = 0; i < N; i++) {
        adj_matrix[i] = new int[N];
        for (int j = 0; j < N; j++) {
            adj_matrix[i][j] = 0;
        }
    }
    init_graph(adj_matrix);

    char start_peak;
    std::cout << "Введите название вершины (A-I): ";
    std::cin >> start_peak;

    int* cost_of_path = dijkstra(adj_matrix, N, char_to_peak(start_peak));

    std::cout << "Минимальные расстояния от вершины " << start_peak << ":\n";
    for (int i = A; i < N; i++) {
        if (cost_of_path[i] == INF)
            std::cout << i << ": INF\n";
        else
            std::cout << peak_to_char(i) << ": " << cost_of_path[i] << "\n";
    }

    delete[] cost_of_path;
    for (int i = 0; i < N; i++) {
        delete[] adj_matrix[i];
    }
    delete[] adj_matrix;

    return 0;
}

int* dijkstra(int **adj_matrix, int N, int start_peak) {
    int* dist = new int[N];
    bool* visited = new bool[N];

    for (int i = 0; i < N; i++) {
        dist[i] = INF;
        visited[i] = false;
    }
    dist[start_peak] = 0;

    for (int i = 0; i < N - 1; i++) {
        int u = min_element_idx(dist, visited, N);
        if (u == -1) break;

        visited[u] = true;

        for (int v = 0; v < N; v++) {
            if (adj_matrix[u][v] > 0 && !visited[v] && dist[u] != INF &&
                dist[u] + adj_matrix[u][v] < dist[v]) {
                dist[v] = dist[u] + adj_matrix[u][v];
            }
        }
    }

    delete[] visited;
    return dist;
}
char peak_to_char(int idx) {
    return 'A' + idx;
}
int char_to_peak(char peak) {
    peak = toupper(peak);
    if (peak < 'A' || peak > 'I') return -1;
    return peak - 'A';
}
void init_graph(int **adj_matrix) {
    adj_matrix[A][B] = adj_matrix[B][A] = 7;
    adj_matrix[A][C] = adj_matrix[C][A] = 10;
    adj_matrix[B][G] = adj_matrix[G][B] = 27;
    adj_matrix[B][F] = adj_matrix[F][B] = 9;
    adj_matrix[C][F] = adj_matrix[F][C] = 8;
    adj_matrix[G][I] = adj_matrix[I][G] = 15;
    adj_matrix[I][H] = adj_matrix[H][I] = 15;
    adj_matrix[H][D] = adj_matrix[D][H] = 17;
    adj_matrix[D][I] = adj_matrix[I][D] = 21;
    adj_matrix[F][H] = adj_matrix[H][F] = 11;
    adj_matrix[C][E] = adj_matrix[E][C] = 31;
    adj_matrix[E][D] = adj_matrix[D][E] = 32;
}
int min_element_idx(int* dist, bool* visited, int N) {
    int min_val = INF, min_idx = -1;
    for (int i = 0; i < N; i++) {
        if (!visited[i] && dist[i] < min_val) {
            min_val = dist[i];
            min_idx = i;
        }
    }
    return min_idx;
}