using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    protected static T _instance;

    //Returns the instance of this singleton.
    public static T Instance {
        get {
            if (_instance == null) {
                _instance = (T)FindObjectOfType(typeof(T));

                if (_instance == null) {
                    Debug.LogError("Active scene must contain instance of " + typeof(T) + " but none was found.");
                }
            }

            return _instance;
        }
    }
}
