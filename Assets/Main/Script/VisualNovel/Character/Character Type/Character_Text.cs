using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTER
{
    public class Character_Text : Character
    {
        //'base: ' use to call the base constructer of a parent class
        public Character_Text(string name, CharacterConfigData config) : base(name, config)
        {
            Debug.Log($"Created Text Character: '{name}'");
        }
    }
}