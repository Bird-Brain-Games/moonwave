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
    public int playerID;
}
public class bulletColour : MonoBehaviour
{

    //An array of bools to determine what colours have been used
    //false means it hasnt been used, true means it has.
    public bool[] array;
    //The colours themselves
    public Dictionary<COLOURS, Color> colours;
    //A counter to show how many elements we have in total
    uint counter;
    //White colour to return if the colour is already being used;
    Color white;

    // Use this for initialization
    void Awake()
    {
        array = new bool[10];
        counter = 0;
        white = new Color(0, 0, 0);
        colours = new Dictionary<COLOURS, Color>();
        AddColours();

    }

    void AddColours()
    {
        colours.Add(COLOURS.red, new Color(250.0f / 255.0f, 79 / 255.0f, 32 / 255.0f));

        counter++;
        colours.Add(COLOURS.blue, new Color(79 / 255.0f, 187 / 255.0f, 255 / 255.0f));

        counter++;
        colours.Add(COLOURS.green, new Color(26 / 255.0f, 156 / 255.0f, 41 / 255.0f));


        counter++;
        colours.Add(COLOURS.purple, new Color(165 / 255.0f, 36 / 255.0f, 197 / 255.0f));

        counter++;
        colours.Add(COLOURS.gray, new Color(115 / 255.0f, 117 / 255.0f, 128 / 255.0f));

        counter++;
        colours.Add(COLOURS.dark_purple, new Color(25 / 255.0f, 8 / 255.0f, 108 / 255.0f));
    }

    //Returns the colour data and whether it is free or not.
    public ColourData GetColour(COLOURS a_colours)
    {
        ColourData r_colours = new ColourData();
        r_colours.isFree = !array[(int)a_colours];

        //Checks whether the colour is free
        if (r_colours.isFree == true)
        {
            r_colours.colour = colours[a_colours];
            array[(int)a_colours] = true;
            r_colours.playerID = (int)a_colours;
        }
        else
        {
            r_colours.colour = white;
            r_colours.playerID = 10;
        }

        return r_colours;
    }

    //Free's the colour for other use [cam]
    public void FreeColour(COLOURS a_colours)
    {
        //Set the colour to be available [cam]
        array[(int)a_colours] = false;
    }
    //Loops through and finds the next available colour [cam]
    public ColourData GetNextAvailableColour()
    {
        ColourData r_colours = new ColourData();
        for (int i = 0; i <= counter; i++)
        {
            if (array[i] == false)
            {
                //if the colour is free then set it and return.
                r_colours.isFree = true;
                array[i] = true;
                r_colours.colour = colours[(COLOURS)i];
                r_colours.playerID = i;
                return r_colours;
            }
        }
        r_colours.isFree = false;
        r_colours.colour = white;
        r_colours.playerID = 10;
        return r_colours;
    }
}