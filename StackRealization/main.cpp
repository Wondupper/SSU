#include<iostream>
#include<string>

using namespace std;

class Stack
{
    class Element
    {
        public:
            string inf;
            Element *next;
            Element (string a, Element *b) : inf(a),next(b)
            {}
    };
    public:
        Element *head;
        Stack():head(0)
        {}
        bool Proverka()
        {
            return head==0;
        }
        string Pop()
        {
            Element *q=head;
            string a=q->inf;
            head=q->next;
            delete q;
            return a;
        }
        void PrintStack()
        {
            Element *q=head;
            string a=q->inf;
            head=q->next;
            delete q;
            if(Proverka()==0)
            {
                PrintStack();
            }
        }
        void Push(string a)
        {
            head=new Element(a,head);
        }
};

int main()
{
    Stack stac;
    string s;
    int index;
    int n;
    cin>>n;
    string *a=new string[n];
    for(int i=0;i<n;i++)
    {
        string s1;
        cin>>s1;
        s=s1;
        a[i]=s1;
    }
    for(int i=0;i<n-1;i++)
    {
        if(a[i]==s)
            index=i;
    }
    for(int i=n-1;i>=0;i--)
    {
        stac.Push(a[i]);
    }
    for(int i=0;i<=index;i++)
    {
        cout<<stac.Pop()<<endl;
    }
    stac.PrintStack();
    return 0;
}


















