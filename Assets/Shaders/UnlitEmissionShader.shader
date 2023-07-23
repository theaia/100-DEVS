Shader "Unlit/Transparent Cutout with Emission" {
    Properties {
        _MainTex ("Base (RGB) Trans (A)", 2D) = "white" { }
        _Cutoff ("Alpha cutoff", Range(0.000000,1.000000)) = 0.500000
        _EmissionColor ("Emission Color", Color) = (0, 0, 0, 1)
    }
    SubShader { 
        LOD 100
        Tags { "QUEUE"="AlphaTest" "IGNOREPROJECTOR"="true" "RenderType"="TransparentCutout" }

        Pass {
            Tags { "QUEUE"="AlphaTest" "IGNOREPROJECTOR"="true" "RenderType"="TransparentCutout" }
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float4 emission : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Cutoff;
            float4 _EmissionColor;

            v2f vert (appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.emission = _EmissionColor;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                fixed4 col = tex2D(_MainTex, i.texcoord);
                clip(col.a - _Cutoff);

                fixed4 emissionColor = i.emission * col;
                col.rgb += emissionColor.rgb;
                col.a = max(col.a, emissionColor.a);
                return col;
            }
            ENDCG
        }
    }
}
