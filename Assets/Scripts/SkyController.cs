using UnityEngine;

public class SkyController : MonoBehaviour
{
    public Material skyMat;
    public float rotationSpeed = 5f;

    private int mSkyRotationID;
    private float mSkyRotationValue;

    private void Start()
    {
        mSkyRotationID = Shader.PropertyToID("_Rotation");
    }

    private void Update()
    {
        mSkyRotationValue += rotationSpeed * Time.deltaTime;
        skyMat.SetFloat(mSkyRotationID, mSkyRotationValue);
    }
}
