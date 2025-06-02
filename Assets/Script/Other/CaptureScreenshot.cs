using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaptureScreenshot : MonoBehaviour
{
    public string FileName;
    public RectTransform UIRect;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            string fileName = Application.dataPath + "/Resources/Image/Character/" + FileName + ".png";
            StartCoroutine(Capture(fileName));
        }
    }

    public IEnumerator Capture(string fileName) 
    {
        yield return new WaitForEndOfFrame();
        int width = (int)UIRect.rect.width;
        int height = (int)UIRect.rect.height;
        Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, false);

        float x = UIRect.transform.position.x + UIRect.rect.xMin;
        float y = UIRect.transform.position.y + UIRect.rect.yMin;
        texture.ReadPixels(new Rect(x, y, width, height), 0, 0);
        texture.Apply();
        byte[] bytes = texture.EncodeToPNG();
        System.IO.File.WriteAllBytes(fileName, bytes);
    }
}
