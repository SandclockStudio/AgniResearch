Shader "Custom/Burn-Wood"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white"{}
		_BurntTexture("Burn Texture", 2D) = "white"{}
		_BurnMap("Burn Map", 2D) = "white"{}
		
		_NoiseTex("Noise Texture", 2D) = "white"{}

		_NoiseValue("Noise Value", Range(0.01, 2.5)) = 0.8
		_BurnThreshold("Burn Threshold", Range(0.0, 1.0)) = 0.0
		_GradientThreshold("Gradient Threshold", Range(0.0, 1.0)) = 0.768
	}

	SubShader
	{
		CGPROGRAM
		#pragma surface surf Lambert
		
		sampler2D _MainTex;
		sampler2D _NoiseTex;
		sampler2D _BurnMap;
		sampler2D _BurntTexture;

		float _NoiseValue;
		float _BurnThreshold;
		float _GradientThreshold;

		struct Input 
		{
 			half2 uv_MainTex;
			half2 uv_NoiseTex;
			half2 uv_BurnMap;
    	};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			// Get color from the main texture
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex);

			// Get the value from the noise texture (0 to 1)
			fixed noise = tex2D(_NoiseTex, IN.uv_NoiseTex).r * _NoiseValue;
			
			// Get the burn amount from the burn map texture
			fixed burnAmount = tex2D(_BurnMap, IN.uv_BurnMap).r * noise;

			// The burn gradient color
			half3 burntColor = tex2D(_BurntTexture, IN.uv_MainTex);

			// Decide whether the point burnt
			int isBurnt = int(burnAmount - (_BurnThreshold) + 0.99);

			// Change albedo to the burnt color if the point is burnt, or keep it with the main texture.
			o.Albedo = lerp(o.Albedo, burntColor, isBurnt);
		}

		ENDCG
	}
}