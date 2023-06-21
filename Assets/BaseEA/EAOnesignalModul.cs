using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OneSignalSDK;

public class EAOnesignalModul : MonoBehaviour
{
        private string ONESIGNAL_ID = "f6281fc5-373a-447b-912b-818fa9b06ecd";
    // Start is called before the first frame update
    void Start()
    {
        OneSignal.Default.Initialize(ONESIGNAL_ID);
    }

}
