// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

Shader "Custom1/DisolveShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseTex("Burn Map (RGB)", 2D) = "black"{}
		_BurnGradient("Burn Gradient (RGB)", 2D) = "white"{}
		_DissolveValue("Value", Range(0,1)) = 1.0
		_GradientAdjust("Gradient", Range(0.1,10.0)) = 10.0
		_LargestVal("Largest Value", float) = 1.0
	}
	SubShader
	{

		Tags{ "Queue" = "Transparent" }


		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha
			Cull back
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			
			sampler2D _MainTex;
			sampler2D _NoiseTex;
			sampler2D _BurnGradient;
			float _DissolveValue;
			float _GradientAdjust;
			float _LargestVal;
			float4 _HitPos;

			struct vIN
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};
			struct vOUT
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 hitPos : TEXCOORD1;
				float3 oPos : TEXCOORD2;
			};

			float sqrMagnitude(float3 v)
			{
				return (v.x*v.x + v.y*v.y + v.z*v.z);
			}

			vOUT vert (vIN v)
			{
				vOUT o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				o.oPos = v.vertex;
				o.hitPos = mul(unity_WorldToObject, _HitPos).xyz;
				return o;
			}
			


			fixed4 frag (vOUT i) : COLOR
			{
				fixed4 mainTex = tex2D(_MainTex, i.uv);
				fixed noiseVal = tex2D(_NoiseTex, i.uv).r;



				fixed toPoint = (length(i.oPos.xyz - i.hitPos.xyz) / ((1.0001 - _DissolveValue) * _LargestVal));
				fixed d = ((2.0 * _DissolveValue + noiseVal) * toPoint * noiseVal) - 1.0;

				fixed overOne = saturate(d * _GradientAdjust);

				fixed4 burn = tex2D(_BurnGradient, float2(overOne, 0.5));
				return mainTex * burn;

			}
			ENDCG
		}
	}
}
