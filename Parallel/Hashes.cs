using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Hashes
{
    class Program
    {
        static void Main(string[] args)
        {
            
            var path = "C:\\jetbrains";
            
            Stopwatch timer = Stopwatch.StartNew();

            var str = GetHash(path);
            
            timer.Stop();
            
            Console.WriteLine("Hash: " + str);
            Console.WriteLine("Time elapsed: {0}", timer.Elapsed);

        }

        static string GetHash(string path)
        {
            return HashToStr(CollectHash(path));
        }

        static string HashToStr(byte[] hash)
        {
            StringBuilder strB = new StringBuilder();
            foreach (byte b in hash)
            {
                strB.Append(b.ToString("X2"));
            }
            return strB.ToString();
        }

        static byte[] CollectHash(string dir)
        {
            StringBuilder strB = new StringBuilder(dir.Split('\\').Last());
            
            var files = Directory.GetFiles(dir);
            var folders = Directory.GetDirectories(dir);
            
            List<Task<byte[]>> tasks = new List<Task<byte[]>>();

            foreach (var folder in folders)
            {
                tasks.Add(Task<byte[]>.Run( () => CollectHash(folder) ));
            }
            
            using (MD5 md5 = MD5.Create())
            {
                foreach (var file in files)
                {
                    strB.Append(HashToStr(md5.ComputeHash(File.OpenRead(file))));
                }
            }

            Task.WaitAll(tasks.ToArray());

            foreach (var task in tasks)
            {
                strB.Append(HashToStr(task.Result));
            }

            using (MD5 md5 = MD5.Create())
            {
                return md5.ComputeHash(Encoding.UTF8.GetBytes(strB.ToString()));
            }

        }

    }
}
