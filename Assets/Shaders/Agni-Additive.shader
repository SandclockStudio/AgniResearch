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
			
			// Default Unity stuff
			#pragma target 2.0
			#pragma multi_compile_particles
			#pragma multi_compile_fog

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
				

				UNITY_FOG_COORDS(1)
				UNITY_VERTEX_OUTPUT_STEREO
			};
			
			float4 _MainTex_ST; // I actually don't know why we have to do this

			// This is the vertex shader function
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

            	// Vertex color stays the same
				o.color = v.color;

				UNITY_TRANSFER_FOG(o,o.vertex);
				
				return o;
			}

			// This is the fragment shader function
			fixed4 frag (v2f i) : SV_Target
			{
				float3 normalDirection = normalize(i.normal);
				float3 viewDirection = normalize(i.viewDir);

				// Get the dot product of the face's normal direction and the view direction
				float dotProduct = dot(viewDirection, normalDirection);
				// And its absolute so the sign doesn't alter the result
				float absolute = abs(dotProduct);
				// And let the alphaValue be 'absolute' to the power of 3 (exponential produces a better visual result than linear)
				float alphaValue = min(1.0, pow(absolute, 3));

				// Calculate the final color
				// ****** 2.0f to enhance the color (beacuse it's additive and otherwise would be too soft)
				// ****** i.color (the vertex color, usually it is not used)
				// ****** _TintColor (the color we want to tint the result with)
				// ****** and finally the shader's texture with whatever it has inside
				fixed4 col = 2.0f * i.color * _TintColor * tex2D(_MainTex, i.texcoord);
				
				// Now we change the color's alpha value to the one we got before
				col.a = alphaValue;

				// More unity stuff
				UNITY_APPLY_FOG_COLOR(i.fogCoord, col, fixed4(0,0,0,0)); // fog towards black due to our blend mode

				// Return the color
				return col;
			}

			ENDCG 
		}
	}	
}
}
