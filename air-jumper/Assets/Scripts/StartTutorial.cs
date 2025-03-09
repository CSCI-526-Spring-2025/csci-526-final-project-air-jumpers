using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialImage;
    private bool isTutorialActive = false;

    private Dictionary<CollectibleType, bool> hasTutorialShown = new Dictionary<CollectibleType, bool>();
    public GameObject currentLevel;

    void OnEnable()
    {
        CollectibleSpawner.Instance.OnCollectibleSpawned += OnCollectbleSpawned;
    }

    void OnDestroy()
    {
        CollectibleSpawner.Instance.OnCollectibleSpawned -= OnCollectbleSpawned;
    }

    void OnCollectbleSpawned(CollectibleType collectibleType, Vector3 pos)
    {
        if (currentLevel.name != "Level1")
        {
            return;
        }

        if (collectibleType == CollectibleType.b_BlockCollectible)
        {
            if (!hasTutorialShown.ContainsKey(collectibleType))
            {
                hasTutorialShown[collectibleType] = true;

                GameObject textObject = new GameObject("TextMeshPro");

                TextMeshPro tmp = textObject.AddComponent<TextMeshPro>();

                tmp.text = "Drag To Create & Double Jumb On The New Platform";
                tmp.fontSize = 6;
                tmp.color = Color.white;
                tmp.alignment = TextAlignmentOptions.Center;

                tmp.transform.position = pos + new Vector3(0, 1, 0);
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            ToggleTutorial();
        }
    }

    void ToggleTutorial()
    {
        isTutorialActive = !isTutorialActive;
        tutorialImage.SetActive(isTutorialActive);

        if (isTutorialActive)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
