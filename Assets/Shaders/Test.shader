Shader "Custom/Test" {
Properties {
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("Texture Sheet", 2D) = "white" {}
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
	Blend SrcAlpha One
	Cull Off
	Lighting Off
	ZWrite Off
	AlphaTest Greater 0.01
	
	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_particles
			#pragma multi_compile_fog

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
				UNITY_FOG_COORDS(1)
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			float4 _MainTex_ST;

			v2f vert (appdata v)
			{
				v2f o;

				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

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

				UNITY_TRANSFER_FOG(o,o.vertex);
				
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float3 normalDirection = normalize(i.normal);
				float3 viewDirection = normalize(i.viewDir);

				float dotProduct = dot(viewDirection, normalDirection);
				float absolute = abs(dotProduct);
				float alphaValue = min(1.0, pow(absolute, 3));

				fixed4 tex = tex2D(_MainTex, i.texcoord);

				fixed4 col = i.color * _TintColor * tex;
				/*
				if (col.r > 0.1) {
					col.r = i.vertex.y / 300;
					col.g = i.vertex.y / 400;
					col.b = i.vertex.y / 300;
				}
				col *= tex;				
				*/

				col.a = alphaValue;

				UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0)); // fog towards black due to our blend mode

				return col;
			}

			ENDCG 
		}
	}	
}
}
