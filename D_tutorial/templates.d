module templates;
import std.stdio;


T max(T)(T a, T b)
{
    return a > b ? a : b;
}

void main()
{
    writeln(max(5, 2));
}