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

        Blend SrcAlpha OneMinusSrcAlpha // ���� ���� ����

        CGPROGRAM
        #pragma surface surf Lambert alpha:fade // ���̵� ���� ���

        struct Input {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed4 _Color;
        float _Cutoff;

        void surf (Input IN, inout SurfaceOutput o) {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a * _Cutoff; // ���� ���� cutoff ����
        }
        ENDCG
    }
    FallBack "Diffuse"
}