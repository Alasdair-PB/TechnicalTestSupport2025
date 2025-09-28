using UnityEngine;
using UnityEngine.InputSystem;

public class SpinningWheel : MonoBehaviour
{
    public Rigidbody m_rigidBody;

    // Changed to [SerializeField] for testing
    [SerializeField] private float m_spinSpeed = 5f;
    [SerializeField] private float m_boostStartMult = 3f;
    [SerializeField] private float m_boostForce = 500f;
    private PauseScreenManager pauseManager;

    private float m_scaledBoostForce;
    private InputAction boostAction;

    private float timer;
    private bool physicsFlag;

    // Moved action binding & pauseManager assignment to Awake due to personal preference
    private void Awake()
    {
        Physics.simulationMode = SimulationMode.Script;
        boostAction = InputSystem.actions.FindAction("Boost");
        pauseManager = FindFirstObjectByType<PauseScreenManager>();
    }

    // Added Event cleanup on disable & pause binding to avoid multiple multiple scripts performing the same pause check. 
    // Additionally wanted to avoid a scenario where the PauseSceenManager would have to call a method on the SpinningWheel (Cyclic dependencies)
    private void OnEnable()
    {
        pauseManager.onPause += OnPause;
        pauseManager.ForceSlider.onValueChanged.AddListener(OnJumpSliderChange);
    }

    private void OnDisable()
    {
        pauseManager.onPause -= OnPause;
        pauseManager.ForceSlider.onValueChanged.RemoveListener(OnJumpSliderChange);
    }

    private void Start()
    {
        physicsFlag = !pauseManager.GetPauseState();
        m_scaledBoostForce = m_boostForce * pauseManager.ForceSlider.value;
    }

    private void DebugPhysics() 
        => Debug.Log(m_rigidBody.GetAccumulatedTorque(Time.fixedDeltaTime));
    
    private void SimulatePhysics()
    {
        // DebugPhysics(); // Used to check accumulated torque per frame during testing
        Physics.Simulate(Time.fixedDeltaTime);
        m_rigidBody.AddTorque(Vector3.forward * m_spinSpeed);
        if (boostAction.IsPressed()) Boost();
    }

    // Modified to only advance the simulation when a time period of Time.fixedDeltaTime has past then simulate in portions of Time.fixedDeltaTime
    // (https://docs.unity3d.com/ScriptReference/Physics.Simulate.html) 
    // Why not just use FixedUpdate => 'the interval between fixed updates isn’t fixed'. 
    // (https://docs.unity3d.com/6000.2/Documentation/Manual/fixed-updates.html#:~:text=The%20fixed%20update%20loop%20simulates,are%20not%20in%20perfect%20sync).
    private void UpdateSimulation()
    {
        if (!physicsFlag) 
            return;

        timer += Time.deltaTime;

        while (timer >= Time.fixedDeltaTime)
        {
            timer -= Time.fixedDeltaTime;
            SimulatePhysics();
        }
    }

    /* There are two Input checks for the boostAction 
    This first occurs here where a check for the initial boost frame press is done. If true a boost start up burst of speed is added
    The second occurs each frame the boostAction is held during the physicsSimulation step 
    This startup and held boost was adding for game feel */
    private void CheckInutBindings() 
    {
        if (boostAction.WasPressedThisFrame())
            BoostStartUp();
    }

    public void Update()
    {
        UpdateSimulation();
        CheckInutBindings();
    }

    private void BoostStartUp()
    {
        m_rigidBody.AddTorque((Vector3.forward * m_boostStartMult) * m_scaledBoostForce);
    }

    private void Boost()
    {
        m_rigidBody.AddTorque(Vector3.forward * m_scaledBoostForce);
    }

    private void OnPause(bool pauseState)
    {
        physicsFlag = !pauseState;
    }

    private void OnJumpSliderChange(float value)
    {
        m_scaledBoostForce = m_boostForce * value;
    }
}
