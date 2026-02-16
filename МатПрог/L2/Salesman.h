#pragma once
#include "Combi.h"

#define INF 0x7fffffff

int salesman(       // функция возвращает длину оптимального маршрута
    int n,          // количество городов
    const int* d,   // массив расстояний
    int* r          // массив маршрутов
);