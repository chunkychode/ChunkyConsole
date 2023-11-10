using ChunkyConsole.UI;
using ChunkyConsoleExamples.Commands;
using System;

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