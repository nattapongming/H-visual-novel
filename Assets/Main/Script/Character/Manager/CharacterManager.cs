using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DIALOGUE;
using Unity.VisualScripting.FullSerializer;
using Unity.VisualScripting;

namespace CHARACTER
{
    public class CharacterManager : MonoBehaviour
    {
        public static CharacterManager instance { get; private set; }
        private Dictionary<string, Character> characters = new Dictionary<string, Character>();
        private CharacterConfigSO config => DialogueSystem.instance.config.characterConfigurationAsset;

        private void Awake()
        {
            instance = this;
        }

        //Create > GetInfo > Creater from info
        public Character CreateCharacter(string characterName)
        {
            if (characters.ContainsKey(characterName.ToLower()))
            {
                Debug.LogWarning($"A Character called '{characterName}' already exists. Did not create the character.");
                return null;
            }

            CHARACTER_INFO info = GetCharacterInfo(characterName);

            Character character = CreateCharacterFromInfo(info);

            //add created character into characters dictionary
            characters.Add(characterName.ToLower(), character);

            return null;
        }

        private CHARACTER_INFO GetCharacterInfo(string characterName)
        {
            CHARACTER_INFO result = new CHARACTER_INFO();

            result.name = characterName;

            result.config = config.GetConfig(characterName);


            return result;
        }

        private Character CreateCharacterFromInfo(CHARACTER_INFO info)
        {
            CharacterConfigData config = info.config;
            
            switch (config.characterType)
            {
                case Character.CharacterType.Text:
                    return new Character_Text(info.name, config);

                case Character.CharacterType.Sprite:
                case Character.CharacterType.SpriteSheet:
                    return new Character_Sprite(info.name, config);

                case Character.CharacterType.Live2D:
                    return new Character_Live2D(info.name, config);

                case Character.CharacterType.Model3D:
                    return new Character_Model3D(info.name, config);

                default:
                    return null;
            }
                
        }

        private class CHARACTER_INFO
        {
            public string name = "";

            public CharacterConfigData config = null;
        } 
    }


}