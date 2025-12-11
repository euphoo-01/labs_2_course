#include <stack>
#include <vector>
#include <iostream>
#include "../Headers/RPN.h"


int getPriority(char op) {
    switch (op) {
        case '+':
        case '-':
            return 1;
        case '*':
        case '/':
            return 2;
        default:
            return 0;
    }
}

bool PolishNotation(int lextable_pos, LT::LexTable &lextable, IT::IdTable &idtable) {
    std::vector<LT::Entry> output;
    std::stack<LT::Entry> operators;
    int initial_pos = lextable_pos;
    int final_pos = 0;

    for (int i = lextable_pos; i < lextable.size; ++i) {
        LT::Entry current = lextable.table[i];
        final_pos = i;

        if (current.lexema == LEX_SEMICOLON) {
            break;
        }

        switch (current.lexema) {
            case LEX_ID: {
                IT::Entry id_entry = IT::GetEntry(idtable, current.idxTI);
                if (id_entry.idtype == IT::F) {
                    operators.push(current);
                } else {
                    output.push_back(current);
                }
                break;
            }
            case LEX_LITERAL: {
                output.push_back(current);
                break;
            }
            case LEX_LEFTHESIS: {
                operators.push(current);
                break;
            }
            case LEX_RIGHTHESIS: {
                while (!operators.empty() && operators.top().lexema != LEX_LEFTHESIS) {
                    output.push_back(operators.top());
                    operators.pop();
                }
                if (operators.empty()) {
                    std::cerr << "Error: Mismatched parentheses in RPN conversion." << std::endl;
                    return false;
                }
                operators.pop();

                if (!operators.empty() && operators.top().lexema == LEX_ID &&
                    IT::GetEntry(idtable, operators.top().idxTI).idtype == IT::F) {
                    output.push_back(operators.top());
                    operators.pop();
                }
                break;
            }
            case LEX_COMMA: {
                while (!operators.empty() && operators.top().lexema != LEX_LEFTHESIS) {
                    output.push_back(operators.top());
                    operators.pop();
                }
                break;
            }
            case '+':
            case '-':
            case '*':
            case '/': {
                while (!operators.empty() && getPriority(operators.top().lexema) >= getPriority(current.lexema)) {
                    output.push_back(operators.top());
                    operators.pop();
                }
                operators.push(current);
                break;
            }
        }
    }

    while (!operators.empty()) {
        if (operators.top().lexema == LEX_LEFTHESIS) {
            std::cerr << "Error: Mismatched parentheses in RPN conversion." << std::endl;
            return false;
        }
        output.push_back(operators.top());
        operators.pop();
    }

    std::cout << "\n\n ПОЛИЗ для строки " << lextable.table[lextable_pos].sn << ": ";
    for (const auto &entry : output) {
        if (entry.lexema == LEX_ID || entry.lexema == LEX_LITERAL) {
            std::cout << idtable.table[entry.idxTI].id << " ";
        } else {
            std::cout << entry.lexema << " ";
        }
    }
    std::cout << std::endl;

    int len_original = final_pos - initial_pos;
    if (lextable.table[final_pos].lexema == LEX_SEMICOLON) {
        len_original++;
    }


    int len_rpn = output.size();

    int write_limit = lextable_pos + len_original;

    for (int i = 0; i < len_rpn && (lextable_pos + i) < write_limit; ++i) {
        lextable.table[initial_pos + i] = output[i];
    }
    for (int i = len_rpn; (lextable_pos + i) < write_limit; ++i) {
        lextable.table[initial_pos + i] = {'#', lextable.table[initial_pos].sn, -1};
    }
    
    if (lextable.table[final_pos].lexema == LEX_SEMICOLON) {
        lextable.table[lextable_pos + len_original -1] = {LEX_SEMICOLON, lextable.table[lextable_pos + len_original - 1].sn, -1};
    } else if (final_pos < lextable.size && lextable.table[final_pos].lexema == LEX_SEMICOLON) {
        lextable.table[initial_pos + len_rpn] = {LEX_SEMICOLON, lextable.table[final_pos].sn, -1};
        for (int i = len_rpn + 1; (lextable_pos + i) < write_limit; ++i) {
             lextable.table[initial_pos + i] = {'#', lextable.table[initial_pos].sn, -1};
        }
    }


    return true;
}