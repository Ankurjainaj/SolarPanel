using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean;
using Lean.Touch;


public class ObjectSelection : MonoBehaviour
{
    RaycastHit hit;

    public void deselectAll()
    {
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("solar"))
        {
            g.GetComponent<LeanTranslate>().CanTranslate = false;
            g.GetComponent<LeanRotateCustomAxis>().CanRotate = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition),out hit))
            {
                if(hit.collider.gameObject.tag=="solar")
                {
                    deselectAll();
                    hit.collider.gameObject.GetComponent<LeanTranslate>().CanTranslate = true;
                    hit.collider.gameObject.GetComponent<LeanRotateCustomAxis>().CanRotate = true;

                }
            }
        }
    }
}
