#pragma once
#include "Error.h"
#include "GRB.h"
#include "LT.h"

#define NS(n) GRB::Rule::Chain::N(n)
#define TS(n) GRB::Rule::Chain::T(n)
#define ISNS(n)	GRB::Rule::Chain::isN(n)

namespace GRB
{
	// S -> Program
	// D -> FunctionDeclarations
	// F -> Function
	// T -> Type
	// P -> Parameters
	// R -> RemainingParameters
	// L -> StatementList
	// O -> Statement
	// E -> Expression
	// M -> MoreExpression (for operations)
	// A -> Arguments
	// K -> RemainingArguments
    // X -> DifferBlock

	Greibach greibach(NS('S'), TS('$'),
		13,
		// S -> D m { L }
		Rule(NS('S'), GRB_ERROR_SERIES + 0, 1,
			Rule::Chain(5, NS('D'), TS(LEX_MAIN), TS(LEX_LEFTBRACE), NS('L'), TS(LEX_BRACELET))
		),
		// D -> F D | epsilon
		Rule(NS('D'), GRB_ERROR_SERIES + 0, 2,
			Rule::Chain(2, NS('F'), NS('D')),
			Rule::Chain()
		),
		// F -> func T id ( P ) { L }
		Rule(NS('F'), GRB_ERROR_SERIES + 0, 1,
			Rule::Chain(9, TS(LEX_FUNC), NS('T'), TS(LEX_ID), TS(LEX_LEFTHESIS), NS('P'), TS(LEX_RIGHTHESIS), TS(LEX_LEFTBRACE), NS('L'), TS(LEX_BRACELET))
		),
		// T -> unsigned integer | char | logic
		Rule(NS('T'), GRB_ERROR_SERIES + 1, 3,
			Rule::Chain(2, TS(LEX_UNSIGNED), TS(LEX_INTEGER)),
			Rule::Chain(1, TS(LEX_CHAR)),
			Rule::Chain(1, TS(LEX_LOGIC))
		),
		// P -> T id R | epsilon
		Rule(NS('P'), GRB_ERROR_SERIES + 3, 2,
			Rule::Chain(3, NS('T'), TS(LEX_ID), NS('R')),
			Rule::Chain()
		),
		// R -> , T id R | epsilon
		Rule(NS('R'), GRB_ERROR_SERIES + 3, 2,
			Rule::Chain(4, TS(LEX_COMMA), NS('T'), TS(LEX_ID), NS('R')),
			Rule::Chain()
		),
		// L -> O L | send E ; L | epsilon
		Rule(NS('L'), GRB_ERROR_SERIES + 1, 3,
			Rule::Chain(2, NS('O'), NS('L')),
			Rule::Chain(4, TS(LEX_SEND), NS('E'), TS(LEX_SEMICOLON), NS('L')),
			Rule::Chain()
		),
		// O -> T id ; | T id = E ; | id = E ; | id(A) ; | if (E) {L} X
		Rule(NS('O'), GRB_ERROR_SERIES + 1, 5,
			Rule::Chain(3, NS('T'), TS(LEX_ID), TS(LEX_SEMICOLON)),
			Rule::Chain(5, NS('T'), TS(LEX_ID), TS(LEX_EQUAL), NS('E'), TS(LEX_SEMICOLON)),
			Rule::Chain(4, TS(LEX_ID), TS(LEX_EQUAL), NS('E'), TS(LEX_SEMICOLON)),
			Rule::Chain(5, TS(LEX_ID), TS(LEX_LEFTHESIS), NS('A'), TS(LEX_RIGHTHESIS), TS(LEX_SEMICOLON)),
			Rule::Chain(8, TS(LEX_IF), TS(LEX_LEFTHESIS), NS('E'), TS(LEX_RIGHTHESIS), TS(LEX_LEFTBRACE), NS('L'), TS(LEX_BRACELET), NS('X'))
        ),
        // X -> differ { L } | epsilon
        Rule(NS('X'), GRB_ERROR_SERIES + 1, 2,
            Rule::Chain(4, TS(LEX_DIFFER), TS(LEX_LEFTBRACE), NS('L'), TS(LEX_BRACELET)),
            Rule::Chain()
        ),
		// E -> id M | literal M | ( E ) M
		Rule(NS('E'), GRB_ERROR_SERIES + 2, 3,
			Rule::Chain(2, TS(LEX_ID), NS('M')),
			Rule::Chain(2, TS(LEX_LITERAL), NS('M')),
			Rule::Chain(4, TS(LEX_LEFTHESIS), NS('E'), TS(LEX_RIGHTHESIS), NS('M'))
		),
		// M -> + E | - E | * E | : E | ... | epsilon
		Rule(NS('M'), GRB_ERROR_SERIES + 2, 10,
			Rule::Chain(2, TS(LEX_PLUS), NS('E')),
			Rule::Chain(2, TS(LEX_MINUS), NS('E')),
			Rule::Chain(2, TS(LEX_STAR), NS('E')),
			Rule::Chain(2, TS(LEX_COLON), NS('E')),
			Rule::Chain(2, TS(LEX_EQUAL_EQUAL), NS('E')),
			Rule::Chain(2, TS(LEX_NOT_EQUAL), NS('E')),
			Rule::Chain(2, TS(LEX_LESS), NS('E')),
			Rule::Chain(2, TS(LEX_GREATER), NS('E')),
            Rule::Chain(2, TS(LEX_LESS_EQUAL), NS('E')),
            Rule::Chain(2, TS(LEX_GREATER_EQUAL), NS('E')),
			Rule::Chain()
		),
		// A -> E K | epsilon
		Rule(NS('A'), GRB_ERROR_SERIES + 3, 2,
			Rule::Chain(2, NS('E'), NS('K')),
			Rule::Chain()
		),
		// K -> , E K | epsilon
		Rule(NS('K'), GRB_ERROR_SERIES + 3, 2,
			Rule::Chain(3, TS(LEX_COMMA), NS('E'), NS('K')),
			Rule::Chain()
		)
	);
}
