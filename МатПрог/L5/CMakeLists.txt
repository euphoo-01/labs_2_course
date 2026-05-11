cmake_minimum_required(VERSION 3.10)
project(TransportOptimization)
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

set(SOURCE_FILES
    main.cpp
    TransportTask.cpp
    TransportTask.h
)

add_executable(transport_solver ${SOURCE_FILES})
