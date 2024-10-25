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