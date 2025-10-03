#include <csignal>
#include <cstring>
#include <iostream>
#include <unistd.h>
#include <sys/socket.h>
#include <arpa/inet.h>
#include <vector>
#include <list>
#include <tuple>
#include <pthread.h>

const int BUFSIZE = 4096;
const int PORT = 1288;
const char *ADDR = "127.0.0.1";

#define FILTER_REQ "FILTER:"
#define VIEW_REQ "VIEW:"
#define NEWREC_REQ "NEW:"
#define DELREC_REQ "DEL:"
#define UPDREC_REQ "UPD:"

int running = 1;

enum PRODUCTS_INFO {
    COUNTRY = 0,
    MANUFACTURER = 1,
    PRODUCT_NAME = 2,
    COUNT = 3,
};

std::list<std::tuple<std::string, std::string, std::string, int>> products;
pthread_mutex_t products_mutex = PTHREAD_MUTEX_INITIALIZER;

void signal_handler(int sig) {
    running = 0;
}

void *handle_client(void *arg) {
    int client_socket = *(int*)arg;
    free(arg);
    char buffer[BUFSIZE];
    while (true) {
        ssize_t data_l = recv(client_socket, buffer, BUFSIZE, 0);
        if (data_l <= 0) {
            std::cout << "Клиент отключился" << std::endl;
            break;
        }

        buffer[data_l] = '\0';
        std::string request(buffer);
        std::cout << "Получено: " << request << std::endl;

        std::string response;

        pthread_mutex_lock(&products_mutex);
        if (request.rfind(FILTER_REQ, 0) == 0) {
            std::string country = request.substr(strlen(FILTER_REQ));
            for (auto &p : products) {
                if (std::get<COUNTRY>(p) == country) {
                    response += "СТРАНА: " + std::get<COUNTRY>(p) +
                                ". ПРОДУКТ: " + std::get<PRODUCT_NAME>(p) +
                                ". ПРОИЗВОДИТЕЛЬ: " + std::get<MANUFACTURER>(p) +
                                ". КОЛ-ВО: " + std::to_string(std::get<COUNT>(p)) + "\n";
                }
            }
            if (response.empty()) {
                response = "Нет записей для страны: " + country;
            }
        }
        else if (request.rfind(VIEW_REQ, 0) == 0) {
            for (auto &p : products) {
                response += "СТРАНА: " + std::get<COUNTRY>(p) +
                            ". ПРОДУКТ: " + std::get<PRODUCT_NAME>(p) +
                            ". ПРОИЗВОДИТЕЛЬ: " + std::get<MANUFACTURER>(p) +
                            ". КОЛ-ВО: " + std::to_string(std::get<COUNT>(p)) + "\n";
            }
            if (response.empty()) response = "База пуста";
        }
        else if (request.rfind(NEWREC_REQ, 0) == 0) {
            std::string args = request.substr(strlen(NEWREC_REQ));
            size_t p1 = args.find(',');
            size_t p2 = args.find(',', p1+1);
            size_t p3 = args.find(',', p2+1);

            if (p1!=std::string::npos && p2!=std::string::npos && p3!=std::string::npos) {
                std::string country = args.substr(0, p1);
                std::string manufacturer = args.substr(p1+1, p2-p1-1);
                std::string product = args.substr(p2+1, p3-p2-1);
                int count = std::stoi(args.substr(p3+1));
                products.push_back({country, manufacturer, product, count});
                response = "Новая запись добавлена";
            } else {
                response = "Ошибка: неверный формат NEW";
            }
        }
        else if (request.rfind(DELREC_REQ, 0) == 0) {
            std::string product = request.substr(strlen(DELREC_REQ));
            bool removed = false;
            for (auto it = products.begin(); it != products.end(); ) {
                if (std::get<PRODUCT_NAME>(*it) == product) {
                    it = products.erase(it);
                    removed = true;
                } else {
                    ++it;
                }
            }
            response = removed ? "Запись удалена" : "Не найден продукт: " + product;
        }
        else if (request.rfind(UPDREC_REQ, 0) == 0) {
            std::string args = request.substr(strlen(UPDREC_REQ));
            size_t p1 = args.find(',');
            size_t p2 = args.find(',', p1+1);
            size_t p3 = args.find(',', p2+1);
            size_t p4 = args.find(',', p3+1);

            if (p1!=std::string::npos && p2!=std::string::npos &&
                p3!=std::string::npos && p4!=std::string::npos) {
                std::string old_product = args.substr(0, p1);
                std::string country = args.substr(p1+1, p2-p1-1);
                std::string manufacturer = args.substr(p2+1, p3-p2-1);
                std::string new_product = args.substr(p3+1, p4-p3-1);
                int count = std::stoi(args.substr(p4+1));

                bool updated = false;
                for (auto &p : products) {
                    if (std::get<PRODUCT_NAME>(p) == old_product) {
                        p = {country, manufacturer, new_product, count};
                        updated = true;
                        break;
                    }
                }
                response = updated ? "Запись обновлена" : "Не найден продукт: " + old_product;
            } else {
                response = "Ошибка: неверный формат UPD";
            }
        }
        else {
            response = "Неизвестная команда";
        }
        pthread_mutex_unlock(&products_mutex);

        send(client_socket, response.c_str(), response.size(), 0);
    }

    close(client_socket);
    return nullptr;
}

int main() {
    signal(SIGINT, signal_handler);

    products.push_back({"Belarus", "Белагропром", "Молоко", 100});
    products.push_back({"Germany", "Mercedes-Benz Motors", "Турбина", 5});
    products.push_back({"Germany", "Karcher Inc.", "Пылесос", 23});
    products.push_back({"Belarus", "Белагропром", "Картошка", 2500});
    products.push_back({"Germany", "Karcher Inc.", "Танк", 100});
    products.push_back({"Belarus", "Шиман Дмитрий Васильевич", "Волосы", 2});

    int lead_socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
    if (lead_socket == -1) {
        std::cerr << "Ошибка создания сокета!" << std::endl;
        return 1;
    }

    int option_value = 1;
    setsockopt(lead_socket, SOL_SOCKET, SO_REUSEADDR, &option_value, sizeof(option_value));

    sockaddr_in address;
    address.sin_family = AF_INET;
    address.sin_port = htons(PORT);
    inet_pton(AF_INET, ADDR, &address.sin_addr);

    if (bind(lead_socket, (struct sockaddr *) &address, sizeof(address)) == -1) {
        std::cerr << "Ошибка bind!" << std::endl;
        close(lead_socket);
        return 1;
    }

    if (listen(lead_socket, 5) == -1) {
        std::cerr << "Ошибка listen!" << std::endl;
        close(lead_socket);
        return 1;
    }

    std::cout << "Сервер слушает " << ADDR << ":" << PORT << std::endl;

    while (running) {
        int client_socket = accept(lead_socket, nullptr, nullptr);
        if (client_socket == -1) {
            if (running) {
                std::cerr << "Ошибка accept!" << std::endl;
            }
            continue;
        }
y
        std::cout << "Клиент подключился" << std::endl;

        int *client_sock_ptr = new int(client_socket);

        pthread_t thread_id;
        if (pthread_create(&thread_id, nullptr, handle_client, client_sock_ptr) != 0) {
            std::cerr << "Ошибка создания потока!" << std::endl;
            delete client_sock_ptr;
            close(client_socket);
            continue;
        }

        pthread_detach(thread_id);
    }

    close(lead_socket);
    pthread_mutex_destroy(&products_mutex);
    return 0;
}