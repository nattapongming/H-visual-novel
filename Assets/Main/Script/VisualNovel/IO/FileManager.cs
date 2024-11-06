using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FileManager 
{
    //find from fath

    // relative path is only use with in the same root
    // absulote path can be point to anywhere even out side game folder

    public static List<string> ReadTextFile(string filePath, bool includeBlankLines = true)
    {
        //if don't start with / make it absulote path
        if (!filePath.StartsWith('/'))
            filePath = FilePaths.root + filePath;

        //try create line in path
        List<string> lines = new List<string>();
        try
        {
            //read each row in txt. file

            using (StreamReader sr = new StreamReader(filePath))
            {
                while (!sr.EndOfStream)
                {
                    //read each line if have black or space line then add line to list

                    string line = sr.ReadLine();
                    if (includeBlankLines || !string.IsNullOrWhiteSpace(line))
                        lines.Add(line);
                }
            }
        }

        // if have any error then debug error

        catch (FileNotFoundException ex)
        {
            Debug.LogError($"File not found: '{ex.FileName}'");
        }

        return lines;
    }

    //find from asset
    public static List<string> ReadTextAsset(string filePath, bool includeBlankLines = true)
    {
        TextAsset asset = Resources.Load<TextAsset>(filePath);
        if (asset == null)
        {
            Debug.LogError($"Asset not found: '{filePath}'");
            return null;
        }

        return ReadTextAsset(asset, includeBlankLines);
    }
    public static List<string> ReadTextAsset(TextAsset asset, bool includeBlankLines = true)
    {
        List<string> lines = new List<string>();

        //read each row in string

        using (StringReader sr = new StringReader(asset.text))
        {
            // check if any line available

            while (sr.Peek() > -1)
            {
                string line = sr.ReadLine();
                if (includeBlankLines || !string.IsNullOrWhiteSpace(line))
                    lines.Add(line);
            }
        }

        return lines;
    }
}
