using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace DIALOGUE
{
    public class dialogueParser
    {
        // '\' or escape is used to cancel effect of char after it
        // '\\' mean cancel the escape affect of the next escape

        // create regex to recognize command pattern
        //exsample "\w" to find word 

        private const string commandRegexPattern = "\\w*[^\\s]\\(";

        //translate: find any world at any lenght except white space unit found '('
        //           |     \\w     |      *      |  ^   |    \\s    |      \\C

        public static DIALOGUE_LINE Parse(string rawLine)
        {
            Debug.Log($"Prasing line '{rawLine}'");

            //seperate line into 3 sector speaker, dialogue command

            (string speaker, string dialogue, string commands) = RipConntent(rawLine);

            Debug.Log($"Speaker = '{speaker}'\nDialogue = '{dialogue}'\nCommands = '{commands}'");

            return new DIALOGUE_LINE(speaker, dialogue, commands);
        }

        /*
        spreate speaker, dialoque ana command from line like these:

        after the first quote will be dialogue 
        while being dialogue check for another quote 
        if there's '\' before quote then ingore that quote
        after dialogue then it's a commands
        */

        private static (string, string, string) RipConntent(string rawLine)
        {
            string speaker = "", dialogue = "", commands = "";

            
            int dialogueStart = -1;
            int dialogueEnd = -1;
            bool isEscaped = false;

            for(int i = 0; i < rawLine.Length; i++)
            {
                char current = rawLine[i];
                if (current == '\\')
                    isEscaped = !isEscaped;

                //if found '"' maark as dialogue

                else if (current == '"' && !isEscaped)
                {
                    if (dialogueStart == -1) { dialogueStart = i; }
                    else if (dialogueEnd == -1) 
                    { 
                        dialogueEnd = i;
                        break;
                    }
                }
                else
                {
                    isEscaped = false ;  
                }
            }

            //Identify Commands pattern
            Regex commandPegex = new Regex(commandRegexPattern);

            //if match keep the info in here
            Match match = commandPegex.Match(rawLine);
            int commandStart = -1;
            if(match.Success)
            {
                commandStart = match.Index;

                //if there aren't any dialogue assume the rest are commands and get rid of left over white space behind it
                if (dialogueStart == -1 && dialogueEnd == -1)
                {
                    return ("", "", rawLine.Trim());
                }
            }

            //what we have is either dialogue of mutli word arguments in commands.
            // aka. Is this dialogue?
            if(dialogueStart != -1 && dialogueEnd != -1 && (commandStart == -1 || commandStart > dialogueEnd))
            {
                //ok this is a dialogue
                speaker = rawLine.Substring(0, dialogueStart).Trim();
                dialogue = rawLine.Substring(dialogueStart + 1, dialogueEnd - dialogueStart - 1).Replace("\\\"", "\"");
                if (commandStart != -1)
                    commands = rawLine.Substring(commandStart).Trim();
            }
            else if (commandStart != -1 && dialogueStart > commandStart)
            {
                //this dialogue just argument in command
                commands = rawLine;
            }
            else
            {
                speaker = rawLine;
            }

            return (speaker, dialogue, commands);
        }
    }
}
