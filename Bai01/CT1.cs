using System;
using System.IO;
using System.Threading;

class CT1
{
    static bool sendStop;
    static Random random;
    static int exitCode;

    static void WriteNumber(int number)
    {
        while (true)
        {
            try
            {
                using (var file = new FileStream("dulieu.dat", FileMode.Create,
                    FileAccess.Write, FileShare.None))
                using (var writer = new BinaryWriter(file))
                    writer.Write(number);
                return;
            }
            catch (IOException ex)
            {
                int code = ex.HResult & 0xffff;
                if (code != 32 && code != 33) throw;
                Thread.Sleep(5); 
            }
        }
    }

    static void Generate()
    {
        try
        {
            while (true)
            {
                int number = random.Next(0, int.MaxValue);
                if (number % 2021 == 0)
                {
                    Console.WriteLine("CT1: gap {0}, chia het cho 2021.", number);
                    if (sendStop)
                    {
                        WriteNumber(number);
                        Console.WriteLine("CT1: da ghi so ket thuc cho CT2 (--send-stop).");
                    }
                    break;
                }
                WriteNumber(number);
                Console.WriteLine("CT1 ghi: {0}", number);
                Thread.Sleep(20);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("CT1: " + ex.Message);
            exitCode = 1;
        }
    }

    static int Main(string[] args)
    {
        int seed = Environment.TickCount;
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == "--send-stop") sendStop = true;
            else if (args[i] == "--seed" && i + 1 < args.Length)
                seed = int.Parse(args[++i]);
            else { Console.Error.WriteLine("Dung: CT1.exe [--send-stop] [--seed N]"); return 1; }
        }
        random = new Random(seed);
        var worker = new Thread(Generate);
        worker.Start();
        worker.Join();
        return exitCode;
    }
}
