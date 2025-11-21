#pragma once
#include "In.h"
#include "LT.h"
#include "IT.h"
#include "FST.h"

namespace Lexer
{
    struct LEX {
        LT::LexTable lextable;
        IT::IdTable idtable;
    };
    void Analyze(In::IN &in, LT::LexTable &lextable, IT::IdTable &idtable);
}
