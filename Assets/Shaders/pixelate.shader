Shader "CUSTOM/pixelate" {
    Properties {
        main_tex ("Texture", 2D) = "white" {}
    }

    SubShader {
        Tags {
            "RenderType"="Opaque"
        }

        Cull Off
        ZWrite Off
        ZTest Always

        Pass {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D main_tex;
            float2 block_count;
            float2 block_size;

            fixed4 frag(const v2f_img i) : SV_Target
            {
                const float2 block_pos = floor(i.uv * block_count);
                const float2 block_center = block_pos * block_size + block_size * 0.5f;

                float4 tex = tex2D(main_tex, block_center);
                return tex;
            }
            ENDCG
        }
    }
}