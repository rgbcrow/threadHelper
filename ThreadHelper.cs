using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Worker
{
    public class ThreadHelper
    {
        private int max_thread = 1;
        private List<SuperWorker> workers;
        private bool watchCompletedStatus;

        public int MaxThreads { get => max_thread; set => max_thread = value; }
        public List<SuperWorker> Threads { get => workers; set => workers = value; }
        public bool WatchCompletedStatus { get => watchCompletedStatus; set => watchCompletedStatus = value; }

        public ThreadHelper()
        {
            workers = new List<SuperWorker>();
        }

        public void CreateThreads()
        {
            if (BusyCount > 0)
                return;

            for(int i = 0; i < max_thread; i++)
            {
                var worker = new SuperWorker()
                {
                    Name = "Thread " + i.ToString(),
                    ID = i,
                    WorkerSupportsCancellation = true,
                    WorkerReportsProgress = true
                };

                worker.DoWork += Worker_DoWork;
                worker.ProgressChanged += Worker_ProgressChanged;
                worker.RunWorkerCompleted += Worker_RunWorkerCompleted;

                workers.Add(worker);
            }
        }

        public void StartAll()
        {
            workers.ForEach((t) => 
            { 
                if(!t.IsBusy)
                {
                    t.RunWorkerAsync();
                }
            });

            if(watchCompletedStatus)
            {
                var watcher = new Thread(() => {

                    while (this.Completed != true)
                    {
                        Thread.Sleep(1000);
                    }
                    JobCompleted?.Invoke(this, true);
                });

                watcher.Start();
            }
        }

        public void StopAll()
        {
            if (workers == null)
                throw new NullReferenceException();

            workers.ForEach((worker) => 
            {
                if (worker.IsBusy || worker.CancellationPending == false)
                    worker.CancelAsync();
            });
        }

        public int Count
        {
            get
            {
                return workers.Count;
            }
        }

        public int BusyCount
        {
            get
            {
                return workers.Count((t) => t.IsBusy == true);
            }
        }

        public bool Completed
        {
            get
            {
                return !workers.Any((t) => t.IsBusy == true);
            }
        }


        protected virtual void Worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            WorkCompleted?.Invoke(sender, e);
        }

        protected virtual void Worker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            ProgressChanged?.Invoke(sender, e);
        }

        protected virtual void Worker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            DoWork?.Invoke(sender, e);
        }

        public event EventHandler<System.ComponentModel.DoWorkEventArgs> DoWork;
        public event EventHandler<System.ComponentModel.RunWorkerCompletedEventArgs> WorkCompleted;
        public event EventHandler<System.ComponentModel.ProgressChangedEventArgs> ProgressChanged;
        public event EventHandler<bool> JobCompleted;
    }
}
