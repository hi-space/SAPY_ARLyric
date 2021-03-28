using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARLyricPlay : MonoBehaviour
{
    public GameObject placeObject;
    GameObject spawnObject;

    public ARSessionOrigin m_SessionOrigin;
    ARRaycastManager m_RaycastManager;
    AudioSource m_AudioSource;

    public void Start()
    {
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        PlaceObjectByTouch();
    }

    private void PlaceObjectByTouch()
    {
        Touch touch = Input.GetTouch(0);
        if (Input.touchCount > 0)
        {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (m_RaycastManager.Raycast(touch.position, hits, TrackableType.Planes))
            {
                Pose hitPose = hits[0].pose;

                if (!spawnObject)
                {
                    spawnObject = Instantiate(placeObject, hitPose.position, hitPose.rotation);
                }
                else
                {
                    spawnObject.transform.position = hitPose.position;
                    spawnObject.transform.rotation = hitPose.rotation;
                }

                m_AudioSource.Play();
            }
        }
    }
}
