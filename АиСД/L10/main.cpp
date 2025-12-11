#include <iostream>
#include <vector>
#include <cmath>
#include <cstdlib>
#include <ctime>
#include <limits>
#include <iomanip>
#include <random>

using namespace std;

const double EVAPORATION_RATE = 0.5;
const double Q_VAL = 100.0;          // константа для обновления феромонов

struct Ant {
    vector<int> tour;
    vector<bool> visited;
    double tour_length;

    Ant(int n) : visited(n, false), tour_length(0.0) {
        tour.reserve(n);
    }

    void reset(int n) {
        tour.clear();
        visited.assign(n, false);
        tour_length = 0.0;
    }
};

class AntColony {
private:
    int n_cities;
    double init_pheromone;
    double alpha;
    double beta;
    int max_iterations;

    vector<vector<double>> dist_matrix;
    vector<vector<double>> pheromones;

    vector<int> best_global_tour;
    double best_global_length;

    // генератор случайных чисел
    mt19937 rng;

public:
    AntColony(int n, double pher, double a, double b, int iter)
        : n_cities(n), init_pheromone(pher), alpha(a), beta(b), max_iterations(iter) {

        best_global_length = numeric_limits<double>::max();
        rng.seed(static_cast<unsigned int>(time(nullptr)));

        dist_matrix.resize(n, vector<double>(n));
        pheromones.resize(n, vector<double>(n, init_pheromone));
    }

    void generateDistances() {
        // симметричные значения
        for (int i = 0; i < n_cities; ++i) {
            for (int j = i + 1; j < n_cities; ++j) {
                double d = 1.0 + (rng() % 100); // расстояние от 1 до 100
                dist_matrix[i][j] = dist_matrix[j][i] = d;
            }
            dist_matrix[i][i] = 0.0;
        }
    }

    // выбор следующего города на основе рулетки
    int selectNextCity(int current, const vector<bool>& visited) {
        vector<double> probs(n_cities, 0.0);
        double sum_probs = 0.0;

        for (int i = 0; i < n_cities; ++i) {
            if (!visited[i]) {
                double tau = pow(pheromones[current][i], alpha);
                double eta = pow(1.0 / dist_matrix[current][i], beta);
                probs[i] = tau * eta;
                sum_probs += probs[i];
            }
        }

        if (sum_probs == 0.0) return -1;

        uniform_real_distribution<double> dist(0.0, sum_probs);
        double r = dist(rng);

        double current_slice = 0.0;
        for (int i = 0; i < n_cities; ++i) {
            if (!visited[i]) {
                current_slice += probs[i];
                if (r <= current_slice) {
                    return i;
                }
            }
        }

        // возврат последнего доступного города как fallback
        for (int i = 0; i < n_cities; ++i) {
            if (!visited[i]) return i;
        }
        return -1;
    }

    void run() {
        vector<Ant> ants(n_cities, Ant(n_cities));

        for (int iter = 1; iter <= max_iterations; ++iter) {

            // сброс состояния муравьев перед каждой итерацией
            for (int i = 0; i < n_cities; ++i) {
                ants[i].reset(n_cities);
                int start_city = rng() % n_cities;
                ants[i].tour.push_back(start_city);
                ants[i].visited[start_city] = true;
            }

            // построение маршрутов для всех муравьев
            for (int step = 0; step < n_cities - 1; ++step) {
                for (auto& ant : ants) {
                    int current = ant.tour.back();
                    int next = selectNextCity(current, ant.visited);
                    if (next != -1) {
                        ant.tour.push_back(next);
                        ant.visited[next] = true;
                        ant.tour_length += dist_matrix[current][next];
                    }
                }
            }

            // замыкание маршрута и обновление лучшего результата
            for (auto& ant : ants) {
                if (ant.tour.size() == n_cities) {
                    ant.tour_length += dist_matrix[ant.tour.back()][ant.tour.front()];

                    if (ant.tour_length < best_global_length) {
                        best_global_length = ant.tour_length;
                        best_global_tour = ant.tour;
                    }
                }
            }

            // испарение феромонов
            for (int i = 0; i < n_cities; ++i) {
                for (int j = 0; j < n_cities; ++j) {
                    pheromones[i][j] *= (1.0 - EVAPORATION_RATE);
                }
            }

            // обновление феромонов
            for (const auto& ant : ants) {
                double contribution = Q_VAL / ant.tour_length;
                for (size_t k = 0; k < ant.tour.size(); ++k) {
                    int from = ant.tour[k];
                    int to = ant.tour[(k + 1) % ant.tour.size()];
                    pheromones[from][to] += contribution;
                    pheromones[to][from] += contribution;
                }
            }

            printIterInfo(iter);
        }
    }

    void printIterInfo(int iter) {
        cout << "Номер итерации: " << iter << endl;
        cout << "Лучший маршрут: ";
        for (int city : best_global_tour) {
            cout << city << " -> ";
        }
        cout << best_global_tour[0]; // возврат в начало
        cout << endl;
        cout << "Расстояние: " << fixed << setprecision(2) << best_global_length << endl;
        cout << "-----------------------------" << endl;
    }
};

int main() {
    setlocale(LC_ALL, "");

    int n, iter;
    double ph, a, b;

    cout << "Введите количество городов (N): ";
    if (!(cin >> n) || n < 2) {
        cerr << "Ошибка: N должно быть >= 2." << endl;
        return 1;
    }

    cout << "Введите начальное значение феромонов: ";
    cin >> ph;
    cout << "Введите значение альфа: ";
    cin >> a;
    cout << "Введите значение бета: ";
    cin >> b;
    cout << "Введите количество итераций: ";
    cin >> iter;

    AntColony ac(n, ph, a, b, iter);

    ac.generateDistances();
    ac.run();

    return 0;
}