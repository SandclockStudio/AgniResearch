Shader "Custom/Test"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_BumpMap("Bumpmap", 2D) = "bump" {}
	}
	SubShader
	{
		AlphaTest NotEqual 0.0
		
		Pass {

			Tags{ "LightMode" = "ForwardAdd" }
			Blend One One

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;

			struct VertexInput {
				float4 vertex : POSITION;
				float4 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct VertexOutput {
				float4 pos : POSITION;
				float4 color : COLOR;
				float2 uv : TEXCOORD0;
			};

			VertexOutput vert (VertexInput input) {
				VertexOutput output;
				output.pos = mul(UNITY_MATRIX_MVP, input.vertex);
				output.color = input.color;
				output.uv = (float2)input.uv;
				return output;
			}

			float4 frag (VertexInput input) : COLOR {
				float4 diffuseColor = tex2D(_MainTex, input.uv);
				float3 ambientLighting = (float3)UNITY_LIGHTMODEL_AMBIENT * (float3)diffuseColor * (float3)input.color;
				
				return float4(ambientLighting, diffuseColor.a);
			}
			
			ENDCG
		}
		

	}
	
	Fallback "Diffuse"

}
