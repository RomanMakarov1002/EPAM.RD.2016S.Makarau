﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace PingPong
{
    class Program
    {
        static void Main(string[] args)
        {
            var start = new ManualResetEventSlim(false);
            var pingEvent = new AutoResetEvent(false);
            var pongEvent = new AutoResetEvent(false);

            CancellationTokenSource cts = new CancellationTokenSource();  // TODO: Create a new cancellation token source.
            CancellationToken token= cts.Token; // TODO: Assign an appropriate value to token variable.
            Action ping = () =>
            {
                Console.WriteLine("ping: Waiting for start.");
                start.Wait();

                bool continueRunning = true;

                while (continueRunning)
                {
                    Console.WriteLine("ping!");

                    // TODO: write ping-pong functionality here using pingEvent and pongEvent here.
                    pongEvent.Set();
                    Thread.Sleep(1000);
                    pingEvent.WaitOne();
                    continueRunning = !token.IsCancellationRequested;
                }

                // TODO: Fix issue with blocked pong task.
                pingEvent.WaitOne();
                Console.WriteLine("ping: done");
            };

            Action pong = () =>
            {
                Console.WriteLine("pong: Waiting for start.");
                start.Wait();

                bool continueRunning = true;

                while (continueRunning)
                {
                    Console.WriteLine("pong!");
                    // TODO: write ping-pong functionality here using pingEvent or pongEvent here.
                    pingEvent.Set();
                    Thread.Sleep(1000);
                    // TODO: write ping-pong functionality here using pingEvent or pongEvent here.
                    pongEvent.WaitOne();
                    continueRunning = !token.IsCancellationRequested;
                }
                // TODO: Fix issue with blocked ping task.
                pongEvent.WaitOne();
                Console.WriteLine("pong: done");
            };


            var pingTask = Task.Run(ping);
            var pongTask = Task.Run(pong);

            Console.WriteLine("Press any key to start.");
            Console.WriteLine("After ping-pong game started, press any key to exit.");
            Console.ReadKey();
            start.Set();

            Console.ReadKey();
            // TODO: cancel both tasks using cancellation token.
            cts.Cancel();
            Console.WriteLine("Press any key to exit.");
            Console.ReadKey();
        }
    }
}
