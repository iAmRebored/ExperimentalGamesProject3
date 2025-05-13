Shader "Custom/ColorToGreyscaleBlend"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Blend ("Greyscale Blend", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc" // ✅ Required for TRANSFORM_TEX

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Blend;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); // ✅ Now this works
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float grey = dot(col.rgb, float3(0.299, 0.587, 0.114));
                col.rgb = lerp(col.rgb, grey.xxx, _Blend);
                return col;
            }
            ENDCG
        }
    }
}