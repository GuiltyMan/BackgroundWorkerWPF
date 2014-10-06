using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
namespace BackgroundWorkerWPF
{
    /// <summary>
    /// Main Window View Model
    /// </summary>
    class MainWindowViewModel : ViewModelBase
    {
        private BackgroundWorker _backgroundWorker;
        private int _currentProgress;
        
        /// <summary>
        /// Default MainWindowViewModel Constructor.
        /// </summary>
        public MainWindowViewModel()
        {
            // Create BackgroundWorker object.
            _backgroundWorker = new BackgroundWorker();

            // Set BackgroundWorker properties.
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.WorkerSupportsCancellation = true;

            // Connect handlers to BackgroundWorker object.
            _backgroundWorker.DoWork += DoWork_Handler;
            _backgroundWorker.ProgressChanged += ProgressChanged_Handler;
            _backgroundWorker.RunWorkerCompleted += RunWorkerCompleted_Handler;
        }

        /// <summary>
        /// Process Command
        /// </summary>
        public ICommand ProcessCommand
        {
            get
            {
                // return the Process relay command
                return new RelayCommand(Process, CanExecuteProcess);
            }
        }

        /// <summary>
        /// Cancel Command
        /// </summary>
        public ICommand CancelCommand
        {
            get
            {
                // return the Cancel relay command
                return new RelayCommand(Cancel, CanExecuteCancel);
            }
        }

        /// <summary>
        /// Current Progress
        /// </summary>
        public int CurrentProgress
        {
            get
            {
                // return current progress
                return _currentProgress;
            }
            set
            {
                // validate if the current progress is not equal with the given value
                if (_currentProgress != value)
                {
                    // Update the progress value and raise property changed event on current progress
                    _currentProgress = value;
                    OnPropertyChanged("CurrentProgress");
                }
            }
        }

        /// <summary>
        /// Process
        /// </summary>
        private void Process()
        {
            // Validate if the worker is not busy
            if (!_backgroundWorker.IsBusy)
            {
                // Run the Asychronous worker
                _backgroundWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Determine if the Process can be Executed.
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteProcess()
        {
            return true;
        }

        /// <summary>
        /// Cancel
        /// </summary>
        private void Cancel()
        {
            // Cancel the Asynchronous worker
            _backgroundWorker.CancelAsync();
        }

        /// <summary>
        /// Determine if the Cancel can be Executed
        /// </summary>
        /// <returns></returns>
        private bool CanExecuteCancel()
        {
            // Just return true for now
            return true;
        }

        /// <summary>
        /// Progress Changed Event Handler
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="args">Progress Changed Event Arguments</param>
        private void ProgressChanged_Handler(object sender, ProgressChangedEventArgs args)
        {
            // Update the current progress value
            CurrentProgress = args.ProgressPercentage;
        }

        /// <summary>
        /// Do Work Event Handler
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="args">Do Work Event Arguments</param>
        private void DoWork_Handler(object sender, DoWorkEventArgs args)
        {
            // Casting the sender as BackgroundWorker
            BackgroundWorker worker = sender as BackgroundWorker;

            // Simple loop to show the progress
            for (int i = 1; i <= 10; i++)
            {
                // Always check if the process was cancelled
                if (worker.CancellationPending)
                {
                    // Break the process and set the Cancel flag as true
                    args.Cancel = true;
                    break;
                }
                else
                {
                    // Update the Progress Bar
                    worker.ReportProgress(i * 10);

                    // Sleep for 500ms to show the changes
                    Thread.Sleep(500);
                }
            }
        }

        /// <summary>
        /// Run Worker Completed Event Handler
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="args">Run Worker Completed Event Args</param>
        private void RunWorkerCompleted_Handler(object sender, RunWorkerCompletedEventArgs args)
        {
            // Reset the Progress Bar
            CurrentProgress = 0;

            // Validate if the process is cancelled
            if (args.Cancelled)
            {
                // Tell the user that the process was cancelled
                MessageBox.Show("Process was cancelled.", "Process Cancelled");
            }
            else
            {
                // Tell the user that the process completed normally
                MessageBox.Show("Process completed normally.", "Process Completed");
            }
        }
    }
}
