Shader "Custom/NewSurfaceShader"
{
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags { "Queue" = "Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha // 알파 블렌딩 설정

        CGPROGRAM
        #pragma surface surf Lambert alpha:fade // 페이드 알파 사용

        struct Input {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed4 _Color;
        float _Cutoff;

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a * _Cutoff; // 알파 값에 cutoff 적용
        }
        ENDCG
    }
    FallBack "Diffuse"
}