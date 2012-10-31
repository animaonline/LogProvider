using System;
using System.Diagnostics;

namespace Animaonline.Utils.Logging
{
    [DebuggerStepThrough]
    public class LogProvider
    {
        #region Public Constructors

        public LogProvider(LogReceiver receiver)
        {
            this.Receiver = receiver;
        }

        public LogProvider(Action<LogEntry> onReceive)
        {
            this.Receiver = new LogReceiver(onReceive);
        }

        #endregion

        #region Public Properties

        public LogReceiver Receiver { get; set; }

        #endregion

        #region Public Methods

        #region StackTrace

        public void StackTrace(LogEntryType entryType, string value, string tag = null, Exception exception = null)
        {
            var logEntry = new LogEntry(entryType, value, tag, exception);

            var capturedStackTrace = new StackTrace(1); //1: skip this method.

            logEntry.StackTrace = capturedStackTrace;

            this.Log(logEntry);
        }

        #endregion

        #region Log

        public void Log(LogEntry logEntry)
        {
            this.Receiver.SignalReceive(logEntry);
        }

        public void Log(LogEntryType entryType, string value, string tag = null, Exception exception = null)
        {
            var logEntry = new LogEntry(entryType, value, tag, exception);

            this.Receiver.SignalReceive(logEntry);
        }

        #endregion

        #region Debug

        public void Debug(LogEntry logEntry)
        {
            this.Log(logEntry);
        }

        public void Debug(string value, string tag = null, Exception exception = null)
        {
            this.Log(LogEntryType.DEBUG, value, tag, exception);
        }

        #endregion

        #region Info

        public void Info(LogEntry logEntry)
        {
            this.Log(logEntry);
        }

        public void Info(string value, string tag = null, Exception exception = null)
        {
            this.Log(LogEntryType.INFO, value, tag, exception);
        }

        #endregion

        #region Warn

        public void Warn(LogEntry logEntry)
        {
            this.Log(logEntry);
        }

        public void Warn(string value, string tag = null, Exception exception = null)
        {
            this.Log(LogEntryType.WARN, value, tag, exception);
        }

        #endregion

        #region Error

        public void Error(LogEntry logEntry)
        {
            this.Log(logEntry);
        }

        public void Error(string value, string tag = null, Exception exception = null)
        {
            this.Log(LogEntryType.ERROR, value, tag, exception);
        }

        #endregion

        #region Fatal

        public void Fatal(LogEntry logEntry)
        {
            this.Log(logEntry);
        }

        public void Fatal(string value, string tag = null, Exception exception = null)
        {
            this.Log(LogEntryType.DEBUG, value, tag, exception);
        }

        #endregion

        #endregion

        #region Child Classes

        public enum LogEntryType
        {
            DEBUG,
            INFO,
            WARN,
            ERROR,
            FATAL,
            UNKNOWN
        }

        public class LogReceiver
        {
            #region Public Constructors

            public LogReceiver(Action<LogEntry> onReceive)
            {
                if (onReceive == null)
                    throw new ArgumentException("No action subscriber provided.");
                this.OnReceive = onReceive;
            }

            #endregion

            #region Public Fields

            public Action<LogEntry> OnReceive { get; set; }

            #endregion

            #region Private Methods

            public void SignalReceive(LogEntry logEntry)
            {
                if (this.OnReceive != null)
                    this.OnReceive(logEntry);
            }

            #endregion
        }

        public class LogEntry
        {
            #region Public Constructors

            public LogEntry(LogEntryType entryType, string value, string tag = null, Exception error = null)
            {
                this.TimeStamp = DateTime.Now;

                this.EntryType = entryType;

                this.Value = value;

                if (!string.IsNullOrEmpty(tag))
                    this.Tag = tag;

                if (error != null)
                    this.Error = error;
            }

            #endregion

            #region Public Properties

            public LogEntryType EntryType { get; set; }
            public string Value { get; set; }
            public DateTime TimeStamp { get; set; }
            public string Tag { get; set; }
            public Exception Error { get; set; }
            public StackTrace StackTrace { get; set; }

            public bool HasError
            {
                get { return Error != null; }
            }

            public bool HasStackTrace
            {
                get { return this.StackTrace != null; }
            }

            #endregion

            #region Overridden Methods

            public override string ToString()
            {
                if (!string.IsNullOrEmpty(this.Value) && this.EntryType != LogEntryType.UNKNOWN && string.IsNullOrEmpty(this.Tag))
                    return string.Format("{0} [{1}] - {2}", this.TimeStamp, this.EntryType, this.Value);
                if (!string.IsNullOrEmpty(this.Value) && this.EntryType != LogEntryType.UNKNOWN && !string.IsNullOrEmpty(this.Tag))
                    return string.Format("{0} [{1} ({2})] - {3}", this.TimeStamp, this.EntryType, this.Tag, this.Value);

                return base.ToString();
            }

            #endregion
        }

        #endregion
    }
}