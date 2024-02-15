using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using AYellowpaper.SerializedCollections;

public class BodyText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI content;
    [SerializedDictionary("BodyTextType", "Description")]
    public SerializedDictionary<BodyTextType, BodyTextConfig> BodyTextConfigaration;


    public void SetText(BodyTextType bodyTextType, string inputContent = null)
    {
        if(inputContent != null)
        {
            content.text = inputContent;
        }
        else
        {
            content.text = BodyTextConfigaration[bodyTextType].content;
        }
        content.fontSize = BodyTextConfigaration[bodyTextType].sizeText;
        content.color = BodyTextConfigaration[bodyTextType].color;
    }

}
[System.Serializable]
public struct BodyTextConfig
{
    public string content;
    public float sizeText;
    public Color color;
}

public enum BodyTextType
{
    Requirement_Title,
    Requirement_Content,
}
