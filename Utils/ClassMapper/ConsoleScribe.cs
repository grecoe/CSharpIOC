using System;
using System.Collections.Generic;

namespace Utils.ClassMapper
{
    public class ConsoleScribe : IScribe
    {
        private Dictionary<string, DateTime> MethodExecutionTime { get; set; }

        public ConsoleScribe()
        {
            this.MethodExecutionTime = new Dictionary<string, DateTime>();
        }

        public void LogEntry(string method_name, object[] parameters)
        {
            string output_string = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "\t";
            output_string += $"Enter Method > {method_name}, Parameters > ";

            List<string> actual_params = new List<string>();
            if (parameters != null && parameters.Length > 0)
            {
                // We might not be tracking to anything
                foreach (object param in parameters)
                {
                    actual_params.Add($"{param.GetType().Name} = {param.ToString()}");
                }
            }

            if (actual_params.Count > 0)
            {
                output_string += String.Join(',', actual_params);
            }
            else
            {
                output_string += "NONE";
            }

            // Method execution
            if (!this.MethodExecutionTime.ContainsKey(method_name))
            {
                this.MethodExecutionTime.Add(method_name, DateTime.Now);
            }

            this.MethodExecutionTime[method_name] = DateTime.Now;

            Console.WriteLine(output_string);
        }

        public void LogException(string method_name, Exception ex)
        {
            string output_string = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "\t";
            output_string += $"Method Exception > {method_name} : ";
            output_string += ex.ToString();
            Console.WriteLine(output_string);
        }

        public void LogExit(string method_name, object return_value)
        {
            string output_string = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss") + "\t";
            output_string += $"Exit Method > {method_name} ";

            if (this.MethodExecutionTime.ContainsKey(method_name))
            {
                TimeSpan span = DateTime.Now - this.MethodExecutionTime[method_name];
                output_string += $"[{span.TotalMilliseconds}] ";
            }

            this.MethodExecutionTime[method_name] = DateTime.Now;

            output_string += $": Return Value > ";


            if (return_value == null)
            {
                output_string += "null";
            }
            else
            {
                Type returnValueType = return_value.GetType();
                String output_value = return_value.ToString();
                output_string += $"{return_value.GetType().Name} : {output_value}";
            }

            Console.WriteLine(output_string);
        }
    }
}
