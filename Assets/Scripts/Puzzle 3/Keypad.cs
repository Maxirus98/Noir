using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Keypad : MonoBehaviour
{
    public string correctCode = "";
    private string currentInput = "";

    public TextMeshProUGUI displayText;
    public GameObject door;
    public GameObject behindDoor;
    public GameObject keypad;
    public KeypadActive keypadActivescript;


    private void Start()
    {
       
    }
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
        Debug.Log($"Current Input: {currentInput}");
        displayText.text = string.Join(" ", currentInput.ToCharArray());
    }

    public void Enter()
    {
        // To come back from a char array
        currentInput = currentInput.ToString().Trim();
        correctCode = correctCode.ToString().Trim();
        if (currentInput.Equals(correctCode))
        {
            keypadActivescript.PuzzelSolved();
            door.SetActive(false);
            behindDoor.SetActive(true);
            keypad.SetActive(false);
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
