#include <csignal>
#include <cstring>
#include <iostream>
#include <unistd.h>
#include <sys/socket.h>
#include <arpa/inet.h>
#include <vector>
#include <list>
#include <tuple>

const int BUFSIZE = 1024;
const int PORT = 1288;
const char *ADDR = "127.0.0.1";
int running = 1;
void *client_worker(void *args);

enum PRODUCTS_INFO {
    COUNTRY = 0,
    MANUFACTURER = 1,
    PRODUCT_NAME = 2,
    COUNT = 3,
};

std::list<std::tuple<char*,char*,char*,int>> products;

void signal_handler(int sig) {
    running = 0;
}

int main() {
    signal(SIGINT, signal_handler);

    const std::tuple<char*,char*,char*,int> el1 = {"Belarus", "Белагропром","Молоко", 100};
    const std::tuple<char*,char*,char*,int> el2 = {"Germany", "Mercedes-Benz Motors","Турбина", 5};
    const std::tuple<char*,char*,char*,int> el3 = {"Germany", "Karcher Inc.","Пылесос", 23};
    const std::tuple<char*,char*,char*,int> el4 = {"Belarus", "Белагропром","Картошка", 2500};
    const std::tuple<char*,char*,char*,int> el5 = {"Germany", "Karcher Inc.","Танк", 100};
    const std::tuple<char*,char*,char*,int> el6 = {"Belarus", "Шиман Дмитрий Васильевич","Волосы", 2};

    products.push_back(el1);
    products.push_back(el2);
    products.push_back(el3);
    products.push_back(el4);
    products.push_back(el5);
    products.push_back(el6);
    int lead_socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
    if (lead_socket == -1) {
        std::cerr << "Ошибка создания сокета!" << std::endl;
        close(lead_socket);
        return 1;
    }

    int option_value = 1;
    if (setsockopt(lead_socket, SOL_SOCKET, SO_REUSEADDR, &option_value, sizeof(option_value)) == -1) {
        std::cerr << "Ошибка повторного использования адреса сервера!" << std::endl;
        close(lead_socket);
        return 1;
    }

    sockaddr_in address;
    address.sin_family = AF_INET;
    address.sin_port = htons(PORT);
    inet_pton(AF_INET, ADDR, &address.sin_addr);

    if (bind(lead_socket, (struct sockaddr *) &address, sizeof(address)) == -1) {
        std::cerr << "Ошибка назначения домена!" << std::endl;
    }
    if (listen(lead_socket, 5) == 0) {
        std::cout << "Прослушиваем " << ADDR << ":" << PORT << std::endl;
    };



    while (running) {
        int client_socket = accept(lead_socket, NULL, NULL);
        if (client_socket != -1) {
            std::cout << "Установлено соединение" << std::endl;
        }
        int *cl_socket_ptr = new int(client_socket);

        pthread_t newThread;
        if (pthread_create(&newThread, NULL, client_worker, cl_socket_ptr) != 0) {
            std::cerr << "Ошибка создания потока для сокета " << client_socket << std::endl;
            close(client_socket);
            delete cl_socket_ptr;
            continue;
        }
        pthread_join(newThread, NULL);

    }
    return 0;
}

void *client_worker(void *args) {
    int client_socket = *(int *) args;
    delete (int *) args;

    char buffer[BUFSIZE];
    ssize_t data_l = recv(client_socket, buffer, BUFSIZE, 0);
    if (data_l == -1) {
        std::cerr << "Не удалось получить сообщение от сокета " << client_socket << "!" << std::endl;
    } else {
        buffer[data_l] = '\0';
        std::cout << "Клиент " << client_socket << ": " << buffer << std::endl;

        char* response = new char[BUFSIZE];
        response[0] = '\0';  // обнуляем строку

        bool found = false;

        for (auto product : products) {
            char* country      = std::get<COUNTRY>(product);
            char* manufacturer = std::get<MANUFACTURER>(product);
            char* product_name = std::get<PRODUCT_NAME>(product);
            int count          = std::get<COUNT>(product);

            if (strcmp(buffer, country) == 0) {   // ✅ фильтрация по стране
                char productInfo[BUFSIZE];
                snprintf(productInfo, BUFSIZE,
                         "СТРАНА: %s. ИМЯ: %s. ПРОИЗВЕЛ: %s. КОЛ-ВО: %d\n",
                         country, product_name, manufacturer, count);

                if (strlen(response) + strlen(productInfo) < BUFSIZE - 1) {
                    strcat(response, productInfo);
                }
                found = true;
            }
        }

        if (!found) {
            snprintf(response, BUFSIZE, "Нет данных по стране: %s\n", buffer);
        }

        if (send(client_socket, response, strlen(response), 0) != -1) {
            std::cout << "Ответ сокету " << client_socket << " отправлен успешно." << std::endl;
        } else {
            std::cerr << "Ошибка отправки сообщения сокету " << client_socket << "!" << std::endl;
        }

        delete[] response;
    }
    close(client_socket);
    return nullptr;
}

