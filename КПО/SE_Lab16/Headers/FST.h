#pragma once

namespace FST {
    struct RELATION {
        char symbol; // символ перехода
        short nnode; // номер смежной вершины
        RELATION(
            char c = 0x00, // символ перехода
            short n = NULL // новое состояние
        );
    };

    struct NODE // вершина графа
    {
        short n_relation; // количество инцидентных ребер
        RELATION *relations; // инцидентные ребра
        NODE();

        NODE(
            short n, // количество инцидентных ребер
            RELATION rel, ... // список ребер
        );
    };

    struct FST {
        // недетерменированный конечный автомат
        char *string; // цепочка, строка, завершаящаяся 0x00
        short position; // текущая позиция в цепочке
        short nstates; // количество состояний в автомате
        NODE *nodes; // граф переходов: [0] - начальное состояние, [nstate-1] - конечное
        short *rstates; // возможные состояния графа на данном этапе
        FST(
            char *s, // цепочка
            short ns, // количество состояний автомата
            NODE n, ... // список состояний (граф переходов)
        );
    };

    bool execute(
        FST &fst);
}
