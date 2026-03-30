using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Keypad : MonoBehaviour
{
    public string correctCode = ".-.---..-..";
    private string currentInput = "";

    public TextMeshProUGUI displayText;
    public DoorManager door;
    public GameObject keypad;
    public KeypadActive keypadActivescript;



    void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            keypad.SetActive(false);
            Clear();
        }
    }


    public void PressDot()
    {
        AddSymbol(".");
    }

    public void PressDash()
    {
        AddSymbol("-");
    }

    void AddSymbol(string symbol)
    {
        currentInput += symbol;
        displayText.text = string.Join(" ", currentInput.ToCharArray());
    }

    public void Enter()
    {
        if (currentInput == correctCode)
        {
            door.Interact();
            keypad.SetActive(false);

            keypadActivescript.PuzzelSolved();
        }
        else
        {
            Debug.Log("Wrong code!");
            Clear();
        }


    }

    public void Clear()
    {
        currentInput = "";
        displayText.text = "";
    }
}
