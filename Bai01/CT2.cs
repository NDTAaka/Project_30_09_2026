using System;
using System.IO;
using System.Threading;

class CT2
{
    static int exitCode;

    static void ReadNumbers()
    {
        try
        {
            while (true)
            {
                int number;
                try
                {
                    using (var file = new FileStream("dulieu.dat", FileMode.Open,
                        FileAccess.Read, FileShare.None))
                    using (var reader = new BinaryReader(file))
                    {
                        if (file.Length != 4) { Thread.Sleep(5); continue; }
                        number = reader.ReadInt32();
                    } // Dong file truoc khi hien thi.
                }
                catch (FileNotFoundException) { Thread.Sleep(10); continue; }
                catch (IOException ex)
                {
                    int code = ex.HResult & 0xffff;
                    if (code != 32 && code != 33) throw;
                    Thread.Sleep(5);
                    continue;
                }
                Console.WriteLine("CT2 doc: {0}", number);
                if (number % 2021 == 0) break;
                Thread.Sleep(10);
            }
            Console.WriteLine("CT2: dung vi so doc duoc chia het cho 2021.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("CT2: " + ex.Message);
            exitCode = 1;
        }
    }

    static int Main()
    {
        var worker = new Thread(ReadNumbers);
        worker.Start();
        worker.Join();
        return exitCode;
    }
}
