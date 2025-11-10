#include <iostream>
#include <vector>
#include <map>

using namespace std;

typedef struct _product {
    string name = "безымянный";
    int weight = 0;
    int cost = 0;
} Product;

int main() {
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
    for (const auto &product_name : selected_items) {
        for (const auto &product : products) {
            if (product.name == product_name) {
                total_weight += product.weight;
                break;
            }
        }
    }
    cout << "Вес: " << total_weight << endl;

    cout << "\nТовары:" << endl;
    for (const auto &[product_name, count] : product_count) {
        cout << product_name << ": " << count << " шт." << endl;
    }

    return 0;
}