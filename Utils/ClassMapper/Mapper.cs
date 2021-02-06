using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Utils.ClassMapper
{
    class Mapped
    {
        public MethodBase Method { get; set; }
        public Type ReturnType { get; set; }
    }

    public class Mapper<T, TScribe>
        where T : notnull
        where TScribe : IScribe
    {

        private T Parent { get; set; }
        private TScribe Scribe { get; set; }

        private Dictionary<string, Mapped> Mapped { get; set; }

        public Mapper(T parent, TScribe scribe)
        {
            this.Parent = parent;
            this.Scribe = scribe;
            this.Mapped = new Dictionary<string, Mapped>();

            this._CollectMethodAttributes();
        }

        private void _CollectMethodAttributes()
        {
            Type parentType = this.Parent.GetType();

            // MethodInfo[] parentMethods = parentType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
            MethodInfo[] parentMethods = (MethodInfo[])parentType.GetRuntimeMethods();

            foreach (MethodInfo info in parentMethods)
            {
                if (!this.Mapped.ContainsKey(info.Name))
                {
                    Mapped mappedItem = new Mapped() { Method = info, ReturnType = info.ReturnType };
                    this.Mapped.Add(info.Name, mappedItem);
                }
            }
        }

        public object ExecuteMethod(string name, object[] method_params = null)
        {
            object returnValue = null;
            if (this.Mapped.ContainsKey(name))
            {
                // Record the function is about to be called and with which parameters. 
                if (this.Scribe != null)
                {
                    this.Scribe.LogEntry(name, method_params, this.Parent);
                }


                /*
                 * Determine if (1) it has no return type and (2) if it's an async task.
                 * 
                 * A Task will require we wait for the call, so using this wrapper it's not async anymore as it 
                 * will internally wait for the result. 
                 */ 
                bool called_return = !(this.Mapped[name].ReturnType.Name == "Void");
                bool isTask = this.Mapped[name].ReturnType.BaseType.Name == "Task";
                try
                {
                    if (called_return)
                    {
                        // It has a return value, so we want to collect it for the return. In case of a task the caller will
                        // still need to wait on it (noop since it's done) and get the result.
                        if (isTask)
                        {
                            var response = this.InvokeAsync(this.Mapped[name].Method as MethodInfo, this.Parent, method_params); 
                            if (response != null)
                            {
                                returnValue = response.Result;
                            }
                        }
                        else
                        {
                            returnValue = this.Mapped[name].Method.Invoke(this.Parent, method_params);
                        }
                    }
                    else
                    {
                        if (isTask)
                        {
                            this.InvokeAsync(this.Mapped[name].Method as MethodInfo, this.Parent, method_params).Wait();
                        }
                        else
                        {
                            this.Mapped[name].Method.Invoke(this.Parent, method_params);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Report any exceptions to the logging mechanism, but don't capture 
                    // it fom bubbling out...
                    if (this.Scribe != null)
                    {
                        this.Scribe.LogException(name, ex, this.Parent);
                    }

                    throw ex;
                }

                // Log out the end of the function call (default IScribe will capture execution time as well)
                if (this.Scribe != null)
                {
                    this.Scribe.LogExit(name, returnValue, this.Parent);
                }
            }

            return returnValue;
        }


#pragma warning disable CS1998
        private async Task<object> InvokeAsync(MethodInfo minfo, object obj, params object[] parameters)
        {
            dynamic awaitable = minfo.Invoke(obj, parameters);
            return awaitable.GetAwaiter().GetResult();
        }

        private object InvokeSync(MethodInfo methodInfo, object obj, params object[] parameters)
        {
            return methodInfo.Invoke(obj, parameters);
        }
    }
}
