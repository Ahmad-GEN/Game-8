using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AlternateGlow : MonoBehaviour
{
    Canvas parentCanvasObj;
    Canvas childCanvasObj;
    PotaTween tweenObj;
    Image imgObj;
    public int parentSortOrder;
    public int childSortOrder;
    public Vector3 scaleFrom;
    public Vector3 scaleTo;
    public AnimationCurve curve;
    public Color imgColor;
    public GameObject alternateGlowObject;
    private RectTransform alternateObjectsRectTransform;
    public bool glowAtStart = false;
    private void Start()
    {
        if (glowAtStart)
        {
            StartCoroutine(_InitializeAlternateGlow());
        }

    }

    public void InitializeGlow()
    {
        StartCoroutine(_InitializeAlternateGlow());
        
    }

    private IEnumerator _InitializeAlternateGlow()
    {
         
        yield return new WaitUntil(()=>GameManager.Instance.graphicsCheck==true);
        if (gameObject.transform.childCount > 1)
        {
            yield break;
        }
        
        if (gameObject.GetComponent<GraphicRaycaster>())
        {
            gameObject.GetComponent<GraphicRaycaster>().ignoreReversedGraphics = false;
        }
        else
        {
            gameObject.AddComponent<GraphicRaycaster>().ignoreReversedGraphics=false;
        }
        if (GameManager.Instance.lowGraphics)
        {

            GetComponent<Image>().material = null;
            alternateGlowObject = Instantiate(alternateGlowObject, transform);
            if (parentCanvasObj = gameObject.GetComponent<Canvas>())
            { }
            else
            {
                parentCanvasObj = gameObject.AddComponent<Canvas>();
            }
            parentCanvasObj.overrideSorting = true;
            parentCanvasObj.sortingOrder = parentSortOrder;

            tweenObj = alternateGlowObject.GetComponent<PotaTween>();
            imgObj = alternateGlowObject.GetComponent<Image>();
            childCanvasObj = alternateGlowObject.GetComponent<Canvas>();
            alternateObjectsRectTransform = alternateGlowObject.GetComponent<RectTransform>();

            alternateObjectsRectTransform.anchorMin = new Vector2(0f, 0f);
            alternateObjectsRectTransform.anchorMax = new Vector2(1f, 1f);
            alternateObjectsRectTransform.pivot = new Vector2(0.5f, 0.5f);


            imgObj.sprite = transform.GetComponent<Image>().sprite;
            imgObj.color = imgColor;
            childCanvasObj.overrideSorting = true;
            childCanvasObj.sortingOrder = childSortOrder;

            Debug.Log(tweenObj);
            tweenObj.PlayOnEnable = true;
            tweenObj.PlayOnStart= true;
            tweenObj.Loop = LoopType.PingPong;
            tweenObj.SetCurve(curve);
            tweenObj.SetScale(scaleFrom, scaleTo);
            
        }
    }


}
