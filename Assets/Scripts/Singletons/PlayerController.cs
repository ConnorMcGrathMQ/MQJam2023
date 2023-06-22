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

    private PlantObject targetPlant;
    public Plant plantPrefab;

    public Tile targetTile;

    public int plantsDrawn;
    public bool erasing;

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

    void Start() {
        UIManager.Instance.SetDrawingMode();
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
        StopDrawing();
    }

    public void StopDrawing() {
        if(pressRoutine != null) {
            StopCoroutine(pressRoutine);
        }
        Debug.Log("Trying fade!");
        UIManager.Instance.fadingRoutine = StartCoroutine(UIManager.Instance.HideLengthText());
    }

    private IEnumerator DragMovement() {
        // Tile targetTile;
        Vector3Int targetCell;
        Plant targetPlant = null;
        // Vector2 diff;
        while(true) {
            targetCell = Board.Instance.GetComponent<Grid>().WorldToCell(
                    Camera.main.ScreenToWorldPoint(new Vector3(positionAction.ReadValue<Vector2>().x, 
                        positionAction.ReadValue<Vector2>().y, 0)));
            if(targetCell.x < 0) {
                targetCell = new Vector3Int(0, targetCell.y, 0);
            } else if(targetCell.x >= Board.Instance.width) {
                targetCell = new Vector3Int(Board.Instance.width - 1, targetCell.y, 0);
            }
            if(targetCell.y < 0) {
                targetCell = new Vector3Int(targetCell.x, 0, 0);
            } else if(targetCell.y >= Board.Instance.height) {
                targetCell = new Vector3Int(targetCell.x, Board.Instance.height - 1, 0);
            }
            targetTile = Board.Instance.GetTile(targetCell);
            // Debug.Log(targetCell);
            // Debug.Log(targetTile);
            // Debug.Log(targetTile.ToString());
            // diff = ClampVector(dragAction.ReadValue<Vector2>());
            if(erasing) {
                if(targetTile is Plant p) {
                    // Debug.Log("Trying to erase!");
                    Board.Instance.DestroyPlantFrom(p);
                }
            } else {
                if(targetPlant == null) {
                    if(targetTile is Plant p && p.remainingDist > 0) {
                        targetPlant = p;
                        UIManager.Instance.ShowLengthText();
                    } else if (targetTile is PlantPoint point && plantsDrawn < Board.Instance.GetCurrentLevel().plants.Count) {
                        targetPlant = Instantiate(plantPrefab);
                        targetPlant.connector = point;
                        // targetPlant.species = point.species;
                        targetPlant.species = Board.Instance.GetCurrentLevel().plants[plantsDrawn];
                        targetPlant.pos = point.pos;
                        targetPlant.transform.position = new Vector3(1000, 0, 0);
                        point.Connected(targetPlant);
                        targetPlant.remainingDist = targetPlant.species.maxLength;
                        targetPlant.identifier = plantsDrawn;
                        plantsDrawn++;
                        UIManager.Instance.ShowLengthText();
                    }
                } else if (targetTile is Empty && Board.Instance.AreAnyAdjacentPlants(targetTile)) {
                    // Debug.Log("Added Plant to "+targetTile);
                    Board.Instance.AddTile(targetPlant, targetTile.pos);
                } else if(targetTile is PlantPoint point && targetPlant.connector.partner.Equals(point)) {
                    point.Connected(targetPlant);
                    targetPlant.EnableParticles();
                } else if (targetTile is Plant plant && (plant.next != null || !plant.species.Equals(targetPlant.species))) {
                    Debug.Log($"Intersected Plant! Ended drawing");
                    yield break;
                } else if(targetTile is Obstacle) {
                    Debug.Log("Intersected Obstacle! Ending drawing");
                    yield break;
                }    
            }
            // } else {
            //     Debug.Log("Stop!");
            //     yield break;
            // }

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
    //     Debug.Log(Mouse.current.leftButton.isPressed);
    // }

    // void OnPress() {
    //     Debug.Log("Pressed!");
    // }
}
