#pragma once
#include <list>

namespace graph
{
    struct AList;                        
    struct AMatrix 
    {
        int  n_vertex;                  
        int  *mr;                       
        AMatrix(int n);                 
        AMatrix(int n, int mr[]);       
        AMatrix(const AMatrix& am);     
        AMatrix(const AList& al);       
        void set(int i, int j, int r);  
        int  get(int i, int j)const;    
    };

    struct AList 
    {
        int  n_vertex;                 
        std::list<int> *mr;            
        void create(int n);            
        AList(int n);                  
        AList(int n, int mr[]);        
        AList(const AMatrix& am);      
        AList(const AList& al);        
        void add(int i, int j);        
        int  size(int i) const;        
        int  get(int i, int j)const;   
    };
};