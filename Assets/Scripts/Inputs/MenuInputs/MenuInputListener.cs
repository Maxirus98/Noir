using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuInputListener: MonoBehaviour
{
    public static event Action<Vector2> UINavigate;

    private PlayerMapActions inputs;
    private MenuManager menuManager;
    private AudioManager audioManager;

    private const string SCENE_MAIN_MENU = "MainMenu";
    private const string SCENE_LEVEL = "Level";

    [SerializeField] private float deadZone = 0.5f;
    [SerializeField] private float firstRepeatDelay = 0.15f;
    [SerializeField] private float repeatDelay = 0.1f;

    private Vector2 currentDir;
    private float nextRepeatTime;
    private bool isHolding;

    private void Awake()
    {
        if (InputManager.Instance == null)
        {
            Debug.LogError("InputManager not ready");
            return;
        }

        inputs = InputManager.Instance.Inputs;
    }

    private void Start()
    {
        menuManager = MenuManager.Instance;
        audioManager = AudioManager.Instance;
    }

    private void OnEnable()
    {
        if (inputs == null)
            return;

        inputs.UI.Enable();
        inputs.UI.Cancel.performed += OnCancel;
        inputs.UI.Submit.performed += OnSubmit;
        inputs.UI.Navigate.performed += OnNavigate;
        inputs.UI.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        if (inputs == null)
            return;

        inputs.UI.Cancel.performed -= OnCancel;
        inputs.UI.Submit.performed -= OnSubmit;
        inputs.UI.Navigate.performed -= OnNavigate;
        inputs.UI.Pause.performed -= OnPause;
    }

    private void Update()
    {
        if (!isHolding)
            return;

        if (Time.unscaledTime < nextRepeatTime)
            return;

        UINavigate?.Invoke(currentDir);

        nextRepeatTime = Time.unscaledTime + repeatDelay;
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (IsMainMenuScene() && menuManager.HasOpenMenu && !menuManager.IsMainMenuActive())
        {
            menuManager.CloseMenu();
            audioManager.PlaySound("UI_Back");
        }
    }

    private void OnSubmit(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        //if (menuManager.HasOpenMenu)
        //{
        //    audioManager.PlaySound("UI_Submit");
        //}
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (!IsGameplayScene())
            return;

        if (menuManager == null)
        {
            menuManager = MenuManager.Instance;
            if (menuManager == null)
                return; // scène en transition
        }

        if (!menuManager.IsPauseMenuActive())
        {
            var pauseMenu = menuManager.GetPauseMenu();
            if (pauseMenu != null)
                menuManager.OpenMenu(pauseMenu);
        }
        else
        {
            menuManager.CloseMenu();
        }
    }


    private bool IsMainMenuScene() =>
        SceneManager.GetActiveScene().name == SCENE_MAIN_MENU;

    private bool IsGameplayScene() =>
        SceneManager.GetActiveScene().name == SCENE_LEVEL;

    private void OnNavigate(InputAction.CallbackContext context)
    {
        Vector2 raw = context.ReadValue<Vector2>();

        // Deadzone
        if (raw.magnitude < deadZone)
        {
            isHolding = false;
            currentDir = Vector2.zero;
            return;
        }

        // Axis snap (supprime diagonales)
        Vector2 dir;

        if (Mathf.Abs(raw.x) > Mathf.Abs(raw.y))
            dir = new Vector2(Mathf.Sign(raw.x), 0);
        else
            dir = new Vector2(0, Mathf.Sign(raw.y));

        // Si direction identique - ignore (repeat géré dans Update)
        if (dir == currentDir)
            return;

        currentDir = dir;
        isHolding = true;

        UINavigate?.Invoke(currentDir);

        nextRepeatTime = Time.unscaledTime + firstRepeatDelay;
    }

}