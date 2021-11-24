using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class GlowManager : MonoBehaviour
{
    public Material glowMat;
    public List<Transform> glowObjects;
    public List<Transform> originalGlowObjects;
    [Tooltip("Fetched in Awake")]
    [HideInInspector]
    public BloomControl BloomController;
    public bool canGlow = false;
    public bool isButton = false;
    public bool glowOnEnable = false;
    private void OnEnable()
    {
        if (glowOnEnable)
        {
            MouseExit();//To start glow
        }
    }
    public void SetCanGlow(bool _value)
    {
        canGlow = _value;
    }
    private void Awake()
    {
        BloomController = FindObjectOfType<BloomControl>();
    }
    private void Start()
    {
        originalGlowObjects = new List<Transform>(glowObjects);
        StartCoroutine(_WaitForGraphicsCheck());
        StartCoroutine(_WaitForGlowCheck());

    }
    private IEnumerator _WaitForGlowCheck()
    {
        yield return new WaitUntil(() => canGlow == true);
        foreach (Transform item in glowObjects)
        {
            if (item.gameObject.activeSelf)
            {
                item.GetComponent<AlternateGlow>().InitializeGlow();
            }
        }
    }
    private IEnumerator _WaitForGraphicsCheck()
    {
        yield return new WaitUntil(() => GameManager.Instance.graphicsCheck == true);
        if (isButton)
        {
            glowMat = GameManager.Instance.buttonGlowMat;
            foreach (Transform item in glowObjects)
            {
                item.GetComponent<Image>().material = glowMat;
            }
        }
        else
        {
            glowMat = GameManager.Instance.imgGlowMat;
        }

    }

    public void RemoveGlowPoint(Transform _obj)
    {
        _obj.GetComponent<Image>().material = null;
        glowObjects.Remove(_obj);
    }
    public void RemoveAboveCheckpoint()
    {
        glowObjects[0].GetComponent<Image>().raycastTarget=false;
        glowObjects[0].GetComponent<Image>().material = null;
        glowObjects.RemoveAt(0);
    }
    public void ResetCheckpoints()
    {
        glowObjects.Clear();
        glowObjects.AddRange(originalGlowObjects);
        foreach (Transform item in glowObjects)
        {
            item.GetComponent<Image>().raycastTarget = true;
        }
        MouseExit();//To Restart Glow

    }
    public void MouseEnter(Transform _obj)
    {
       // Debug.Log("Enter");
        if (!canGlow)
            return;
        BloomController.OnHoverEnter();
        _obj.GetComponent<Image>().material = glowMat;

        foreach (Transform item in glowObjects)
        {
            if (item != null)
            {
                if (!item.name.Equals(_obj.name))
                {
                    item.GetComponent<Image>().material = null;
                }
            }
            
        }
    }

    public void MouseExitBall(Transform _obj)
    {
        if (_obj.GetComponent<Image>().raycastTarget)
            return;
        BloomController.OnHoverExit();
        _obj.GetComponent<Image>().material = null;
        glowObjects.Remove(_obj);
    }
    public void MouseExit()
    {
        //Debug.Log("Exit"+glowMat.ToString());
        if (!canGlow)
            return;
       
        BloomController.OnHoverExit();

        foreach (Transform item in glowObjects)
        {
            if(item !=null)
                item.GetComponent<Image>().material = glowMat;
        }
    }


}
