Shader "Hidden/pixelate" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _PixelSize ("Pixel Size", Int) = 16
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            uniform sampler2D _MainTex;
            uniform int _PixelSize;

            struct appdata {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD1;
            };

            v2f vert(appdata v) {
                v2f o;
                o.uv = v.texcoord;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            half4 frag(v2f i) : SV_Target {
                float pixelWidth = 1.0f / _PixelSize;
                float pixelHeight = 1.0f / _PixelSize;
                half2 uv = half2((int)(i.uv.x / pixelWidth) * pixelWidth, (int)(i.uv.y / pixelHeight) * pixelHeight);
                half4 col = tex2D(_MainTex, uv);
                return col;
            }
            ENDCG
        }
    }
}