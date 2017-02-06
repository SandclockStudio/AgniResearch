Shader "Custom/Burn-Unlit"
{
	Properties
	{
		_MainTex("Main Texture", 2D) = "white"{}
		_BurnMap("Burn Map", 2D) = "white"{}
		
		_DissolveVal("Burn Value", Range(-0.2, 1.2)) = 1.2
		_EdgeWidth("Line Width", Range(0.0, 0.2)) = 0.05
		_EdgeColor("Line Color", Color) = (1.0, 0.2, 0.0, 1.0)
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
		sampler2D _BurnMap;
		
		float4 _EdgeColor;
		float _DissolveVal;
		float _EdgeWidth;
		
		struct Input 
		{
 			half2 uv_MainTex;
 			half2 uv_BurnMap;
    	};

		void surf (Input IN, inout SurfaceOutput o) 
		{
			o.Albedo = tex2D(_MainTex, IN.uv_MainTex);

			half4 dissolve = tex2D(_BurnMap, IN.uv_BurnMap);
			
			// Transparent color for clear point
			half4 clear = half4(0.0, 0.0, 0.0, 0.0);

			// Decide whether the point is clear, edge, or regular
			int isClear = int(dissolve.r - (_DissolveVal + _EdgeWidth) + 0.99);
			int isAtLeastEdge = int(dissolve.r - (_DissolveVal) + 0.99);


			// half3 modifier = half3(sin(36.0) * o.Albedo.r * 1.1, sin(10.0) * o.Albedo.r / 3, 0.0);

			// Set the alternate color to the edge color, or to transparent if it's clear 
			half3 altCol = lerp(_EdgeColor, clear, isClear);
			
			// Set the albeo to the texture, or to the alternate color if it's edge or it's clear
			o.Albedo = lerp(o.Albedo, altCol, isAtLeastEdge);
			
			// Set alpha to 1 if it's not clear, or to 0 if it is clear
			o.Alpha = lerp(1.0, 0.0, isClear);
		}

		ENDCG
	}
}