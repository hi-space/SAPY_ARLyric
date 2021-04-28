using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


public class ARLyricPlay : MonoBehaviour
{
    public GameObject[] objectPrefab;
    public AudioClip[] audioPrefab;

    ARRaycastManager raycastManager;

    List<GameObject> placedObject = new List<GameObject>();
    
    AudioSource currentAudio;

    int index;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        currentAudio = GetComponent<AudioSource>();

        foreach (GameObject obj in objectPrefab)
        {
            GameObject newObj = Instantiate(obj, Vector3.zero, Quaternion.identity);
            newObj.SetActive(false);
            placedObject.Add(newObj);
        }

        index = 0;
    }

    void Update()
    {
        if (!currentAudio.isPlaying)
        {
            index = index >= objectPrefab.Length ? 0 : index + 1;

            currentAudio.clip = audioPrefab[index];
            currentAudio.Play();
        }

        if (Input.touchCount > 0)
        {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            if (raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.All))
            {
                Pose hitPose = hits[0].pose;

                foreach (GameObject obj in placedObject)
                {
                    obj.SetActive(false);
                }

                GameObject currentObject = placedObject[index];
                currentObject.transform.position = hitPose.position;
                currentObject.transform.rotation = hitPose.rotation;
                currentObject.SetActive(true);
            }            
        }
    }
}
