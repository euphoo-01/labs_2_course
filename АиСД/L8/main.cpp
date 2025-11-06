#include <iostream>
#include <vector>
#include <map>

using namespace std;

typedef struct _product {
    string name = "undefined";
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
    vector<vector<string> > selected(N + 1);

    for (int w = 1; w <= N; w++) {
        for (int i = 0; i < products.size(); i++) {
            if (products[i].weight <= w) {
                if (dp[w] < dp[w - products[i].weight] + products[i].cost) {
                    dp[w] = dp[w - products[i].weight] + products[i].cost;
                    selected[w] = selected[w - products[i].weight];
                    selected[w].push_back(products[i].name);
                }
            }
        }
    }

    int max_cost = dp[N];
    cout << "Максимальная стоимость: " << max_cost << endl;

    map<string, int> product_count;
    for (const auto &product_name: selected[N]) {
        product_count[product_name]++;
    }

    cout << "Всего товаров: " << selected[N].size() << endl;

    int total_weight = 0;
    for (const auto &product_name: selected[N]) {
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


    return 0;
}
