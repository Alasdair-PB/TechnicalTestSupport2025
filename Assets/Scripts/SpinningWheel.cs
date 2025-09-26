using UnityEngine;
using UnityEngine.InputSystem;

public class SpinningWheel : MonoBehaviour
{
    public Rigidbody m_rigidBody;

    [SerializeField] private float m_spinSpeed = 5f; // Changed to [SerializeField] while testing
    [SerializeField] private float m_boostForce = 500f;
    
    private float m_scaledBoostForce;
    
    private InputAction boostAction;


    private float timer;

    private void Awake()
    {
        Physics.simulationMode = SimulationMode.Script;
    }

    private void Start()
    {
        boostAction = InputSystem.actions.FindAction("Boost");
        
        PauseScreenManager pauseManager = FindFirstObjectByType<PauseScreenManager>();
        m_scaledBoostForce = m_boostForce * pauseManager.ForceSlider.value;

        pauseManager.ForceSlider.onValueChanged.AddListener(OnJumpSliderChange);
    }

    private void UpdateSimultationLegacy()
        => Physics.Simulate(Time.fixedDeltaTime);

    private void SimulatePhysics()
    {
        Debug.Log(m_rigidBody.GetAccumulatedTorque(Time.fixedDeltaTime));
        Physics.Simulate(Time.fixedDeltaTime);
        m_rigidBody.AddTorque(Vector3.forward * m_spinSpeed);

        if (boostAction.IsPressed()) Boost();
        Debug.Log(m_rigidBody.GetAccumulatedTorque(Time.fixedDeltaTime));
    }

    // Modified to only advance the simulation when a time period of Time.fixedDeltaTime has past then simulate in portions of Time.fixedDeltaTime
    // https://docs.unity3d.com/ScriptReference/Physics.Simulate.html 
    private void UpdateSimulation()
    {
        timer += Time.deltaTime;

        while (timer >= Time.fixedDeltaTime)
        {
            timer -= Time.fixedDeltaTime;
            SimulatePhysics();
        }
    }

    public void Update()
    {
        UpdateSimulation();

        if (boostAction.WasPressedThisFrame())
            Boost();
    }

    private void Boost()
    {
        m_rigidBody.AddTorque(Vector3.forward * m_scaledBoostForce);
    }

    private void OnJumpSliderChange(float value)
    {
        m_scaledBoostForce = m_boostForce * value;
    }
}
