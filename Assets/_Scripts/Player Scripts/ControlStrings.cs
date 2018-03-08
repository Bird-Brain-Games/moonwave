using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlStrings : MonoBehaviour
{

    public string append;

    string m_name;
    string m_jump;
    string m_shoot;
    string m_joystickH;
    string m_joystickV;
    string m_aimH;
    string m_aimV;

    bool boost;




    //temp variables to differentiate players
    public Color colour;
    public Color colourdull;


    // Use this for initialization
    void Start()
    {
        m_name = append + "name";
        m_jump = append + "jump";
        m_shoot = append + "shoot";
        m_joystickH = append + "joystickH";
        m_joystickV = append + "joystickV";
        m_aimH = append + "aimH";
        m_aimV = append + "aimV";

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }
    public string get_name() { return m_name; }
    public string get_jump() { return m_jump; }
    public string get_shoot() { return m_shoot; }
    public string get_joystickH() { return m_joystickH; }
    public string get_joystickV() { return m_joystickV; }
    public string get_aimH() { return m_aimH; }
    public string get_aimV() { return m_aimV; }
    public bool get_boost() { return boost; }
    public void set_boost(bool set) { boost = set; }

}
