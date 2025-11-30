#include <stack>
#include <vector>
#include <iostream>
#include "../Headers/RPN.h"
// Removed: #include "../Headers/Log.h"

// Function to determine operator precedence
int getPriority(char op) {
    switch (op) {
        case '+':
        case '-':
            return 1;
        case '*':
        case '/':
            return 2;
        default:
            return 0; // For parentheses and others
    }
}

// Updated PolishNotation function signature
bool PolishNotation(int lextable_pos, LT::LexTable &lextable, IT::IdTable &idtable) {
    std::vector<LT::Entry> output;
    std::stack<LT::Entry> operators;
    int initial_pos = lextable_pos;
    int final_pos = 0;

    for (int i = lextable_pos; i < lextable.size; ++i) {
        LT::Entry current = lextable.table[i];
        final_pos = i;

        if (current.lexema == LEX_SEMICOLON) {
            break; // End of expression
        }

        switch (current.lexema) {
            case LEX_ID: {
                IT::Entry id_entry = IT::GetEntry(idtable, current.idxTI);
                if (id_entry.idtype == IT::F) {
                    operators.push(current); // It's a function, push to operators stack
                } else {
                    output.push_back(current); // It's an operand (variable)
                }
                break;
            }
            case LEX_LITERAL: {
                output.push_back(current); // Operand (literal)
                break;
            }
            case LEX_LEFTHESIS: { // '('
                operators.push(current);
                break;
            }
            case LEX_RIGHTHESIS: { // ')'
                while (!operators.empty() && operators.top().lexema != LEX_LEFTHESIS) {
                    output.push_back(operators.top());
                    operators.pop();
                }
                if (operators.empty()) {
                    // Error: Mismatched parentheses (no opening parenthesis)
                    std::cerr << "Error: Mismatched parentheses in RPN conversion." << std::endl;
                    return false;
                }
                operators.pop(); // Pop '('

                // If after popping '(', the top is a function identifier, pop it to output
                if (!operators.empty() && operators.top().lexema == LEX_ID &&
                    IT::GetEntry(idtable, operators.top().idxTI).idtype == IT::F) {
                    output.push_back(operators.top());
                    operators.pop();
                }
                break;
            }
            case LEX_COMMA: { // ','
                // Pop operators until '(' is found. This separates function arguments.
                while (!operators.empty() && operators.top().lexema != LEX_LEFTHESIS) {
                    output.push_back(operators.top());
                    operators.pop();
                }
                // Don't pop '(' or push ','
                break;
            }
            case '+':
            case '-':
            case '*':
            case '/': { // Operators
                while (!operators.empty() && getPriority(operators.top().lexema) >= getPriority(current.lexema)) {
                    output.push_back(operators.top());
                    operators.pop();
                }
                operators.push(current);
                break;
            }
        }
    }

    // Pop any remaining operators from the stack to the output
    while (!operators.empty()) {
        if (operators.top().lexema == LEX_LEFTHESIS) {
            // Error: Mismatched parentheses (extra opening parenthesis)
            std::cerr << "Error: Mismatched parentheses in RPN conversion." << std::endl;
            return false;
        }
        output.push_back(operators.top());
        operators.pop();
    }

    // Output result to console
    std::cout << "\n\n Polish Notation (RPN) for expression at line " << lextable.table[lextable_pos].sn << ": ";
    for (const auto &entry : output) {
        if (entry.lexema == LEX_ID || entry.lexema == LEX_LITERAL) {
            std::cout << idtable.table[entry.idxTI].id << " ";
        } else {
            std::cout << entry.lexema << " ";
        }
    }
    std::cout << std::endl;

    // Modify lextable
    int len_original = final_pos - initial_pos;
    if (lextable.table[final_pos].lexema == LEX_SEMICOLON) { // If it ended on a semicolon, include it in length
        len_original++;
    }


    int len_rpn = output.size();

    // Ensure we don't write beyond the original expression's bounds
    int write_limit = lextable_pos + len_original;

    for (int i = 0; i < len_rpn && (lextable_pos + i) < write_limit; ++i) {
        lextable.table[initial_pos + i] = output[i];
    }
    // Fill remaining space with placeholders up to the original expression's length
    for (int i = len_rpn; (lextable_pos + i) < write_limit; ++i) {
        lextable.table[initial_pos + i] = {'#', lextable.table[initial_pos].sn, -1}; // '#' - placeholder
    }
    
    // Ensure the semicolon is kept if it was part of the original expression boundary
    if (lextable.table[final_pos].lexema == LEX_SEMICOLON) {
        lextable.table[lextable_pos + len_original -1] = {LEX_SEMICOLON, lextable.table[lextable_pos + len_original - 1].sn, -1};
    } else if (final_pos < lextable.size && lextable.table[final_pos].lexema == LEX_SEMICOLON) {
        // Handle case where expression did not "consume" the semicolon in the loop, but it's the next token
        // This handles cases where RPN ends before the semicolon
        lextable.table[initial_pos + len_rpn] = {LEX_SEMICOLON, lextable.table[final_pos].sn, -1};
        // Fill remaining original lexemes up to the semicolon
        for (int i = len_rpn + 1; (lextable_pos + i) < write_limit; ++i) {
             lextable.table[initial_pos + i] = {'#', lextable.table[initial_pos].sn, -1};
        }
    }


    return true;
}