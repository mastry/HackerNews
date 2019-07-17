using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HackerNews.MVVM
{
    sealed class AsyncDataBinding<T> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public AsyncDataBinding(Task<T> task)
        {
            this.Task = task;
            if(!task.IsCompleted)
            {
                var _ = WatchTaskAsync(task);
            }
        }

        async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch 
            {
                // Don't throw an exception on the UI thread - 
                //      error handling should be accomplished through data binding on this object's properties
            }

            if (PropertyChanged == null) return;

            NotifyPropChanged(nameof(Status));
            NotifyPropChanged(nameof(IsCompleted));

            if(task.IsCanceled)
                NotifyPropChanged(nameof(IsCanceled));

            if (task.IsFaulted)
            {
                NotifyPropChanged(nameof(IsFaulted));
                NotifyPropChanged(nameof(Exception));
                NotifyPropChanged(nameof(InnerException));
                NotifyPropChanged(nameof(ErrorMessage));
            }
            else
            {
                NotifyPropChanged(nameof(Result));
            }
        }

        public Task<T> Task { get; set; }
        public T Result
        {
            get
            {
                return Task.Status == TaskStatus.RanToCompletion ? Task.Result : default(T);
            }
        }
        public TaskStatus Status { get { return Task.Status; } }
        public bool IsCompleted { get { return Task.IsCompleted; } }
        public bool IsCanceled { get { return Task.IsCanceled; } }
        public bool IsFaulted { get { return Task.IsFaulted; } }
        public bool RanToCompletion { get { return Task.Status == TaskStatus.RanToCompletion; } }
        public AggregateException Exception { get { return Task.Exception; } }
        public Exception InnerException { get { return Exception?.InnerException ?? Exception; } }
        public string ErrorMessage { get { return InnerException?.Message; } }

        void NotifyPropChanged(string name)
        {
            var args = new PropertyChangedEventArgs(name);
            PropertyChanged?.Invoke(this, args);
        }
    }
}
