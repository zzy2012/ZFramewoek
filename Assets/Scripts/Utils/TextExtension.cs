
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Text (UGUI) 组件扩展类
/// 功能
/// 实现文字过长省略号显示
/// </summary>
public static class TextExtension
{
    /// <summary>
    /// 超出部分省略号显示
    /// </summary>
    /// <param name="textComponent"></param>
    /// <param name="value"></param>
    public static void SetTextWithEllipsis(this Text textComponent, string value)
    {
        var generator = new TextGenerator();
        var rectTransform = textComponent.GetComponent<RectTransform>();
        var settings = textComponent.GetGenerationSettings(rectTransform.rect.size);
        generator.Populate(value, settings);

        var characterCountVisible = generator.characterCountVisible;
        var updatedText = value;
        if (value.Length > characterCountVisible)
        {
            updatedText = value.Substring(0, characterCountVisible - 1);
            updatedText += "…";
        }
        textComponent.text = updatedText;
    }

    /// <summary>
    /// 返回Text中组件中显示的文本
    /// </summary>
    /// <param name="textComponent"></param>
    /// <param name="value"></param>
    public static string GetShowText(this Text textComponent, string value)
    {
        var generator = new TextGenerator();
        var rectTransform = textComponent.GetComponent<RectTransform>();
        var settings = textComponent.GetGenerationSettings(rectTransform.rect.size);
        generator.Populate(value, settings);
        var characterCountVisible = generator.characterCountVisible;
        return value.Length <= characterCountVisible ? value : value.Substring(0, characterCountVisible + 1);
    }


    /// <summary>
    /// 返回Text组件中未显示的文本
    /// </summary>
    /// <param name="textComponent"></param>
    /// <param name="value"></param>
    public static string GetNoShowText(this Text textComponent, string value)
    {
        var generator = new TextGenerator();
        var rectTransform = textComponent.GetComponent<RectTransform>();
        var settings = textComponent.GetGenerationSettings(rectTransform.rect.size);
        generator.Populate(value, settings);

        var characterCountVisible = generator.characterCountVisible;
        var updatedText = value;
        if (value.Length > characterCountVisible)
        {
            updatedText = value.Substring(0, characterCountVisible+1);
        }
        textComponent.text = updatedText;
        return value.Substring(updatedText.Length);
    }

    /// <summary>
    /// 超过指定长度的文本显示字符串
    /// </summary>
    /// <param name="textComponent"></param>
    /// <param name="value"></param>
    /// <param name="characterVisibleCount">指定长度</param>
    public static void SetTextWithEllipsis(this Text textComponent, string value, int characterVisibleCount)
    {
        var updatedText = value;
        if (value.Length > characterVisibleCount)
        {
            updatedText = value.Substring(0, characterVisibleCount - 1);
            updatedText += "…";
        }
        textComponent.text = updatedText;
    }
}