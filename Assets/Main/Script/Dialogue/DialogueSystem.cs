using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DIALOGUE
{
    public class DialogueSystem : MonoBehaviour
    {
        // using dialogue container script
        public DialogueContainer dialogueContainer = new DialogueContainer();
        private conversationManager conversationManager;
        private TextArchitect architect;

        public static DialogueSystem instance;

        //when this event trigger anything subscribe to this method will do theirs logic
        public delegate void DialogueSystemEvent();
        public event DialogueSystemEvent onUserPrompt_Next;

        public bool isRunningConversation => conversationManager.isRunning;

        private void Awake()
        {
            if (instance == null) 
            { 
                instance = this;
                Initialize();
            }
            else { DestroyImmediate(gameObject); }

        }

        bool _initialize = false;
        private void Initialize()
        {
            if (_initialize) return;

            architect = new TextArchitect(dialogueContainer.dialogueText);
            conversationManager = new conversationManager(architect);
        }
        
        public void OnUserPrompt_Next()
        {
            onUserPrompt_Next?.Invoke();
        }
        public void ShowSpeakerName(string speakerName = "")
        {
            if (speakerName.ToLower() != "narrator")
                dialogueContainer.nameContainer.Show(speakerName);
        }

        public void HideSpeakerName() => dialogueContainer.nameContainer.Hide();

        //change input to list string
        public void Say(string speaker, string dialogue)
        {
            List<string> conversation = new List<string>() { $"{speaker} \"{dialogue}\""};
            Say(conversation);
        }

        //then send to conversation manager to run it
        public void Say(List<string> conversation)
        {
            conversationManager.StartConversation(conversation);
        }
    }

}
