using UnityEngine;
using System.Collections;

//Cannot be a monobehavior see http://answers.unity3d.com/questions/653904/you-are-trying-to-create-a-monobehaviour-using-the-2.html
public class InputController
{
    private string systemType;
    private int controllerNumber;
    private string inputString;

    //specify controller numbers 0 to 3 (0,1,2,3)
    public InputController(ControllerNumber num)
    {
        switch (num)
        {
            case ControllerNumber.ONE: this.controllerNumber = 0; break;
            case ControllerNumber.TWO: this.controllerNumber = 1; break;
            case ControllerNumber.THREE: this.controllerNumber = 2; break;
            case ControllerNumber.FOUR: this.controllerNumber = 3; break;
        }

        //controller axis on mac are different beasts with different axis
        if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer) systemType = "Mac";
        else systemType = "PC";

        inputString = "_" + controllerNumber + "_" + systemType + "_";
    }

    public bool A() { return Input.GetButtonDown("Joystick" + inputString + "A"); }
    public bool B() { return Input.GetButtonDown("Joystick" + inputString + "B"); }
    public bool X() { return Input.GetButtonDown("Joystick" + inputString + "X"); }
    public bool Y() { return Input.GetButtonDown("Joystick" + inputString + "Y"); }
    public bool L1() { return Input.GetButtonDown("Joystick" + inputString + "L1"); }
    public float L2() { return Input.GetAxis("Joystick" + inputString + "L2"); }
    public bool L3() { return Input.GetButtonDown("Joystick" + inputString + "L3"); }
    public bool R1() { return Input.GetButtonDown("Joystick" + inputString + "R1"); }
    public bool R1Up() { return Input.GetButtonUp("Joystick" + inputString + "R1"); }
    public float R2() { return Input.GetAxis("Joystick" + inputString + "R2"); }
    public bool R3() { return Input.GetButtonDown("Joystick" + inputString + "R3"); }
    public float Move_X() { return Input.GetAxis("Joystick" + inputString + "Move_X"); }
    public float Move_Y() { return Input.GetAxis("Joystick" + inputString + "Move_Y"); }
    public float Aim_X() { return Input.GetAxis("Joystick" + inputString + "Aim_X"); }
    public float Aim_Y() { return Input.GetAxis("Joystick" + inputString + "Aim_Y"); }
    public bool Start() { return Input.GetButtonDown("Joystick" + inputString + "Start"); }
    public bool Back() { return Input.GetButtonDown("Joystick" + inputString + "Back"); }

    //Turns out the D_Pad works differently on Pc / Mac
    public float D_up()
    {
        if (systemType == "Mac") { return Input.GetAxis("Joystick" + inputString + "D_up"); }
        else { return Input.GetAxis("Joystick" + inputString + "D_up_down"); }
    }
    public float D_down()
    {
        if (systemType == "Mac") { return Input.GetAxis("Joystick" + inputString + "D_down"); }
        else { return -Input.GetAxis("Joystick" + inputString + "D_up_down"); }
    }
    public float D_left()
    {
        if (systemType == "Mac") { return Input.GetAxis("Joystick" + inputString + "D_left"); }
        else { return Input.GetAxis("Joystick" + inputString + "D_left_right"); }
    }
    public float D_right()
    {
        if (systemType == "Mac") { return Input.GetAxis("Joystick" + inputString + "D_right"); }
        else { return -Input.GetAxis("Joystick" + inputString + "D_left_right"); }
    }
    //Oddly enough, Only Osx drivers know about the center (XBOX) button for input.
    public bool Xfn()
    {
        if (systemType == "Mac") { return Input.GetButtonDown("Joystick" + inputString + "Xfn"); }
        else { return false; }
    }
}

public enum ControllerNumber { ONE, TWO, THREE, FOUR };