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
        var a = (zeroObject.transform.localPosition.x - this.transform.localPosition.x);
        var b = (zeroObject.transform.localPosition.y - this.transform.localPosition.y);

        coordinate.x = Mathf.RoundToInt(zeroObject.transform.localPosition.x - this.transform.localPosition.x) / tileWidth;
        coordinate.y = Mathf.RoundToInt(zeroObject.transform.localPosition.y - this.transform.localPosition.y) / tileHeight;
    }

    public void SetXY(GameObject zeroObject, out int x, out int y)
    {
        var a = (zeroObject.transform.localPosition.x - this.transform.localPosition.x);
        var b = (zeroObject.transform.localPosition.y - this.transform.localPosition.y);

        x = Mathf.RoundToInt(zeroObject.transform.localPosition.x - this.transform.localPosition.x) / tileWidth;
        y = Mathf.RoundToInt(zeroObject.transform.localPosition.y - this.transform.localPosition.y) / tileHeight;
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
