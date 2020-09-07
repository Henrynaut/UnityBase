Shader "Fading/Surface/Dissolve" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		//_spread ("dissolveSpread", Range(0,1)) = 1.0 //when commented here - the global variable value is used 

		_SectionColor ("Section Color", Color) = (1,0,0,1)
		[Toggle] _inverse("inverse", Float) = 0
		[Toggle] _doubleSided ("doubleSided", Float) = 1
		[Toggle(RETRACT_BACKFACES)] _retractBackfaces("retractBackfaces", Float) = 0
		[HideInInspector][Toggle(DISSOLVE)] _dissolve("dissolveTexture", Float) = 1
	}
	SubShader {
		Tags { "RenderType"="Clipping" }
		LOD 200

		// ------------------------------------------------------------------


		Cull off
				
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard addshadow vertex:vert
		#pragma multi_compile __ FADE_PLANE FADE_SPHERE
		#pragma shader_feature RETRACT_BACKFACES
		#pragma shader_feature DISSOLVE
		#include "CGIncludes/section_clipping_CS.cginc"

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float myface : VFACE;
		};

		half _BackfaceExtrusion;

		void vert (inout appdata_full v) {
			#if RETRACT_BACKFACES
			float3 viewDir = ObjSpaceViewDir(v.vertex);
			float dotProduct = dot(v.normal, viewDir);
			if(dotProduct<0) {
				float3 worldPos = mul(unity_ObjectToWorld, float4(v.vertex.xyz, 1)).xyz;
				float3 worldNorm = UnityObjectToWorldNormal(v.normal);
				worldPos -= worldNorm * _BackfaceExtrusion;
				v.vertex.xyz = mul(unity_WorldToObject, float4(worldPos, 1)).xyz;
			}
			#endif
		}

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _SectionColor;
		fixed _doubleSided;

		void surf (Input IN, inout SurfaceOutputStandard o) {

			if(IN.myface<0&&_doubleSided==0) discard; else PLANE_CLIP(IN.worldPos);
			
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			#if (FADE_PLANE || FADE_SPHERE)&&DISSOLVE
			half4 fade = PLANE_FADE(IN.worldPos);
			o.Albedo *= fade.rgb*2;
			//o.Alpha *= fade.a;
			#endif
			
			// Metallic and smoothness come from slider variables
		if(IN.myface>0) 
		{
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		else
		{
			o.Emission = 0.5*o.Albedo;
			o.Albedo = half3(0,0,0);
			//o.Smoothness = float3(1,1,1);
		}

		}
		ENDCG
	}
	FallBack "Diffuse"
}
