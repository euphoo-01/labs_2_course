#include <iostream>
#include "./Headers/FST.h"

using namespace std;

int main() {
    FST::FST fst("init",
                 28,
                 FST::NODE(1, FST::RELATION('i', 1)), //0
                 FST::NODE(1, FST::RELATION('f', 2)), //1
                 FST::NODE(1, FST::RELATION('(', 3)), //2
                 FST::NODE(1, FST::RELATION(' ', 4)), //3
                 FST::NODE(1, FST::RELATION(')', 5)), //4

                 FST::NODE(4, FST::RELATION('i', 1), FST::RELATION('(', 7), FST::RELATION('{', 19), FST::RELATION(' ', 5)), //5
                 FST::NODE(3, FST::RELATION('(', 7), FST::RELATION('{', 19), FST::RELATION(' ', 6)), //6

                 FST::NODE(2, FST::RELATION('c', 8), FST::RELATION('a', 15)), //7
                 FST::NODE(2, FST::RELATION('o', 9), FST::RELATION('o', 12)), //8
                 FST::NODE(1, FST::RELATION('n', 10)), //9
                 FST::NODE(1, FST::RELATION('s', 11)), //10
                 FST::NODE(1, FST::RELATION('t', 16)), //11

                 FST::NODE(1, FST::RELATION('u', 13)), //12
                 FST::NODE(1, FST::RELATION('n', 14)), //13
                 FST::NODE(1, FST::RELATION('t', 16)), //14

                 FST::NODE(1, FST::RELATION('b', 16)), //15

                 FST::NODE(1, FST::RELATION(' ', 17)), //16
                 FST::NODE(3, FST::RELATION(' ', 17), FST::RELATION(')', 18), FST::RELATION(')', 6)), //17

                 FST::NODE(2, FST::RELATION(' ', 18), FST::RELATION('}', 19)), //18
                 FST::NODE(1, FST::RELATION('r', 20)), //19
                 FST::NODE(1, FST::RELATION('e', 21)), //20
                 FST::NODE(1, FST::RELATION('t', 22)), //21
                 FST::NODE(1, FST::RELATION('u', 23)), //22
                 FST::NODE(1, FST::RELATION('r', 24)), //23
                 FST::NODE(1, FST::RELATION('n', 25)), //24
                 FST::NODE(1, FST::RELATION('}', 26)), //25
                 FST::NODE(1, FST::RELATION(';', 27)), //26
                 FST::NODE()); //27

    fst.string = "if( ){return};";
    cout << fst.string << " -> " << (execute(fst) ? "распознано" : "не распознано") << endl;

    fst.string = "if(  ){return};";
    cout << fst.string << " -> " << (execute(fst) ? "распознано" : "не распознано") << endl;

    fst.string = "if( )(const ){return};";
    cout << fst.string << " -> " << (execute(fst) ? "распознано" : "не распознано") << endl;

    fst.string = "if( )(ab    ){return};";
    cout << fst.string << " -> " << (execute(fst) ? "распознано" : "не распознано") << endl;

    fst.string = "if( )(ab )(count ){return};";
    cout << fst.string << " -> " << (execute(fst) ? "распознано" : "не распознано") << endl;

    fst.string = "if( )      {return};";
    cout << fst.string << " -> " << (execute(fst) ? "распознано" : "не распознано") << endl;

    fst.string = "if( )if( )if( ){return};";
    cout << fst.string << " -> " << (execute(fst) ? "распознано" : "не распознано") << endl;

    fst.string = "if( )(const ){return}";
    cout << fst.string << " -> " << (execute(fst) ? "распознано" : "не распознано") << endl;

    fst.string = "if( ab )({return};";
    cout << fst.string << " -> " << (execute(fst) ? "распознано" : "не распознано") << endl;

    return 0;
}
