using UnityEngine;

public class VampirismRadiusView : MonoBehaviour
{
    [SerializeField] private GameObject _radiusVisual;
    
    public void Show()
    {
        _radiusVisual.SetActive(true);
    }

    public void Hide()
    {
        _radiusVisual.SetActive(false);
    }

    private void Start()
    {
        Hide();
    }
}
