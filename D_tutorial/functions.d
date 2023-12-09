module functions;
import std.stdio;

int add(int x, int y)
{
    return x + y;
}

void main()
{
    int a = 5;
    int b = 20;
    int answer = add(a,b);
    writeln(answer);
}
