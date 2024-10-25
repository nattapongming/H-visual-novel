using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace COMMANDS
{
    public abstract class CMD_DatabaseExtension
    {
        //another child class will extend command in CommandDataBase
        public static void Extend(CommandDataBase dataBase) { }
    }
}