using ChunkyConsole.Prompts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ChunkyConsole.UI
{
    internal class ReflectionMenuCreator
    {
        private static List<Type> getClassesFromAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes();
            var these = types
                .Where(t => !t.IsAbstract && t.GetInterfaces().Contains(typeof(Commands.ICommand)))
                .ToList();
            return these;
        }
        private static ConsoleKey GetConsoleKeyFromFirstLetter(string input, Dictionary<string, ConsoleKey> keyMap)
        {
            if (string.IsNullOrEmpty(input))
                return ConsoleKey.F1; // Default value if input is null or empty

            int idx = 0;
            do
            {
                char firstChar = char.ToUpper(input[idx]);
                if (keyMap.ContainsKey(firstChar.ToString()))
                {
                    idx++;
                    continue;
                }
                if (Enum.TryParse(firstChar.ToString(), out ConsoleKey key))
                {
                    keyMap.Add(firstChar.ToString(), key);
                    return key;
                }
                idx++;

            } while (idx < input.Length);

            return ConsoleKey.F1; // Default value if no valid ConsoleKey is found
        }
        private static Assembly GetCallingAssembly()
        {
            var stackTrace = new System.Diagnostics.StackTrace();
            var frames = stackTrace.GetFrames();

            // Skip the current assembly and find the first external caller
            foreach (var frame in frames)
            {
                var method = frame.GetMethod();
                var assembly = method.DeclaringType?.Assembly;

                if (assembly != Assembly.GetExecutingAssembly())
                {
                    return assembly;
                }
            }

            return Assembly.GetExecutingAssembly(); // Fallback to the current assembly
        }

        internal static Menu CreateReflectionMenu()
        {
            Dictionary<string, ConsoleKey> keyMap = new Dictionary<string, ConsoleKey>();
            var types = getClassesFromAssembly(GetCallingAssembly());
            var these = types
                .Where(t => !t.IsAbstract 
                && t.GetInterfaces().Contains(typeof(Commands.ICommand)) 
                && t.GetConstructor(Type.EmptyTypes) != null) 

                .SelectMany(t =>
                {
                    var ret = new List<MenuItem>();
                    ret.Add(new MenuItem()
                    {
                        Title = t.Name,
                        Key = GetConsoleKeyFromFirstLetter(t.Name, keyMap),
                        Command = Activator.CreateInstance(t) as Commands.ICommand
                    }
                    );
                    var addSubMenus = GetMethodMenuItemsFromtype(t, keyMap);
                    if(addSubMenus.Count > 0)
                    {
                        ret.AddRange(addSubMenus);
                    }
                    

                        
                    
                    
                    return ret;
                }
                ).ToList();
            var m = new Menu();
            m.MenuItems.AddRange(these);
            return m;
        }
        private static List<MenuItem> GetMethodMenuItemsFromtype(Type type, Dictionary<string, ConsoleKey> keyMap)
        {
            var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                .Where(m => m.GetCustomAttributes(typeof(IncludeMethodAttribute), true).Length > 0)
                .Select(m => new MenuItem()
                {
                    Title = $"\t{m.Name}",
                    Key = GetConsoleKeyFromFirstLetter(m.Name, keyMap),
                    Action = setAction(m),
                }).ToList();
            return methods;
        }
        static Action setAction(MethodInfo m)
        {
            if (m.IsStatic)
                return () => m.Invoke(null, GetMethodParameters(m));
            else
            {
                return () => m.Invoke(Activator.CreateInstance(m.DeclaringType), GetMethodParameters(m));
            }

        }
        static object[] GetMethodParameters(MethodInfo method)
        {
            var parameters = method.GetParameters();
            var arguments = new object[parameters.Length];
            var prompter = new Prompter();

            for (int i = 0; i < parameters.Length; i++)
            {
                string promptMessage = $"Enter value for {parameters[i].Name} ({parameters[i].ParameterType.Name}):";
                prompter.AddLast(new PromptItem()
                {
                    Prompt = promptMessage,
                    DefaultValue = parameters[i].HasDefaultValue ? parameters[i].DefaultValue.ToString() : string.Empty,
                    CancelPrompt = false,
                    Validator = Validators.ValidatorFactory.GetValidatorForParameter(parameters[i])
                });
            }

            prompter.Prompt();

            for (int i = 0; i < parameters.Length; i++)
            {
                var input = prompter.GetByIndex(i); 
                try
                {
                    arguments[i] = Convert.ChangeType(input, parameters[i].ParameterType);
                }
                catch
                {
                    arguments[i] = parameters[i].HasDefaultValue ? parameters[i].DefaultValue : Activator.CreateInstance(parameters[i].ParameterType);
                }
            }

            return arguments;
        }
    }
}
