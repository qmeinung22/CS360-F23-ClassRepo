module arrays_slices;
import std.stdio;


void main()
{
    int[] arr = [1,2,3,4,5]; //Dynamic Array
    int[] slice = arr[0..3];
    writeln(arr);
    writeln(slice);
}