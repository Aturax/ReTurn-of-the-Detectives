using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogPanelWindow : MonoBehaviour
{
    [SerializeField] private TMP_Text _headerText;
    [SerializeField] private TMP_Text _bodyText;
    [SerializeField] private Button _acceptButton;
    [SerializeField] private Button _cancelButton;

    public event Action AcceptButtonClicked;

    private void Awake()
    {
        _acceptButton.onClick.AddListener(() => { AcceptButtonClicked(); });
        
        if (_cancelButton != null)
            _cancelButton.onClick.AddListener(() => { gameObject.SetActive(false); });
    }

    public void ShowWindow(bool status)
    {
        gameObject.SetActive(status);
    }

    public void FillTextData(string header, string body)
    {
        _headerText.text = header;
        _bodyText.text = body;
    }
}
