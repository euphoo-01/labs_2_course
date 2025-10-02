#include <algorithm>
#include <cstring>
#include <sys/socket.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <netinet/in.h>
#include <iostream>
#include <tuple>
#include <limits>

#define BUFSIZE 4096
#define PORT 1288
#define ADDR "127.0.0.1"

#define FILTER_REQ "FILTER:"
#define VIEW_REQ "VIEW:"
#define NEWREC_REQ "NEW:"
#define DELREC_REQ "DEL:"
#define UPDREC_REQ "UPD:"

using namespace std;

int main() {
    int client_socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
    if (client_socket == -1) {
        cerr << "Ошибка создания сокета." << endl;
        return 1;
    }

    sockaddr_in address{};
    address.sin_family = AF_INET;
    inet_pton(AF_INET, ADDR, &address.sin_addr);
    address.sin_port = htons(PORT);
    socklen_t address_len = sizeof(address);

    if (connect(client_socket, (sockaddr *) &address, address_len) < 0) {
        cerr << "Ошибка подключения к серверу." << endl;
        return 1;
    }

    int choice;
    do {
        cout << "Меню: " << endl
                << "1. Фильтр по стране" << endl
                << "2. Просмотр всего содержимого" << endl
                << "3. Создать новую запись" << endl
                << "4. Редактировать запись по названию" << endl
                << "5. Удалить запись по названию" << endl
                << "0. Выйти из программы" << endl;
        cin >> choice;
        cin.ignore(numeric_limits<streamsize>::max(), '\n'); // <<< очищаем ввод

        string request;
        switch (choice) {
            case 1: {
                string country;
                cout << "Введите название страны: ";
                getline(cin, country);

                request = FILTER_REQ + country;
                send(client_socket, request.c_str(), request.length(), 0);
                break;
            }
            case 2: {
                request = VIEW_REQ;
                send(client_socket, request.c_str(), request.length(), 0);
                break;
            }
            case 3: {
                string product_name, country, manufacturer;
                int count;

                cout << "Введите название товара: ";
                getline(cin, product_name);

                cout << "Введите страну-производителя: ";
                getline(cin, country);

                cout << "Введите компанию производителя: ";
                getline(cin, manufacturer);

                cout << "Введите количество товара: ";
                cin >> count;
                cin.ignore(numeric_limits<streamsize>::max(), '\n'); // очистка буфера !!!

                string request = NEWREC_REQ + country + "," + manufacturer + "," +
                                 product_name + "," + to_string(count);

                if (request.size() >= BUFSIZE) {
                    cerr << "Длина запроса превышает допустимую." << endl;
                    break;
                }

                send(client_socket, request.c_str(), request.size(), 0);
                break;
            }

            case 4: {
                string product_name, new_product_name, country, manufacturer;
                int count;

                cout << "Введите название редактируемой записи: ";
                getline(cin, product_name);

                cout << "Введите новое название: ";
                getline(cin, new_product_name);

                cout << "Введите новую страну: ";
                getline(cin, country);

                cout << "Введите новую компанию: ";
                getline(cin, manufacturer);

                cout << "Введите новое количество: ";
                cin >> count;
                cin.ignore(numeric_limits<streamsize>::max(), '\n'); // опять чистим буфер

                string request = UPDREC_REQ + product_name + "," + country + "," +
                                 manufacturer + "," + new_product_name + "," +
                                 to_string(count);

                send(client_socket, request.c_str(), request.size(), 0);
                break;
            }

            case 5: {
                string product_name;
                cout << "Введите название удаляемого товара: ";
                getline(cin, product_name);

                request = DELREC_REQ + product_name;
                send(client_socket, request.c_str(), request.length(), 0);
                break;
            }
            case 0:
                cout << "Выход из программы." << endl;
                break;
            default:
                cerr << "Такого варианта нет!" << endl;
                break;
        }

        if (choice != 0) {
            char response[BUFSIZE];
            ssize_t response_length = recv(client_socket, response, BUFSIZE - 1, 0);
            if (response_length > 0) {
                response[response_length] = '\0';
                cout << "Ответ от сервера: " << response << endl;
            } else if (response_length == 0) {
                cout << "Соединение с сервером закрыто." << endl;
                break;
            }
        }
    } while (choice != 0);

    close(client_socket);
    return 0;
}
