using System;
using System.Linq;


namespace ChunkyConsole.Prompts
{
    public delegate void OnAfterPromptEventHandler(Prompter source, PromptItem item, ref bool cancel);
    public delegate void OnPromptsCompleteEventHandler(Prompter source, bool result);
    public class Prompter : System.Collections.Generic.LinkedList<PromptItem>
    {

        public event OnAfterPromptEventHandler OnAfterPropmt;
        public event OnPromptsCompleteEventHandler OnPropmtsComplete;
        public Prompter() : base() { }

        public string UserValue(int index)
        {
            return this.Skip(index - 1).Take(1).FirstOrDefault().UserValue;
        }
        public T Get<T>(int index)
        {
            if (index < Count && this.Skip(index - 1).Take(1).FirstOrDefault().Validator != null)
                return (this.Skip(index - 1).Take(1).FirstOrDefault().Validator as Validators.IValue<T>).Value;

            throw new ArgumentException("Index must in range and validator may not be null");
        }
        public T Get<T>(string name)
        {
            var thisone = this.FirstOrDefault(f => f.Prompt.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (thisone != null)
            {
                var retthisval = (thisone.Validator as Validators.IValue<T>);
                if (retthisval != null)
                    return retthisval.Value;

                throw new InvalidCastException(string.Format("{0} is not of type {1}", name, typeof(T).FullName));
            }
            else
                throw new ArgumentException(string.Format("{0} was not found", name));
        }

        public string this[string key]
        {
            get
            {
                return this.FirstOrDefault(f => String.Compare(f.Prompt, key, StringComparison.OrdinalIgnoreCase) == 0).UserValue;
            }
        }

        public bool Prompt()
        {
            bool bret = true;
            try
            {
                var a = this.First;
                while (a != null)
                {
                    if (a.Value.CancelPrompt)
                        continue;

                    bool cancel = false;
                    a.Value.UserValue = Utils.ConsoleHelper.LineFromUser(a.Value.Prompt, a.Value.IsPassword ? string.Empty : a.Value.DefaultValue, a.Value.IsPassword, a.Value.UseFileAutoComplete);
                    while (!a.Value.Validator.Validate(a.Value.UserValue))
                    {
                        Console.WriteLine(string.IsNullOrEmpty(a.Value.ErrorPrompt) ? a.Value.Validator.ErrorMessage : a.Value.ErrorPrompt);
                        a.Value.UserValue = Utils.ConsoleHelper.LineFromUser(a.Value.Prompt, a.Value.IsPassword ? string.Empty : a.Value.DefaultValue, a.Value.IsPassword, a.Value.UseFileAutoComplete);
                        if (string.IsNullOrEmpty(a.Value.UserValue))
                        {
                            bret = false;
                            return false;
                        }
                    }
                    if (OnAfterPropmt != null)
                        OnAfterPropmt(this, a.Value, ref cancel);

                    if (cancel)
                    {
                        bret = true;
                        break;
                    }
                    else if (!a.Value.Validator.Validate(a.Value.UserValue))
                    {
                        bret = false;
                        break;
                    }
                    a = a.Next;
                }
            }
            finally
            {
                if (this.OnPropmtsComplete != null)
                    OnPropmtsComplete(this, bret);
            }
            return bret;

        }
        public override string ToString()
        {
            return (from a in this
                    select a.ToString()).Aggregate((a, b) => a += "\r\n" + b);
        }
    }
}
