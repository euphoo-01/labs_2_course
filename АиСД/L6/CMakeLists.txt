cmake_minimum_required(VERSION 3.28)

set(CMAKE_EXPERIMENTAL_CXX_MODULE_CMAKE_API "OFF")
set(CMAKE_EXPERIMENTAL_CXX_MODULE_DYNDEP 0)
set(CMAKE_CXX_SCAN_FOR_MODULES OFF)

project(L6)
set(CMAKE_CXX_STANDARD 20)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

add_compile_options(-fno-modules -fno-modules-ts)

add_executable(L6
        main.cpp
        huffman.cpp
)
