using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

/*
--------------------------------------------------------------------------------------
Development Journal
--------------------------------------------------------------------------------------

Notes for before submission to myself @alasdair
** Add assembly defintions
** Add namespaces

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
Modified anchor positions and axis values on all Carriages such that the piviot point was consistent for each point. (original pivots that varied on the x axis made the torque unstable)
Increaed Angular dampening (not required, however a strong force on the ferris wheel caused carriages to swing with high values of momentum) 
Decisions around Rigidbody & constraints were made mostly on Game Feel- it's possible to make the carriages momentum potentially more realisitc by removing this dampening and modifying other properties such as
Limits, et al but these dampening modification felt best (personally) when playtesting. 
Comments on code explain all other changes.

** Task 2 ** 
Installed Android Build support Module for this Unity Editor version then switched the build target to android in this project
Deployed to personal android device (OnePlus) as an initial test before adding plugin complexity

Installed C++ android development tools and C++ Clang tools. 
Created dynamic shared library project in Visual studio, but encountered path issues due to outdated template -> attempted to refactor the project paths to updated ndk path support 
Refactor proved more time consuming than generating a new project so reconsidered .so development options. 
Created an android Studio App and reworked the CMake for .so build support

Issues encountered
fno-exceptions Unity warning -> reworked CMake to account for this
Shared Library is not 16kb aligned -> reworked to support Arm64 and retargeted player settings to correct android platform for my Oneplus Device 
Plugin not found errors -> silly errors on my part keeping #define DLLExport __declspec(dllexport) despite not targeting windows platforms and not excluding the lib prefix from the extern plugin name reference

While debugging Plugin not found errors discovered full unity documentation for android plugin development and samples after heading to developer.android.com 
To confirm currently player settings imported sample .so file from unity sample and deployed to my Android device


Discovered graphical issue on android device- (sparkling white dots see gif provided)


** Task 3 ** 
Looked at Documetation for IParallelJobs otherwise no noteworthy implementation details other than code in RSummation.cs. 


** Notes on AI and LLMs **

No LLMs were used to generate any code that appears in this project 
ChatGPT was used a couple of times for basic documentation queries where a LLM was more pratcical than search results

sample:
"Whats the syntax for assembly variables in Unity. i.e constants in the ide that ignores code when set to false (uses # syntax in cpp)?"
as a shortcut to reading documentation on define symbols and the location of scripting define symbols in the Editor
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
