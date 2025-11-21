#pragma once
#include "GRB.h"
#include <deque>
#include "Lexer.h"
#include <fstream>
#define MFST_DIAGN_NUMBER 3

// Отладка
#define MFST_TRACE_START(wr) wr << std::setw(4)<<std::left<<"Шаг"<<":"\
<< std::setw(20)<<std::left<<" Правило"\
<< std::setw(30)<<std::left<<" Входная лента"\
<< std::setw(20)<<std::left<<" Стек"\
<< std::endl;

#define MFST_TRACE1(wr)		 wr << std::setw(4)<<std::left<<++FST_TRACE_n<<": "\
<< std::setw(20)<<std::left<<rule.getCRule(rbuf,nrulechain)\
<< std::setw(30)<<std::left<<getCLenta(lbuf,lenta_position)\
<< std::setw(20)<<std::left<<getCSt(sbuf)\
<< std::endl;

#define MFST_TRACE2(wr)		 wr << std::setw(4)<<std::left<<FST_TRACE_n<<": "\
<< std::setw(20)<<std::left<<" "\
<< std::setw(30)<<std::left<<getCLenta(lbuf,lenta_position)\
<< std::setw(20)<<std::left<<getCSt(sbuf)\
<< std::endl;

#define MFST_TRACE3(wr)		 wr << std::setw(4)<<std::left<<++FST_TRACE_n<<": "\
<< std::setw(20)<<std::left<<" "\
<< std::setw(30)<<std::left<<getCLenta(lbuf,lenta_position)\
<< std::setw(20)<<std::left<<getCSt(sbuf)\
<< std::endl;

#define MFST_TRACE4(c, wr)		wr <<std::setw(4)<<std::left << ++FST_TRACE_n << ": "<<std::setw(20)<< std::left <<c<<std::endl;
#define MFST_TRACE5(c, wr)		wr <<std::setw(4)<<std::left << FST_TRACE_n << ": "<<std::setw(20)<< std::left <<c<<std::endl;
#define MFST_TRACE6(c,k, wr)	wr <<std::setw(4)<<std::left << FST_TRACE_n << ": "<<std::setw(20)<< std::left << c << k <<std::endl;
#define MFST_TRACE7(wr)			wr<<std::setw(4)<<std::left << state.lenta_position << ": "\
<<std::setw(20)<< std::left << rule.getCRule(rbuf,state.nrulechain)\
<<std::endl;

// Кастомная реализация стека с доступом к элементам
class MFSTSTSTACK {
private:
    std::deque<short> container;

public:
    void push(short value) { container.push_back(value); }

    void pop() {
        if (!container.empty()) container.pop_back();
    }

    short top() const {
        if (!container.empty()) return container.back();
        return 0;
    }

    bool empty() const { return container.empty(); }
    size_t size() const { return container.size(); }

    short &operator[](size_t index) { return container[index]; }
    const short &operator[](size_t index) const { return container[index]; }

    std::deque<short> &getContainer() { return container; }
    const std::deque<short> &getContainer() const { return container; }
};

namespace MFST {
    struct MfstState {
        short lenta_position;
        short nrule;
        short nrulechain;
        MFSTSTSTACK st;

        MfstState();

        MfstState(
            short pposition,
            MFSTSTSTACK pst,
            short pnrulechain);

        MfstState(
            short pposition,
            MFSTSTSTACK pst,
            short pnrule,
            short pnrulechain);
    };

    // Кастомная реализация стека состояний
    class MFSTSTATE {
    private:
        std::deque<MfstState> container;

    public:
        void push(const MfstState &value) { container.push_back(value); }

        void pop() {
            if (!container.empty()) container.pop_back();
        }

        MfstState &top() {
            return container.back();
        }

        const MfstState &top() const {
            return container.back();
        }

        bool empty() const { return container.empty(); }
        size_t size() const { return container.size(); }

        // Доступ к элементам как к массиву
        MfstState &operator[](size_t index) { return container[index]; }
        const MfstState &operator[](size_t index) const { return container[index]; }

        // Доступ к внутреннему контейнеру
        std::deque<MfstState> &getContainer() { return container; }
        const std::deque<MfstState> &getContainer() const { return container; }
    };

    struct Mfst // магазинный автомат
    {
        enum RC_STEP {
            // код возврата функции step
            NS_OK, // найдено правило и цепочка, цепочка записана в стек
            NS_NORULE, // не найдено правило грамматики (ошибка в грамматике)
            NS_NORULECHAIN, // не найдена подходящая цепочка правила (ошибка в исходном коде)
            NS_ERROR, // неизвестный нетерминальный символ грамматики
            TS_OK, // тек. символ ленты == вершине стека, продвинулась лента, pop стека
            TS_NOK, // тек. символ ленты != вершине стека, восстановлено состояние
            LENTA_END, // текущая позиция ленты >= lenta_size
            SURPRISE // неожиданный код возврата (ошибка в step)
        };

        struct MfstDiagnosis // диагностика
        {
            short lenta_position; // позиция на ленте
            RC_STEP rc_step; // код завершения шага
            short nrule; // номер правила
            short nrule_chain; // номер цепочки правила
            MfstDiagnosis();

            MfstDiagnosis( // диагностика
                short plenta_position, // позиция на ленте
                RC_STEP prc_step, // код завершения шага
                short pnrule, // номер правила
                short pnrule_chain // номер цепочки правила
            );
        } diagnosis[MFST_DIAGN_NUMBER]; // последние самые глубокие сообщения
        GRBALPHABET *lenta; // перекодированная (TS/NS) лента (из Lexer)
        short lenta_position; // текущая позиция на ленте
        short nrule; // номер текущего правила
        short nrulechain; // номер текущей цепочки, текущего правила
        short lenta_size; // размер ленты
        GRB::Greibach greibach; // грамматика Грейбах
        Lexer::LEX lex; // результат работы лексического анализатора
        MFSTSTSTACK st; // стек автомата
        MFSTSTATE storestate; // стек для сохранения состояний
        Mfst();

        Mfst(
            Lexer::LEX plex, // результат работы лексического анализатора
            GRB::Greibach pgreibach // грамматика Грейбах
        );

        char *getCSt(char *buf);

        char *getCLenta(char *buf, short pos, short n = 25);

        char *getDiagnosis(short n, char *buf);

        bool savestate(std::ofstream &wr);

        bool reststate(std::ofstream &wr);

        bool push_chain(GRB::Rule::Chain chain);

        RC_STEP step(std::ofstream &wr);

        bool start(std::ofstream &wr);

        bool savediagnosis(
            RC_STEP pprc_step // код завершения шага
        );

        void printrules(std::ofstream &wr);

        struct Deducation {
            short size;
            short *nrules;
            short *nrulechains;

            Deducation() {
                size = 0;
                nrules = 0;
                nrulechains = 0;
            };
        } deducation;

        bool savededucation();
    };
}
