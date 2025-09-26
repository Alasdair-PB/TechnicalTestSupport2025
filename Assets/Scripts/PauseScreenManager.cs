using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/*
--------------------------------------------------------------------------------------
Development Journal
--------------------------------------------------------------------------------------

**Project Setup**
When importing the package there were many missing references, input bindings and package dependencies
I initialy opened project in 2022.3.35 (my default lts editor) errors in the input system led me to import the input package
More errors made me consider my editor version since InputSystem.actions did not exist in prior editor versions so I upgraded the project to the latest release 6000.0.57f1
Custom packages in prior versions display in the package manager since this package import in Unity 6, but wasn't in the UI
    => investigated if this was an issue with the import (no issue just Unity 6 changes- image provided shows differences in Assets folder)
Shader errors led me to install the URP package then assign the MobileRP asset to the default render pipeline
FindAction("Cancel"); caused null references in playmode so I Assigned the InputSystem as the project wide input system and created new Input mapping for 'Boost' action 
TextMeshPro outline shaders were still missing => Window->TextMeshPro->Import TMP Essential Resources to include missing shaders 
Assigned hinge joint references to carriages as these were also missing on import
ReadMe.Asset in the project had Missing Mono script -> opened the file in an IDE to check if this ReadMe might be important however it was just a generic readme for the URP => Removed it from the project
--------------------------------------------------------------------------------------

** Task 1 ** 

Scene changes
Modified anchor positions and axis values on all Carriages such that the piviot point was consistent for each point. 
Increaed Angular dampening (not required, however a strong force on the ferris wheel caused carriages to swing with momentum that impacted the wheel) 

*/

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

    private void Start()
    {
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
