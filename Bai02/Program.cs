using System;
using System.Threading;

class Bai02
{
    static string st = ""; 
    static readonly object gate = new object();

    static void Task1()
    {
        while (true)
        {
            string line = Console.ReadLine();
            string value = line == null ? "bye" : line.Trim(' ');
            lock (gate) st = value;
            if (value == "bye") break;
        }
    }

    static void Task2()
    {
        while (true)
        {
            string value;
            lock (gate) value = st;
            Console.WriteLine("Task2: st = [{0}]", value);
            if (value == "bye") break;
            Thread.Sleep(200); 
        }
    }

    static void Main()
    {
        Console.WriteLine("BAI 02 - Nhap xau, nhap 'bye' de dung .");
        var task1 = new Thread(Task1);
        var task2 = new Thread(Task2);
        task2.Start();
        task1.Start();
        task1.Join();
        task2.Join();
        Console.WriteLine("Hai tac vu da ket thuc.");
    }
}
