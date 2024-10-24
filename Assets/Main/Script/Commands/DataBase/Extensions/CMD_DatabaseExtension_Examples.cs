using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMD_DatabaseExtension_Examples : CMD_DatabaseExtension
{
    //Over ride parent extend method
    new public static void Extend(CommandDataBase database)
    {
        //Add action with no parameters
        database.AddCommand("print", new Action(PrintDefaultMessage));
    }

    //all of command must be static

    private static void PrintDefaultMessage()
    {
        Debug.Log("Printing a default message to console.");
    }
}
