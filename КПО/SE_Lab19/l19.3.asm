section .data
    myBytes     db 10h, 20h, 30h, 40h
    myWords     dw 8Ah, 3Bh, 44h, 5Fh, 99h
    myDoubles   dd 1, 2, 3, 4, 5, 6
    myPointer   dq myDoubles
    
    array       dd 15, -3, 7, 42, 8, -1
    array_size  equ 6
    
    msg_sum     db "Сумма элементов массива: "
    msg_sum_len equ $ - msg_sum
    
    sum_str     db "         "  ; buf
    newline     db 0xA         
    
    msg_zero    db "Нулевой элемент найден", 0xA
    msg_zero_len equ $ - msg_zero
    
    msg_no_zero db "Нулевой элемент не найден", 0xA
    msg_no_zero_len equ $ - msg_no_zero
    
    press_key   db 0xA, "Для завершения работы программы нажмите клавишу Enter...", 0
    press_key_len equ $ - press_key

section .bss
    input_buffer resb 1

section .text
    global _start

_start:
    mov rdi, 2                   
    mov rax, [myWords + rdi*2]   
    mov rbx, [myWords]
    

    mov rsi, array
    mov rcx, array_size
    xor rax, rax                 
sum_loop:
    add eax, [rsi]
    add rsi, 4
    loop sum_loop
    
    mov rdi, sum_str + 8
    mov byte [rdi], 0            
    
    mov rbx, 10                  
    mov rcx, rax                 ; сохраняем число
    test rax, rax
    jns .convert
    neg rax
.convert:
    xor rdx, rdx
    div rbx                      ; делим rax на 10, т.е. смещаемся на 1 цифру левее
    add dl, '0'
    dec rdi
    mov [rdi], dl
    test rax, rax
    jnz .convert
    test rcx, rcx
    jns .sum_done
    dec rdi
    mov byte [rdi], '-'
.sum_done:
    mov r8, rdi                  ; сохраняем указатель на начало числа
    
    mov rsi, array
    mov rcx, array_size
    mov rbx, 1
check_loop:
    cmp dword [rsi], 0
    jne .not_zero
    mov rbx, 0
    jmp .check_done
.not_zero:
    add rsi, 4
    loop check_loop
.check_done:
    
    mov rax, 1
    mov rdi, 1
    mov rsi, msg_sum
    mov rdx, msg_sum_len
    syscall
    

    mov rax, 1
    mov rdi, 1
    mov rsi, r8                  
    mov rdx, sum_str + 9         
    sub rdx, rsi                 
    syscall
    
    mov rax, 1
    mov rdi, 1
    mov rsi, newline
    mov rdx, 1
    syscall
    
    cmp rbx, 0
    jne .output_no_zero
    
    mov rax, 1
    mov rdi, 1
    mov rsi, msg_zero
    mov rdx, msg_zero_len
    syscall
    jmp .wait_key
    
.output_no_zero:
    mov rax, 1
    mov rdi, 1
    mov rsi, msg_no_zero
    mov rdx, msg_no_zero_len
    syscall

.wait_key:
    mov rax, 1
    mov rdi, 1
    mov rsi, press_key
    mov rdx, press_key_len
    syscall
    
    mov rax, 0
    mov rdi, 0
    mov rsi, input_buffer
    mov rdx, 1
    syscall
    
    mov rax, 60
    xor rdi, rdi
    syscall