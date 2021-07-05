# SAPY_ARLyric

## Build

- [iOS Build Guide](Build_iOS.md)
- [Android Build Guide](Build_Android.md)

## Codes

```csharp
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARLyricPlay : MonoBehaviour
{
		// 사용할 3D 오브젝트와 오디오 파일들을 툴에서 입력받기 위해
    public GameObject[] objectPrefab;
    public AudioClip[] audioPrefab;
		
		// 터치한 영역의 포인트를 얻어오기 위해
    ARRaycastManager raycastManager;

		// 현재 사용되고 있는 audio와 3d 오브젝트 정보를 저장하기 위해
    List<GameObject> placedObject = new List<GameObject>();
    AudioSource currentAudio;

		// 몇번째 오브젝트인지 체크하기 위해
    int index = 0;

		// 초기화. Update 이전에 가장 처음으로 불리는 함수
    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        currentAudio = GetComponent<AudioSource>();
				
				// 배치될 오브젝트를 실체화해서 미리 만들어놓기
        foreach (GameObject obj in objectPrefab)
        {
            GameObject newObj = Instantiate(obj);
            newObj.SetActive(false);
            placedObject.Add(newObj);
        }
    }

		// 매 프레임마다 불리는 함수
    void Update()
    {
				// 음악 재생이 끝나면
        if (!currentAudio.isPlaying)
        {
						// 다음 오브젝트/음악 선택하기 위해
            index = index >= objectPrefab.Length ? 0 : index + 1;

						// 음악 선택 후 재생
            currentAudio.clip = audioPrefab[index];
            currentAudio.Play();
        }

				// 배치할 오브젝트 선택
        GameObject currentObject = placedObject[index];

				// 노래와 페어되는 3D 오브젝트만 보이게 하기 위해서
        foreach (GameObject obj in placedObject)
        {
            obj.SetActive(false);
        }
        currentObject.SetActive(true);

				// 터치 인식
        if (Input.touchCount > 0)
        {
						// 터치한 좌표
						Vector2 touchPosition = Input.GetTouch(0).position;

						// Ray에 충돌된 객체들을 저장하기 위해
            List<ARRaycastHit> hits = new List<ARRaycastHit>();

						// 터치한 좌표 방향으로 Ray를 쏘기
            if (raycastManager.Raycast(touchPosition, hits, TrackableType.All))
            {
								// Ray에 충돌된 첫번째 객체 좌표
                Pose hitPose = hits[0].pose;

								// 오브젝트 위치, 방향
                currentObject.transform.position = hitPose.position;
                currentObject.transform.rotation = hitPose.rotation * Quaternion.Euler(90, 0, 0);
            }            
        }
    }
}
```