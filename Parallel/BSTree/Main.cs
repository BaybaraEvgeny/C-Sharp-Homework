using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BinaryTree
{
    class Program
    {
        public static void Main(string[] args)
        {
          
            Tree tree = new Tree();
            int n = 100;

            int k = 2;

            Console.WriteLine("Choose action:");
            Console.WriteLine("1. Performance test");
            Console.WriteLine("2. Sequential control menu");
            try
            {
                k = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            switch (k)
            {
                case 1:
                    Random random = new Random();
                    int r;
                    
                    Stopwatch timer = Stopwatch.StartNew();

                    for (int i = 0; i < n; i++)
                    {
                        r = random.Next(n);
                        tree.insert(r, r);
                    }
                    timer.Stop();
                    
                    Console.WriteLine("Sequential insertion of {0} elemements is done", n);
                    Console.WriteLine("Time elapsed: {0}", timer.Elapsed);
                    tree.PrintTree(tree.root);
                    
                    tree = new Tree();
                    timer.Restart();

                    var tasks = new List<Task>();
                    for (int i = 0; i < n; i++)
                    {
                        r = random.Next(n);
                        var task = new Task(() => tree.insert(r, r));
                        tasks.Add(task);
                        task.Start();
                        Console.Write(" ");
                    }
                    Task.WaitAll(tasks.ToArray());
                    timer.Stop();
                    
                    
                    Console.WriteLine();
                    Console.WriteLine("Parallel insertion of {0} elemements is done", n);
                    Console.WriteLine("Time elapsed: {0}", timer.Elapsed);
                    tree.PrintTree(tree.root);

                    Console.ReadKey();
                    
                    break;
                case 2:
                    while (true)
                    {
                        Console.WriteLine("Choose action:");
                        Console.WriteLine("1. Insert");
                        Console.WriteLine("2. Delete");
                        Console.WriteLine("3. Search");
                        Console.WriteLine("4. Print tree");

                        int s;
                        try
                        {
                            s = Convert.ToInt32(Console.ReadLine());
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            continue;
                        }

                        switch (s)
                        {
                            case 1:
                                Console.WriteLine("Enter the key");
                                int ins;
                                try
                                {
                                    ins = Convert.ToInt32(Console.ReadLine());
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    continue;
                                }
                                tree.insert(ins, ins);
                                break;
                            case 2:
                                Console.WriteLine("Enter the key");
                                int del;
                                try
                                {
                                    del = Convert.ToInt32(Console.ReadLine());
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    continue;
                                }
                                if (tree.SearchNode(del) != null)
                                {
                                    tree.delete(del);
                                    Console.WriteLine(del + " is deleted");
                                }
                                else Console.WriteLine("Not found");
                                break;
                            case 3:
                                Console.WriteLine("Enter the data");
                                int find;
                                try
                                {
                                    find = Convert.ToInt32(Console.ReadLine());
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine(e);
                                    continue;
                                }
                                if (tree.SearchNode(find) == null) Console.WriteLine("Not found");
                                else Console.WriteLine("Found " + find);
                                break;
                            case 4:
                                Console.WriteLine("-------------------------");
                                tree.PrintTree(tree.root);
                                Console.WriteLine("-------------------------");
                                break;
                            default:
                                continue;
                        }
                    }
            }

        }
    }
}
