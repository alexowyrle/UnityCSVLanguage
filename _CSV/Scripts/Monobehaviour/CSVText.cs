using UnityEngine;
using TMPro;

public class CSVText : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private string id = "";
    private TextMeshProUGUI textMeshProUGUI;
    private void Start()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        SetText();

        //If the language changes the text gets updated
        CSVLanguage.i.RegisterOnChangeLanguage(SetText);
    }

    private void SetText()
    {
        string _text = CSVLanguage.i.GetText(id);
        textMeshProUGUI.SetText(_text);
    }
}
