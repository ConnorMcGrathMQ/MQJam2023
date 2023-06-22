using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;
    public static UIManager Instance
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
    public Image drawButton;
    public Image eraseButton;
    public Image textContainer;
    public TextMeshProUGUI lengthText;
    
    public Color activeButtonColour;
    public Color inactiveButtonColour;
    public AnimationCurve fadeOutCurve;
    public float fadeOutDuration = 1;

    private Coroutine fadingRoutine;

    void Awake() {
        if(UIManager.Instance == null) {
            Instance = this;
        } else {
            Debug.Log("Multiple UI Managers exist! Destroying...");
            Destroy(this.gameObject);
        }
    }

    public void SetDrawingMode() {
        PlayerController.Instance.erasing = false;
        drawButton.color = activeButtonColour;
        eraseButton.color = inactiveButtonColour;

    }

    public void SetErasingMode() {
        PlayerController.Instance.erasing = true;
        drawButton.color = inactiveButtonColour;
        eraseButton.color = activeButtonColour;

    }

    public void UpdateLengthText(Plant p) {
        lengthText.text = $"Length: {p.remainingDist}";
    }

    public IEnumerator HideLengthText() {
        float timer = 0;
        while(timer < fadeOutDuration) {
            // Debug.Log($"Fading at {timer} | Color is {textContainer.color.a}");
            timer += Time.deltaTime;
            textContainer.color = new Color(textContainer.color.r, 
                textContainer.color.g, textContainer.color.b, fadeOutCurve.Evaluate(timer / fadeOutDuration));
            lengthText.color = new Color(lengthText.color.r, 
                lengthText.color.g, lengthText.color.b, fadeOutCurve.Evaluate(timer / fadeOutDuration));
            
            yield return null;
        }
    }

    public void ShowLengthText() {
        if(fadingRoutine != null) {
            StopCoroutine(fadingRoutine);
        }
        textContainer.color = new Color(textContainer.color.r, 
                textContainer.color.g, textContainer.color.b, 1);
        lengthText.color = new Color(lengthText.color.r, 
                lengthText.color.g, lengthText.color.b, 1);
    }
}
