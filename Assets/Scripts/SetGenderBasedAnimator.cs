using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGenderBasedAnimator : MonoBehaviour
{
    [SerializeField] private Animator m_MaleAnimator, m_femaleAnimator;
    [SerializeField] private CameraScrollController CSCInstance;
    [SerializeField] private GameObject maleParent;
    [SerializeField] private GameObject femaleParent;

    private void Start()
    {
        if (GameDataManager.Instance != null)
        {
            Debug.Log("isMale: " + GameDataManager.Instance.sceneSequenceData.isMale); // Check the gender flag

            if (GameDataManager.Instance.sceneSequenceData.isMale)
            {
                maleParent.gameObject.SetActive(true);
                m_MaleAnimator.gameObject.SetActive(true);
                
                Debug.Log("Male Animator Active: " + m_MaleAnimator.gameObject.activeSelf); // Debug for male animator*/

                femaleParent.gameObject.SetActive(false);
                m_femaleAnimator.gameObject.SetActive(false);
                
                Debug.Log("Female Animator Active: " + m_femaleAnimator.gameObject.activeSelf); // Debug for female animator*/

                CSCInstance.animator = m_MaleAnimator;
            }
            else
            {
                maleParent.gameObject.SetActive(false);
                m_MaleAnimator.gameObject.SetActive(false);
            
                Debug.Log("Male Animator Active: " + m_MaleAnimator.gameObject.activeSelf); // Debug for male animator*/

                femaleParent.gameObject.SetActive(true);
                m_femaleAnimator.gameObject.SetActive(true);
                
                Debug.Log("Female Animator Active: " + m_femaleAnimator.gameObject.activeSelf); // Debug for female animator*/

                CSCInstance.animator = m_femaleAnimator;
            }
        }
    }
}
