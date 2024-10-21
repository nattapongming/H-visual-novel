using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace DIALOGUE
{
    //contain gameobjcet and tmp_pro for dialogue
    [System.Serializable]
    public class DialogueContainer
    {
        public GameObject root;
        public NameContainer nameContainer;
        public TextMeshProUGUI dialogueText;
    }

}