using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DIALOGUE;

namespace CHARACTER
{
    public abstract class Character 
    {
        public string name = "";
        public string displayName = "";
        public RectTransform root = null;
        //contain each character data 
        public CharacterConfigData config;
        public DialogueSystem dialogueSystem => DialogueSystem.instance;

        public Character(string name, CharacterConfigData config)
        {
            this.name = name;
            displayName = name;
            this.config = config;
        }

        //convert dialogue to string to make character say 
        //alterative way other than run text asset/ dialogue file
        public Coroutine Say(string dialogue) => Say(new List<string> { dialogue });
        public Coroutine Say(List<string> dialogue)
        {
            dialogueSystem.ShowSpeakerName(displayName);
            UpdateTextCustomizationOnScreen();
            return dialogueSystem.Say(dialogue);
        }

        public void SetNameFont(TMP_FontAsset font) => config.nameFont = font;
        public void SetDialogueFont(TMP_FontAsset font) => config.dialogueFont = font;
        public void SetNameColor(Color color) => config.nameColor = color;
        public void SetDialogueColor(Color color) => config.dialogueColor = color;
        public void ResetConfigurationData() => config = CharacterManager.instance.GetCharacterConfig(name);
        public void UpdateTextCustomizationOnScreen() => dialogueSystem.ApplySpeakerDataToDialogueContainer(config);

        public enum CharacterType
        {
            Text,
            Sprite,
            SpriteSheet,
            Live2D,
            Model3D
        }
    }
}