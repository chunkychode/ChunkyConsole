
namespace ChunkyConsole.Commands
{
    public abstract class Command : ICommand
    {
        public string Title { get; set; }
        public abstract void Execute();
    }
}
