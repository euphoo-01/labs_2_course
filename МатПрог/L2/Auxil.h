#pragma once
#include <cstdlib>
#include <ctime>

namespace auxil
{
    void start()
    {
        srand((unsigned)time(NULL));
    }

    int iget(int min, int max)
    {
        return min + rand() % (max - min + 1);
    }
}