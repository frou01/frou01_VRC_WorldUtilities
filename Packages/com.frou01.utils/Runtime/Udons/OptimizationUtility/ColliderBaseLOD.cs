
using frou01.util;
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class ColliderBaseLOD : UdonSharpBehaviour
{
    [SerializeField] public GameObject[] NearObjects;
    [SerializeField] public GameObject[] DistObjects;
    void Start()
    {
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerChaser>() != null)
        {
            foreach (GameObject go in NearObjects)
            {
                if(go != null) go.SetActive(true);
            }
            foreach (GameObject go in DistObjects)
            {
                if (go != null) go.SetActive(false);
            }
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerChaser>() != null)
        {
            foreach (GameObject go in NearObjects)
            {
                if (go != null) go.SetActive(false);
            }
            foreach (GameObject go in DistObjects)
            {
                if (go != null) go.SetActive(true);
            }
        }
    }
}
