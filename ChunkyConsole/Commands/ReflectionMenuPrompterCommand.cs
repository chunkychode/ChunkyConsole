using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace ChunkyConsole.Commands
{
    public class ReflectionMenuPrompterCommand : Commands.MenuPromptCommand
    {
        private bool _includeOnlyAttributedMembers;

        public ReflectionMenuPrompterCommand(bool AutoAdd)
        {
            _includeOnlyAttributedMembers = true;
            this.MenuOrder = Commands.MenuOrder.AfterPrompt;

            if (AutoAdd)
            {
                AddMembers(_includeOnlyAttributedMembers);
                AddProperties(_includeOnlyAttributedMembers);
            }
        }

        public ReflectionMenuPrompterCommand(Commands.MenuOrder menuOrder = Commands.MenuOrder.AfterPrompt, bool includeOnlyAttributedMembers = true, bool AutoAdd = true)
        {
            _includeOnlyAttributedMembers = includeOnlyAttributedMembers;

            this.MenuOrder = MenuOrder;
            if (AutoAdd)
            {
                AddMembers(includeOnlyAttributedMembers);
                AddProperties(includeOnlyAttributedMembers);
            }
        }

        protected void Populate()
        {
            AddMembers(_includeOnlyAttributedMembers);
            AddProperties(_includeOnlyAttributedMembers);
        }
        private void AddProperties(bool includeOnlyAttributedMembers = true)
        {
            Func<PropertyInfo, string> GetV = new Func<PropertyInfo, string>(pp => {
                try
                {
                    return pp.GetValue(this, null).ToString();
                }
                catch { return ""; }
                ;
            });

            foreach (var pr in (from m in this.GetType().GetProperties()
                                let atts = m.GetCustomAttributes(typeof(IncludeMethodAttribute), false)
                                where includeOnlyAttributedMembers ? (atts != null && atts.Count() > 0) : true
                                let v = GetV(m)
                                select new { va = v, pi = m, pwd = includeOnlyAttributedMembers ? (atts[0] as IncludeMethodAttribute).IsPassword : false }))
            {
                Add(pr.pi, this, pr.pwd, pr.va);
            }


        }

        private void AddMembers(bool includeOnlyAttributedMembers = true)
        {
            foreach (var mem in (GetMethods(includeOnlyAttributedMembers)))
            {
                var newcmd = new Commands.DynamicPrompterCommand(mem, this);

                AddMemberParamPrompts(mem, newcmd);

                Add(mem.Name, ConsoleKey.D0 + this.Menu.MenuItems.Count, newcmd);
            }
        }

        private static void AddMemberParamPrompts(MethodInfo mem, DynamicPrompterCommand newcmd)
        {
            foreach (var par in mem.GetParameters())
                newcmd.Prompter.AddLast(new Prompts.PromptItem() { Prompt = par.Name, Validator = Validators.BaseValidator<string>.InstanceByType(par.ParameterType) });
        }

        private IEnumerable<MethodInfo> GetMethods(bool includeOnlyAttributedMembers)
        {
            return from m in this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.DeclaredOnly)
                   where !m.Name.StartsWith("get_")
                   let atts = m.GetCustomAttributes(typeof(IncludeMethodAttribute), false)
                   where includeOnlyAttributedMembers ? (atts != null && atts.Count() > 0) : true
                   select m;
        }
        public override void Execute() { }

    }
}
