using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HiddenObjectsLevel : MonoBehaviour
{
    public static HiddenObjectsLevel instance;
    private GameObject doors;
    [SerializeField] ParticleSystem doorsParticles;
    [SerializeField] TextMeshProUGUI task1;
    [SerializeField] List<HiddenObjectData> hiddenObjectsList;

    private int founded = 0;
    private int objectsToFindNumber;
    private string taskText;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) Destroy(gameObject);
    }

    private void Start()
    {
        taskText = task1.text.Trim();
        objectsToFindNumber = hiddenObjectsList.Count;
        doors = GameObject.FindWithTag("Exit");
        doors.SetActive(false);
    }

    private void Update()
    {
        task1.text = taskText + string.Format(" {0}/{1}", founded, objectsToFindNumber);
        FindObjs();
        ExitAppear();
    }

    private void FindObjs()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(position, Vector3.zero);
            if (hit && hit.collider != null)
            {
                foreach (var item in hiddenObjectsList)
                {
                    if (item.name == hit.collider.gameObject.name)
                    {
                        hit.collider.gameObject.SetActive(false);
                        founded++;
                    }
                }
            }
        }
    }

    private void ExitAppear()
    {
        if (founded == objectsToFindNumber)
        {
            doors.SetActive(true);
            doorsParticles.Play();
        }
    }
}
