using DoubleListFront;
using System.Globalization;
 
Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("Lista doblemente ligada genérica");
Console.WriteLine("¿Qué tipo de datos va a manejar la lista?");
Console.WriteLine("1. Enteros");
Console.WriteLine("2. Decimales");
Console.WriteLine("3. Texto");
Console.Write("Opción: ");

switch (Console.ReadLine()?.Trim())
{
    case "2":
        // Se usa punto como separador decimal para no chocar con la coma
        // que separa varios valores al adicionar.
        new ListMenu<double>((string? s, out double v) =>
            double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out v)).Run();
        break;

    case "3":
        new ListMenu<string>((string? s, out string v) =>
        {
            v = s?.Trim() ?? "";
            return v.Length > 0;
        }).Run();
        break;

    default:
        new ListMenu<int>(int.TryParse).Run();
        break;
}
