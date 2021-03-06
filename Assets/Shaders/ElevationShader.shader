﻿Shader "Custom/ElevationShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
		_CentrePoint ("Centre", Vector) = (0, 0, 0, 0)
        _HeightMin ("Height Min", Float) = -1
	    _HeightMid ("Height Mid", Float) = 0
        _HeightMax ("Height Max", Float) = 1
        _ColorMin ("Tint Color At Min", Color) = (0,0,0,1)
	    _ColorMid ("Tint Color At Mid", Color) = (0,0,1,1)
        _ColorMax ("Tint Color At Max", Color) = (1,1,1,1)

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;
		        fixed4 _ColorMin;
        fixed4 _ColorMid;
        fixed4 _ColorMax;
        float _HeightMin;
        float _HeightMid;
        float _HeightMax;
        float4 _CentrePoint;

        struct Input
        {
            float2 uv_MainTex;
			float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			float curDistance = distance(_CentrePoint.xyz, IN.worldPos);
			fixed4 tintColor;
			float h = (_HeightMax-curDistance)/(_HeightMax-_HeightMin);

			if(curDistance < _HeightMid){
                tintColor = lerp(_ColorMid.rgba, _ColorMin.rgba, h*20);
            }else{
                tintColor = lerp(_ColorMax.rgba, _ColorMid.rgba, h*20);
            }

            o.Albedo = c.rgb*tintColor.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a * tintColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
