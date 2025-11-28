#include <iostream>
#include <fstream>
#include <string>
#include <vector>
#include <cstdint>
#include <iomanip>

const uint8_t TYPE_INT = 0x01;
const uint8_t TYPE_STRING = 0x02;

int main() {
    std::cout << "Проект Deserializer запущен." << std::endl;

    std::string input_file_path = "../../Serializer/cmake-build-debug/serialized_data.bin";
    std::string output_asm_path = "output.asm";

    std::ifstream ifs(input_file_path, std::ios::binary);
    if (!ifs.is_open()) {
        std::cerr << "Ошибка: Не удалось открыть входной файл: " << input_file_path << std::endl;
        return 1;
    }

    std::ofstream ofs_asm(output_asm_path);
    if (!ofs_asm.is_open()) {
        std::cerr << "Ошибка: Не удалось открыть выходной asm-файл: " << output_asm_path << std::endl;
        ifs.close();
        return 1;
    }

    std::vector<std::string> int_vars;
    std::vector<std::string> string_vars;

    ofs_asm << "section .data\n";
    ofs_asm << "  newline db 0xA\n";

    uint8_t type_byte;
    int var_counter = 0;

    while (ifs.read(reinterpret_cast<char*>(&type_byte), sizeof(type_byte))) {
        if (type_byte == TYPE_INT) {
            int32_t value;
            if (!ifs.read(reinterpret_cast<char*>(&value), sizeof(value))) {
                std::cerr << "Ошибка: Не удалось прочитать целочисленное значение." << std::endl;
                break;
            }
            std::string var_name = "var_" + std::to_string(var_counter++);
            int_vars.push_back(var_name);
            ofs_asm << "  " << var_name << " dd " << value << "\n";
            std::cout << "Десериализовано целое число: " << value << std::endl;
        } else if (type_byte == TYPE_STRING) {
            uint8_t length;
            if (!ifs.read(reinterpret_cast<char*>(&length), sizeof(length))) {
                std::cerr << "Ошибка: Не удалось прочитать длину строки." << std::endl;
                break;
            }
            std::vector<char> buffer(length);
            if (!ifs.read(buffer.data(), length)) {
                std::cerr << "Ошибка: Не удалось прочитать данные строки." << std::endl;
                break;
            }
            std::string s(buffer.begin(), buffer.end());
            
            std::string escaped_s = "";
            for (char c : s) {
                if (c == '"') {
                    escaped_s += "\\\"";
                } else {
                    escaped_s += c;
                }
            }

            std::string var_name = "var_" + std::to_string(var_counter++);
            string_vars.push_back(var_name);
            ofs_asm << "  " << var_name << " db \"" << escaped_s << "\", 0\n";
            ofs_asm << "  " << var_name << "_len equ $ - " << var_name << " - 1\n";
            std::cout << "Десериализована строка: \"" << s << "\" (Длина: " << (int)length << ")" << std::endl;
        } else {
            std::cerr << "Ошибка: Обнаружен неизвестный байт типа: 0x"
                      << std::hex << std::setw(2) << std::setfill('0')
                      << static_cast<int>(type_byte) << std::endl;
            break;
        }
    }
    
    ofs_asm << "\nsection .bss\n";
    ofs_asm << "  int_buffer resb 12\n";

    ofs_asm << "\nsection .text\n";
    ofs_asm << "  global _start\n";
    
    ofs_asm << "_print_newline:\n";
    ofs_asm << "  mov rax, 1\n";
    ofs_asm << "  mov rdi, 1\n";
    ofs_asm << "  mov rsi, newline\n";
    ofs_asm << "  mov rdx, 1\n";
    ofs_asm << "  syscall\n";
    ofs_asm << "  ret\n";
    
    ofs_asm << "_print_int:\n";
    ofs_asm << "  mov rdi, int_buffer\n";
    ofs_asm << "  add rdi, 10\n";
    ofs_asm << "  xor r11, r11\n";
    ofs_asm << "  test rax, rax\n";
    ofs_asm << "  jns .positive_loop\n";
    ofs_asm << "  neg rax\n";
    ofs_asm << "  mov r11, 1\n";
    ofs_asm << ".positive_loop:\n";
    ofs_asm << "  mov rbx, 10\n";
    ofs_asm << "  xor rdx, rdx\n";
    ofs_asm << "  div rbx\n";
    ofs_asm << "  add rdx, '0'\n";
    ofs_asm << "  dec rdi\n";
    ofs_asm << "  mov [rdi], dl\n";
    ofs_asm << "  test rax, rax\n";
    ofs_asm << "  jnz .positive_loop\n";
    ofs_asm << "  cmp r11, 1\n";
    ofs_asm << "  jne .print\n";
    ofs_asm << "  dec rdi\n";
    ofs_asm << "  mov byte [rdi], '-'\n";
    ofs_asm << ".print:\n";
    ofs_asm << "  mov rsi, rdi\n";
    ofs_asm << "  mov rdx, int_buffer\n";
    ofs_asm << "  add rdx, 11\n";
    ofs_asm << "  sub rdx, rsi\n";
    ofs_asm << "  mov rax, 1\n";
    ofs_asm << "  mov rdi, 1\n";
    ofs_asm << "  syscall\n";
    ofs_asm << "  ret\n";

    ofs_asm << "_start:\n";

    for (const auto& var_name : int_vars) {
        ofs_asm << "  movsx rax, dword [ " << var_name << "]\n";
        ofs_asm << "  call _print_int\n";
        ofs_asm << "  call _print_newline\n";
    }

    for (const auto& var_name : string_vars) {
        ofs_asm << "  mov rax, 1\n";
        ofs_asm << "  mov rdi, 1\n";
        ofs_asm << "  mov rsi, " << var_name << "\n";
        ofs_asm << "  mov rdx, " << var_name << "_len\n";
        ofs_asm << "  syscall\n";
        ofs_asm << "  call _print_newline\n";
    }

    ofs_asm << "\n  mov rax, 60\n";
    ofs_asm << "  mov rdi, 0\n";
    ofs_asm << "  syscall\n";

    ifs.close();
    ofs_asm.close();
    std::cout << "Данные успешно десериализованы и ассемблерный код NASM сгенерирован в " << output_asm_path << std::endl;

    return 0;
}