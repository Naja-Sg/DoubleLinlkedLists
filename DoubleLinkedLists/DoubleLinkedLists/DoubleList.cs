
using DoubleLinkedLists;
using System.Text;
using System.Xml.Linq;

namespace DoubleLinkedLists;

public class DoubleList<T>
{
    private Node<T>? _head;
    private Node<T>? _tail;

    private bool _descending; //Using  with false for ascending order and True for descending.

    public int Count { get; private set; }

    public bool IsEmpty => _head == null;

    //MEthods

    //3.1
    public void Add(T item)
    {
        var node = new Node<T>(item);

        if (_head == null)
        {
            _head = _tail = node;
            Count++;
            return;
        }

        var current = _head; //Buscar el primero nodo delante del cual va el nuevo

        while (current != null && !GoesBefore(item, current.Data))

        {
            current = current.Next;
        }

        if (current == null) //insertar al final

        {
            node.Prev = _tail;
            _tail!.Next = node;
            _tail = node;

        }

        else if (current.Prev == null) //insertar al inicio

        {
            node.Next = _head;
            _head.Prev = node;
            _head = node;

        }

        else

        {

            node.Prev = current.Prev;
            node.Next = current;
            current.Prev.Next = node;
            current.Prev = node;

        }

        Count++;

    }

    private int Compare(T a, T b)

    {
        return Comparer<T>.Default.Compare(a, b);
    }

    private bool GoesBefore(T a, T b)

    {
        int comparison = Compare(a, b);
        return _descending ? comparison > 0 : comparison < 0;
    }

    public IEnumerable<T> Forward()
    {
        var current = _head;
        while (current != null)
        {
            yield return current.Data;
            current = current.Next;
        }
    }

    public IEnumerable<T> Backward()
    {
        var current = _tail;
        while (current != null)
        {
            yield return current.Data;
            current = current.Prev;
        }
    }

    //3.2

    public void SortDescending()
    {
        if (_descending || _head == null)

        {
            _descending = true;
            return;
        }

        var current = _head;
        while (current != null)
        {
            (current.Next, current.Prev) = (current.Prev, current.Next);
            current = current.Prev;
        }

        (_head, _tail) = (_tail, _head);
        _descending = true;
    }

    private IEnumerable<(T Value, int Count)> Groups()
    {
        var current = _head;
        while (current != null)
        {
            T value = current.Data;
            int count = 0;
            while (current != null && Compare(current.Data, value) == 0)
            {
                count++;
                current = current.Next;
            }
            yield return (value, count);
        }
    }

    //3.3

    public List<T> Modes()
    {
        var modes = new List<T>();
        int max = 0;

        foreach (var (value, count) in Groups())
        {
            if (count > max)
            {
                max = count;
                modes.Clear();
                modes.Add(value);
            }
            else if (count == max)
            {
                modes.Add(value);
            }
        }

        return modes;
    }

    //3.4

    public string Graph()
    {
        var sb = new StringBuilder();
        foreach (var (value, count) in Groups())
        {
            sb.AppendLine($"{value} {new string('*', count)}");
        }
        return sb.ToString();
    }

    //3.5

    public bool Exists(T item)
    {
        var current = _head;
        while (current != null)
        {
            if (Compare(current.Data, item) == 0)
            {
                return true;
            }
            current = current.Next;
        }
        return false;
    }

    //3.6

    public bool RemoveFirst(T item)
    {
        var current = _head;
        while (current != null)
        {
            if (Compare(current.Data, item) == 0)
            {
                Unlink(current);
                return true;
            }
            current = current.Next;
        }
        return false;
    }

    public int RemoveAll(T item)
    {
        int removed = 0;
        var current = _head;
        while (current != null)
        {
            var next = current.Next; // guardar antes de desenlazar
            if (Compare(current.Data, item) == 0)
            {
                Unlink(current);
                removed++;
            }
            current = next;
        }
        return removed;
    }

    private void Unlink(Node<T> node)
    {
        if (node.Prev != null) node.Prev.Next = node.Next;
        else _head = node.Next;

        if (node.Next != null) node.Next.Prev = node.Prev;
        else _tail = node.Prev;

        node.Next = node.Prev = null;
        Count--;
    }
}
