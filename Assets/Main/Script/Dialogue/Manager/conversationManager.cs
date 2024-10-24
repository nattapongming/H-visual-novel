using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;

namespace DIALOGUE
{
    public class ConversationManager 
    {
        private DialogueSystem dialogueSystem => DialogueSystem.instance;

        private Coroutine process = null;
        public bool isRunning => process != null;

        private TextArchitect architect = null;
        private bool userPrompt = false;

        //ascess to text architect dicrectly to have the same architext as dialogue system
        public ConversationManager(TextArchitect architect)
        {
            this.architect = architect;

            //when ever logic in dialoguesytem class trigger this will trigger too
            dialogueSystem.onUserPrompt_Next += OnUserPrompt_Next;
        }

        private void OnUserPrompt_Next()
        {
            userPrompt = true;
        }

        public void StartConversation(List<string> conversation)
        {
            StopConversation();

            process = dialogueSystem.StartCoroutine(RunningConversation(conversation));
        }

        public void StopConversation()
        {
            if(!isRunning)
                return;

            dialogueSystem.StopCoroutine(process);
            process = null;
        }
        IEnumerator RunningConversation(List<string> conversation)
        {
            //skip check blank line or try to run logic on them
            for (int i = 0; i < conversation.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(conversation[i]))
                    continue;

                //try prase line
                DIALOGUE_LINE line = DialogueParser.Parse(conversation[i]);
                
                //show dialogue
                if (line.hasDialogue)
                {
                    yield return Line_RunDialogue(line);
                }

                //run commands
                if (line.hasCommands)
                {
                    yield return Line_RunCommands(line);
                }

                if (line.hasDialogue)
                    //Wait for user input
                    yield return WaitForUserInput();

            }
        }

        IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {
            //if there's speaker name then shower otherside don't
            if (line.hasSpeaker)
                dialogueSystem.ShowSpeakerName(line.speakerData.displayname);
            

            //Build dialogue
            yield return BuildLineSegments(line.dialogueData);           
        }

        IEnumerator Line_RunCommands(DIALOGUE_LINE line)
        {
            List<DL_COMMAND_DATA.Command> commands = line.commandData.commands;
            
            foreach(DL_COMMAND_DATA.Command command in commands)
            {
                if (command.waitForCompletion)
                    yield return CommandManager.Instance.Execute(command.name, command.arguments);
                else
                    CommandManager.Instance.Execute(command.name, command.arguments);
            }

            yield return null ;
        }

        IEnumerator BuildLineSegments(DL_DIALOGUE_DATA line)
        {
            for(int i = 0; i < line.segments.Count; i++)
            {
                DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment = line.segments[i];

                yield return WaitForDialogueSegmantSignalTobeTriggered(segment);

                yield return BuildDialogue(segment.dialogue, segment.appendText);
            }

            
        }

        IEnumerator WaitForDialogueSegmantSignalTobeTriggered(DL_DIALOGUE_DATA.DIALOGUE_SEGMENT segment)
        {
            switch(segment.startSignal)
            {
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.C:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.A:
                    yield return WaitForUserInput();
                    break;

                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WA:
                case DL_DIALOGUE_DATA.DIALOGUE_SEGMENT.StartSignal.WC:
                    yield return new WaitForSeconds(segment.signalDelay);
                    break;

                default:
                    break;
            }
        }
        IEnumerator BuildDialogue(string dialogue, bool append = false)
        {
            //Build dialogue
            if (!append)
                architect.Build(dialogue);
            else 
                architect.Append(dialogue);

            //wait for dialogue to complete
            while (architect.isBuilding)
            {
                if (userPrompt)
                {
                    if (!architect.hurryUp)
                        architect.hurryUp = true;
                    else
                        architect.ForceComplete();

                    userPrompt = false;
                }
                yield return null;
            }
        }
        IEnumerator WaitForUserInput()
        {
            while (!userPrompt)
                yield return null ;

            userPrompt = false;
        }
    }
}

