//setting the global shader variables in inspector in editor
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace WorldSpaceTransitions
{
    [ExecuteInEditMode]
    public class FadingTransition : MonoBehaviour
    {
        public enum SurfaceType
        { plane, sphere };
        public enum FadingCentre
        { gizmo, camera };
        public SurfaceType surfaceType = SurfaceType.plane;
        public FadingCentre fadingCentre = FadingCentre.gizmo;


        [Range(0, 2f)]
        public float transitionSpread = 1f;

        [Space(10)]
        public Texture2D ScreenNoiseTexture;
        [Space(10)]
        public Texture2D NoiseAtlasTexture;
        /*
        [Space(10)]
        public Texture3D noiseTexture3D;
        //gave up because of some limitations on webgl
        */
        [Space(10)]
        public Texture2D noiseWorldspace2D;
        [Space(10)]

        public bool useDynamicTexture = true;
        public bool useWorldSpace2DTexture = false;
        public Texture2D transitionGradient;
        private Vector3 gizmoPos;
        private Quaternion gizmoRot;
        private Transform gizmo;
        public float radius = 3f;
        public float distance = 0;//this is a transition plane distance from camera when in "tied to camera" mode.
        [Range(0.025f, 2f)]
        public float noiseScaleWorld = 1;
        [Range(0.025f, 5f)]
        public float noiseScaleScreen = 5;
        private bool dontValidate = false;

        public static FadingTransition instance;

        private void Awake()
        {
            dontValidate = true;
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        void Start()
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlayingOrWillChangePlaymode) return;
#endif
            dontValidate = true;
            Shader.SetGlobalFloat("_spread", transitionSpread);
            if (ScreenNoiseTexture != null) Shader.SetGlobalTexture("_ScreenNoise", ScreenNoiseTexture);
            if (NoiseAtlasTexture != null)
            {
                Shader.SetGlobalTexture("_NoiseAtlas", NoiseAtlasTexture);
                int atlasSize = Mathf.RoundToInt(Mathf.Pow(NoiseAtlasTexture.width,1f/3));
                //Debug.Log(atlasSize.ToString());
                Shader.SetGlobalFloat("_atlasSize", atlasSize);
            }
            /*
            if (noiseTexture3D != null) Shader.SetGlobalTexture("_Noise3D", noiseTexture3D);
            //gave up because of some limitations on webgl
            */
            if (noiseWorldspace2D != null) Shader.SetGlobalTexture("_Noise2D", noiseWorldspace2D);

            gizmo = FindObjectOfType<GizmoFollow>().transform;
            gizmoPos = gizmo.position;
            gizmoRot = gizmo.rotation;

            Plane sPlane = new Plane(gizmoRot * Vector3.forward, gizmoPos);
            Ray cameraRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            if (sPlane.Raycast(cameraRay, out distance))
            {
                //Debug.DrawLine(Camera.main.transform.position, gizmo.position, Color.red, 10f);
            }

            if (!useDynamicTexture) Shader.SetGlobalTexture("_TransitionGradient", transitionGradient);
            if (useWorldSpace2DTexture)
            {
                Shader.EnableKeyword("WORLDMAP2D");
            }
            else
            {
                Shader.DisableKeyword("WORLDMAP2D");
            }

            dontValidate = false;
#if UNITY_EDITOR
            OnValidate();
#endif

        }

        void OnEnable()
        {
            if (surfaceType == SurfaceType.plane)
            {
                Shader.EnableKeyword("FADE_PLANE");
                Shader.EnableKeyword("CLIP_PLANE");
            }
            if (surfaceType == SurfaceType.sphere)
            {
                Shader.EnableKeyword("FADE_SPHERE");
                Shader.EnableKeyword("CLIP_SPHERE");
            }
        }

        void OnDisable()
        {
            Shader.DisableKeyword("FADE_PLANE");
            Shader.DisableKeyword("CLIP_PLANE");
            Shader.DisableKeyword("FADE_SPHERE");
            Shader.DisableKeyword("CLIP_SPHERE");
        }

        void OnApplicationQuit()
        {

        }

#if UNITY_EDITOR
        void OnValidate()
        {
            if (ScreenNoiseTexture != null) Shader.SetGlobalTexture("_ScreenNoise", ScreenNoiseTexture);
            if (NoiseAtlasTexture != null)
            {
                Shader.SetGlobalTexture("_NoiseAtlas", NoiseAtlasTexture);
                int atlasSize = Mathf.RoundToInt(Mathf.Pow(NoiseAtlasTexture.width, 1f/3));
                //Debug.Log(atlasSize.ToString());
                Shader.SetGlobalFloat("_atlasSize", atlasSize);
            }
            /*
            if (noiseTexture3D != null) Shader.SetGlobalTexture("_Noise3D", noiseTexture3D);
            //gave up because of some limitations on webgl
            */
            if (noiseWorldspace2D != null) Shader.SetGlobalTexture("_Noise2D", noiseWorldspace2D);
            if (useWorldSpace2DTexture)
            {
                Shader.EnableKeyword("WORLDMAP2D");
            }
            else
            {
                Shader.DisableKeyword("WORLDMAP2D");
            }



            if (dontValidate || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                return;
            }
            Shader.SetGlobalFloat("_spread", transitionSpread);

            Shader.SetGlobalTexture("_TransitionGradient", (useDynamicTexture ? TransitionGradient.texture : transitionGradient));

            if (surfaceType == SurfaceType.plane)
            {
                Shader.DisableKeyword("FADE_SPHERE");
                Shader.DisableKeyword("CLIP_SPHERE");
                Shader.EnableKeyword("FADE_PLANE");
                Shader.EnableKeyword("CLIP_PLANE");
            }
            if (surfaceType == SurfaceType.sphere)
            {
                Shader.DisableKeyword("FADE_PLANE");
                Shader.DisableKeyword("CLIP_PLANE");
                Shader.EnableKeyword("FADE_SPHERE");
                Shader.EnableKeyword("CLIP_SPHERE");
            }

            if (fadingCentre == FadingCentre.gizmo)
            {
                Shader.SetGlobalVector("_SectionPoint", gizmoPos);
                Shader.SetGlobalVector("_SectionPlane", gizmoRot * Vector3.forward);
                Shader.SetGlobalVector("_SectionPlane2", gizmoRot * Vector3.right);
            };

            if (fadingCentre == FadingCentre.camera)
            {
                Vector3 fpos = Camera.main.transform.position + Camera.main.transform.forward * distance;

                Shader.SetGlobalVector("_SectionPoint", fpos);
                Shader.SetGlobalVector("_SectionPlane", -Camera.main.transform.forward);
                Shader.SetGlobalVector("_SectionPlane2", Camera.main.transform.right);
            };
            Shader.SetGlobalFloat("_ScreenNoiseScale", noiseScaleScreen);
            Shader.SetGlobalFloat("_Noise3dScale", noiseScaleWorld);
            Shader.SetGlobalFloat("_Radius", radius);
        }
#endif

    }
}
