using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    //find player's transform.position
    public Transform mPlayer;

    TPCBase mThirdPersonCamera;

    public Vector3 mPositionOffset = new Vector3(0.0f, 2.0f, -2.5f);

    public Vector3 mAngleOffset = new Vector3(0.0f, 0.0f, 0.0f);
    [Tooltip("The damping factor to smooth the changes in position and rotation of the camera.")]
    public float mDamping = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        // Set the game constant parameters to the GameConstants class.
        GameConstants.Damping = mDamping;
        GameConstants.CameraPositionOffset = mPositionOffset;
        GameConstants.CameraAngleOffset = mAngleOffset;

        mThirdPersonCamera = new TPCTrack(transform, mPlayer);

        //mThirdPersonCamera = new TPCTrack(transform, mPlayer);
        mThirdPersonCamera = new TPCFollowTrackPosition(transform, mPlayer);

        //mThirdPersonCamera = new TPCTrack(transform, mPlayer);
        //mThirdPersonCamera = new TPCFollowTrackPosition(transform, mPlayer);
        mThirdPersonCamera = new TPCFollowTrackPositionAndRotation(transform, mPlayer);
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
    public static Vector3 CameraAngleOffset { get; set; }
    public static Vector3 CameraPositionOffset { get; set; }
    public static float Damping { get; set; }

}

public abstract class TPCFollow : TPCBase
{
    public TPCFollow(Transform cameraTransform, Transform playerTransform)
    : base(cameraTransform, playerTransform)
    {
    }

    public override void Update()
    {
        // Now we calculate the camera transformed axes. 
        // We do this because our camera's rotation might have changed 
        // in the derived class Update implementations. Calculate the new 
        // forward, up and right vectors for the camera. 
        Vector3 forward = mCameraTransform.rotation * Vector3.forward;
        Vector3 right = mCameraTransform.rotation * Vector3.right;
        Vector3 up = mCameraTransform.rotation * Vector3.up;

        // We then calculate the offset in the camera's coordinate frame.  
        // For this we first calculate the targetPos 
        Vector3 targetPos = mPlayerTransform.position;

        // Add the camera offset to the target position. 
        // Note that we cannot just add the offset. 
        // You will need to take care of the direction as well. 
        Vector3 desiredPosition = targetPos + forward * GameConstants.CameraPositionOffset.z + right * GameConstants.CameraPositionOffset.x + up * GameConstants.CameraPositionOffset.y;

        // Finally, we change the position of the camera,  
        // not directly, but by applying Lerp. 
        Vector3 position = Vector3.Lerp(mCameraTransform.position,desiredPosition, Time.deltaTime * GameConstants.Damping);

        mCameraTransform.position = position;
    }
}
public class TPCFollowTrackPosition : TPCFollow
{
    public TPCFollowTrackPosition(Transform cameraTransform, Transform playerTransform)
        : base(cameraTransform, playerTransform)
    {
    }

    public override void Update()
    {
        // Create the initial rotation quaternion based on the 
        // camera angle offset.
        Quaternion initialRotation = Quaternion.Euler(GameConstants.CameraAngleOffset);

        // Now rotate the camera to the above initial rotation offset.
        // We do it using damping/Lerp
        // You can change the damping to see the effect.
        mCameraTransform.rotation = Quaternion.RotateTowards(mCameraTransform.rotation,initialRotation,Time.deltaTime * GameConstants.Damping);

        // We now call the base class Update method to take care of the
        // position tracking.
        base.Update();
    }
}
public class TPCFollowTrackPositionAndRotation : TPCFollow
{
    public TPCFollowTrackPositionAndRotation(Transform cameraTransform, Transform playerTransform)
        : base(cameraTransform, playerTransform)
    {
    }

    public override void Update()
    {
        // We apply the initial rotation to the camera.
        Quaternion initialRotation = Quaternion.Euler(GameConstants.CameraAngleOffset);

        // Allow rotation tracking of the player
        // so that our camera rotates when the Player rotates and at the same
        // time maintain the initial rotation offset.
        mCameraTransform.rotation = Quaternion.Lerp(mCameraTransform.rotation, mPlayerTransform.rotation * initialRotation,Time.deltaTime * GameConstants.Damping);

        base.Update();
    }
}

