module loops;
import std.stdio;

void main()
{
    //While Loop
    int i = 0;
    while (i <= 5)
    {
        writeln("The value for i is: ", i);
        ++i;
    }

    //Do While Loop
    int j = 0;
    do
    {
        writeln("J starts out at:", j);
        ++j;
    } while (j < 5);

    //For While Loop wtih even twist
    for(int k = 0; k <= 20; ++k)
    {
        if (k % 2 == 0)
        {
            writeln("k as a even value is: ", k);
        }
    }

}