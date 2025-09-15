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

int main() {
    int client_socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

    if (client_socket == -1) {
        cerr << "Ошибка создания сокета." << endl;
    }

    sockaddr_in address;
    address.sin_family = AF_INET;
    inet_pton(AF_INET, ADDR, &address.sin_addr);
    address.sin_port = htons(PORT);
    socklen_t address_len = sizeof(address);

    connect(client_socket, (sockaddr*) &address, address_len);
    char buffer[BUFSIZE];
    cout << "Введите сообщение: ";
    cin.get(buffer, BUFSIZE);
    cout << buffer << endl;

    send(client_socket, buffer, strlen(buffer), 0);

    char response[BUFSIZE];
    size_t response_length = recv(client_socket, response, BUFSIZE, 0);
    if (response_length != 0) {
        response[response_length] = '\0';
        cout << "Ответ от сервера: " << response << endl;
    }

    close(client_socket);

    return 0;
}
