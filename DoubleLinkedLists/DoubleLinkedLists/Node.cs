namespace DoubleLinkedLists;


public class Node<T> // Pointter. SAves data and points to both previous and next node
{
    public T Data { get; set; }
    public Node<T>? Next { get; set; }
    public Node<T>? Prev { get; set; }

    public Node(T data)
    {
        Data = data;
    }

}