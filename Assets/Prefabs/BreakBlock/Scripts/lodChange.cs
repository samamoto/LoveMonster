using UnityEngine;
using System.Collections;

public class lodChange : MonoBehaviour
{
    public GameObject fragmentObjects;

    void Start()
    {
        fragmentObjects.SetActive(false);
    }

    //Playerタグがついているものと触れ合うとアクティブ化（ひび割れ）
    void OnTriggerEnter(Collider other)
    {
        if (other.tag.Contains("Player"))
        {
            fragmentObjects.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }

}
