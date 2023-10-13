using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class CardView: MonoBehaviour
{
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _number;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _feature;
    [SerializeField] private AudioSource _audioSource;

    public AudioSource AudioSource => _audioSource;

    public void Init(CardSO cardSO)
    {
        _icon.sprite = cardSO.Icon;
        _number.text = cardSO.Number.ToString();
        _name.text = cardSO.Name;
        _feature.text = cardSO.Feature;
        _audioSource.clip = cardSO.AwakeSound;
    }

    [ContextMenu("DefineAllComponents")]
    private void DefineAllComponents()
    {
        DefineAudioSource();
    }

    [ContextMenu("DefineAudioSource")]
    private void DefineAudioSource()
    {
        AutomaticFillComponents.DefineComponent(this, ref _audioSource, ComponentLocationTypes.InThis);
    }
}
