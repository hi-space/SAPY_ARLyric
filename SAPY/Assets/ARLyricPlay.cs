using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARLyricPlay : MonoBehaviour
{
    GameObject spawnObject;

    public ARSessionOrigin m_SessionOrigin;
    ARRaycastManager m_RaycastManager;
    AudioSource m_AudioSource;

    public GameObject[] m_plcaeObjects;
    public AudioClip[] m_AudioClips;

    Pose hitPose;

    int clickCount;
    bool m_isStart;

    public void Start()
    {
        m_isStart = false;
        clickCount = 0;

        m_SessionOrigin = GetComponent<ARSessionOrigin>();
        m_RaycastManager = GetComponent<ARRaycastManager>();
        m_AudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_isStart && !m_AudioSource.isPlaying)
        {
            if (spawnObject)
                Destroy(spawnObject);

            spawnObject = Instantiate(m_plcaeObjects[(clickCount) % m_plcaeObjects.Length], hitPose.position, hitPose.rotation);

            m_AudioSource.loop = false;
            m_AudioSource.clip = m_AudioClips[(clickCount) % m_AudioClips.Length];
            m_AudioSource.Play();

            clickCount++;
        }
        
        PlaceObjectByTouch();
    }

    private void PlaceObjectByTouch()
    {
        Touch touch = Input.GetTouch(0);
        
        if (Input.touchCount > 0)
        {
            m_isStart = true;

            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (m_RaycastManager.Raycast(touch.position, hits, TrackableType.PlaneEstimated))
            {
                hitPose = hits[0].pose;

                spawnObject.transform.position = hitPose.position;
                spawnObject.transform.rotation = hitPose.rotation;
            }
        }
    }
}
