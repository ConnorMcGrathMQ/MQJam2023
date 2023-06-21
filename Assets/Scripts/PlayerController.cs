using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static PlayerController instance;
    public static PlayerController Instance
    {
        get
        {
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    private PlayerInput input;

    private InputAction pressAction;
    private InputAction dragAction;
    private InputAction positionAction;
    private Coroutine pressRoutine;

    void Awake() {
        if(PlayerController.Instance == null) {
            PlayerController.Instance = this;
            input = GetComponent<PlayerInput>();
            if(input == null) {
                Debug.LogError("ERROR: Player Controller must have a Player Input!!");
                return;
            }
            pressAction = input.actions["Press"];
            positionAction = input.actions["Position"];
            dragAction = input.actions["Delta"];
        } else {
            Debug.Log("Multiple Player Controllers Exist!");
        }
    }

    void OnEnable() {
        pressAction.started += PressStarted;
        pressAction.canceled += PressStopped;
    }

    void OnDisable() {
        pressAction.started -= PressStarted;
        pressAction.canceled -= PressStopped;
    }

    private void PressStarted(InputAction.CallbackContext context) {
        pressRoutine = StartCoroutine(DragMovement());
    }

    private void PressStopped(InputAction.CallbackContext context) {
        if(pressRoutine != null) {
            StopCoroutine(pressRoutine);
        }
    }

    private IEnumerator DragMovement() {
        while(true) {
            Debug.Log("Pressing!");
            yield return null;
        }
    }

    private Vector2 ClampVector(Vector2 target) {
        target = target.normalized;
        if(target.x > 0.5f) {
            target = new Vector2(1, target.y);
        } else if(target.x < -0.5f) {
            target = new Vector2(-1, target.y);
        } else {
            target = new Vector2(0, target.y);
        }
        if(target.y > 0.5f) {
            target = new Vector2(target.x, 1);  
        } else if(target.y < -0.5f) {
            target = new Vector2(target.x, -1);
        } else {
            target = new Vector2(target.x, 0);
        }
        return target;
    }

    // void Update() {
    //     Debug.Log(pressAction.ReadValue<float>());
    // }

    void OnPress() {
        Debug.Log("Pressed!");
    }
}
