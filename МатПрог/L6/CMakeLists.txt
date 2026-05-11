cmake_minimum_required(VERSION 4.3)
project(L6)

set(CMAKE_CXX_STANDARD 17)

add_executable(L6 main.cpp
        Graph.h
        Graph.cpp
        BFS.h
        BFS.cpp
        DFS.h
        DFS.cpp)
