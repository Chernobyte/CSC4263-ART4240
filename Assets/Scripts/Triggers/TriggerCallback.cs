
using System;
using UnityEngine;

public class TriggerCallback: MonoBehaviour {

    Action<Collider2D> enterFunctionToCall;
    Action<Collider2D> exitFunctionToCall;
    Action<Collider2D> stayFunctionToCall;

	void Start () {
	
	}

	void Update () {
		
	}

    public void Init(Action<Collider2D> enter, Action<Collider2D> exit, Action<Collider2D> stay)
    {
        enterFunctionToCall = enter;
        exitFunctionToCall = exit;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (enterFunctionToCall != null)
            enterFunctionToCall(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (exitFunctionToCall != null)
            exitFunctionToCall(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (stayFunctionToCall != null)
            stayFunctionToCall(collision);
    }
}
