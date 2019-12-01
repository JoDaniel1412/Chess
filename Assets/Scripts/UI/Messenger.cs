using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class Messenger : MonoBehaviour
    {
        public float displayTime;
        
        
        // Starts a coroutine that display the message in the displayTime lapse
        public void DisplayText(string message)
        {
        ActiveChildren(true);
        GetComponentInChildren<Text>().text = message;
        StartCoroutine(DisplayTimer());
        }

        private IEnumerator DisplayTimer()
        {
            yield return new WaitForSeconds(displayTime);
            ActiveChildren(false);
        }
        
        private void ActiveChildren(bool state)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(state);
            }
        }
    }
}
