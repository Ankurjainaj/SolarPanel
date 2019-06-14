using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.EventSystems;
using GoogleARCore.Examples.Common;

public class DragHandler : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public GameObject prefab;
    GameObject hoverPrefab;
    DetectedPlaneGenerator detectedPlaneGenerator;
    List<TrackableHit> trackableHitList = new List<TrackableHit>();

    void Start()
    {
        hoverPrefab = Instantiate(prefab);
        AdjustPrefabAlpha();
        hoverPrefab.SetActive(false);
        detectedPlaneGenerator = FindObjectOfType<DetectedPlaneGenerator>();
    }

    void AdjustPrefabAlpha()
    {
        MeshRenderer[] meshRenderers = hoverPrefab.GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            Material mat = meshRenderers[i].material;
            meshRenderers[i].material.color = new Color(mat.renderQueue, mat.color.g, mat.color.b);
        }
    }

    void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
    {

    }
    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Frame.RaycastAll(ray.origin, ray.direction, trackableHitList, Mathf.Infinity, TrackableHitFlags.Default);
        if(trackableHitList !=null && trackableHitList.Count>0)
        {
            int terrainCollderQuadIndex = GetTerrainColliderQuadIndex(trackableHitList);
            if(terrainCollderQuadIndex!=-1)
            {
                hoverPrefab.transform.position = trackableHitList[terrainCollderQuadIndex].Pose.position;
                hoverPrefab.SetActive(true);
            }
            else
            {
                hoverPrefab.SetActive(false);
            }
        }
    }
    int GetTerrainColliderQuadIndex(List<TrackableHit> hits)
    {
        for(int i=0;i<hits.Count;i++)
        {
            if(hits[i].Trackable is DetectedPlane)
            {
                return i;
            }
        }
        return -1;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(hoverPrefab.activeSelf)
        {
            Instantiate(prefab, hoverPrefab.transform.position, Quaternion.identity);
            detectedPlaneGenerator.DisablePlanes();
        }
        hoverPrefab.SetActive(false);
    }
}
