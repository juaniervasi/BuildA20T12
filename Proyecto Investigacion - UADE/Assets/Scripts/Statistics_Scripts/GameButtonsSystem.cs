using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameButtonsSystem : MonoBehaviour
{
    [SerializeField] private GameObject _helpButton;
    [SerializeField] private GameObject _backButton;
    [SerializeField] private GameObject _finishMatchButton;
    [SerializeField] private GameObject _optionsGraphsButtons;
    [SerializeField] private GameObject _endGameButtonRef;

    [SerializeField] private GameObject _playAgainButton;
    [SerializeField] private float _delayToShowPlayAgainButton = 2.3f;

    [SerializeField] private Vector3 _maxSize;

    [SerializeField] private float _fadeInTime = 4f;
    [SerializeField] private float _fadeOutTime = 2f;

    [SerializeField] private CanvasGroup _gameSceneCanvasGroup;
    [SerializeField] private CanvasGroup _statsSceneCanvasGroup;
    [SerializeField] private CanvasGroup _helpSceneCanvasGroup;

    private GameManager _gameManagerRef;
    private GameObject _currentObjectToShow;
    [SerializeField] private ChangeScene _changeSceneRef;

    private float _currentFadeOutTime = 0;
    private float _fadeOutCounter = 0;
    public List<GameObject> _nextButtonsToUnhide;

    private void Awake()
    {
        _gameManagerRef = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        _gameManagerRef.OnFinishPlays += OnFinishPlaysHandler;
    }

    public void OnPlayAgainButtonPressed()
    {
        _statsSceneCanvasGroup.blocksRaycasts = false;
        StartCoroutine(DelayToBlockRaycast(_gameSceneCanvasGroup, _delayToShowPlayAgainButton));
    }

    public void OnEndMatchButtonPressed()
    {
        _gameSceneCanvasGroup.blocksRaycasts = false;
        StartCoroutine(DelayToBlockRaycast(_statsSceneCanvasGroup, _delayToShowPlayAgainButton));
    }

    public void OnHelpButtonPressed(GameObject newGraphToShow) //Fade out scene , scale graph
    {
        _helpButton.SetActive(false);

        _nextButtonsToUnhide.Clear();
        _nextButtonsToUnhide.Add(_backButton);
        _nextButtonsToUnhide.Add(_optionsGraphsButtons);

        _changeSceneRef.OnChangeOneSceneAlpha(0);

        //newGraphToShow.transform.localRotation = Quaternion.Euler(0, 0, 0);
        newGraphToShow.SetActive(true);
        StartCoroutine(LerpGraphSize(newGraphToShow, Vector3.zero, _maxSize, _fadeInTime, false));

        _currentObjectToShow = newGraphToShow;
    }

    public void OnBackFromHelpButtonPressed()//scale graph , Fade in scene  
    {
        _backButton.SetActive(false);

        _nextButtonsToUnhide.Clear();
        _nextButtonsToUnhide.Add(_helpButton);
        _nextButtonsToUnhide.Add(_finishMatchButton);

        _changeSceneRef.OnChangeOneSceneAlpha(1);
        StartCoroutine(LerpGraphSize(_currentObjectToShow, _maxSize, Vector3.zero, _fadeOutTime, true));
        _optionsGraphsButtons.SetActive(false);
    }

    public void OnChangeGraphPressed(GameObject newGraphToShow)
    {
        _currentObjectToShow.SetActive(false);

        _nextButtonsToUnhide.Clear();
        _nextButtonsToUnhide.Add(_backButton);

        newGraphToShow.SetActive(true);
        StartCoroutine(LerpGraphSize(newGraphToShow, Vector3.zero, _maxSize, _fadeInTime, false));
        _currentObjectToShow = newGraphToShow;
    }

    private void OnFinishPlaysHandler()
    {
        _playAgainButton.SetActive(false);

        _endGameButtonRef.SetActive(true);
        _statsSceneCanvasGroup.blocksRaycasts = false;
        StartCoroutine(DelayToBlockRaycast(_statsSceneCanvasGroup, _delayToShowPlayAgainButton));
    }

    IEnumerator LerpGraphSize(GameObject newGraphToShow, Vector3 initialScaleSize, Vector3 endScaleSize, float scaleTime, bool shouldHideAfterScale)
    {
        _currentFadeOutTime = scaleTime;
        _fadeOutCounter = 0;
        newGraphToShow.transform.localScale = initialScaleSize;

        ManageButtonsActivation(false);

        while (_fadeOutCounter < scaleTime)
        {
            _fadeOutCounter += Time.deltaTime;
            newGraphToShow.transform.localScale = Vector3.Lerp(newGraphToShow.transform.localScale, endScaleSize, _fadeOutCounter / scaleTime);
            yield return null;
        }

        ManageButtonsActivation(true);
        _nextButtonsToUnhide.Clear();
        if (shouldHideAfterScale) { newGraphToShow.SetActive(false); }
    }

    private void ManageButtonsActivation(bool shouldActivate)
    {
        if (shouldActivate)
        {
            foreach (var button in _nextButtonsToUnhide)
            {
                button.SetActive(true);
            }
        }
        else
        {
            foreach (var button in _nextButtonsToUnhide)
            {
                button.SetActive(false);
            }
        }
    }

    IEnumerator DelayToBlockRaycast(CanvasGroup canvasToBlockRaycast, float time)
    {
        canvasToBlockRaycast.blocksRaycasts = false;
        yield return new WaitForSeconds(time);
        canvasToBlockRaycast.blocksRaycasts = true;
    }
}
