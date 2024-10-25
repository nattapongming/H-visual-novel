using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTER
{
    [CreateAssetMenu(fileName = "Character Configuration Asset", menuName = "Dialogue System/Character Configuration Asset")]
    public class CharacterConfigSO : ScriptableObject
    {
        //'SO' mean scriptable object

        public CharacterConfigData[] characters;

        //find want character with in config if found make copy of that data
        public CharacterConfigData GetConfig(string characterName)
        {
            characterName = characterName.ToLower();

            for (int i = 0; i < characters.Length; i++)
            {
                CharacterConfigData data = characters[i];

                if (string.Equals(characterName, data.name.ToLower()) || string.Equals(characterName, data.alias.ToLower()))
                    return data.Copy();
            }

            return CharacterConfigData.Default;
        }
    }
}