using System.Collections;
using UnityEngine;
using TMPro;

public class TextArchitect 
{
    private TextMeshProUGUI tmp_ui;
    private TextMeshPro tmp_world;
    // if have tmp_ui, use it for tmpro or use tmp_world instead
    public TMP_Text tmpro => tmp_ui != null ? tmp_ui : tmp_world;

    public string currentText => tmpro.text;

    //can be used public but assign private and have default empty
    public string targetText { get; private set; } = "";
    public string preText { get; private set; } = "";
    private int preTextLenght = 0;

    public string fullTargetText => preText + targetText;

    /*
    set how text will create

    instant = text appear instant
    typewriter = text appear for each letter
    fade = same as typewriter but all letter will set alpha to 0 then add more alpha per letter
    */
    public enum BuildMethod { instant, typewriter,fade}
    public BuildMethod buildMethod = BuildMethod.typewriter;

    //change color
    public Color textColor { get { return tmpro.color; } set { tmpro.color = value; } }

    // how fast text will appear
    public float speed { get { return baseSpeed * speedMultiplier; } set { speedMultiplier = value; } }
    private const float baseSpeed = 1;
    private float speedMultiplier = 1;

    public int characterPerCycle { get { return speed <= 2 ? characterMultiplier : speed <= 2.5f ? characterMultiplier * 2 : characterMultiplier * 3; } }
    private int characterMultiplier = 1;

    //click more to make text appear faster
    public bool hurryUp = false;

    //apply text
    public TextArchitect(TextMeshProUGUI tmp_ui)
    {
        this.tmp_ui = tmp_ui;
    }

    public TextArchitect(TextMeshPro tmp_world)
    {
        this.tmp_world = tmp_world;
    }

    //making text stop appear
    public Coroutine Build(string text)
    {
        preText = "";
        targetText = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    public Coroutine Append(string text)
    {
        preText = tmpro.text;
        targetText = text;

        Stop();

        buildProcess = tmpro.StartCoroutine(Building());
        return buildProcess;
    }

    //making is text appearing condition
    private Coroutine buildProcess = null;
    public bool isBuilding => buildProcess != null;

    public void Stop()
    {
        if (!isBuilding) return;

        tmpro.StopCoroutine(buildProcess);
        buildProcess = null;
    }

    //check which buildMethod to create
    IEnumerator Building()
    {
        Prepare();

        switch(buildMethod)
        {
            case BuildMethod.typewriter:
                yield return Build_TpyeWriter(); break;
                
            case BuildMethod.fade:
                yield return Build_Fade(); break;
        }

        OnComplete();
    }

    private void OnComplete()
    {
        buildProcess = null;
        hurryUp = false;
    }

    public void ForceComplete()
    {
        switch (buildMethod)
        {
            case BuildMethod.typewriter:
                tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
                break;
            case BuildMethod.fade:
                //tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
                break;

        }

    }

    //prepare build method
    private void Prepare()
    {
        switch (buildMethod)
        {
            case BuildMethod.instant:
                Prepare_Instant(); break;

            case BuildMethod.typewriter:
                Prepare_TypeWriter(); break;

            case BuildMethod.fade: 
                Prepare_Fade(); break;
        }
    }

    /*
    [instant]
    reset color to visable -> assign letter -> force update -> make every character visable
    */
    private void Prepare_Instant()
    {
        tmpro.color = tmpro.color;
        tmpro.text = fullTargetText;
        tmpro.ForceMeshUpdate();
        tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
    }

    /*
    [typewriter]
    same with instant but have character appear slower
    */
    private void Prepare_TypeWriter()
    {
        tmpro.color = tmpro.color;
        tmpro.maxVisibleCharacters = 0;
        tmpro.text = preText;

        if(preText != "")
        {
            tmpro.ForceMeshUpdate();
            tmpro.maxVisibleCharacters = tmpro.textInfo.characterCount;
        }
        tmpro.text += targetText;
        tmpro.ForceMeshUpdate();
    }
    /*
    [Fade]
    same with typewriter but set alpha of target text to zero and add alpha later
    */

    private void Prepare_Fade()
    {
        tmpro.text = preText;
        if(preText != "")
        {
            tmpro.ForceMeshUpdate();
            preTextLenght = tmpro.textInfo.characterCount;
        }
        else { preTextLenght = 0; }
        
        tmpro.text += targetText;
        tmpro.maxVisibleCharacters = int.MaxValue;
        tmpro.ForceMeshUpdate();

        TMP_TextInfo textInfo = tmpro.textInfo;

        //set visable / hidden color
        Color colorVisable = new Color(textColor.r, textColor.g, textColor.b, 1);
        Color colorHidden = new Color(textColor.r, textColor.g, textColor.b, 0);

        //get vertex color of text, only get material from first index 
        Color32[] vertexColor = textInfo.meshInfo[textInfo.characterInfo[0].materialReferenceIndex].colors32;

        for(int i = 0; i < textInfo.characterCount; i++)
        {
            TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

            if (!charInfo.isVisible ) continue;

            //if pretext make each char visable else make hidden

            if (i < preTextLenght)
            {
                for(int V = 0; V < 4; V++)
                {
                    vertexColor[charInfo.vertexIndex + V] = colorVisable;
                }
            }
            else
            {
                for (int V = 0; V < 4; V++)
                {
                    vertexColor[charInfo.vertexIndex + V] = colorHidden;
                }
            }

            tmpro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);
        }
    }

    private IEnumerator Build_TpyeWriter()
    {
        while(tmpro.maxVisibleCharacters < tmpro.textInfo.characterCount)
        {
            tmpro.maxVisibleCharacters += hurryUp ? characterPerCycle * 5: characterPerCycle;

            yield return new WaitForSeconds(0.015f / speed); 
        }
    }

    private IEnumerator Build_Fade()
    {
        //set how many character to gradually visable 
        int minRage = preTextLenght;
        int maxRange = minRage + 1;

        byte alphaThreshold = 15;

        TMP_TextInfo textInfo = tmpro.textInfo;

        Color32[] vertexColor = textInfo.meshInfo[textInfo.characterInfo[0].materialReferenceIndex].colors32;
        float[] alphas = new float[textInfo.characterCount];

        while (true)
        {
            float fadeSpeed = ((hurryUp ? characterPerCycle * 5 : characterPerCycle) * speed) * 4f;

            for(int i = minRage; i < maxRange; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];

                if (!charInfo.isVisible) continue;

                int vertexIndex = textInfo.characterInfo[i].vertexIndex;

                //change curret index alpha value to 255 by fadespeed

                alphas[i] = Mathf.MoveTowards(alphas[i], 255, fadeSpeed);

                for (int V = 0; V < 4; V++)
                {
                    vertexColor[charInfo.vertexIndex + V].a = (byte)alphas[i];
                }

                if (alphas[i] > 255)
                {
                    minRage++;
                }

                tmpro.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32);

                //check if last character is need to fade

                bool lastCharacterIsVisible = !textInfo.characterInfo[maxRange - 1].isVisible;

                //if alpha of max range is greater than alpha threshold os last char is visible

                if (alphas[maxRange - 1] > alphaThreshold || lastCharacterIsVisible)
                {
                    if (maxRange < textInfo.characterCount) { maxRange++; }
                    else if (alphas[maxRange - 1] >= 255) { break; }
                } 

                yield return new WaitForEndOfFrame();
            }

            
        }
    }
}
