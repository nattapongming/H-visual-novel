using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class DL_DIALOGUE_DATA 
{
    // this class use to spit line from text asset to different segments

    // @ mean that antthing in quote ("") is STRING even escape (\)

    public List<DIALOGUE_SEGMENT> segments;

    //translate find string for letter c || a or w and then c || a space out then number(aka. digit) if there're digit then collect them too
    //                  @          \{[ca]\}   |     \{w      [ca]      \s                \d                      *\.?          \d*\}

    private const string segmentIdentifierPattern = @"\{[ca]\}|\{w[ca]\s\d*\.?\d*\}";

    public bool hasDialogue => segments.Count > 0;

    public DL_DIALOGUE_DATA(string rawDialogue)
    {
        segments = RipSegments(rawDialogue);
    }

    public List<DIALOGUE_SEGMENT> RipSegments(string rawDialogue)
    {
        List<DIALOGUE_SEGMENT> segments = new List<DIALOGUE_SEGMENT>();
        MatchCollection matches = Regex.Matches(rawDialogue, segmentIdentifierPattern);

        int lastIndex = 0;

        //find ONLY the first or only segment in the file
        DIALOGUE_SEGMENT segment = new DIALOGUE_SEGMENT();

        //if there aren't match segment, then use that dialogue, else use that dialogue UNTIL meet the first index's match
        segment.dialogue = (matches.Count == 0 ? rawDialogue : rawDialogue.Substring(0, matches[0].Index));
        segment.startSignal = DIALOGUE_SEGMENT.StartSignal.NONE;
        segment.signalDelay = 0;
        segments.Add(segment);

        if (matches.Count == 0) { return segments; }
        else lastIndex = matches[0].Index;

        for (int i = 0; i < matches.Count; i++)
        {
            Match match = matches[i];
            segment = new DIALOGUE_SEGMENT();

            //get the start signal for the segment
            //we will get like this {content} or {A}

            string signalMatch = match.Value; 

            //remove '{' and '}'

            signalMatch = signalMatch.Substring(1, match.Length - 2);
           
            //make sure that if we get somethings like{WA} we only get WA not the delay

            string[] signalSplit = signalMatch.Split(' ');
            
            //change string to enum

            segment.startSignal = (DIALOGUE_SEGMENT.StartSignal)Enum.Parse(typeof(DIALOGUE_SEGMENT.StartSignal), signalSplit[0].ToUpper());

            //Get the signal delay
            if (signalSplit.Length > 1)
                float.TryParse(signalSplit[1], out segment.signalDelay);

            //Check ending of the segments 
            //if i + 1 < how many matches are found then use NEXT matches index else use dialogue.leght

            int nextIndex = i + 1 < matches.Count ? matches[i + 1].Index : rawDialogue.Length;

            //Get the dialogue of the segment

            segment.dialogue = rawDialogue.Substring(lastIndex + match.Length, nextIndex - (lastIndex + match.Length));
            lastIndex = nextIndex;

            segments.Add(segment);
        }

        return segments;
    }

    public struct DIALOGUE_SEGMENT
    {
        public string dialogue;
        public StartSignal startSignal;
        public float signalDelay;

        //none, clear, append, wait (seconds) append, wait clear
        public enum StartSignal { NONE, C, A, WA, WC}

        public bool appendText => (startSignal == StartSignal.A || startSignal == StartSignal.WA);
    }
}
