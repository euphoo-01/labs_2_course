#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <cstdint>

const uint8_t TYPE_INT = 0x01;
const uint8_t TYPE_STRING = 0x02;

int main() {

    int32_t my_int = 123456789;
    std::string my_string = "Какашки мне в кармашки";
    std::string another_string = "serialized";
    std::string another_string2 = "Yedren baton";
    int32_t another_int = -98765;

    if (my_string.length() > 127 || another_string.length() > 127) {
        std::cerr << "Ошибка: Длина строки превышает максимально допустимую (127 байт)." << std::endl;
        return 1;
    }

    std::ofstream ofs("serialized_data.bin", std::ios::binary);
    if (!ofs.is_open()) {
        std::cerr << "Ошибка: Не удалось открыть файл для записи!" << std::endl;
        return 1;
    }

    ofs.write(reinterpret_cast<const char*>(&TYPE_INT), sizeof(TYPE_INT));
    ofs.write(reinterpret_cast<const char*>(&my_int), sizeof(my_int));
    std::cout << "Сериализовано целое число: " << my_int << std::endl;

    uint8_t string_length = static_cast<uint8_t>(my_string.length());
    ofs.write(reinterpret_cast<const char*>(&TYPE_STRING), sizeof(TYPE_STRING));
    ofs.write(reinterpret_cast<const char*>(&string_length), sizeof(string_length));
    ofs.write(my_string.c_str(), string_length);
    std::cout << "Сериализована строка: \"" << my_string << "\" (Длина: " << (int)string_length << ")" << std::endl;
    
    ofs.write(reinterpret_cast<const char*>(&TYPE_INT), sizeof(TYPE_INT));
    ofs.write(reinterpret_cast<const char*>(&another_int), sizeof(another_int));
    std::cout << "Сериализовано целое число: " << another_int << std::endl;

    uint8_t another_string_length = static_cast<uint8_t>(another_string.length());
    ofs.write(reinterpret_cast<const char*>(&TYPE_STRING), sizeof(TYPE_STRING));
    ofs.write(reinterpret_cast<const char*>(&another_string_length), sizeof(another_string_length));
    ofs.write(another_string.c_str(), another_string_length);
    std::cout << "Сериализована строка: \"" << another_string << "\" (Длина: " << (int)another_string_length << ")" << std::endl;


    uint8_t another_string2_length = static_cast<uint8_t>(another_string2.length());
    ofs.write(reinterpret_cast<const char*>(&TYPE_STRING), sizeof(TYPE_STRING));
    ofs.write(reinterpret_cast<const char*>(&another_string2_length), sizeof(another_string2_length));
    ofs.write(another_string2.c_str(), another_string2_length);
    std::cout << "Сериализована строка: \"" << another_string2 << "\" (Длина: " << (int)another_string2_length << ")" << std::endl;

    ofs.close();
    std::cout << "Данные успешно сериализованы" << std::endl;

    return 0;
}