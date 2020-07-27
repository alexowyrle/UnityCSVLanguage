using System;
using UnityEngine;

public class CSVLanguage : MonoBehaviour
{
    #region Variables
    [Header("Variables")]
    [SerializeField] private string csvPath = @"";
    private string language = "English";    //Must have the same name that in the CSV file and be in the first row

    [Header("Local variables")]
    private TextAsset csvFile = default;
    private int languageRow = 0;
    #endregion

    private Action onChangeLanguage;

    private static CSVLanguage _i;
    public static CSVLanguage i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("CSVLanguage")) as GameObject).GetComponent<CSVLanguage>();
            return _i;
        }
    }

    private void Awake()
    {
        csvFile = CSVParser.GetTextAsset(csvPath);
        ChangeLanguage(language);
    }

    #region Local Methods
    /// <summary>
    /// Get the number of the column with your language
    /// </summary>
    /// <param name="_languageSplittedRow">Row with all the languages</param>
    private int GetLanguageColumn(string[] _languageSplittedRow)
    {
        for (int i = 0; i < _languageSplittedRow.Length; i++)
        {
            //We Trim the string since it will have a white space or new line. With that we delete it and get the same string.
            if(String.Equals(language.Trim(), _languageSplittedRow[i].Trim()))
            {
                languageRow = i;
                return i;
            }
        }
        return 0;
    }
    #endregion
    #region Public Methods
    public string GetText(string id)
    {
        //No language assigned or error getting it.
        if(languageRow == 0)
        {
            Debug.LogError("Language not found! Error getting the string");
            return null;
        }

        int _rowToSearchInto = 0;
        string[][] _splittedRows = CSVParser.GetCSVSplittedRows(csvFile.text);

        //Search in which row we must search. Compare the first field (ID) to the parameter.
        for (int i = 0; i < _splittedRows.Length; i++)
        {
            string _firstField = _splittedRows[i][0];
            if(String.Equals(id.Trim(), _firstField.Trim()))
            {
                _rowToSearchInto = i;
                break;
            }
        }

        //Now that we got the row we get that field
        string _fieldText = _splittedRows[_rowToSearchInto][languageRow];
        return _fieldText;
    }
    public void ChangeLanguage(string _language)
    {
        language = _language;
        string[] _languageSplittedRow = CSVParser.GetCSVSplittedRow(csvFile.text, 0);
        languageRow = GetLanguageColumn(_languageSplittedRow);

        if(onChangeLanguage != null)
        {
            onChangeLanguage();
        }
    }
    #endregion
    #region Register
    public void RegisterOnChangeLanguage(Action _action)
    {
        onChangeLanguage += _action;
    }
    public void UnregisterOnChangLanguage(Action _action)
    {
        onChangeLanguage -= _action;
    }
    #endregion
}
