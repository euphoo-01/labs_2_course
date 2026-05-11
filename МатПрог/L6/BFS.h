#pragma once
#include "Graph.h"
#include <queue>

struct BFS 
{
    const static int INF = 0x7fffffff;
    const static int NIL = -1;
    enum Color {WHITE, GRAY, BLACK};
    const graph::AList *al;
    Color *c;
    int   *d;
    int   *p;
    std::queue<int> q;
    BFS(const graph::AList& al, int s);
    BFS(const graph::AMatrix& am, int s);
    void init(const graph::AList& al, int s);
    int get();
};