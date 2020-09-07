//Script to store the transition gradient textures and set a selected one as "_TransitionGradient" global shader variable at play time.
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WorldSpaceTransitions
{
    public class GradientSelector : MonoBehaviour
    {

        public List<Texture2D> gradientTextures;
        public GameObject gradientTextureTemplate;
       

        void Start()
        {
            if (gradientTextures.Count == 0) gameObject.SetActive(false);
            setupGradientTextures();
        }


        void setupGradientTextures()
        {
            if (FadingTransition.instance.transitionGradient != null)
            {
                gradientTextures.Insert(0, FadingTransition.instance.transitionGradient);
            }

            for (int i = 0; i < gradientTextures.Count; i++)
            {
                GameObject newItem = Instantiate(gradientTextureTemplate);
                newItem.transform.SetParent(gradientTextureTemplate.transform.parent);
                newItem.transform.SetSiblingIndex(i+1);
                newItem.SetActive(true);
                newItem.GetComponentInChildren<RawImage>().texture = gradientTextures[i];
                Toggle t = newItem.GetComponent<Toggle>();
                Texture2D tex = gradientTextures[i];
                t.onValueChanged.AddListener(delegate
                {
                    Shader.SetGlobalTexture("_TransitionGradient", tex);
                    //Debug.Log(tex.name);
                });
                t.isOn = (i == 0);

            }
            Shader.SetGlobalTexture("_TransitionGradient", gradientTextures[0]);
        }
    }
}
