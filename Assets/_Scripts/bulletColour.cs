using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A hack to let us use the dictionary in the inspector

public enum COLOURS
{
    red,
    blue,
    green,
    purple,
    gray,
    dark_purple
}

public struct ColourData
{
    public Color colour;
    public bool isFree;
    public int  playerID;
    public string name;
    public int itr;
}
public class bulletColour : MonoBehaviour
{

    //An array of bools to determine what colours have been used
    //false means it hasnt been used, true means it has.
    public bool[] array;
    //The colours themselves
    //public Dictionary<COLOURS, Color> colours;
    public ColourData[] colours;
    //A counter to show how many elements we have in total
    int counter;
    //White colour to return if the colour is already being used;
    Color white;

    // When entering a new color select lobby, set this to true [jack]
    bool newColors;

    // Use this for initialization
    void Awake()
    {
        array = new bool[10];
        counter = 0;
        white = new Color(0, 0, 0);
        //colours = new Dictionary<COLOURS, Color>();
        colours = new ColourData[5];
        AddColours();

    }

    void AddColours()
    {
        //colours.Add(COLOURS.red, new Color(250.0f / 255.0f, 79 / 255.0f, 32 / 255.0f));
        colours[0].colour = new Color(250.0f / 255.0f, 79 / 255.0f, 32 / 255.0f);
        colours[0].name = "RED";
        colours[0].isFree = true; 

        //counter++;
        //colours.Add(COLOURS.blue, new Color(79 / 255.0f, 187 / 255.0f, 255 / 255.0f));
        colours[1].colour = new Color(79 / 255.0f, 187 / 255.0f, 255 / 255.0f);
        colours[1].name = "BLUE";
        colours[1].isFree = true;

        //counter++;
        //colours.Add(COLOURS.green, new Color(26 / 255.0f, 156 / 255.0f, 41 / 255.0f));
        colours[2].colour = new Color(26 / 255.0f, 156 / 255.0f, 41 / 255.0f);
        colours[2].name = "GREEN";
        colours[2].isFree = true;

        //counter++;
        //colours.Add(COLOURS.purple, new Color(165 / 255.0f, 36 / 255.0f, 197 / 255.0f));
        colours[3].colour = new Color(165 / 255.0f, 36 / 255.0f, 197 / 255.0f);
        colours[3].name = "PURPLE";
        colours[3].isFree = true;

        //counter++;
        //colours.Add(COLOURS.gray, new Color(115 / 255.0f, 117 / 255.0f, 128 / 255.0f));
        colours[4].colour = new Color(115 / 255.0f, 117 / 255.0f, 128 / 255.0f);
        colours[4].name = "GRAY";
        colours[4].isFree = true;

        // //counter++;
        // //colours.Add(COLOURS.dark_purple, new Color(25 / 255.0f, 8 / 255.0f, 108 / 255.0f));
        // colours[5].colour = new Color(25 / 255.0f, 8 / 255.0f, 108 / 255.0f);
        // colours[5].name = "MIDNIGHT";
        // colours[5].isFree = true;

        // colours[6].colour = new Color(1 / 255.0f, 1 / 255.0f, 1 / 255.0f);
        // colours[6].name = "WHITE";
        // colours[6].isFree = true;

    }

    //Returns the colour data and whether it is free or not.
    //public ColourData GetColour(COLOURS a_colours)
    //{
    //    ColourData r_colours = new ColourData();
    //    r_colours.isFree = !array[(int)a_colours];
    //
    //    //Checks whether the colour is free
    //    if (r_colours.isFree == true)
    //    {
    //        r_colours.colour = colours[a_colours];
    //        array[(int)a_colours] = true;
    //        r_colours.playerID = (int)a_colours;
    //    }
    //    else
    //    {
    //        r_colours.colour = white;
    //        r_colours.playerID = 10;
    //    }
    //
    //    return r_colours;
    //}

    //Free's the colour for other use [cam]
    //public void FreeColour(COLOURS a_colours)
    //{
    //    //Set the colour to be available [cam]
    //    array[(int)a_colours] = false;
    //}
    //Loops through and finds the next available colour [cam]

    public ColourData SpawnColour()
    {
        ColourData r_colours = new ColourData();
        for (int i = 0 + 1; i <= colours.Length; i++)
        {

            if (colours[i].isFree == true)
            {
                colours[i].itr = i;
                //if the colour is free then set it and return.
                colours[i].isFree = false;
                colours[i].playerID = i - 1;
                r_colours = colours[i];
                Debug.Log(colours[i].name);
                Debug.Log(r_colours.playerID);
                newColors = true;

                return r_colours;
            }

        }

        newColors = true;
        //r_colours.isFree = false;
        //r_colours.colour = white;
        //r_colours.playerID = 10;
        return r_colours;


    }
    public ColourData GetNextAvailableColour(int startPoint) // iterate through an array of colors to see what colors can be picked from
    {
        //colours[startPoint].isFree = true;

        if (startPoint > colours.Length)
        {
            startPoint = -1;
        }

        ColourData r_colours = new ColourData();
        for (int i = startPoint + 1; i < colours.Length; i++)
        {

            if (colours[i].isFree == true)
            {
                colours[i].itr = i ;
                //if the colour is free then set it and return.
                //colours[i].isFree = false;
                colours[i].playerID = i - 1;
                r_colours = colours[i];
                Debug.Log(colours[i].name);
                Debug.Log(r_colours.playerID);

                return r_colours;
            }
            
        }

        //r_colours.isFree = false;
        //r_colours.colour = white;
        //r_colours.playerID = 10;
        return r_colours;


    }

    public ColourData GetPreviousAvailableColour(int startPoint) // iterate through an array of colors to see what colors can be picked from
    {
        //colours[startPoint].isFree = true;

        if (startPoint == 0)
        {
            startPoint = colours.Length;
        }

        ColourData r_colours = new ColourData();
        for (int i = startPoint - 1; i >= 0; i--)
        {

            if (colours[i].isFree == true)
            {
                colours[i].itr = i;
                //if the colour is free then set it and return.
                //colours[i].isFree = false;
                colours[i].playerID = i - 1;
                r_colours = colours[i];
                Debug.Log(colours[i].name);
                Debug.Log(r_colours.playerID);

                return r_colours;
            }

        }

        //r_colours.isFree = false;
        //r_colours.colour = white;
        //r_colours.playerID = 10;
        return r_colours;


    }

    public void freeColors()
    {
        if (newColors)
        {
            for (int i = 0; i < colours.Length; i++)
            {
                colours[i].isFree = true;
            }
            newColors = false;
        }
    }

    public void selectColor(int startPoint)
    {
        if (colours[startPoint].isFree == true)
            colours[startPoint].isFree = false;
        else
        {
            ColourData r_colours = new ColourData();
            for (int i = startPoint + 1; i <= colours.Length; i++)
            {
                if (startPoint > 5)
                {
                    startPoint = -1;
                }

                if (colours[i].isFree == true)
                {
                    colours[i].itr = i;
                    //if the colour is free then set it and return.
                    //colours[i].isFree = false;
                    colours[i].playerID = i - 1;
                    r_colours = colours[i];
                    Debug.Log(colours[i].name);
                    Debug.Log(r_colours.playerID);

                }

            }
        }
    }

    public void unselectColor(int startPoint)
    {
        if (colours[startPoint].isFree == false)
            colours[startPoint].isFree = true;
    }
}