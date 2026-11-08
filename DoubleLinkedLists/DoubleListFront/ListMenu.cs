using DoubleLinkedLists;
using System;
using System.Collections.Generic;
using System.Text;

namespace DoubleListFront;

public delegate bool TryParser<T>(string? input, out T value);

public class ListMenu<T>
{
    private readonly DoubleList<T> _list = new();
    private readonly TryParser<T> _parser;

    public ListMenu(TryParser<T> parser)
    {
        _parser = parser;
    }

    public void Run()
    {
        string? option;
        do
        {
            Console.WriteLine();
            Console.WriteLine("========== MENÚ ==========");
            Console.WriteLine("1. Adicionar.");
            Console.WriteLine("2. Mostrar hacia adelante.");
            Console.WriteLine("3. Mostrar hacia atrás.");
            Console.WriteLine("4. Ordenar descendentemente.");
            Console.WriteLine("5. Mostrar la(s) moda(s).");
            Console.WriteLine("6. Mostrar gráfico.");
            Console.WriteLine("7. Existe.");
            Console.WriteLine("8. Eliminar una ocurrencia.");
            Console.WriteLine("9. Eliminar todas las ocurrencias.");
            Console.WriteLine("0. Salir.");
            Console.Write("Opción: ");
            option = Console.ReadLine()?.Trim();
            Console.WriteLine();

            switch (option)
            {
                case "1": AddItems(); break;
                case "2": IfNotEmpty(() => Console.WriteLine(string.Join(", ", _list.Forward()))); break;
                case "3": IfNotEmpty(() => Console.WriteLine(string.Join(", ", _list.Backward()))); break;
                case "4":
                    IfNotEmpty(() =>
                    {
                        _list.SortDescending();
                        Console.WriteLine("Lista ordenada descendentemente:");
                        Console.WriteLine(string.Join(", ", _list.Forward()));
                    });
                    break;
                case "5": IfNotEmpty(ShowModes); break;
                case "6": IfNotEmpty(() => Console.Write(_list.Graph())); break;
                case "7": IfNotEmpty(CheckExists); break;
                case "8": IfNotEmpty(RemoveOne); break;
                case "9": IfNotEmpty(RemoveAll); break;
                case "0": Console.WriteLine("¡Hasta luego!"); break;
                default: Console.WriteLine("Opción no válida."); break;
            }
        } while (option != "0");
    }

    private void AddItems()
    {
        Console.Write("Ingrese uno o varios valores separados por coma: ");
        var parts = (Console.ReadLine() ?? "").Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        int added = 0;
        foreach (var part in parts)
        {
            if (_parser(part, out var value))
            {
                _list.Add(value);
                added++;
            }
            else
            {
                Console.WriteLine($"  \"{part}\" no es un valor válido, se omite.");
            }
        }
        Console.WriteLine($"Se adicionaron {added} elemento(s).");
    }

    private void ShowModes()
    {
        var modes = _list.Modes();
        Console.WriteLine(modes.Count == 1
            ? $"La moda es: {modes[0]}"
            : $"Las modas son: {string.Join(" y ", modes)}");
    }

    private void CheckExists()
    {
        T value = ReadValue("Valor a buscar: ");
        Console.WriteLine(_list.Exists(value)
            ? $"Sí existe el {value} en la lista."
            : $"No existe el {value} en la lista.");
    }

    private void RemoveOne()
    {
        T value = ReadValue("Valor a eliminar (primera ocurrencia): ");
        Console.WriteLine(_list.RemoveFirst(value)
            ? $"Se eliminó una ocurrencia de {value}."
            : $"No existe el {value} en la lista.");
    }

    private void RemoveAll()
    {
        T value = ReadValue("Valor a eliminar (todas las ocurrencias): ");
        int removed = _list.RemoveAll(value);
        Console.WriteLine(removed > 0
            ? $"Se eliminaron {removed} ocurrencia(s) de {value}."
            : $"No existe el {value} en la lista.");
    }

    private T ReadValue(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            if (_parser(Console.ReadLine(), out var value))
            {
                return value;
            }
            Console.WriteLine("Valor no válido, intente de nuevo.");
        }
    }

    private void IfNotEmpty(Action action)
    {
        if (_list.IsEmpty)
        {
            Console.WriteLine("La lista está vacía.");
            return;
        }
        action();
    }
}
