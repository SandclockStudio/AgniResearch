Shader "Custom/Burn-Wood"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white"{}
		_BurntTexture("Burn Texture", 2D) = "white"{}
		_BurnGradient("Burn Gradient (RGB)", 2D) = "white"{}
		_BurnMap("Burn Map", 2D) = "white"{}
		
		_NoiseTex("Noise Texture", 2D) = "white"{}

		_NoiseValue("Noise Value", Range(0.01, 2.5)) = 0.8
		_BurnThreshold("Burn Threshold", Range(0.0, 1.0)) = 0.0
		_EdgeWidth("Edge Width", Range(0.01, 0.99)) = 0.05
	}

	SubShader
	{
		CGPROGRAM
		#pragma surface surf Lambert
		
		sampler2D _MainTex;
		sampler2D _NoiseTex;
		sampler2D _BurnMap;
		sampler2D _BurntTexture;
		sampler2D _BurnGradient;

		float _NoiseValue;
		float _BurnThreshold;
		float _EdgeWidth;

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

			// The coordinate to use for the burn gradient
			half2 burnCoord = half2(burnAmount, 0.5);
			
			// The burning gradient
			half3 burningColor = tex2D(_BurnGradient, burnCoord);

			// The burnt texture
			half3 burntColor = tex2D(_BurntTexture, IN.uv_MainTex);

			// Decide whether the point burnt
			int isBurning = int(burnAmount - (_BurnThreshold + _EdgeWidth) + 0.99);
			int isBurnt = int(burnAmount - (_BurnThreshold) + 0.99);

			// Change albedo to the burnt color if the point is burnt, or keep it with the main texture.
			half3 burn = lerp(burningColor, burntColor, isBurning);

			o.Albedo = lerp(o.Albedo, burn, isBurnt);
		}

		ENDCG
	}
}