Shader "Custom/Fresnel-Alpha-Additive" {
Properties {
	// Define the shader properties and their default values
	_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
	_MainTex ("Texture Sheet", 2D) = "white" {}
}

Category {
	// This is copied from the Unity Particles/Additive shader
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" }
	
	Blend SrcAlpha One // Blend it additively (check out https://en.wikibooks.org/wiki/Cg_Programming/Unity/Transparency)
	Cull Off // Render both sides of faces
	Lighting Off // Don't use light
	ZWrite Off // Don't occlude objects behind
	
	SubShader {

		Pass {
		
			CGPROGRAM
			// Check out https://en.wikibooks.org/wiki/Cg_Programming/Unity/Minimal_Shader
			#pragma vertex vert 
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			// Declare again the properties as variables (otherwise it won't compile)
			sampler2D _MainTex;
			fixed4 _TintColor;

			// Define the structure of the input vertex data (check out https://en.wikibooks.org/wiki/Cg_Programming/Unity/Debugging_of_Shaders)
			struct appdata {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
				float3 normal : NORMAL;
				float4 color : COLOR;
			};

			// Define the structure of the output vertex data
			struct v2f {
				float4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;

				// We use TEXCOORD1 and TEXCOORD2 for this because they are available float3 variables that are not used elsewhere
				float3 normal : TEXCOORD1;
				float3 viewDir : TEXCOORD2;
			};
			
			float4 _MainTex_ST; // I actually don't know why we have to do this

			// This is the vertex shader function
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

            	// Vertex color stays the same
				o.color = v.color;

				return o;
			}

			// This is the fragment shader function
			fixed4 frag (v2f i) : SV_Target
			{
				// Get the dot product of the face's normal direction and the view direction
				// And its absolute so the sign doesn't alter the result
				// And let the alphaValue be 'absolute' to the power of 3 (exponential produces a better visual result than linear)
				float alphaValue = min(1.0, pow(abs(dot(i.viewDir, i.normal)), 3));

				// Calculate the final color
				// ****** 2.0f to enhance the color (beacuse it's additive and otherwise would be too soft)
				// ****** i.color (the vertex color, usually it is not used)
				// ****** _TintColor (the color we want to tint the result with)
				// ****** and finally the shader's texture with whatever it has inside
				fixed4 col = 2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord);
				
				// Now we change the color's alpha value to the one we got before
				col.a = alphaValue;

				// Return the color
				return col;
			}

			ENDCG 
		}
	}	
}
}
