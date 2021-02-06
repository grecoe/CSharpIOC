using System;
using System.Collections.Generic;
using System.Text;

namespace Utils.ClassMapper
{
    public interface IScribe
    {
        void LogEntry(string method_name, object[] parameters, object calling_object = null);
        void LogException(string method_name, Exception ex, object calling_object = null);
        void LogExit(string method_name, object return_value, object calling_object = null);
    }
}
