Shader "custom/crt" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        
        LOD 100
        Pass {
            CGPROGRAM
            #include "UnityCG.cginc"
            #pragma vertex vert
            #pragma fragment frag
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            int _MonochormeOnOff;
            float _ScreenJumpLevel;
            float _FlickeringStrength;
            float _FlickeringCycle;
            int _SlippageOnOff;
            float _SlippageStrength;
            float _SlippageInterval;
            float _SlippageScrollSpeed;
            float _SlippageNoiseOnOff;
            float _SlippageSize;
            float _ChromaticAberrationStrength;
            int _ChromaticAberrationOnOff;
            int _MultipleGhostOnOff;
            float _MultipleGhostStrength;
            float GetRandom(float x);
            float EaseIn(float t0, float t1, float t);

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                float2 uv = i.uv;
                // Slippage
                float scrollSpeed = _Time.x * _SlippageScrollSpeed;
                float slippageMask = pow(abs(sin(i.uv.y * _SlippageInterval + scrollSpeed)), _SlippageSize);
                float stepMask = round(sin(i.uv.y * _SlippageInterval + scrollSpeed - 1));
                uv.x = uv.x + (_SlippageNoiseOnOff * _SlippageStrength * slippageMask * stepMask) * _SlippageOnOff;

                // Chromatic Aberration
                float red = tex2D(_MainTex, float2(uv.x - _ChromaticAberrationStrength * _ChromaticAberrationOnOff,uv.y)).r;
                float green = tex2D(_MainTex, float2(uv.x, uv.y)).g;
                float blue = tex2D(_MainTex, float2(uv.x + _ChromaticAberrationStrength * _ChromaticAberrationOnOff,uv.y)).b;
                float4 color = float4(red, green, blue, 1);
                
                // Multiple Ghost
                float4 ghost1st = tex2D(_MainTex, uv - float2(1, 0) * _MultipleGhostStrength * _MultipleGhostOnOff);
                float4 ghost2nd = tex2D(_MainTex, uv - float2(1, 0) * _MultipleGhostStrength * 2 * _MultipleGhostOnOff);
                color = color * 0.8 + ghost1st * 0.15 + ghost2nd * 0.05;
                
                // Monochorome
                if (_MonochormeOnOff == 1) {
                    color.xyz = 0.299f * color.r + 0.587f * color.g + 0.114f * color.b;
                }
                return color;
            }

            float GetRandom(float x) {
                return frac(sin(dot(x, float2(12.9898, 78.233))) * 43758.5453);
            }
            float EaseIn(float t0, float t1, float t) {
                return 2.0 * smoothstep(t0, 2.0 * t1 - t0, t);
            }
            ENDCG
        }
    }
}