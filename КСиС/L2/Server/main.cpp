#include <arpa/inet.h>
#include <netinet/in.h>
#include <sys/socket.h>
#include <unistd.h>

#include <iostream>
#include <string>

const int PORT = 1281;
const char *ADDR = "127.0.0.1";

int count_brackets(const std::string &s) {
    int cnt = 0;
    for (char c: s) {
        if (c == '(' || c == ')' ||
            c == '[' || c == ']' ||
            c == '{' || c == '}' ||
            c == '<' || c == '>') {
            cnt++;
        }
    }
    return cnt;
}

int main() {
    int s = socket(AF_INET, SOCK_DGRAM, IPPROTO_UDP);
    if (s < 0) {
        std::cerr << "Ошибка: не удалось создать сокет" << std::endl;
        return 1;
    }

    sockaddr_in servaddr{};
    servaddr.sin_family = AF_INET;
    inet_pton(AF_INET, ADDR, &servaddr.sin_addr);
    servaddr.sin_port = htons(PORT);

    if (bind(s, (sockaddr *) &servaddr, sizeof(servaddr)) < 0) {
        std::cerr << "Ошибка: не удалось привязать сокет к порту " << PORT << std::endl;
        close(s);
        return 1;
    }

    std::cout << "UDP-сервер запущен. Порт: " << PORT << std::endl;

    char buffer[1024];
    while (true) {
        sockaddr_in cliaddr{};
        socklen_t len = sizeof(cliaddr);
        ssize_t n = recvfrom(s, buffer, sizeof(buffer) - 1, 0,
                             (sockaddr *) &cliaddr, &len);
        if (n < 0) {
            std::cerr << "Ошибка: не удалось получить данные" << std::endl;
            continue;
        }
        buffer[n] = '\0';
        std::string input(buffer);

        std::cout << "Получено сообщение: " << input << std::endl;

        int result = 0;
        if (input.size() % 5 == 0) {
            result = count_brackets(input);
        }

        std::string reply = std::to_string(result);
        if (sendto(s, reply.c_str(), reply.size(), 0,
                   (sockaddr *) &cliaddr, len) < 0) {
            std::cerr << "Ошибка: не удалось отправить ответ клиенту" << std::endl;
        } else {
            std::cout << "Отправлен ответ клиенту: " << reply << std::endl;
        }
    }

    close(s);
    return 0;
}
