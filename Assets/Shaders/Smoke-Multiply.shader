Shader "Custom/Black-As-Alpha-Additive" {
Properties {
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("Texture Sheet", 2D) = "white" {}
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }

	SubShader {
		Pass {
			Blend SrcAlpha One
			Cull Off
			ZWrite Off
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			sampler2D _MainTex;
			fixed4 _TintColor;

			struct appdata {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
				float4 color : COLOR;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float3 viewDir : TEXCOORD2;
			};
			
			float4 _MainTex_ST;

			v2f vert (appdata v)
			{
				v2f o;

				// Set vertex position and texture coordinate
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord,_MainTex);

				float4x4 modelMatrix = unity_ObjectToWorld;
            	float4x4 modelMatrixInverse = unity_WorldToObject; 
				
				// Get the normal and the view direction
				o.normal = normalize(
               		mul(float4(v.normal, 0.0), modelMatrixInverse).xyz);
            	o.viewDir = normalize(_WorldSpaceCameraPos 
               		- mul(modelMatrix, v.vertex).xyz);

				o.color = v.color;

				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float alphaValue = min(1.0, pow(abs(dot(i.viewDir, i.normal)), 3));

				fixed4 col = i.color * _TintColor * tex2D(_MainTex, i.texcoord);
				
				col.a = saturate(alphaValue * (log(length(col.xyz) * 50)));

				return col;
			}

			ENDCG 
		}
	}	
}
}
