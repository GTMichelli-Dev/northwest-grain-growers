using System;

class Program
{
    static void Main(string[] args)
    {
        // Replace with your actual IP, port, and slaveId as needed
        var vegaReader = new VegaReader("10.0.1.68", 50000, 1);
        double pv = vegaReader.ReadPrimaryVariable();
        Console.WriteLine($"Primary Variable: {pv}");
    }
}