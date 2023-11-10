
namespace ChunkyConsole.Prompts
{
    public class PromptItem : IPrompt
    {

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

    }
}
