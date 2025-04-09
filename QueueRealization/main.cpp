#include<iostream>
#include<string>

using namespace std;

class Qaue
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
        Element *end;
        Qaue():head(0),end(0)
        {}
        string Pop()
        {
            Element *q=head;
            string a=q->inf;
            head=q->next;
            delete q;
            return a;
        }
        void Push(string a)
        {
            Element *q=end;
            end = new Element(a,head);
            if(!head)
            {
                head=end;
            }
            else
            {
                q->next=end;
            }
        }
};

int main()
{
    Qaue stac;
    string s;
    int index,k=0;
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
        {
            index=i;
            k++;
        }
    }
    for(int i=0;i<n-1;i++)
    {
        stac.Push(a[i]);
    }
    for(int i=0;i<=index;i++)
    {
        cout<<stac.Pop()<<endl;
    }
    cout<<k<<endl;
    return 0;
}





