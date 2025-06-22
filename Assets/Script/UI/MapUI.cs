using Explore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : MonoBehaviour
{
    private readonly float _scale = 20;

    public RawImage Map;
    public Image Mask;
    public Image Player;
    public GameObject BG;

    private int _width;
    private int _height;
    private Texture2D _mapTexture;
    private Color _color = new Color32(255, 236, 191, 255);
    private Dictionary<Vector2Int, GameObject> _iconDic = new Dictionary<Vector2Int, GameObject>();
    private Dictionary<ExploreEnemyController, GameObject> _enemyDic = new Dictionary<ExploreEnemyController, GameObject>();

    public void InitMap(int width, int height)
    {
        _width = width;
        _height = height;
        _mapTexture = new Texture2D(width, height);
        _mapTexture.filterMode = FilterMode.Point;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {

                _mapTexture.SetPixel(i, j, _color);

            }
        }

        _mapTexture.Apply();
        Map.rectTransform.sizeDelta = new Vector2(width * _scale, height * _scale);
        Map.texture = _mapTexture;
    }

    public void SetMap(Vector2Int position, Color color)
    {
        _mapTexture.SetPixel(position.x, position.y, color);
        _mapTexture.Apply(false);
    }

    public void SetPlayerPosition(Vector2 position) 
    {
        Vector2 v2 = new Vector2(position.x - _width / 2f + 0.5f, position.y - _height / 2f + 0.5f);
        Map.transform.localPosition = v2 * -_scale;
        Player.transform.localPosition = v2 * _scale;
    }

    public void SetIcon(Vector2Int position, string name) 
    {
        GameObject gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Icon/" + name), Vector3.zero, Quaternion.identity);
        gameObj.transform.SetParent(Map.transform);
        Vector2 v2 = new Vector2(position.x - _width / 2f + 0.5f, position.y - _height / 2f + 0.5f);
        gameObj.transform.localPosition = v2 * _scale;
        _iconDic.Add(position, gameObj);
    }

    public void ClearIcon(Vector2Int position)
    {
        GameObject obj = _iconDic[position];
        Destroy(obj);
        _iconDic.Remove(position);
    }

    public void ShowEnemy(Vector3 position, ExploreEnemyController enemy)
    {
        Vector2 v2 = new Vector2(position.x - _width / 2f + 0.5f, position.z - _height / 2f + 0.5f);
        if (!_enemyDic.ContainsKey(enemy))
        {
            GameObject gameObj = (GameObject)GameObject.Instantiate(Resources.Load("Prefab/Explore/Icon/Enemy"), Vector3.zero, Quaternion.identity);
            gameObj.transform.SetParent(Map.transform);
            gameObj.transform.localPosition = v2 * _scale;
            _enemyDic.Add(enemy, gameObj);
        }
        else
        {
            _enemyDic[enemy].SetActive(true);
            _enemyDic[enemy].transform.localPosition = v2 * _scale;
        }
    }

    public void HideEnemy(ExploreEnemyController enemy) 
    {
        if (_enemyDic.ContainsKey(enemy))
        {
            _enemyDic[enemy].SetActive(false);
        }
    }

    public void ClearEnemy(ExploreEnemyController enemy)
    {
        GameObject obj = _enemyDic[enemy];
        Destroy(obj);
        _enemyDic.Remove(enemy);
    }

    public void ShowBigMap() 
    {
        BG.SetActive(true);
        Mask.transform.localPosition = Vector3.zero;
        Mask.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        ExploreManager.Instance.Player.Enable = false;
    }

    public void HideBigMap() 
    {
        BG.SetActive(false);
        Mask.transform.localPosition = new Vector3(Screen.width / 2f - 100, Screen.height / 2f - 100, 0);
        Mask.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        ExploreManager.Instance.Player.Enable = true;
    }
}
