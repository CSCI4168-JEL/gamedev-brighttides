using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {
    protected static T instance;

    //Returns the instance of this singleton.
    public static T Instance {
        get {
            if (instance == null) {
                instance = (T)FindObjectOfType(typeof(T));

                if (instance == null) {
                    Debug.LogError("Active scene must contain instance of " + typeof(T) + " but none was found.");
                }
            }

            return instance;
        }
    }
}
