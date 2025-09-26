using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/* Development notes

Opened project is 2022 (my default lts editor) errors in the input system led me to import the input package
More errors made me consider my editor version since InputSystem.actions did not exist in prior editor versions.
Custom packages in prior versions display in the package manager since this package import in Unity 6, but wasn't in the UI
    => I investigated if there was an issue with the import (no issue just Unity 6 changes)
Shader errors led me to install the URP package then assign the MobileRP asset to the default render pipeline
Assigned InputSystem to project wide input system to clear console errors for null reference on FindAction("Cancel"); onPlay

[Could this technical test be changed to include dependencies these package in the future?]

// Assigned hinge joint references to carriages as these were missing on import
// Created new Input mapping for 'Boost' action
*/

public class PauseScreenManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private Slider forceSlider;
    public Slider ForceSlider => forceSlider;
    
    private bool m_paused = false;
    
    private InputAction pauseAction;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
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
    }
}
