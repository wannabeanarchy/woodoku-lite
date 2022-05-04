using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [SerializeField] private int tileWidth = 80;
    [SerializeField] private int tileHeight = 80;
    [SerializeField] private float fadeTime = 0.5f; 

    [HideInInspector] public Coordinate coordinate;
    [HideInInspector] public bool isActive { get; set; }

    void Start()
    {
        StartCoroutine(FadeIn());
        isActive = true;
    }


    public void SetXY(GameObject zeroObject)
    { 
        Vector3 _zeroObjectPosition = zeroObject.transform.localPosition;
        Vector3 _targetObjectPosition = this.transform.localPosition;

        float _x = (_zeroObjectPosition.x - _targetObjectPosition.x) / tileWidth;
        float _y = (_zeroObjectPosition.y - _targetObjectPosition.y) / tileHeight;
        
        coordinate.x = (int)Math.Round(_x, 0);
        coordinate.y = (int)Math.Round(_y, 0); 
    }

    public void SetXY(GameObject zeroObject, out int x, out int y)
    {
        Vector3 _zeroObjectPosition = zeroObject.transform.localPosition;
        Vector3 _targetObjectPosition = this.transform.localPosition;

        x = (int)Math.Round(((_zeroObjectPosition.x - _targetObjectPosition.x) / tileWidth), 0);
        y = (int)Math.Round(((_zeroObjectPosition.y - _targetObjectPosition.y) / tileHeight), 0);
    } 

    public void NeedDestroy()
    {
        isActive = false;
        StartCoroutine(FadeOut(true)); 
    }

    private IEnumerator FadeIn()
    {
        float _counter = 0;
        Image _image = GetComponent<Image>();
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0);

        while (_counter < fadeTime)
        {
            _counter += Time.deltaTime;

            float _alpha = Mathf.Lerp(0, 1, _counter / fadeTime);
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _alpha);
            yield return null;
        }
    }

    private IEnumerator FadeOut(bool needDestroy)
    { 
        float _counter = 0;
        Image _image = GetComponent<Image>();

        while (_counter < fadeTime)
        {
            _counter += Time.deltaTime;  
            float _alpha = Mathf.Lerp(1, 0, _counter / fadeTime); 
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, _alpha); 
            yield return null;
        }

        if (needDestroy)
            Destroy(gameObject);
    }
}
