using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProvinceBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private InteractiveMapCameraController _cameraControllerRef;
    [SerializeField] private ProvinceProperties _provincePropertiesScriptable;

    //Buttons Variables
    [SerializeField] private Vector3 _backButtonOffset;
    [SerializeField] private Vector3 _playButtonOffset;
    [SerializeField] private GameObject _backButtonRef;
    [SerializeField] private GameObject _playGameButtonRef;
    [SerializeField] private float _delayBeforeShowingButtons = 1;

    //Image Effects Variables
    [SerializeField] private GameObject _blurImage;
    [SerializeField] private Sprite _whiteDotSprite;

    //Province descriptions
    [SerializeField] private GameObject _provinceDescription;
    [SerializeField] private AudioClip _hoverProvinceAudio;
    [SerializeField] private AudioClip _clickProvinceAudio;

    private Image _imageRef;
    private Sprite _initialSprite;

    private Color _currentImageColor;
    private InteractiveMapButtons _interactiveMapButtonRef;
    private bool _isFocused = false;
    private AudioSource _provinceAudioSource;

    private void Awake()
    {
        _provinceAudioSource = GetComponent<AudioSource>();

        _interactiveMapButtonRef = _backButtonRef.GetComponent<InteractiveMapButtons>();
        _interactiveMapButtonRef.OnBackButtonPressed += OnBackButtonPressedHandler;

        InitializeImageAndSprite();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!_isFocused)
        {
            _cameraControllerRef.AssignNewFieldViewAndTarget(10, this.transform, this.transform);
            StartCoroutine(AdjustButtons());

            _imageRef.enabled = true;

            _blurImage.transform.SetAsLastSibling();
            _blurImage.SetActive(true);
            this.transform.SetAsLastSibling();

            _provinceDescription.SetActive(true);

            _isFocused = true;
            _currentImageColor.a = 100;
            _imageRef.color = _currentImageColor;

            GameManager.instance.ResetInitValues();
            GameManager.instance.InitGameManagerProperties(_provincePropertiesScriptable);

            _provinceAudioSource.clip = _clickProvinceAudio;
            _provinceAudioSource.Play();
        }        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_isFocused)
        {
            _imageRef.sprite = _initialSprite;
            _imageRef.SetNativeSize();

            _currentImageColor.a = 100;
            _imageRef.color = _currentImageColor;

            _provinceAudioSource.clip = _hoverProvinceAudio;
            _provinceAudioSource.Play();
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_isFocused)
        {
            _currentImageColor.a = 0;
            _imageRef.color = _currentImageColor;

            _imageRef.sprite = _whiteDotSprite;
            _imageRef.SetNativeSize();
        }
    }

    IEnumerator AdjustButtons()
    {
        yield return new WaitForSeconds(_delayBeforeShowingButtons);
        ButtonsAdjustments();
    }

    private void ButtonsAdjustments()
    {
        _backButtonRef.transform.position = this.transform.position - _backButtonOffset;
        _backButtonRef.SetActive(true);

        _playGameButtonRef.transform.position = this.transform.position + _playButtonOffset;
        _playGameButtonRef.SetActive(true);
    }

    private void OnBackButtonPressedHandler()
    {
        _provinceDescription.SetActive(false);

        _currentImageColor.a = 0;
        _imageRef.color = _currentImageColor;

        _blurImage.SetActive(false);
        _isFocused = false;
    }

    private void InitializeImageAndSprite()
    {
        _imageRef = this.GetComponentInChildren<Image>();

        _currentImageColor = _imageRef.color;
        _currentImageColor.a = 0;
        _imageRef.color = _currentImageColor; // Ahora, cuando vuelvas lo que vas a tener que hacer es que el cambio de alfa lo haga cada vez que te paras encima y cuando salis

        _initialSprite = _imageRef.sprite;
        _imageRef.sprite = _whiteDotSprite;
        _imageRef.SetNativeSize();

        _provinceDescription.SetActive(false);
    }
}
