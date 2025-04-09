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
        Element(string x): inf(x),next(0)
        {}
    };
    Element *head;
    int size;
    Element *Find (int index)
    {
        Element *q=head;
        for(int i=1;i<index;i++)
        {
            q=q->next;
        }
        return q;
    }
    List(): head(0),size(0)
    {}
    int GetLength()
    {
        return size;
    }
    string Get(int index)
    {
        Element *r=Find(index);
        string i=r->inf;
        return i;
    }
    void Insert(int index, string data)
    {
        Element *x=new Element(data);
        size=GetLength() + 1;
        if(index==1)
        {
            x->next=head;
            head=x;
        }
        else
        {
            Element *y=Find(index-1);
            x->next=y->next;
            y->next=x;
        }
    }
    void Delete_element(int index)
    {
        Element *q;
        size--;
        if(index==1)
        {
            q=head;
            head=head->next;
        }
        else
        {
            Element *y=Find(index-1);
            q=y->next;
            y->next=q->next;
        }
        q->next=NULL;
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
    string s;
    cin>>n;
    for(int i=1;i<=n;i++)
    {
        string s1;
        cin>>s1;
        s=s1;
        stac.Insert(i,s1);
    }
    int k=0;
    for(int i=n-1;i>=1;i--)
    {
        if(stac.Get(i)==s)
        {
            k++;
            stac.Delete_element(i);
        }
    }
    cout<<k<<endl;
    stac.Print();
    return 0;
}


































