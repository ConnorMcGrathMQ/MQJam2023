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
    public Image drawText;
    public Image eraseButton;
    public Image eraseText;
    public Image textContainer;
    public TextMeshProUGUI lengthText;
    
    public Color activeButtonColour;
    public Color inactiveButtonColour;
    public AnimationCurve fadeOutCurve;
    public float fadeOutDuration = 1;
    public Coroutine fadingRoutine;
    public Image levelComplete;
    public TextMeshProUGUI completionText;
    public TextMeshProUGUI boardFilledText;
    public TextMeshProUGUI endGameText;
    public Button nextLevelButton;

    public GameObject stars;

    void Awake() {
        if(UIManager.Instance == null) {
            Instance = this;
            levelComplete.gameObject.SetActive(false);
        } else {
            Debug.Log("Multiple UI Managers exist! Destroying...");
            Destroy(this.gameObject);
        }
    }

    public void SetDrawingMode() {
        PlayerController.Instance.erasing = false;
        drawButton.color = activeButtonColour;
        drawText.color = activeButtonColour;
        eraseButton.color = inactiveButtonColour;
        eraseText.color = inactiveButtonColour;

    }

    public void SetErasingMode() {
        PlayerController.Instance.erasing = true;
        drawButton.color = inactiveButtonColour;
        eraseButton.color = activeButtonColour;
        drawText.color = inactiveButtonColour;
        eraseText.color = activeButtonColour;

    }

    public void UpdateLengthText(Plant p) {
        lengthText.text = $"Length: {p.remainingDist}";
    }

    public IEnumerator HideLengthText() {
        yield return new WaitForSeconds(2);
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

    public void OpenLevelComplete() {
        levelComplete.gameObject.SetActive(true);
        completionText.text = $"Level {Board.Instance.CurrentLevel} Complete!";
        boardFilledText.text = $"Filled {Board.Instance.GetFilledPercent()}% of the Board!";
        if (Board.Instance.GetFilledPercent() > 33f) {
            stars.transform.GetChild(1).GetComponent<Image>().color = Color.yellow;
        } 
        if (Board.Instance.GetFilledPercent() > 67f) {
            stars.transform.GetChild(2).GetComponent<Image>().color = Color.yellow;
        }
        if(Board.Instance.CurrentLevel >= Board.Instance.levels.Count) {
            nextLevelButton.gameObject.SetActive(false);
            endGameText.gameObject.SetActive(true);
        }
    }

    public void CloseLevelComplete() {
        levelComplete.gameObject.SetActive(false);
    }
}
