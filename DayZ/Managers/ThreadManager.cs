using System;
using System.Collections.Generic;
using System.Threading;

namespace ChubbyQuokka.DayZ.Managers
{
    internal static class ThreadManager
    {
        static Queue<Action> ThreadedQueue = new Queue<Action>();
        static Queue<Action> ThreadedBuffer = new Queue<Action>();
        static object ThreadedLock = new object();

        static Queue<Action> MainQueue = new Queue<Action>();
        static Queue<Action> MainBuffer = new Queue<Action>();
        static object MainLock = new object();

        static Thread WorkerThread;
        static bool RunThreads = false;

        public static bool IsWorkerThread => Thread.CurrentThread.ManagedThreadId == WorkerThread.ManagedThreadId;

        public static void Initialize()
        {
            RunThreads = true;

            WorkerThread = new Thread(ThreadedWork)
            {
                Name = "SilverWolf.Dayz_WorkerThread"
            };

            WorkerThread.Start();
        }

        public static void Destroy()
        {
            RunThreads = false;

            if (WorkerThread != null && WorkerThread.IsAlive)
            {
                WorkerThread.Join();
            }

            WorkerThread = null;

            ThreadedQueue.Clear();
            ThreadedBuffer.Clear();

            MainQueue.Clear();
            MainBuffer.Clear();
        }

        public static void Update()
        {
            lock (MainLock)
            {
                while (MainBuffer.Count != 0)
                {
                    MainQueue.Enqueue(MainBuffer.Dequeue());
                }
            }

            while (MainQueue.Count != 0)
            {
                MainQueue.Dequeue().Invoke();
            }
        }

        static void ThreadedWork()
        {
            while (RunThreads)
            {
                Thread.Sleep(50);

                lock (ThreadedLock)
                {
                    while (ThreadedBuffer.Count != 0)
                    {
                        ThreadedQueue.Enqueue(ThreadedBuffer.Dequeue());
                    }
                }

                while (ThreadedQueue.Count != 0)
                {
                    ThreadedQueue.Dequeue().Invoke();
                }
            }
        }

        public static void ExecuteMain(Action action)
        {
            if (IsWorkerThread)
            {
                lock (MainLock)
                {
                    MainBuffer.Enqueue(action);
                }
            }
            else
            {
                action.Invoke();
            }
        }

        public static void ExecuteWorker(Action action)
        {
            if (!IsWorkerThread)
            {
                lock (ThreadedLock)
                {
                    ThreadedBuffer.Enqueue(action);
                }
            }
            else
            {
                action.Invoke();
            }
        }
    }
}
