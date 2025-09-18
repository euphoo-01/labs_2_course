#include <csignal>
#include <cstring>
#include <sys/socket.h>
#include <arpa/inet.h>
#include <unistd.h>
#include <netinet/in.h>
#include <iostream>

#define BUFSIZE 1024
#define PORT 1280
#define ADDR "127.0.0.1"

using namespace std;

void normalize_spaces(char *s) {
    int i = 0, j = 0;
    bool space_found = false;
    while (s[i] && isspace(s[i])) i++;

    for (; s[i]; i++) {
        if (isspace(s[i])) {
            space_found = true;
        } else {
            if (space_found && j > 0) {
                s[j++] = ' ';
            }
            s[j++] = s[i];
            space_found = false;
        }
    }

    s[j] = '\0';
}


int main() {
    int peer_socket = socket(AF_INET, SOCK_STREAM, 0);
    if (peer_socket == -1) {
        cerr << "Ошибка создания сокета." << endl;
        close(peer_socket);
    }

    sockaddr_in address;
    address.sin_family = AF_INET;
    inet_pton(AF_INET, ADDR, &address.sin_addr);
    address.sin_port = htons(PORT);
    socklen_t address_len = sizeof(address);

    if (bind(peer_socket, (sockaddr *) &address, address_len) == -1) {
        cerr << "Не удалось забиндить домен." << endl;
    };
    listen(peer_socket, 5);
    int communication_socket = accept(peer_socket, nullptr, nullptr);
    if (communication_socket != -1) {
        cout << "Установлено подключение" << endl;
    }


    char buffer[BUFSIZE];
    ssize_t recieve = recv(communication_socket, buffer, BUFSIZE, 0);
    if (recieve > 0) {
        buffer[recieve] = '\0';
        cout << "Получено от клиента: " << buffer << endl;
        normalize_spaces(buffer);
        if (send(communication_socket, buffer, strlen(buffer), 0) > 0) {
            cout << "Сообщение отправлено и было получено клиентом" << endl;
        } else {
            cerr << "Сообщение было отрпавлено, но не дошло до получателя." << endl;
        }
    }

    close(communication_socket);
    close(peer_socket);

    return 0;
}
