using System.Linq;

namespace ChunkyConsole.Commands
{
    public class DynamicPrompterCommand : PrompterCommand
    {
        System.Reflection.MethodInfo _mem;
        object _source;

        public DynamicPrompterCommand(System.Reflection.MethodInfo mem, object source)
        {
            _mem = mem;
            _source = source;
        }
        public override void Execute()
        {
            _mem.Invoke(_source, Parameters);
        }
        public object[] Parameters
        {
            get
            {
                return (from p in this.Prompter
                        select p.Validator.GetType().GetProperty("Value").GetValue(p.Validator, null)).ToArray<object>();
            }
        }

    }
}
