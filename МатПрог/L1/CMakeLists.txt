cmake_minimum_required(VERSION 4.2)
project(L1)

set(CMAKE_CXX_STANDARD 17)

add_executable(L1 main.cpp
        Auxil.h
        Auxil.cpp)

target_precompile_headers(L1 PRIVATE pch.h)
