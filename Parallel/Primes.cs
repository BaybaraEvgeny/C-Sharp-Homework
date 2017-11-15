using System;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Collections.Generic;

namespace Parallel
{
    class Program
    {
        static void Main(string[] args)
        {
            
            int start = 1;          //Convert.ToInt32(Console.ReadLine());
            int end = 1000000;      //Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("All prime numbers in the interval from {0} to {1}", start, end);
            Console.WriteLine();

            var mid = start + (end - start) / 2;

            //without parallel
            var timer = Stopwatch.StartNew();
            List<int> nums = PrimeInRange(start, end);
            timer.Stop();

            Console.WriteLine("Without parallel time: {0}", timer.Elapsed);
            Console.WriteLine();

            //parallel by 2 threads
            Console.WriteLine("Thread");

            timer.Restart();             
            
            var threads = new Thread[2];
            threads[0] = new Thread(() => PrimeInRange(start, mid));
            
            threads[1] = new Thread(() => PrimeInRange(mid + 1, end));

            threads[0].Start();
            threads[1].Start();

            threads[0].Join();
            threads[1].Join();

            timer.Stop();

            Console.WriteLine("On threads time: {0}", timer.Elapsed);
            Console.WriteLine();

            //parallel by 2 tasks      
            Console.WriteLine("Tasks");

            timer.Restart();

            var tasks = new Task[2];
            tasks[0] = Task.Factory.StartNew(() => PrimeInRange(start, mid));
            tasks[1] = Task.Factory.StartNew(() => PrimeInRange(mid + 1, end));
            Task.WaitAll(tasks);

            timer.Stop();

            Console.WriteLine("On tasks time: {0}", timer.Elapsed);
            Console.WriteLine();

            //parallel by thread in Pool
            Console.WriteLine("Thread Pool");

            timer.Restart();

            var myEvent = new ManualResetEvent(false);
            ThreadPool.QueueUserWorkItem(delegate
            {
                PrimeInRange(start, end);
                myEvent.Set();
            });
            myEvent.WaitOne();

            timer.Stop();

            Console.WriteLine("On threadpool time: {0}", timer.Elapsed);
            Console.ReadLine();

        }

        static List<int> FindPrimes(int start, int end, int max)
        {
            var thr = Thread.CurrentThread.ManagedThreadId;
            var primes = new List<int>();

            if (thr < max)
            {

                var mid = (start + end) / 2;

                var firstHalf = new List<int>();
                var secondHalf = new List<int>();

                var threads = new Thread[2];

                threads[0] = new Thread(() => firstHalf.AddRange(FindPrimes(start, mid, max)));
                threads[1] = new Thread(() => secondHalf.AddRange(FindPrimes(mid + 1, end, max)));

                threads[0].Start();
                threads[1].Start();

                threads[0].Join();
                threads[1].Join();

                primes.AddRange(firstHalf);
                primes.AddRange(secondHalf);

            }
            else
            {
                primes = PrimeInRange(start, end);
            }

            return primes;
        }

        private static List<int> PrimeInRange(int start, int end)
        {
            var result = new List<int>();

            for (int i = start; i < end; i++)
            {

                if (isPrime(i)) result.Add(i);

            }

            return result;
        }

        private static bool isPrime(int n)
        {

            if (n % 2 == 0 && n != 2 || n == 1) return false;
            for (int i = 3; i <= Math.Round(Math.Sqrt(n)); i = i + 2)
            {
                if (n % i == 0) return false;
            }
            return true;
        }

    }
}
