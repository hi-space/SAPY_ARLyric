using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARLyricPlay : MonoBehaviour
{
    public GameObject[] m_PlaceObjects;
    public AudioClip[] m_AudioClips;

    ARSessionOrigin m_SessionOrigin;
    ARRaycastManager m_RaycastManager;
    AudioSource m_AudioSource;
    GameObject m_SpawnObject;

    Pose hitPose;
    int m_ClickCount;
    
    void Start()
    {
        m_ClickCount = 0;

        m_SessionOrigin = GetComponent<ARSessionOrigin>();
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.touchCount > 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (m_SpawnObject)
            {
                Destroy(m_SpawnObject);
            }

            m_SpawnObject = Instantiate(m_PlaceObjects[(m_ClickCount) % m_PlaceObjects.Length], hitPose.position, hitPose.rotation);

            m_AudioSource.loop = true;
            m_AudioSource.clip = m_AudioClips[(m_ClickCount) % m_AudioClips.Length];
            m_AudioSource.Play();

            m_ClickCount++;
        }

        else if (Input.touchCount > 0)
        {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (m_RaycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.All))
            {
                hitPose = hits[0].pose;

                if (m_SpawnObject)
                {
                    m_SpawnObject.transform.position = hitPose.position;
                    m_SpawnObject.transform.rotation = hitPose.rotation * Quaternion.Euler(new Vector3(90, 0, 0));
                }
            }
        }
    }
}
