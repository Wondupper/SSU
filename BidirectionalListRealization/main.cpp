#include<iostream>
#include<string>
using namespace std;

class List
{
public:
    class Element
    {
    public:
        string inf;
        Element *next;
        Element *prev;
        Element(string x): inf(x),next(NULL),prev(NULL)
        {}
    };
    Element *head;
    Element *end;
    int size;
    List(): head(NULL),size(0),end(NULL)
    {}
    Element *Find (int index)
    {
        Element *q=head;
        for(int i=1;i<index;i++)
        {
            q=q->next;
        }
        return q;
    }
    int GetLength()
    {
        return size;
    }
    string Get(int index)
    {
        Element *r=Find(index);
        string s=r->inf;
        return s;
    }
    void InsertRight(int index, string data)
    {
        Element *x=new Element(data);
        size=GetLength() + 1;
        Element *q=Find(index-1);
        if(q==NULL)
        {
            end=x;
            head=x;
        }
        else
        {
            x->next=q->next;
            x->prev=q;
            q->next=x;
            if(q==end)
            {
                end=x;
            }
            else
            {
                q->next->prev=x;
            }
        }
    }
    void Delete_element(int index)
    {
        Element *q=Find(index);
        size--;
        if(q==head)
        {
            head=head->next;
            head->prev=NULL;
        }
        else if(q==end)
        {
            end=end->prev;
            end->next=NULL;
        }
        else
        {
            q->prev->next=q->next;
            q->next->prev=q->prev;
        }
        q->next=NULL;
        q->prev=NULL;
        delete q;
    }
    void Print()
    {
        for(Element *q=head;q!=NULL; q=q->next)
        {
            cout<<q->inf<< ' ';
        }
        cout<<endl;
    }
};

int main()
{
    List stac;
    int n;
    cin>>n;
    for(int i=1;i<=n;i++)
    {
        string s1;
        cin>>s1;
        stac.InsertRight(i,s1);
    }
    stac.Print();
    int k=0;
    for(int i=1;i<stac.GetLength();i++)
    {
        if(stac.Get(i)==stac.Get(stac.GetLength()))
        {
            k++;
            stac.Print();
            stac.Delete_element(i);
            i--;
        }
    }
    stac.Print();
    cout<<k;
    return 0;
}














