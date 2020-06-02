# ThreadHelper - Easy way to create multithreads

## Usage

```csharp
static void Main(string[] args) {
 var thread = new ThreadHelper() {
  MaxThreads = 10
 };
 thread.WatchCompletedStatus = true;
 thread.DoWork += Thread_DoWork;
 thread.WorkCompleted += Thread_WorkCompleted;
 thread.JobCompleted += Thread_JobCompleted;
 thread.CreateThreads();
 thread.StartAll();
 Console.ReadKey();
}
private static void Thread_JobCompleted(object sender, bool e) {
 Console.WriteLine("All thread completed: " + e);
}
private static void Thread_WorkCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e) {
 var currentThread = (SuperWorker) sender;
 Console.WriteLine(currentThread.Name + " Completed!");
}
private static void Thread_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e) {
 var currentThread = (SuperWorker) sender;
 for (int i = 0; i < 10; i++) {
  Console.WriteLine(currentThread.Name);
  Thread.Sleep(new Random().Next(500, 1000));
 }
}
}
```
