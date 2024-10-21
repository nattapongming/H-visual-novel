using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DIALOGUE
{
    public class conversationManager 
    {
        private DialogueSystem dialogueSystem => DialogueSystem.instance;

        private Coroutine process = null;
        public bool isRunning => process != null;

        private TextArchitect architect = null;
        private bool userPrompt = false;

        //ascess to text architect dicrectly to have the same architext as dialogue system
        public conversationManager(TextArchitect architect)
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
                DIALOGUE_LINE line = dialogueParser.Parse(conversation[i]);
                
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

            }
        }

        IEnumerator Line_RunDialogue(DIALOGUE_LINE line)
        {
            //if there's speaker name then shower otherside don't
            if (line.hasSpeaker)
                dialogueSystem.ShowSpeakerName(line.speaker);
            else
                dialogueSystem.HideSpeakerName();

            //Build dialogue
            yield return BuildDialogue(line.dialogue);

            //Wait for user Input
            yield return WaitForUserInput();
        }

        IEnumerator Line_RunCommands(DIALOGUE_LINE line)
        {
            Debug.Log(line.commands);
            yield return null ;
        }

        IEnumerator BuildDialogue(string dialogue)
        {
            architect.Build(dialogue);

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

