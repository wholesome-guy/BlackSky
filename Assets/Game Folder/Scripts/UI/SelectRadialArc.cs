using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class SelectRadialArc : MonoBehaviour
{
    [SerializeField] private Transform _arcTransform;
    [SerializeField] private Image _centreRing;
    [SerializeField] private Color _colour;
    [SerializeField] private TextMeshProUGUI _centreText;
    [SerializeField] private string _text;

    private Sequence _scaleSequence;
    [Header("Tween values")]
    [SerializeField] private float _expandedScale = 1.5f;
    [SerializeField] private float _normalScale = 1.0f;
    [SerializeField] private float _scaleDuration = 0.2f;
    [SerializeField] private Ease _tweenEase;


    public void OnPointerDown()
    {
        ScaleArc();
    }

    protected abstract void OnClick();

    private void ScaleArc()
    {
        _centreRing.color = _colour;
        _centreText.color = _colour;    
        _centreText.text = _text;   

        _scaleSequence = DOTween.Sequence();
        _scaleSequence.Append(_arcTransform.DOScale(_expandedScale, _scaleDuration).SetEase(_tweenEase));
        _scaleSequence.Append(_arcTransform.DOScale(_normalScale, _scaleDuration).SetEase(_tweenEase));
        _scaleSequence.OnComplete(() =>
        {
            _centreRing.color = Color.white;
            ChangeButton.ChangeButtonStateSetter.Invoke(false);
            _centreText.text = ""; 
            OnClick();

        });


    }
}
