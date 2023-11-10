

namespace ChunkyConsole.Prompts
{
    public interface IPrompt
    {
        string Prompt { get; set; }
        string DefaultValue { get; set; }
        string ErrorPrompt { get; set; }
        string UserValue { get; set; }
        bool IsPassword { get; set; }
        bool CancelPrompt { get; set; }
        Validators.IValidator Validator { get; set; }
    }
}
