#pragma once
#include "In.h"
#include "LT.h"
#include "IT.h"
#include "FST.h"

namespace Lexer
{
    struct Tables
    {
        LT::LexTable lexTable;
        IT::IdTable idTable;
    };

    Tables Analyze(In::IN& in);
}
