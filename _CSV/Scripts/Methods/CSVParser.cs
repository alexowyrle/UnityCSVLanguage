using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class CSVParser
{
    private static char rowSplitter = '\n';

    #region Public Methods
    public static TextAsset GetTextAsset(string _path)
    {
        if (File.Exists(_path))
        {
            return new TextAsset(File.ReadAllText(_path));
        }
        else
        {
            Debug.LogError("Error findind csv file. Check that the route is correct and it contains the name and extension of the file.");
            return null;
        }
    }
    /// <summary>
    /// Returns from a csv text the exact row asked
    /// </summary>
    /// <param name="_csvText"></param>
    /// <param name="_row"></param>
    /// <returns></returns>
    public static string[] GetCSVSplittedRow(string _csvText, int _row)
    {
        string[] _rawRows = CSVToRawRows(_csvText);
        string[] _splittedRow = SplitCSVRows(_rawRows[_row]);

        return _splittedRow;
    }
    /// <summary>
    /// Returns an array of arrays with all the rows splitted
    /// </summary>
    /// <param name="_csvText"></param>
    /// <returns></returns>
    public static string[][] GetCSVSplittedRows(string _csvText)
    {
        string[] _rawRows = CSVToRawRows(_csvText);
        string[][] _splittedRowResult = new string[_rawRows.Length][];

        for (int i = 0; i < _rawRows.Length; i++)
        {
            string[] _splittedRow = SplitCSVRows(_rawRows[i]);
            _splittedRowResult[i] = _splittedRow;
        }

        return _splittedRowResult;
    }
    #endregion
    #region Local Methods
    /// <summary>
    /// Split the CSV in rows (with the separators. RAW)
    /// </summary>
    /// <param name="_csv"></param>
    /// <returns>Returns the raw rows</returns>
    private static string[] CSVToRawRows(string _csv)
    {
        string[] _data = _csv.Split(rowSplitter);
        return _data;
    }
    /// <summary>
    /// Splits the row in different strings (Fields)
    /// </summary>
    /// <param name="_row"></param>
    /// <returns>Returns an array with all the fields</returns>
    private static string[] SplitCSVRows(string _row)
    {
        List<string> _result = new List<string>();
        StringBuilder _currentStr = new StringBuilder();
        bool _inQuotes = false;

        for (int i = 0; i < _row.Length; i++) // For each character
        {
            if (_row[i] == '\"') // Quotes are closing or opening
                _inQuotes = !_inQuotes;
            else if (_row[i] == ',') // Comma
            {
                if (!_inQuotes) // If not in quotes, end of current string, add it to result
                {
                    _result.Add(_currentStr.ToString());
                    _currentStr.Clear();
                }
                else
                    _currentStr.Append(_row[i]); // If in quotes, just add it 
            }
            else // Add any other character to current string
                _currentStr.Append(_row[i]);
        }

        _result.Add(_currentStr.ToString());
        return _result.ToArray(); // Return array of all strings
    }
    #endregion
}