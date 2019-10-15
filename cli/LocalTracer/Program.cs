using System;
using System.IO;
using Tracer;

namespace LocalTracer
{
    class Program
    {
        //static string ProjectId { get; set; } = "82e9272f-c9ef-468b-a870-b0244c4fd393";
        static void Main(string[] args)
        {
            var tracker = new LocalTransactionTracker(30, Directory.GetCurrentDirectory());
            tracker.CreateFile(0, "test");
            tracker.InsertFile(5, "test", 0, 0, "Wwow");
            tracker.InsertFile(35, "test", 0, 0, "NOICE");
            tracker.SaveChanges();

            var projectId = tracker.Project.Id;

            var loader = new LocalTransactionLoader(Directory.GetCurrentDirectory());
            var project = loader.LoadProject(Guid.Parse(projectId));
            var transactionLogs = loader.GetTransactionLogs(project, 0, 100);
            Console.WriteLine(project.Id);
            foreach(var transactionLog in transactionLogs)
            {
                Console.WriteLine(transactionLog.Partition);
                foreach (var transaction in transactionLog.Transactions)
                {
                    Console.WriteLine(transaction.Type);
                    Console.WriteLine(transaction.ToString());
                }
            }
        }
    }
}
