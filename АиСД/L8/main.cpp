#include <iostream>
#include <vector>
#include <map>
// #define WITH_REPEAT

using namespace std;

typedef struct _product {
    string name = "безымянный";
    int weight = 0;
    int cost = 0;
} Product;

int main() {
#ifdef WITH_REPEAT
    string name;
    int weight, cost;

    vector<Product> products;
    while (true) {
        cout << "Введите название товара (q для завершения): ";
        cin >> name;
        if (name == "q") break;
        cout << "Введите вес: ";
        cin >> weight;
        cout << "Введите цену: ";
        cin >> cost;

        Product product;
        product.name = name;
        product.weight = weight;
        product.cost = cost;
        products.push_back(product);
    }

    int N;
    cout << endl << "Введите вместимость рюкзака: ";
    cin >> N;

    vector<int> dp(N + 1, 0);

    for (int i = 0; i < products.size(); i++) {
        for (int w = products[i].weight; w <= N; w++) {
            if (dp[w] < dp[w - products[i].weight] + products[i].cost) {
                dp[w] = dp[w - products[i].weight] + products[i].cost;
            }
        }
    }

    int max_cost = dp[N];
    cout << "Максимальная стоимость: " << max_cost << endl;

    vector<string> selected_items;
    map<string, int> product_count;

    int remaining_weight = N;
    while (remaining_weight > 0) {
        bool found = false;

        for (int i = 0; i < products.size(); i++) {
            if (remaining_weight >= products[i].weight &&
                dp[remaining_weight] == dp[remaining_weight - products[i].weight] + products[i].cost) {
                selected_items.push_back(products[i].name);
                product_count[products[i].name]++;
                remaining_weight -= products[i].weight;
                found = true;
                break;
            }
        }

        if (!found) break;
    }

    cout << "Всего товаров: " << selected_items.size() << endl;

    int total_weight = 0;
    for (const auto &product_name: selected_items) {
        for (const auto &product: products) {
            if (product.name == product_name) {
                total_weight += product.weight;
                break;
            }
        }
    }
    cout << "Вес: " << total_weight << endl;

    cout << "\nТовары:" << endl;
    for (const auto &[product_name, count]: product_count) {
        cout << product_name << ": " << count << " шт." << endl;
    }

#endif

#ifndef WITH_REPEAT
    string name;
    int weight, cost;

    vector<Product> products;
    while (true) {
        cout << "Введите название товара (q для завершения): ";
        cin >> name;
        if (name == "q") break;
        cout << "Введите вес: ";
        cin >> weight;
        cout << "Введите цену: ";
        cin >> cost;

        Product product;
        product.name = name;
        product.weight = weight;
        product.cost = cost;
        products.push_back(product);
    }

    int N;
    cout << endl << "Введите вместимость рюкзака: ";
    cin >> N;

    int n = products.size();
    vector<vector<int> > dp(n + 1, vector<int>(N + 1, 0));

    for (int i = 1; i <= n; i++) {
        for (int w = 1; w <= N; w++) {
            int current_weight = products[i - 1].weight;
            int current_cost = products[i - 1].cost;

            if (current_weight <= w) {
                dp[i][w] = max(dp[i - 1][w],
                               dp[i - 1][w - current_weight] + current_cost);
            } else {
                dp[i][w] = dp[i - 1][w];
            }
        }
    }

    int max_cost = dp[n][N];
    cout << "Максимальная стоимость: " << max_cost << endl;

    vector<string> selected_items;
    int remaining_weight = N;

    for (int i = n; i > 0 && remaining_weight > 0; i--) {
        if (dp[i][remaining_weight] != dp[i - 1][remaining_weight]) {
            selected_items.push_back(products[i - 1].name);
            remaining_weight -= products[i - 1].weight;
        }
    }

    cout << "Всего товаров: " << selected_items.size() << endl;

    int total_weight = 0;
    for (const auto &product_name: selected_items) {
        for (const auto &product: products) {
            if (product.name == product_name) {
                total_weight += product.weight;
                break;
            }
        }
    }
    cout << "Вес: " << total_weight << endl;

    cout << "\nТовары:" << endl;
    map<string, int> product_count;
    for (const auto &product_name: selected_items) {
        product_count[product_name]++;
        cout << product_name << endl;
    }

#endif

    return 0;
}
