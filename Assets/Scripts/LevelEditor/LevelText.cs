using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using UnityEngine;
using UnityEngine.UI;

public class LevelText : MonoBehaviour
{
    public enum State
    {
        Selection,
        Playing
    }

    private State _state = State.Selection;

    private string _levelFileFolder;

    private void Awake()
    {
        _levelFileFolder = Application.persistentDataPath + "/LevelFiles";
        Debug.Log(_levelFileFolder);
    }

    // Start is called before the first frame update
    void ParseAndRun(string xml)
    {
        var document = new XmlDocument();

        document.LoadXml(xml);

        var levelNode = document.SelectSingleNode("Level");

        foreach (XmlElement item in levelNode.ChildNodes)
        {
            var itemName = item.Attributes["name"].Value;
            var itemX = float.Parse(item.Attributes["x"].Value);
            var itemY = float.Parse(item.Attributes["y"].Value);

            var levelItem = Resources.Load<GameObject>(itemName);
            var itemObj = Instantiate(levelItem, transform);
            itemObj.transform.position = new Vector3(itemX, itemY, 0);

            Debug.Log(itemName + itemX + itemY);
        }
    }

    private void OnGUI()
    {
        if (_state == State.Selection)
        {
            int y = 10;

            var filePaths = Directory.GetFiles(_levelFileFolder).Where(f => f.EndsWith("xml"));

            foreach (var filePath in filePaths)
            {
                var fileName = Path.GetFileName(filePath);
                if (GUI.Button(new Rect(10, y, 200, 40), fileName))
                {
                    var xml = File.ReadAllText(filePath);
                    ParseAndRun(xml);

                    _state = State.Playing;
                }

                y += 60;
            }
        }
    }
}
