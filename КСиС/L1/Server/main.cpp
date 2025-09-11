#include <csignal>
#include <cstring>
#include <filesystem>
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
    int in_space = 0;
    while (s[i] && isspace(s[i])) i++;

    for (; s[i]; i++) {
        if (isspace(s[i])) {
            in_space = 1;
        } else {
            if (in_space && j > 0) {
                s[j++] = ' ';
            }
            s[j++] = s[i];
            in_space = 0;
        }
    }

    s[j] = '\0';
}


int main() {
    int peer_socket = socket(AF_INET, SOCK_STREAM, 0);
    if (peer_socket == -1) {
        cerr << "Ошибка создания сокета." << endl;
    }

    sockaddr_in address;
    address.sin_family = AF_INET;
    inet_pton(AF_INET, ADDR, &address.sin_addr);
    address.sin_port = htons(PORT);
    socklen_t address_len = sizeof(address);

    if (bind(peer_socket, (sockaddr *) &address, address_len) == -1) {
        cerr << "Не удалось забиндить домен." << endl;
    };
    listen(peer_socket, PORT);
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
        send(communication_socket, buffer, strlen(buffer), 0);
    }

    close(communication_socket);
    close(peer_socket);

    return 0;
}
