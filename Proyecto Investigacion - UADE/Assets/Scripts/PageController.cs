using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageController : MonoBehaviour
{
    [Header("Reference Buttons")]
    [SerializeField] private Button nextButton;
    [SerializeField] private Button prevButton;

    [Header("Pages"), Space]
    [SerializeField] private List<GameObject> listPage = new List<GameObject>();

    private int index;

    private void Awake()
    {
        index = 0;
    }

    private void Start()
    {
        nextButton.onClick.AddListener(NextButtonHandler);
        prevButton.onClick.AddListener(PrevButtonHandler);

        if (listPage[index] != null)
        {
            listPage[index].SetActive(true);
        }
    }

    private void NextButtonHandler()
    {
        if (index < listPage.Count - 1)
        {
            listPage[index].SetActive(false);
            index++;
            listPage[index].SetActive(true);
        }
    }

    private void PrevButtonHandler()
    {
        if (index > 0)
        {
            listPage[index].SetActive(false);
            index--;
            listPage[index].SetActive(true);
        }
    }
}
