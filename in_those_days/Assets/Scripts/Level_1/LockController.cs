using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LockController : MonoBehaviour
{
    public int KeysNeeded;
    public int CurrentKeys;
    public UnityEvent Unlocked;

    /// <summary>
    /// Increments lock progress
    /// </summary>
    public void Unlock()
    {
        CurrentKeys++;
        if(CurrentKeys >= KeysNeeded)
        {
            Unlocked.Invoke();
            Destroy(gameObject);
        }
    }
    public void NextLevel()
    {
        SceneManager.LoadScene("cutscene2");
    }
}
