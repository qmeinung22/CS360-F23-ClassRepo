module oop;
import std.stdio;

class Animal
{
    string name;
    void speak() {}
}

class Cow : Animal
{
    override void speak()
    {
        writeln("Moo");
    }
}

void main()
{
    Cow cow1 = new Cow();
    cow1.speak(); //Outputs: "Moo"
}