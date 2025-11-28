section .data
    str1       db "Моя первая программа", 0
    str1_len   equ $ - str1 - 1
    str2       db "Привет всем!", 0
    str2_len   equ $ - str2 - 1
    newline    db 10
    filename   db "out.out", 0

section .bss
    fd resq 1 ; quadword

section .text
    global _start


_start:

    ; Открытие файла
    mov rax, 2
    mov rdi, filename
    mov rsi, 0x241
    mov rdx, 0644o
    syscall

    mov [fd], rax

    ; запись в файл
    mov rax, 1
    mov rdi, [fd]
    mov rsi, str1
    mov rdx, str1_len     
    syscall

    mov rax, 1
    mov rdi, [fd] 
    mov rsi, newline
    mov rdx, 1
    syscall

    mov rax, 1
    mov rdi, [fd] 
    mov rsi, str2
    mov rdx, str2_len
    syscall

    ; закрытие файла
    mov rax, 3
    mov rdi, [fd]
    syscall

    ; запись в консоль
    mov rax, 1
    mov rdi, 1
    mov rsi, str1
    mov rdx, str1_len     
    syscall

    mov rax, 1
    mov rdi, 1
    mov rsi, newline
    mov rdx, 1
    syscall

    mov rax, 1
    mov rdi, 1
    mov rsi, str2
    mov rdx, str2_len      
    syscall

    mov rax, 1
    mov rdi, 1
    mov rsi, newline
    mov rdx, 1
    syscall

    ; Точка выхода
    mov rax, 60
    mov rdi, 0
    syscall