
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Text (UGUI) �����չ��
/// ����
/// ʵ�����ֹ���ʡ�Ժ���ʾ
/// </summary>
public static class TextExtension
{
    /// <summary>
    /// ��������ʡ�Ժ���ʾ
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
            updatedText += "��";
        }
        textComponent.text = updatedText;
    }

    /// <summary>
    /// ����Text���������ʾ���ı�
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
    /// ����Text�����δ��ʾ���ı�
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
    /// ����ָ�����ȵ��ı���ʾ�ַ���
    /// </summary>
    /// <param name="textComponent"></param>
    /// <param name="value"></param>
    /// <param name="characterVisibleCount">ָ������</param>
    public static void SetTextWithEllipsis(this Text textComponent, string value, int characterVisibleCount)
    {
        var updatedText = value;
        if (value.Length > characterVisibleCount)
        {
            updatedText = value.Substring(0, characterVisibleCount - 1);
            updatedText += "��";
        }
        textComponent.text = updatedText;
    }
}