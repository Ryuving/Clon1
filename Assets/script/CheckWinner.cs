using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class ChekWinner : MonoBehaviour
{

    public Camera defaultCamera;
    public Camera winnnerCamera;
    public bool isWinner = false;

    public Transform target;
    public float smoothSpeed = 1.0f;

    public static ChekWinner instance;

    public Transform playerRotation;

    public void Awake()
    {
        instance = this;
    }

    void Start()
    {
        defaultCamera.enabled = true;
        winnnerCamera.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isWinner)
        {
            defaultCamera.enabled = false;
            winnnerCamera.enabled = true;
        }        
    }

    private void LateUpdate()
    {
        if (target != null && isWinner)
        {
            Vector3 desiredPosition = new Vector3(target.position.x - 3.0f, target.position.y +1.0f, target.position.z);

            Vector3 smoothedPosition = Vector3.Lerp(winnnerCamera.transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

            winnnerCamera.transform.position = smoothedPosition;

            playerRotation.LookAt(new Vector3(winnnerCamera.transform.position.x, playerRotation.position.y, playerRotation.position.z));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player") && PlayController.instance.groundedPlayer)
        {
            isWinner = true;
        }
    }
}
