using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragInAccessibility : MonoBehaviour
{
    [HideInInspector]
    public Vector3 ObjectCentre; // gameobject centre which is to be dragged
    [HideInInspector]
    public Vector3 TouchPosition; //click or touch transform position
    [HideInInspector]
    public Vector3 Offset; // vector position between touchpoint/mouse click with object centre
    [HideInInspector]
    public Vector3 NewObjectCentre; //new centre of the object during dragging
    public GameObject bottomLeftLimit, topRightLimit;
    Vector3 OldPosition;
    public float moveSpeed = 1f;
    bool flag = false;
    float Width;
    float Height;
    public float WidthPer = 8f;
    public float HeightPer = 8f;
    public static DragInAccessibility ins;
    // Start is called before the first frame update
    void Start()
    {
        ins = this;
        if (Screen.width == 1280 && Screen.height == 720)
        {
            Width = (Screen.width * 6.5f) / 1000;
            Height = (Screen.height * 6.5f) / 1000;
        }
        else if (Screen.width == 850 && Screen.height == 450)
        {
            Width = (Screen.width * 9.6f) / 1000;
            Height = (Screen.height * 9.6f) / 1000;

        }
        else
        {
            Width = (Screen.width * WidthPer) / 1000;
            Height = (Screen.height * HeightPer) / 1000;
        }
    }
    void OnEnable()
    {
        flag = true;
    }


    void Update()
    {
        if (flag)
        {
            if (Input.GetMouseButton(0) && GameManager.Instance.Accessibilty)
            {
                float distance_to_screen = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
                Vector3 pos_move = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance_to_screen));
                transform.position = new Vector3(pos_move.x, pos_move.y, pos_move.z);

                TouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                NewObjectCentre = TouchPosition - Offset;

                if (bottomLeftLimit && topRightLimit)
                {
                    NewObjectCentre.x = Mathf.Clamp(NewObjectCentre.x, bottomLeftLimit.transform.position.x, topRightLimit.transform.position.x);
                    NewObjectCentre.y = Mathf.Clamp(NewObjectCentre.y, bottomLeftLimit.transform.position.y, topRightLimit.transform.position.y);
                }
                else
                {
                    NewObjectCentre.x = Mathf.Clamp(NewObjectCentre.x, -Width, Width);
                    NewObjectCentre.y = Mathf.Clamp(NewObjectCentre.y, -Height, Height);
                }

                transform.position = new Vector3(NewObjectCentre.x, NewObjectCentre.y, pos_move.z);
            }
        }
    }
}
