﻿using System;
using System.IO;
using System.Threading.Tasks;
using Tracer;

namespace LocalTracer
{
    class Program
    {
        static async void DoWork()
        {
            var tracker = new LocalTransactionRecorder(30, Directory.GetCurrentDirectory());
            tracker.CreateFile(0, "test");
            tracker.ModifyFile(5, "test", 0, 0, "Wwow");
            tracker.ModifyFile(35, "test", 0, 0, "NOICE");
            tracker.SaveChanges();

            var project = tracker.Project;

            var loader = new LocalTransactionLoader(Directory.GetCurrentDirectory());
            var transactionLogs = await loader.GetTransactionLogs(project, 0, 100);
            Console.WriteLine(project.Id);
            foreach (var transactionLog in transactionLogs)
            {
                Console.WriteLine(transactionLog.Partition);
                foreach (var transaction in transactionLog.Transactions)
                {
                    Console.WriteLine(transaction.Type);
                    Console.WriteLine(transaction.ToString());
                }
            }
        }
        //static string ProjectId { get; set; } = "82e9272f-c9ef-468b-a870-b0244c4fd393";
        static void Main(string[] args)
        {
            var task = Task.Run(() => DoWork());
            Task.WaitAll(task);
        }
    }
}
