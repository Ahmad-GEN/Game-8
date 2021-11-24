using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeControlsHandler : MonoBehaviour
{

    public static FreezeControlsHandler Instance;
    public GameObject blocker;
    public bool isControllsFreezed;
    // Use this for initialization

    List<int> dontUnfreezeList; // Controlls will remain freezed for these levels (CurrentLevelIndex)
    void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        FreezeControlls();
        dontUnfreezeList = new List<int>();
        AddToList(3, 4, 6, 8, 14, 16, 18);

        // Freeze controlls for first 6 seconds when game is started (waiting for sound to get downloaded)
        // FreezeControlls();
        // StartCoroutine(UnFreezeControllsE(gameStartingFreezeLength));
    }

    public void FreezeControlls()
    {
        if(GameManager.Instance.Accessibilty)
        {
            AccessibilityManager.instance.block.SetActive(true);
        }
        print("FreezeControlsHandler->FreezeControlls()");
        isControllsFreezed = true;
    }

    public void UnFreezeControlls()
    {
        if(GameManager.Instance.Accessibilty)
        {
            AccessibilityManager.instance.block.SetActive(false);
        }
        print("FreezeControlsHandler->UnFreezeControlls()");
        isControllsFreezed = false;
    }

    IEnumerator UnFreezeControllsE(float delay)
    {
        print("FreezeControlsHandler->UnFreezeControllsE()");
        yield return new WaitForSeconds(delay);
        isControllsFreezed = false;
    }

    public void FrezeControlsFor(float time)
    {
			if(GameManager.Instance.Accessibilty)
				StartCoroutine(FrezeControlsForE(time));
    }

    IEnumerator FrezeControlsForE(float time)
    {
        FreezeControlls();
        yield return new WaitForSeconds(time);
        UnFreezeControlls();
    }

    void AddToList(params int[] list)
    {
        dontUnfreezeList.AddRange(list);
    }
}
