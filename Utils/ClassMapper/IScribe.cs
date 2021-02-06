using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ClassMapper
{
    public interface IScribe
    {
        void LogEntry(string method_name, object[] parameters);
        void LogException(string method_name, Exception ex);
        void LogExit(string method_name, object return_value);
    }
}
