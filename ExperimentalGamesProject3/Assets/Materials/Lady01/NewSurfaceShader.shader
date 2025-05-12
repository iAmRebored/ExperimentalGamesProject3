Shader "Custom/RenderBehindEverything"
{
    SubShader
    {
        Tags { "Queue" = "Geometry-10" } // Draw BEFORE normal geometry
        Pass
        {
            ZWrite On
            ZTest LEqual
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(1.0, 1.0, 1.0, 1.0); // White for testing
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}