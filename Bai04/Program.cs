using System;
using System.Threading;

class Bai04
{
    static readonly object gate = new object();
    static int? largest, secondLargest;
    static int count, last;
    static bool finished;
    static Random random;

    static void Task1()
    {
        while (true)
        {
            int number = random.Next(0, 100001);
            lock (gate)
            {
                last = number;
                count++;
                if (!largest.HasValue || number > largest.Value)
                {
                    secondLargest = largest;
                    largest = number;
                }
                else if (number < largest.Value &&
                    (!secondLargest.HasValue || number > secondLargest.Value))
                    secondLargest = number;
                finished = number > 10000 && number % 2021 == 0;
                Monitor.PulseAll(gate);
                if (finished) break;
            }
            Thread.Sleep(1);
        }
    }

    static void Task2()
    {
        int seen = 0;
        while (true)
        {
            int n, current;
            int? first, second;
            bool done;
            lock (gate)
            {
                while (count == seen && !finished) Monitor.Wait(gate);
                n = count;
                current = last;
                first = largest;
                second = secondLargest;
                done = finished;
                seen = n;
            }
            Console.WriteLine("Task2: lan {0}, vua sinh = {1}, max = {2}, max2 = {3}",
                n, current, first, second.HasValue ? second.Value.ToString() : "chua co");
            Console.Beep(1200, 7);
            if (done) break;
        }
    }

    static int Main(string[] args)
    {
        int seed = Environment.TickCount;
        if (args.Length == 2 && args[0] == "--seed") seed = int.Parse(args[1]);
        else if (args.Length != 0) { Console.Error.WriteLine("Dung: Bai04.exe [--seed N]"); return 1; }
        random = new Random(seed);
        Console.WriteLine("BAI 04 - Hai luong sinh so va hien thi hai gia tri lon nhat.");
        var task1 = new Thread(Task1);
        var task2 = new Thread(Task2);
        task2.Start();
        task1.Start();
        task1.Join();
        task2.Join();
        Console.WriteLine("Task1 dung tai {0} (>10000 va chia het cho 2021).", last);
        Console.WriteLine("Hai tac vu da ket thuc.");
        return 0;
    }
}
