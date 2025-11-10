section .data
    out_buf db "Результат сложения: ", 0
    out_buf_len equ $ - out_buf - 1
    newline db 10
    nm1 db 3
    nm2 db 5
    res db 0

section .bss
    buf resb 2

section .text
    global _start

_start:
    mov al, [nm1] 
    add al, [nm2] 
    mov [res], al 
    
    add al, '0'   
    mov [buf], al 
    
    mov rax, 1         
    mov rdi, 1         
    mov rsi, out_buf   
    mov rdx, out_buf_len 
    syscall
    
    mov rax, 1       
    mov rdi, 1       
    mov rsi, buf     
    mov rdx, 1       
    syscall
    
    mov rax, 1       
    mov rdi, 1       
    mov rsi, newline 
    mov rdx, 1       
    syscall
    
    mov rax, 60       
    mov rdi, 0       
    syscall