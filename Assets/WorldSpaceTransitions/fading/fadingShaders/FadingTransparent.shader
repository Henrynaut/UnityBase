Shader "Fading/Surface/Transparent"
{
    
	Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		//_spread ("fadeSpan", Range(0,1)) = 1.0
		[Toggle] _inverse("inverse", Float) = 0
    }
    SubShader
    {
        Tags {"Queue" = "Transparent" "RenderType"="Clipping" }
        LOD 200
		//Cull Off
   
        CGPROGRAM
 
        #pragma surface surf Standard fullforwardshadows alpha:fade
		//make custom standard shader pass to get decent shadows
		#pragma multi_compile __ FADE_PLANE FADE_SPHERE
		#include "CGIncludes/section_clipping_CS.cginc"

        #pragma target 3.0
 
        sampler2D _MainTex;
 
        struct Input {
            float2 uv_MainTex;
			float3 worldPos;
        };
 
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
 
        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
			#if FADE_PLANE || FADE_SPHERE
			float4 fade = PLANE_FADE(IN.worldPos);
			float transp = fade.a;
			if(_inverse==1) transp = 1 - transp;
			//if(fade.a < 0) discard;
			//fade.a = clamp(fade.a, 0.0f, 1.0f);
			//fade.a *=(1-2*_inverse);
	
			o.Albedo *= fade.rgb*2;
			o.Alpha *= transp;
			#endif
        }

        ENDCG
    }
    FallBack "Standard"
}
