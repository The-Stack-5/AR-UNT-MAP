using UnityEngine;

// Requests runtime GPS permissions on Android
public class RuntimePermissionHelper : MonoBehaviour
{
    void Start()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR
        if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.FineLocation))
        {
            UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.FineLocation);
        }
        #endif
    }
}
