#pragma once

namespace FST {
    struct RELATION {
        char symbol; // символ перехода
        short nnode; // номер смежной вершины
        RELATION(
            char c,
            short n
        );
    };

    struct NODE // вершина графа
    {
        short n_relation; // количество инцидентных ребер
        RELATION *relations; // инцидентные ребра
        NODE();

        NODE(
            short,
            RELATION, ...
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
            char*,
            short,
            NODE, ...
        );
    };

    bool execute(
        const FST &fst);
}
