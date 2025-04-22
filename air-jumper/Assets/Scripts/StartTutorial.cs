using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialImage;
    private bool isTutorialActive = false;

    private Dictionary<CollectibleType, bool> hasTutorialShown = new Dictionary<CollectibleType, bool>();
    public string currentLevel = "";

    void OnEnable()
    {
        if (CollectibleSpawner.Instance != null)
        {
            CollectibleSpawner.Instance.OnCollectibleSpawned += OnCollectbleSpawned;
            CollectibleSpawner.Instance.OnCollectibleCollected += OnCollectibeCollected;
        }
    }

    void OnDestroy()
    {
        if (CollectibleSpawner.Instance != null)
        {
            CollectibleSpawner.Instance.OnCollectibleSpawned -= OnCollectbleSpawned;
            CollectibleSpawner.Instance.OnCollectibleCollected -= OnCollectibeCollected;
        }
    }

    void OnCollectibeCollected(CollectibleType collectibleType, Vector3 pos)
    {
        if (currentLevel == "Tutorial1")
        {
            if (collectibleType == CollectibleType.s_PlatformCollectible)
            {
                if (!hasTutorialShown.ContainsKey(collectibleType))
                {
                    hasTutorialShown[collectibleType] = true;
                    GameObject textObject = new GameObject("TextMeshPro");

                    TextMeshPro tmp = textObject.AddComponent<TextMeshPro>();

                    tmp.text = "Press \"W\" to jump, then press SPACE in the air to create a platform";
                    tmp.fontSize = 6;
                    tmp.color = Color.white;
                    tmp.alignment = TextAlignmentOptions.Center;

                    tmp.transform.position = pos + new Vector3(0, 2, 0);
                }
            }
        }

        if (currentLevel == "Tutorial2")
        {
            if (collectibleType == CollectibleType.b_BlockCollectible)
            {
                GameObject tutorial = GameObject.Find("T_Tutorial2");

                //Debug.Log(tutorial);
                TextMeshPro tmp = tutorial.GetComponent<TextMeshPro>();
                if (tmp != null)
                {
                    tmp.text = "Drag from the buttom dock panel to create a Double Jump Platform ¡ý";
                }
            }
        }
    }

    void OnCollectbleSpawned(CollectibleType collectibleType, Vector3 pos)
    {
        if (currentLevel == "Tutorial2")
        {
            GameObject tutorial = GameObject.Find("Hint");
            if (tutorial != null)
            {
                Destroy(tutorial);
            }

            if (collectibleType == CollectibleType.b_BlockCollectible)
            {
                if (!hasTutorialShown.ContainsKey(collectibleType))
                {
                    hasTutorialShown[collectibleType] = true;

                    GameObject textObject = new GameObject("T_Tutorial2");

                    TextMeshPro tmp = textObject.AddComponent<TextMeshPro>();
                    tmp.text = "Collect the collectible!";
                    tmp.fontSize = 6;
                    tmp.color = Color.white;
                    tmp.alignment = TextAlignmentOptions.Center;

                    tmp.transform.position = pos + new Vector3(0, 2.5f, 0);
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
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
