#include <arpa/inet.h>
#include <netinet/in.h>
#include <sys/socket.h>
#include <unistd.h>

#include <iostream>
#include <string>

const int PORT = 1281;
const char *ADDR = "127.0.0.1";

int main() {
    int s = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
    if (s < 0) {
        std::cerr << "Ошибка: не удалось создать сокет" << std::endl;
        return 1;
    }

    sockaddr_in servaddr{};
    servaddr.sin_family = AF_INET;
    servaddr.sin_port = htons(PORT);
    inet_pton(AF_INET, ADDR, &servaddr.sin_addr);

    std::cout << "Введите строку: ";
    std::string input;
    std::getline(std::cin, input);

    if (sendto(s, input.c_str(), input.size(), 0,
               (sockaddr *) &servaddr, sizeof(servaddr)) < 0) {
        std::cerr << "Ошибка: не удалось отправить данные на сервер" << std::endl;
        close(s);
        return 1;
    }

    char buffer[1024];
    socklen_t len = sizeof(servaddr);
    ssize_t n = recvfrom(s, buffer, sizeof(buffer) - 1, 0,
                         (sockaddr *) &servaddr, &len);
    if (n < 0) {
        std::cerr << "Ошибка: не удалось получить ответ от сервера" << std::endl;
        close(s);
        return 1;
    }
    buffer[n] = '\0';

    std::cout << "Ответ сервера: " << buffer << std::endl;

    close(s);
    return 0;
}
