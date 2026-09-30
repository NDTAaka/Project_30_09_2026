using System;
using System.Threading;

class Bai03
{
    static volatile char c = '\0';
    static int inputStarted, beepStarted;
    static readonly ManualResetEvent inputDone = new ManualResetEvent(false);
    static readonly ManualResetEvent beepDone = new ManualResetEvent(false);
    static int exitCode;

    static void Timer1(object state)
    {
        if (Interlocked.Exchange(ref inputStarted, 1) != 0) return;
        try
        {
            while (c != '*')
            {
                if (!Console.IsInputRedirected && !Console.KeyAvailable)
                {
                    Thread.Sleep(15);
                    continue;
                }
                int value = Console.IsInputRedirected ? Console.Read() : Console.ReadKey(true).KeyChar;
                if (value == -1) { c = '*'; Console.WriteLine("EOF: dung timer."); break; }
                c = (char)value;
                Console.WriteLine("Timer1: ky tu [{0}], hexa = 0x{1:X4}", c, (int)c);
                Thread.Sleep(15);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Timer1: " + ex.Message);
            exitCode = 1;
            c = '*';
        }
        finally { inputDone.Set(); }
    }

    static void Timer2(object state)
    {
        if (Interlocked.Exchange(ref beepStarted, 1) != 0) return;
        try
        {
            while (c != '*')
            {
                Console.Beep(1000, 7);
                if (c == '*') break;
                Thread.Sleep(7);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Timer2: " + ex.Message);
            exitCode = 1;
            c = '*';
        }
        finally { beepDone.Set(); }
    }

    static void DisposeAndWait(Timer timer)
    {
        using (var disposed = new ManualResetEvent(false))
        {
            timer.Dispose(disposed);
            disposed.WaitOne();
        }
    }

    static int Main()
    {
        Console.WriteLine("BAI 03 - Timer1: 15 ms; Timer2: 7 ms. Nhan '*' de dung.");
        var timer1 = new Timer(Timer1, null, 0, 15);
        var timer2 = new Timer(Timer2, null, 0, 7);
        inputDone.WaitOne();
        beepDone.WaitOne();
        DisposeAndWait(timer1);
        DisposeAndWait(timer2);
        Console.WriteLine("Timer1 va Timer2 da ket thuc.");
        inputDone.Dispose();
        beepDone.Dispose();
        return exitCode;
    }
}
