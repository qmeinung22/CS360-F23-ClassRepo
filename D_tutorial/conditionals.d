module conditionals;
import std.stdio;

void main()
{
    int a = 5;
    int b = 8;

    if(a < b)
    {
        writeln("a is less than b (5 < 8)");
    }

    if(a > b)
    {
        writeln("a is greater than b (8 > 5)");
    }

    if(a == b)
    {
        writeln("5 is equal to 5");
    }

    if(a != b) 
    {
        writeln("5 is not equal to 8 (5 != 8)");    
    }

    else 
    {
        writeln("Wrong Input");
    }

    //Else than with and/or statements
    int z = 9;
    if (z > 15 && z < 50) 
    {
        writeln("z is greater than 15 and less than 50");
    }

    else if (z > 11 || z % 2 == 0) 
    {
        writeln("z is greater than 15 and is an even number ");
    }

    else 
    {
        writeln("z is not greater than 10");
    }
}