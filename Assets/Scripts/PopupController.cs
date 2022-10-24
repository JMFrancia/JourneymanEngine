using UnityEngine;
using UnityEngine.UI;

public class PopupController : MonoBehaviour
{
    [SerializeField] Text _titleText;
    [SerializeField] Text _descriptionText;
    [SerializeField] Image _image;

    CanvasGroup ParentGroup {
        get {
            if (_parentGroup == null) {
                _parentGroup = GetComponentInParent<CanvasGroup>();
            }
            return _parentGroup;
        }
    }

    CanvasGroup _parentGroup;

    public void Show(PopupConfig config)
    {
        SetConfig(config);
        SetActive(true);
    }

    public void Show(POIDataSO config) {
        PopupConfig newConfig = new PopupConfig(config.Name, config.Description, config.Image);
        Show(newConfig);
    }

    public void Hide()
    {
        SetActive(false);
    }

    void SetConfig(PopupConfig config) {
        _titleText.text = config.TitleText;
        _descriptionText.text = config.DescriptionText;
        _image.sprite = config.Image;
    }

    void SetActive(bool setting) {
        ParentGroup.alpha = setting ? 1 : 0;
        ParentGroup.blocksRaycasts = setting;
        ParentGroup.interactable = setting;
    }
}

public struct PopupConfig {
    public string TitleText;
    public string DescriptionText;
    public Sprite Image;

    public PopupConfig(string title, string description, Sprite image) {
        TitleText = title;
        DescriptionText = description;
        Image = image;
    }
}