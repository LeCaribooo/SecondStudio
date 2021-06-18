using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public GameObject TrackerDisplay;
    public fleche_directionnelle fleche;
    public GameObject Container;
    List<GameObject> buttons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        if(fleche.objectiveManager.mode == Mode.QUEST)
        {
            fleche.gameObject.SetActive(true);
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < fleche.objectiveManager.objectives.Count; i++)
        {
            GameObject tracker = Instantiate(TrackerDisplay, Container.transform.position, Quaternion.identity);
            buttons.Add(tracker);
            tracker.GetComponent<RectTransform>().SetParent(Container.transform);
            tracker.GetComponent<TrackerDisplay>().SetUp(fleche.objectiveManager.objectives[i], i);
            tracker.GetComponent<TrackerDisplay>().fleche = fleche;
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            Destroy(buttons[i]);
        }

        buttons.Clear();
    }

    public void OnClick_Null()
    {
        fleche.objectiveManager.target = null;
    }
}
