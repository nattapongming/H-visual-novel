using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;

namespace TESTING
{
    public class testDialogueFiles : MonoBehaviour
    {
        [SerializeField] private TextAsset fileToRead = null;

        void Start()
        {
            StartConversation();
        }

        //Read text asset and send to Dialogue System
        void StartConversation()
        {
            List<string> lines = FileManager.ReadTextAsset(fileToRead);

            /*foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                DIALOGUE_LINE dl = DialogueParser.Parse(line);

                for (int i = 0; i < dl.commandData.commands.Count; i++)
                {
                    DL_COMMAND_DATA.Command command = dl.commandData.commands[i];
                    Debug.Log($"Command [{i}] '{command.name}' has arguments [{string.Join(", ", command.arguments)}]");
                }
            }*/

            DialogueSystem.instance.Say(lines);
        }
    }
}