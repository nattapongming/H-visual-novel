using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CHARACTER
{
    public class Character_Model3D : Character
    {
        public Character_Model3D(string name, CharacterConfigData config) : base(name, config)
        {
            Debug.Log($"Created 3D Character: '{name}'");
        }
    }
}