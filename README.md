# ChunkyConsole
I use this mainly when i have a console app that i keep using for different tasks, this helps isolate the tasks and exeute them easily

# Example

    class Program
    {
        static void Main(string[] args)
        {
            Menu m = new Menu();
            m.MenuItems.Add(new MenuItem() { Title = "Reflection Example", Key = ConsoleKey.D1, Command = new ReflectionExample() });
            m.MenuItems.Add(new MenuItem() { Title = "Simple Class Example", Key = ConsoleKey.D2, Command = new ClassExample() });
            m.MenuItems.Add(new MenuItem() { Title = "Menu Example", Key = ConsoleKey.D3, Command = new MenuExample() });
            m.MenuItems.Add(new MenuItem() { Title = "Prompt Example", Key = ConsoleKey.D4, Command = new PromptExample() });
            m.Print();
        }
    }
    
# LICENSE
[mit-license](http://www.opensource.org/licenses/mit-license.php)

###
