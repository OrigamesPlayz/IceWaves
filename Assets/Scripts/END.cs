using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class END : MonoBehaviour
{
    public Animator endAnim;
    public GameObject quitButton;
    public PlayerMovement pMove;
    public PlayerCam pCam;
    public WandAttackRaycast wandR;
    public bool ended;
    void Update()
    {
        if (ended)
        {
            endAnim.SetTrigger("End");
            StartCoroutine(ENDBUTTON());
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            pMove.enabled = false;
            pCam.enabled = false;
            wandR.enabled = false;
        }
    }

    IEnumerator ENDBUTTON()
    {
        yield return new WaitForSeconds(1.5f);
        quitButton.SetActive(true);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
		#else
			Application.Quit();
		#endif
    }
}
