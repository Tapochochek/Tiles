using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BatleUnitScript : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Button buttonActivate;
    private bool _isHolding = false;
    private float _timer = 0f;
    private float _holdTime = 0.5f;


    private void Start()
    {
        buttonActivate.onClick.AddListener(clickButton);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _isHolding = true;
        _timer = 0f;
    }

    
    public void OnPointerUp(PointerEventData eventData)
    {
        _isHolding = false;
        if (_timer < _holdTime)
        {
            
            clickButton();
        }
        else
        {
            
        }
    }
    private void Update()
    {
        if (_isHolding)
        {
            _timer += Time.deltaTime;

            if (_timer >= _holdTime)
            {
                _isHolding = false;
                
                clickDownButton();
            }
        }
    }






    public void clickDownButton()
    {
        Debug.Log("Hold");
    }
    public void clickButton()
    {
        Debug.Log("click");
    }


}
