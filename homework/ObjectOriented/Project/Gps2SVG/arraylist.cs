//Quintan Meinung

using System;

public class ArrayList
{
    private const int ARRAYLIST_INITIAL_CAPACITY = 4;
    private object[] body;

    public int Size { get; private set; }
    public int Capacity => body.Length;

    public ArrayList()
    {
        Size = 0;
        body = new object[ARRAYLIST_INITIAL_CAPACITY];
    }

    public void Allocate(int size)
    {
        if (size > Capacity)
        {
            int newCapacity = Capacity;
            while (newCapacity < size)
            {
                newCapacity *= 2;
            }

            Array.Resize(ref body, newCapacity);
        }
    }

    public void Add(object item)
    {
        Allocate(Size + 1);
        body[Size++] = item;
    }

    public object Pop()
    {
        if (Size > 0)
        {
            return body[--Size];
        }
        else
        {
            throw new InvalidOperationException("ArrayList is empty.");
        }
    }

    public object Get(int index)
    {
        if (index < Size)
        {
            return body[index];
        }
        else
        {
            throw new IndexOutOfRangeException("Index is out of range.");
        }
    }

    public void Set(int index, object value)
    {
        if (index < Size)
        {
            body[index] = value;
        }
        else
        {
            throw new IndexOutOfRangeException("Index is out of range.");
        }
    }

    public void Insert(int index, object value)
    {
        Allocate(Size + 1);

        if (index < Size)
        {
            Array.Copy(body, index, body, index + 1, Size - index);
            body[index] = value;
            Size++;
        }
        else if (index == Size)
        {
            body[Size++] = value;
        }
        else
        {
            throw new IndexOutOfRangeException("Index is out of range.");
        }
    }

    public object Remove(int index)
    {
        if (index < Size)
        {
            object removedItem = body[index];
            Array.Copy(body, index + 1, body, index, Size - index - 1);
            Size--;
            return removedItem;
        }
        else
        {
            throw new IndexOutOfRangeException("Index is out of range.");
        }
    }

    public void Clear()
    {
        Size = 0;
    }

    public ArrayList Slice(int index, int length)
    {
        if (index + length <= Size)
        {
            ArrayList newList = new ArrayList();
            newList.Allocate(length);
            Array.Copy(body, index, newList.body, 0, length);
            newList.Size = length;
            return newList;
        }
        else
        {
            throw new ArgumentException("Invalid index and length for slice.");
        }
    }

    public ArrayList SliceEnd(int index)
    {
        if (index < Size)
        {
            return Slice(index, Size - index);
        }
        else
        {
            throw new ArgumentException("Invalid index for slice.");
        }
    }

    public ArrayList Copy()
    {
        return SliceEnd(0);
    }

    public void Join(ArrayList source)
    {
        Splice(source, Size);
    }

    public void Splice(ArrayList source, int index)
    {
        Allocate(Size + source.Size);
        Array.Copy(body, index, body, index + source.Size, Size - index);
        Array.Copy(source.body, 0, body, index, source.Size);
        Size += source.Size;
    }

    public void Destroy()
    {
        body = null;
        Size = 0;
    }
}
