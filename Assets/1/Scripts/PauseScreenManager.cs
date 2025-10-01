using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class PauseScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private Slider forceSlider;
    public event Action<bool> onPause;

    public Slider ForceSlider => forceSlider;
    public bool GetPauseState() => m_paused;
    public InputAction PauseAction => pauseAction;
    
    private bool m_paused = false;
    
    private InputAction pauseAction;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        pauseAction = InputSystem.actions.FindAction("Cancel");
    }

    void Update()
    {
        if (pauseAction.WasPressedThisFrame())
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        m_paused = !m_paused;
        pauseScreen.SetActive(m_paused);
        onPause?.Invoke(m_paused);
    }
}
