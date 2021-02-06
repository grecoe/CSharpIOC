using FunctionWrapper.Interfaces;
using Utils.ClassMapper;

namespace FunctionWrapper.Classes
{
    class NotNakedClass: IBareIt
    {
        private Mapper<IBareIt, IScribe> Mapper { get; set; }

        public NotNakedClass(IBareIt naked, IScribe scribe)
        {
            this.Mapper = new Mapper<IBareIt, IScribe>(naked, scribe);
        }

        public void DoSomething()
        {
            if (this.Mapper != null)
            {
                this.Mapper.ExecuteMethod("DoSomething");
            }
        }
    }
}
