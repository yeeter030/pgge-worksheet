using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    //find player's transform.position
    public Transform mPlayer;

    TPCBase mThirdPersonCamera;

    public Vector3 mPositionOffset = new Vector3(0.0f, 2.0f, -2.5f);

    // Start is called before the first frame update
    void Start()
    {
        GameConstants.CameraPositionOffset = mPositionOffset;
        mThirdPersonCamera = new TPCTrack(transform, mPlayer);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        mThirdPersonCamera.Update();
    }
}
public abstract class TPCBase : MonoBehaviour
{
    protected Transform mCameraTransform;
    protected Transform mPlayerTransform;

    public Transform CameraTransform
    {
        get
        {
            return mCameraTransform;
        }
    }
    public Transform PlayerTransform
    {
        get
        {
            return mPlayerTransform;
        }
    }

    public TPCBase(Transform cameraTransform, Transform playerTransform)
    {
        mCameraTransform = cameraTransform;
        mPlayerTransform = playerTransform;

    }

    public abstract void Update();
}

public class TPCTrack : TPCBase
{
    public float playerHeight;

    public TPCTrack(Transform cameraTransform, Transform playerTransform): base(cameraTransform, playerTransform)
    {

    }

    //override the update and makes the camera look at player
    public override void Update()
    {
        Vector3 targetPos = mPlayerTransform.position;
        targetPos.y += GameConstants.CameraPositionOffset.y;
        mCameraTransform.LookAt(targetPos);
    }
}

public static class GameConstants
{
    public static Vector3 CameraPositionOffset { get; set; }
}
