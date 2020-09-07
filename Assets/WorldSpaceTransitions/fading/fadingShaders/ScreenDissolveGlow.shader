Shader "Fading/Surface/ScreenspaceDissolveGlow" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		//_Noise ("noise", 2D) = "white" {}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		//_spread ("dissolveSpread", Range(0,1)) = 1.0

        _GlowIntensity("Glow Intensity", Range(0.0, 5.0)) = 1
        _GlowScale("Glow Size", Range(0.0, 5.0)) = 1.0
        _Glow("Glow Color", Color) = (1, 0, 0, 1)
        _GlowEnd("Glow End Color", Color) = (1, 1, 0, 1)
        _GlowColFac("Glow Colorshift", Range(0.01, 2.0)) = 0.5

		[Toggle] _inverse("inverse", Float) = 0
		[Toggle] _doubleSided ("doubleSided", Float) = 1
		[Toggle(RETRACT_BACKFACES)] _retractBackfaces("retractBackfaces", Float) = 0
		//[HideInInspector][Toggle(DISSOLVE)] _dissolve("dissolveTexture", Float) = 1
		[Toggle(SCREENDISSOLVE_GLOW)] _screenDissolveGlow("glowdissolve", Float) = 1
	}
	SubShader {
		Tags {"Queue" = "Transparent" "RenderType"="Clipping" }
		LOD 200

		// ------------------------------------------------------------------

		Cull off
				
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard addshadow vertex:vert
		//#pragma multi_compile __ CLIP_PLANE CLIP_SPHERE
		#pragma multi_compile __ FADE_PLANE FADE_SPHERE
		#pragma shader_feature RETRACT_BACKFACES
		#pragma shader_feature SCREENDISSOLVE_GLOW
		//#pragma exclude_renderers d3d11 //?
		#include "CGIncludes/section_clipping_CS.cginc"

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			float3 worldPos;
			float myface : VFACE;
			float4 screenPos;
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

		fixed _doubleSided;

		fixed4 _Glow;
        fixed4 _GlowEnd;
		half _GlowScale;
		half _GlowColFac;
		half _GlowIntensity;


		void surf (Input IN, inout SurfaceOutputStandard o) {
			
			//if(IN.myface<0&&_doubleSided==0) discard;
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			#if (FADE_PLANE || FADE_SPHERE)&& SCREENDISSOLVE_GLOW

			fixed4 glowCol = fixed4(0,0,0,0);

			half4 planefade = PLANE_FADE(IN.worldPos);
			half fade = (1-2*_inverse)*planefade.a;

			float2 screenUV = IN.screenPos.xy / IN.screenPos.w;
			screenUV *= float2(_ScreenNoiseScale*_ScreenParams.x/_ScreenNoise_TexelSize.z,_ScreenNoiseScale*_ScreenParams.y/_ScreenNoise_TexelSize.z);
			half f = tex2D (_ScreenNoise, screenUV).r;

			//if(f>=fade) discard;
			if((IN.myface<0&&_doubleSided==0)||(f>=fade)) discard;
			//fade = clamp(fade, 0.0, 1.0);

            //Combine texture factor with geometry coefficient from vertex routine.
            half dFinal = (fade - f);
 
            //Shift the computed raw alpha value based on the scale factor of the glow.
            //Scale the shifted value based on effect intensity.
            half dPredict = (_GlowScale - dFinal) * _GlowIntensity;
 
            //Change colour interpolation by adding in another factor controlling the gradient.
            half dPredictCol = (_GlowScale * _GlowColFac - dFinal) * _GlowIntensity;                      
                       
            //Calculate and clamp glow colour.
            glowCol = dPredict * lerp(_Glow, _GlowEnd, clamp(dPredictCol, 0.0f, 1.0f));
            glowCol = clamp(glowCol, 0.0f, 1.0f);
			o.Albedo = lerp(glowCol, c.rgb, clamp(dFinal, 0.0f, 1.0f));
			#endif
			
			// Metallic and smoothness come from slider variables
			if(IN.myface>0) 
			{
				#if (FADE_PLANE || FADE_SPHERE)&& SCREENDISSOLVE_GLOW
				o.Emission = glowCol;
				#endif
				o.Metallic = _Metallic;
				o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			}
			else
			{		
				//if(_doubleSided==0) discard;
				#if (FADE_PLANE || FADE_SPHERE)&& SCREENDISSOLVE_GLOW
				glowCol.rgb = lerp(0.4*o.Albedo + glowCol.rgb,  0.4*o.Albedo, dFinal);
				glowCol = clamp(glowCol, 0.0f, 1.0f);
				if(dFinal>1) glowCol.rgb = 0.4*o.Albedo;
				o.Emission = glowCol;
				#else
				o.Emission =  0.4*o.Albedo;
				#endif
				o.Albedo = float3(0,0,0);
			}
		}
		ENDCG
	}
	FallBack "Diffuse"
}
