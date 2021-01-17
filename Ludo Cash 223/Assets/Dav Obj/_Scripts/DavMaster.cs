using System.Collections;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;


public class DavMaster : MonoBehaviour
{

    public static Texture2D RotateTexture2D(Texture2D originalTexture, bool clockwise)
    {
        Color32[] original = originalTexture.GetPixels32();
        Color32[] rotated = new Color32[original.Length];
        int w = originalTexture.width;
        int h = originalTexture.height;

        int iRotated, iOriginal;

        for (int j = 0; j < h; ++j)
        {
            for (int i = 0; i < w; ++i)
            {
                iRotated = (i + 1) * h - j - 1;
                iOriginal = clockwise ? original.Length - 1 - (j * w + i) : j * w + i;
                rotated[iRotated] = original[iOriginal];
            }
        }

        Texture2D rotatedTexture = new Texture2D(h, w);
        rotatedTexture.SetPixels32(rotated);
        rotatedTexture.Apply();
        return rotatedTexture;
    }

    public static Texture2D FlipTexture2D(Texture2D originalTexture)
    {
        Texture2D flipped = new Texture2D(originalTexture.width, originalTexture.height);
        int xN = originalTexture.width;
        int yN = originalTexture.height;


        for (int i = 0; i < xN; i++)
        {
            for (int j = 0; j < yN; j++)
            {
                flipped.SetPixel(xN - i - 1, j, originalTexture.GetPixel(i, j));
            }
        }
        flipped.Apply();

        return flipped;
    }

    public static string Texture2DToString(Texture2D texture)
    {
        Texture2D tex = new Texture2D(texture.width, texture.height);
        tex = texture;
        byte[] bArray = tex.EncodeToPNG();
        string code = Convert.ToBase64String(bArray);
        return code;
    }

    public static Texture2D StringToTexture2D(string code)
    {
        byte[] bArray = Convert.FromBase64String(code);
        return ByteArrayToTexture2D(bArray);
    }

    static Texture2D ByteArrayToTexture2D(byte[] theArray)
    {
        Texture2D tex = new Texture2D(800, 600, TextureFormat.RGBA32, false);
        tex.LoadImage(theArray);
        tex.Apply();
        return tex;
    }

    public static Texture2D StringToTexture2D(string code, Vector2 textureSize)
    {
        byte[] bArray = Convert.FromBase64String(code);
        return ByteArrayToTexture2D(bArray, textureSize);
    }

    static Texture2D ByteArrayToTexture2D(byte[] theArray, Vector2 size)
    {
        Texture2D tex = new Texture2D((int)size.x, (int)size.y, TextureFormat.RGBA32, false);
        tex.LoadImage(theArray);
        tex.Apply();
        return tex;
    }

    public static string[] LongStringToShortStrings(string longString, int shortStringLength)
    {
        int parts = longString.Length / shortStringLength;
        int rem = 0;
        if (longString.Length % shortStringLength != 0)
        {
            rem = longString.Length % shortStringLength;
            parts++;
        }
        string[] shortStrings = new string[parts];
        if (parts > 0)
        {
            for (int i = 1; i <= parts; i++)
            {
                if (i != parts)
                {
                    shortStrings[i - 1] = longString.Substring((i - 1) * shortStringLength, shortStringLength);
                }
                else
                {
                    shortStrings[i - 1] = longString.Substring((i - 1) * shortStringLength, rem);
                }
            }
        }
        else
        {
            shortStrings[0] = longString.Substring(0, rem);
        }
        return shortStrings;
    }

    public static string ConcatenateStrings(string[] shortStrings)
    {
        int parts = shortStrings.Length;
        string realCode = "";
        for (int i = 1; i <= parts; i++)
        {
            realCode += shortStrings[i - 1];
        }
        return realCode;
    }

    public static void CopyRectTransform(RectTransform from, RectTransform to)
    {
        to.anchorMax = from.anchorMax;
        to.anchorMin = from.anchorMin;
        to.pivot = from.pivot;
        to.anchoredPosition = from.anchoredPosition;
        to.sizeDelta = from.sizeDelta;
    }

    public static Vector2 SameRatioCustomPercentage(Vector2 originalSize, float requiredSizePercentage)
    {
        Vector2 temp;
        temp = new Vector2((requiredSizePercentage / 100) * originalSize.x, (requiredSizePercentage / 100) * originalSize.y);
        return temp;
    }

    public static Vector2 SameRatioCustomParameter(Vector2 originalSize, float requiredSize, bool isProvidedWidth)
    {
        Vector2 temp = Vector2.zero;
        if (isProvidedWidth)
        {
            float percentage = requiredSize / originalSize.x * 100;
            temp = new Vector2(requiredSize, percentage / 100 * originalSize.y);
        }
        else
        {
            float percentage = requiredSize / originalSize.y * 100;
            temp = new Vector2(percentage / 100 * originalSize.x, requiredSize);
        }
        return temp;
    }

    public static IEnumerator PopupWithInfo(string info, TextMeshProUGUI infoObject, GameObject popupObject, float popupTime)
    {
        infoObject.text = info;
        popupObject.SetActive(true);
        yield return new WaitForSecondsRealtime(popupTime);
        popupObject.SetActive(false);
        
    }

    public static IEnumerator PopupWithInfo(string info, Text infoObject, GameObject popupObject, float popupTime)
    {
        infoObject.text = info;
        popupObject.SetActive(true);
        yield return new WaitForSecondsRealtime(popupTime);
        popupObject.SetActive(false);
    }

    public static IEnumerator Popup(GameObject popupObject, float popupTime)
    {
        popupObject.SetActive(true);
        yield return new WaitForSecondsRealtime(popupTime);
        popupObject.SetActive(false);
    }


}
