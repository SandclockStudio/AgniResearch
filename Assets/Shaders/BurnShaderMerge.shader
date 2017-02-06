Shader "Custom/Paper-Burn"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white"{}
		_BurnMap("Burn Map", 2D) = "white"{}
		_NoiseTex("Noise Texture", 2D) = "white"{}
		_BurnGradient("Burn Gradient (RGB)", 2D) = "white"{}
		
		_BurnValue("Burn Value", Range(0.0, 1.0)) = 0.5
		_GradientThreshold("Gradient Threshold", Range(0.0, 1.0)) = 0.75
	}

	SubShader
	{
		Tags { "Queue"="Transparent" "RenderType"="Transparent"}
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		AlphaTest Greater 0.75 

		CGPROGRAM
		#pragma surface surf Lambert alpha
		
		sampler2D _MainTex;
		sampler2D _NoiseTex;
		sampler2D _BurnMap;
		sampler2D _BurnGradient;

		float _BurnValue;
		float _GradientThreshold;

		struct Input 
		{
 			half2 uv_MainTex;
			half2 uv_NoiseTex;
			half2 uv_BurnMap;
    	};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex);

			fixed noise = tex2D(_NoiseTex, IN.uv_NoiseTex).r; 
			fixed dissolve = tex2D(_BurnMap, IN.uv_BurnMap).r * noise;

			half2 burnCoord = half2(dissolve, 0.5);

			half3 burn = tex2D(_BurnGradient, burnCoord);

			// Decide whether the point is clear, edge, or regular
			int isBurnt = int(dissolve - (_BurnValue) + 0.99);

			// Set the albeo to the texture, or to the alternate color if it's edge or it's clear
			o.Albedo = lerp(o.Albedo, burn, isBurnt);
			
			// Set alpha to 1 if it's not clear, or to 0 if it is clear
			o.Alpha = lerp(1.0, 0.0, int(dissolve + _GradientThreshold));
		}

		ENDCG
	}
}