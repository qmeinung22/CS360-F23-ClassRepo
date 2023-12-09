module variables_types;
import std.stdio;

//Our Global Variables
int a = 10;
double b = 5.5;
bool isTrue = true;

void main()
{
    writeln(a); //Displays 10
    writeln(b); //Displays 5.5
    writeln(isTrue); //Displays True
    
    //Our Pointer Variable which is also a local variable
    auto ptr = new int(5); //Allocated on the heap
    writeln(ptr); //Displays address for location in memory
    writeln(*ptr); //Displays 5
}
