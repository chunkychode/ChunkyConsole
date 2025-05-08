
using System;

namespace ChunkyConsole.Prompts
{
    public class PromptItem<T> : PromptItem
    {
        public PromptItem(string prompt)
        {
            this.Prompt = prompt;
            this.Validator = ChunkyConsole.Validators.ValidatorFactory.GetValidatorForType(typeof(T));
        }
        public new T UserValue
        {
            get
            {
                if (this.Validator != null)
                    return (this.Validator as Validators.IValue<T>).Value;
                else
                    throw new InvalidCastException(string.Format("{0} is not of type {1}", this.Prompt, typeof(T).FullName));
            }
        }
        public new (bool Success, PromptItem Item, T UserValue) PromptUser()
        {
            var prompt = new Prompter();
            prompt.AddLast(this);
            return (prompt.Prompt(), this, this.UserValue);
        }
    }
    public class PromptItem : IPrompt
    {
        public PromptItem(){}
        public PromptItem(string prompt){ this.Prompt = prompt; this.Validator = new Validators.NonBlankString(); }
        public PromptItem(string prompt, string defaultValue){ this.Prompt = prompt; this.DefaultValue = defaultValue; this.Validator = new Validators.NonBlankString(); }
        public virtual string Prompt { get; set; }
        public string DefaultValue { get; set; }
        public string ErrorPrompt { get; set; }
        public string UserValue { get; set; }
        public bool IsPassword { get; set; }
        public bool UseFileAutoComplete { get; set; }
        public bool CancelPrompt { get; set; }
        public Validators.IValidator Validator { get; set; }
        public override string ToString()
        {
            return string.Format("Prompt [{0}] -> DefaultValue [{1}] -> ErrorPrompt [{2}] -> UserValue [{3}] -> Value [{4}]", Prompt, DefaultValue, ErrorPrompt, UserValue, Validator.GetType().InvokeMember("Value", System.Reflection.BindingFlags.GetProperty, null, Validator, null));
        }

        public (bool Sucsess,PromptItem Item, string UserValue) PromptUser()
        {
            var prompt = new Prompter();
            prompt.AddLast(this);
            return (prompt.Prompt(), this, this.UserValue);
        }
    }
}
