using UnityEngine;
using UnityEngine.InputSystem;

public class SpinningWheel : MonoBehaviour
{
    public Rigidbody m_rigidBody;

    private float m_spinSpeed = 5f;
    private float m_boostForce = 500f;
    
    private float m_scaledBoostForce;
    
    private InputAction boostAction;

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

    public void Update()
    {
        Physics.Simulate(Time.fixedDeltaTime);
        
        m_rigidBody.AddTorque(Vector3.forward * m_spinSpeed);
        
        if (boostAction.WasPressedThisFrame())
        {
            Boost();
        }
    }

    // Cannot be held :(
    private void Boost()
    {
        m_rigidBody.AddTorque(Vector3.forward * m_scaledBoostForce);
        Debug.Log("Boost");
    }

    private void OnJumpSliderChange(float value)
    {
        m_scaledBoostForce = m_boostForce * value;
    }
}
