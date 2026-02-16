cmake_minimum_required(VERSION 4.2)
project(L2)

set(CMAKE_CXX_STANDARD 17)

add_executable(L2 main.cpp
        Auxil.h
        Combi.h
        Combi.cpp
        Salesman.h
        Salesman.cpp)
