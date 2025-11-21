#include <iostream>
#include <vector>
#include <algorithm>
#include <ctime>
#include <utility>

const int NUMBER_OF_CITIES = 8;

std::vector<std::vector<int>> distance_matrix = {
    {0, 29, 20, 999, 16, 31, 100, 12},
    {25, 0, 15, 29, 999, 40, 72, 21},
    {18, 13, 0, 15, 14, 25, 999, 9},
    {21, 999, 17, 0, 4, 12, 90, 17},
    {19, 28, 11, 2, 0, 16, 94, 999},
    {33, 42, 22, 10, 18, 0, 95, 24},
    {999, 70, 81, 93, 91, 92, 0, 90},
    {14, 23, 7, 999, 9, 26, 88, 0}
};

struct Route {
    std::vector<int> ribbs;
    int total_distance = 0;
};


void print_route(const Route& route) {
    for (int city_index : route.ribbs) {
        std::cout << city_index << " -> ";
    }
    std::cout << route.ribbs[0] << " | Длина маршрута: " << route.total_distance << std::endl;
}

void calculate_distance(Route& route) {
    route.total_distance = 0;
    for (size_t i = 0; i < route.ribbs.size() - 1; ++i) {
        route.total_distance += distance_matrix[route.ribbs[i]][route.ribbs[i+1]];
    }
    route.total_distance += distance_matrix[route.ribbs.back()][route.ribbs.front()];
}

std::vector<Route> first_population(int population_size) {
    std::vector<Route> population;
    
    std::vector<int> base_path;
    for (int i = 0; i < NUMBER_OF_CITIES; ++i) {
        base_path.push_back(i);
    }

    for (int i = 0; i < population_size; ++i) {
        Route new_route;
        new_route.ribbs = base_path;
        std::random_shuffle(new_route.ribbs.begin() + 1, new_route.ribbs.end());
        calculate_distance(new_route);
        population.push_back(new_route);
    }
    return population;
}

Route selection(const std::vector<Route>& population, int selection_batch) {
    Route best;
    best.total_distance = -1;

    for (int i = 0; i < selection_batch; ++i) {
        const auto& candidate = population[rand() % population.size()];
        if (best.total_distance == -1 || candidate.total_distance < best.total_distance) {
            best = candidate;
        }
    }
    return best;
}
std::pair<Route, Route> crossover(const Route& parent1, const Route& parent2) {
    Route child1, child2;
    child1.ribbs.resize(NUMBER_OF_CITIES, -1);
    child2.ribbs.resize(NUMBER_OF_CITIES, -1);
    
    int start = rand() % NUMBER_OF_CITIES;
    int end = rand() % NUMBER_OF_CITIES;
    if (start > end) std::swap(start, end);

    std::vector<bool> in_child1(NUMBER_OF_CITIES + 1, false);
    for (int i = start; i <= end; ++i) {
        child1.ribbs[i] = parent1.ribbs[i];
        in_child1[child1.ribbs[i]] = true;
    }

    int current_pos = 0;
    for (int i = 0; i < NUMBER_OF_CITIES; ++i) {
        if (current_pos == start) current_pos = end + 1;
        int city = parent2.ribbs[i];
        if (!in_child1[city]) {
            child1.ribbs[current_pos++] = city;
        }
    }
    
    std::vector<bool> in_child2(NUMBER_OF_CITIES + 1, false);
    for (int i = start; i <= end; ++i) {
        child2.ribbs[i] = parent2.ribbs[i];
        in_child2[child2.ribbs[i]] = true;
    }
    
    current_pos = 0;
    for (int i = 0; i < NUMBER_OF_CITIES; ++i) {
        if (current_pos == start) current_pos = end + 1;
        int city = parent1.ribbs[i];
        if (!in_child2[city]) {
            child2.ribbs[current_pos++] = city;
        }
    }

    return {child1, child2};
}

void mutate(Route& route, float mutation_rate) {
    float r = static_cast<float>(rand()) / static_cast<float>(RAND_MAX);
    if (r < mutation_rate) {
        int pos1 = rand() % NUMBER_OF_CITIES;
        int pos2 = rand() % NUMBER_OF_CITIES;
        std::swap(route.ribbs[pos1], route.ribbs[pos2]);
    }
}


int main() {
    srand(time(0));

    int population_size;
    float mutation_rate;
    int num_generations;
    int selection_batch = 5;

    std::cout << "Введите размер популяции: ";
    std::cin >> population_size;
    std::cout << "Введите коэффициент мутации: ";
    std::cin >> mutation_rate;
    std::cout << "Введите количество итераций: ";
    std::cin >> num_generations;
    std::cout << "----------------------------------------------------" << std::endl;

    auto population = first_population(population_size);
    Route best_route_overall;
    best_route_overall.total_distance = -1;

    for (int generation_number = 0; generation_number < num_generations; ++generation_number) {
        std::sort(population.begin(), population.end(), [](const Route& a, const Route& b) {
            return a.total_distance < b.total_distance;
        });

        if (best_route_overall.total_distance == -1 || population.front().total_distance < best_route_overall.total_distance) {
            best_route_overall = population.front();
        }
        
        std::cout << "\nИтерация " << generation_number + 1 << " | ";
        std::cout << "Просто лучший: " << best_route_overall.total_distance << std::endl;
        
        std::vector<Route> new_population;
        new_population.push_back(best_route_overall);

        while (new_population.size() < population_size) {
            Route parent1 = selection(population, selection_batch);
            Route parent2 = selection(population, selection_batch);
            auto children = crossover(parent1, parent2);
            mutate(children.first, mutation_rate);
            mutate(children.second, mutation_rate);
            calculate_distance(children.first);
            calculate_distance(children.second);
            
            new_population.push_back(children.first);
            if (new_population.size() < population_size) {
                new_population.push_back(children.second);
            }
        }
        population = new_population;
    }

    std::cout << "Лучший маршрут, после " << num_generations << " итераций:" << std::endl;
    print_route(best_route_overall);

    return 0;
}