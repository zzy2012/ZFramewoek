namespace Util
{
    using System.IO;
    using System.Threading;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    using UnityEngine;
    using System.Diagnostics;

    public interface ILogMessage
    {
        void LogFormat(string format, params object[] contents);
        void Result(StreamWriter stream);
    }

    public interface ILogHandler
    {
    }

    public class LogMessage : ILogMessage
    {
        private ILogHandler log_handler_ = null;
        private string who_;
        private string file_;
        private int line_;
        private string thread_name_;
        private string format_;
        private object[] contents_;
        private Mutex message_locker_ = new Mutex();

        public LogMessage(ILogHandler logHandler,
            string whoCallMe,
            string filePath,
            int lineNumber, 
            string threadName)
        {
            log_handler_ = logHandler;
            who_ = whoCallMe;
            file_ = filePath;
            line_ = lineNumber;
            thread_name_ = threadName;
        }

        public void LogFormat(string format, params object[] contents)
        {
            message_locker_.WaitOne();
            format_ = format;
            contents_ = contents;
            message_locker_.ReleaseMutex();
        }

        public void Result(StreamWriter streamWriter)
        {
            lock (message_locker_)
            {
                var date = System.DateTime.Now;
                streamWriter.Write(string.Format("[{0}-{1} {2}:{3}:{4}.{5}]",
                    date.Month, date.Day,
                    date.Hour, date.Minute, date.Second, date.Millisecond));
                streamWriter.Write(string.Format("[{0}]-[{1}:{2}][{3}] ", thread_name_, Path.GetFileName(file_), line_, who_));
                streamWriter.Write(string.Format(format_, contents_));
                streamWriter.Write("\n");
                streamWriter.Flush();
            }
        }
    }

    public class Logger : ILogHandler
    {
        private readonly string logger_base_floder_;
        private Thread logger_looper_;
        private readonly EventWaitHandle message_event_ = new AutoResetEvent(false);
        private bool exit_ = false;
        private List<ILogMessage> messages_ = new List<ILogMessage>();
        private Mutex messages_locker_ = new Mutex();
        private string current_file_name_;

        private static Logger kInstance = null;

        public static void Init()
        {
            if (kInstance != null)
                return;
            kInstance = new Logger();
            kInstance.InternalInit();
        }

        public static void Fini()
        {
            if (kInstance == null)
                return;
            kInstance.InternalFini();
            kInstance = null;
        }

        //public static ILogMessage Trace([CallerMemberName] string whoCallMe = "",
        //    [CallerFilePath] string callerFilePath = "",
        //    [CallerLineNumber] int callerLineNumber = 0)
        //{
        //    if (kInstance == null)
        //        return null;
        //    return kInstance.InternalTrace(whoCallMe, callerFilePath, callerLineNumber);
        //}

        public static Logger Instance
        {
            get { return kInstance; }
        }

        private Logger()
        {
            exit_ = false;
            logger_base_floder_ = Application.persistentDataPath;
        }

        public static string CurrentFile()
        {
            return kInstance.current_file_name_;
        }

        public void Reset()
        {
            if (kInstance == null)
                return;
            kInstance.InternalReset();
        }

        public static void LogFormat(string msgFormat, params object[] contents)
        {
            kInstance.logFormat(msgFormat, contents);
        }

        private void logFormat(string msgFormat, params object[] contents)
        {
            messages_locker_.WaitOne();
            StackFrame stackFrame = new StackFrame(2,true);
            int callerLineNumber = stackFrame.GetFileLineNumber();
            string callerFilePath = stackFrame.GetFileName();
            string whoCallMe = stackFrame.GetMethod().Name;
            string threadName = Thread.CurrentThread.ManagedThreadId.ToString();
            ILogMessage newMessage = new LogMessage(this, whoCallMe, callerFilePath, callerLineNumber, threadName);
            newMessage.LogFormat(msgFormat, contents);
            messages_.Add(newMessage);
            message_event_.Set();
            messages_locker_.ReleaseMutex();
        }

        private void InternalInit()
        {
            exit_ = false;
            if (logger_looper_ == null)
                logger_looper_ = new Thread(onLoggerLooper);
            logger_looper_.Start();
        }

        private void InternalFini()
        {
            exit_ = true;
            if (logger_looper_ != null)
            {
                message_event_.Set();
                logger_looper_.Join();
                message_event_.Reset();
            }

            logger_looper_ = null;
        }

        private void InternalReset()
        {
            InternalFini();
            InternalInit();
        }

        //private ILogMessage InternalTrace(string whoCallMe, string callerFilePath, int callerLineNumber)
        //{
        //    messages_locker_.WaitOne();
        //    ILogMessage newMessage = new LogMessage(this, whoCallMe, callerFilePath, callerLineNumber);

        //    messages_.Add(newMessage);
        //    message_event_.Set();
        //    messages_locker_.ReleaseMutex();
        //    return newMessage;
        //}

        private void onLoggerLooper()
        {
            var fileStream = getLoggerFile();
            var streamWriter = new StreamWriter(fileStream);
            UnityEngine.Debug.Assert(fileStream != null);
            List<ILogMessage> messageSwapper = new List<ILogMessage>();
            while (!exit_)
            {
                if (messages_.Count <= 0 && !exit_)
                {
                    //message_event_.WaitOne();
                    Thread.Sleep(5);
                    continue;
                }

                if (exit_)
                    break;

                messages_locker_.WaitOne();
                {
                    var messages = messages_;
                    messages_ = messageSwapper;
                    messageSwapper = messages;
                    message_event_.Reset();
                }
                messages_locker_.ReleaseMutex();

                foreach (var message in messageSwapper)
                {
                    try
                    {
                        message.Result(streamWriter);
                    }
                    catch
                    { }
                    finally
                    {
                        streamWriter.Flush();
                    }
                }
                messageSwapper.Clear();
            }

            try
            {
                streamWriter.Flush();
                fileStream.Close();
            }
            catch
            {
            }
        }

        private FileStream getLoggerFile()
        {
            string loggerDirectory = Path.Combine(logger_base_floder_, "logger");
            if (!Directory.Exists(loggerDirectory))
                Directory.CreateDirectory(loggerDirectory);
            System.DateTime nowTime = System.DateTime.Now;
            string timeString = nowTime.ToString("yyyyMMddHHmm");
            current_file_name_ = string.Format("{0}.logger", timeString);
            string loggerFile = Path.Combine(loggerDirectory, current_file_name_);
            UnityEngine.Debug.LogFormat("loggerFile={0}", loggerFile);
            return File.Open(loggerFile, FileMode.OpenOrCreate | FileMode.Append, FileAccess.Write, FileShare.Read);
        }
    }
}

