#pragma once
#include <iostream>
#include <locale>
#include "LT.h"
#include "IT.h"

#define EXP1 28
#define EXP2 50
#define EXP3 66

bool PolishNotation(
    int lextable_pos,
    LT::LexTable &lextable,
    IT::IdTable &idtable
);
