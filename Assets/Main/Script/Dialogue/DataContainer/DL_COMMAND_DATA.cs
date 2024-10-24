using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DL_COMMAND_DATA 
{
    public List<Command> commands;
    private const char COMMANDSPLITTER_ID = ',';
    private const char ARGUMENTSCONTAINER_ID = '(';
    private const string WAITCOMMAND_ID = "[wait]";

    public struct Command
    {
        public string name;
        public string[] arguments;
        public bool waitForCompletion;
    }

    public DL_COMMAND_DATA(string rawCommands)
    {
        commands = RipCommands(rawCommands);
    }

    //Rop mutliple commands into single command
    private List<Command> RipCommands(string rawCommands)
    {
        string[] data = rawCommands.Split(COMMANDSPLITTER_ID, System.StringSplitOptions.RemoveEmptyEntries);
        List<Command> result = new List<Command>();

        foreach (string cmd in data) 
        {
            Command command = new Command();

            //find index of '('
            int index = cmd.IndexOf(ARGUMENTSCONTAINER_ID);
            command.name = cmd.Substring(0, index).Trim();

            command.arguments = GetArgs(cmd.Substring(index + 1, cmd.Length - index - 2));
            result.Add(command);
        }

        return result;
    }

    private string[] GetArgs(string args)
    {
        List<string> argList = new List<string>();

        //this method use to build strings letter by letter or use to make better optimize than string1 + string2 etc.
        StringBuilder currentArg = new StringBuilder();
        bool inQuotes = false;

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == '"')
            {
                inQuotes = !inQuotes;
                continue;
            }

            // if found white space outside quote add in argument list and clear curret argument
            if (!inQuotes && args[i] == ' ')
            {
                argList.Add(currentArg.ToString());
                currentArg.Clear();
                continue;
            }

            currentArg.Append(args[i]);
        }

        if (currentArg.Length > 0)
            argList.Add(currentArg.ToString());

        return argList.ToArray();
    }
}
