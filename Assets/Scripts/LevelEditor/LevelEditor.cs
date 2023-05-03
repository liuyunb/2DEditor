using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

public class LevelEditor : MonoBehaviour
{
    public enum OperationMode
    {
        Draw,
        Erase
    }
    
    public enum BrushType
    {
        Ground,
        Player
    }

    private OperationMode _curMode = OperationMode.Draw;
    private BrushType _curBrush = BrushType.Ground;
    
    public SpriteRenderer emptyHighlight;

    private float _curHighLightPosX;
    private float _curHighLightPosY;
    private bool _canDraw;
    private bool _isRefresh;

    private GameObject _objMouseOn;

    private void Start()
    {
        SetCurPos(transform.position);
    }

    private readonly Lazy<GUIStyle> _labelText = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.label)
    {
        fontSize = 30,
        alignment = TextAnchor.MiddleCenter
    });
    
    private readonly Lazy<GUIStyle> _button = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
    {
        fontSize = 30,
    });
    
    private readonly Lazy<GUIStyle> _rButton = new Lazy<GUIStyle>(() => new GUIStyle(GUI.skin.button)
    {
        fontSize = 25,
    });

    private void OnGUI()
    {
        var modelLabelRect = RectHelper.RectForAnchorCenter(Screen.width * 0.5f, 35, 200, 50);

        if (_curMode == OperationMode.Draw)
        {
            GUI.Label(modelLabelRect, _curMode.ToString() + " : " + _curBrush.ToString(), _labelText.Value);
        }
        else
        {
            GUI.Label(modelLabelRect, _curMode.ToString(), _labelText.Value);
        }
        
        

        var drawBtn = new Rect(10, 10, 150, 40);
        if (GUI.Button(drawBtn, "绘制", _button.Value))
        {
            _curMode = OperationMode.Draw;
        }
        
        var eraseBtn = new Rect(10, 60, 150, 40);
        if (GUI.Button(eraseBtn, "橡皮", _button.Value))
        {
            _curMode = OperationMode.Erase;
        }

        if (_curMode == OperationMode.Draw)
        {
            var groundBtn = new Rect(Screen.width - 110, 10, 120, 40);
            if (GUI.Button(groundBtn, "地块", _button.Value))
            {
                _curBrush = BrushType.Ground;
            }
        
            var playerBtn = new Rect(Screen.width - 110, 60, 120, 40);
            if (GUI.Button(playerBtn, "主角", _button.Value))
            {
                _curBrush = BrushType.Player;
            }
        }

        var saveBtn = new Rect(Screen.width - 110, Screen.height - 60, 120, 40);
        if (GUI.Button(saveBtn, "保存", _rButton.Value))
        {
            List<LevelItemInfo> infos = new List<LevelItemInfo>(transform.childCount);

            foreach (Transform child in transform)
            {
                var position = child.position;
                infos.Add(new LevelItemInfo()
                {
                    Name = child.name,
                    X = position.x,
                    Y = position.y
                });
            }

            XmlDocument document = new XmlDocument();
            var declaration = document.CreateXmlDeclaration("1.0", "UTF-8", "");
            document.AppendChild(declaration);

            var level = document.CreateElement("Level");
            document.AppendChild(level);

            foreach (var info in infos)
            {
                var levelItem = document.CreateElement("LevelItem");
                levelItem.SetAttribute("name", info.Name);
                levelItem.SetAttribute("x", info.X.ToString(CultureInfo.InvariantCulture));
                levelItem.SetAttribute("y", info.Y.ToString(CultureInfo.CurrentCulture));

                level.AppendChild(levelItem);
            }
            
            ReadXml(document);

            var levelFileFolder = Application.persistentDataPath + "/LevelFiles";

            if (!Directory.Exists(levelFileFolder))
            {
                Directory.CreateDirectory(levelFileFolder);
            }

            var levelFilePath = levelFileFolder + "/" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".xml";
            
            document.Save(levelFilePath);

        }

    }
    
    public class LevelItemInfo
    {
        public string Name;
        public float X;
        public float Y;
    }

    // Update is called once per frame
    void Update()
    {
        var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouseWorldPos.x = Mathf.Floor(mouseWorldPos.x + 0.5f);
        mouseWorldPos.y = Mathf.Floor(mouseWorldPos.y + 0.5f);
        mouseWorldPos.z = 0;

        emptyHighlight.gameObject.SetActive(GUIUtility.hotControl == 0);


        if (Mathf.Approximately(_curHighLightPosX, mouseWorldPos.x) &&
            Mathf.Approximately(_curHighLightPosY, mouseWorldPos.y) && _isRefresh == false)
        {
            
        }
        else
        {
            var tempPos = mouseWorldPos;
            tempPos.z = -1;
            emptyHighlight.transform.position = tempPos;
            SetCurPos(tempPos);
            
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, Vector2.zero, 20);
            
            if (hit.collider)
            {
                if (_curMode == OperationMode.Draw)
                {
                    emptyHighlight.color = new Color(1, 0, 0, 0.5f);
                }
                else if(_curMode == OperationMode.Erase)
                {
                    emptyHighlight.color = new Color(1, 0.5f, 0, 0.5f);
                }
                
                _canDraw = false;
                _objMouseOn = hit.collider.gameObject;
            }
            else
            {
                if (_curMode == OperationMode.Draw)
                {
                    emptyHighlight.color = new Color(1, 1, 1, 0.5f);
                }
                else if(_curMode == OperationMode.Erase)
                {
                    emptyHighlight.color = new Color(0, 0, 1, 0.5f);
                }
                
                _canDraw = true;
                _objMouseOn = null;
            }

            _isRefresh = false;
        }
        
        if (Input.GetMouseButton(0) && GUIUtility.hotControl == 0)
        {
            if (_canDraw && _curMode == OperationMode.Draw)
            {
                if (_curBrush == BrushType.Ground)
                {
                    var obj = Resources.Load<GameObject>("Ground");
                    var ground = Instantiate(obj, transform);

                    ground.transform.position = mouseWorldPos;
                    ground.name = "Ground";
                    _canDraw = false;
                }
                else if (_curBrush == BrushType.Player)
                {
                    var obj = Resources.Load<GameObject>("Ground");
                    var player = Instantiate(obj, transform);

                    player.transform.position = mouseWorldPos;
                    player.name = "Player";
                    
                    player.GetComponent<SpriteRenderer>().color = Color.cyan;
                    _canDraw = false;
                }
            }
            else if (_objMouseOn && _curMode == OperationMode.Erase)
            {
                Destroy(_objMouseOn);
                _objMouseOn = null;
                _isRefresh = true;
            }
            

        }
        
    }
    

    public void SetCurPos(Vector3 position)
    {
        _curHighLightPosX = Mathf.Floor(position.x + 0.5f);
        _curHighLightPosY = Mathf.Floor(position.y + 0.5f);
    }

    public void ReadXml(XmlDocument document)
    {
        var stringBuilder = new StringBuilder();
        var stringWriter = new StringWriter(stringBuilder);
        var xmlWriter = new XmlTextWriter(stringWriter);
        xmlWriter.Formatting = Formatting.Indented;

        document.WriteTo(xmlWriter);
        Debug.Log(stringBuilder.ToString());
    }
}
