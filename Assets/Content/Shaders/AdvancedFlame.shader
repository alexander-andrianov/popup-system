Shader "Custom/AdvancedFlame"
{
    Properties
    {
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        [HDR]_Color1A ("Color 1A", Color) = (0.9, 0.4, 0.6, 1)
        [HDR]_Color1B ("Color 1B", Color) = (0.9, 0.7, 0.3, 1)
        [HDR]_Color2A ("Color 2A", Color) = (0.2, 0.6, 0.7, 1)
        [HDR]_Color2B ("Color 2B", Color) = (0.6, 0.8, 0.9, 1)
        [HDR]_Color3A ("Color 3A", Color) = (0.9, 0.4, 0.3, 1)
        [HDR]_Color3B ("Color 3B", Color) = (1.0, 0.8, 0.5, 1)
        [HDR]_Color4A ("Color 4A", Color) = (0.2, 0.3, 0.8, 1)
        [HDR]_Color4B ("Color 4B", Color) = (0.9, 0.6, 0.9, 1)
        
        _FlameSway ("Flame Sway", Range(0, 2)) = 0.7
        _FlameSpeed ("Flame Speed", Range(0, 2)) = 1.0
        _FlameSize ("Flame Size", Range(0.1, 3)) = 1.3
        _FlameWidth ("Flame Width", Range(0.1, 3)) = 10.0
        _FlameIntensity ("Flame Intensity", Range(0, 5)) = 2.0
        _AlphaThreshold ("Alpha Threshold", Range(0, 1)) = 0.1
        _TimeScale ("Time Scale", Range(0, 10)) = 1.0
        [Toggle] _DebugMode ("Debug Mode", Float) = 0
    }
    
    SubShader
    {
        Tags { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_OUTPUT_STEREO
            };
            
            sampler2D _NoiseTex;
            float4 _NoiseTex_ST;
            
            float4 _Color1A, _Color1B, _Color2A, _Color2B, _Color3A, _Color3B, _Color4A, _Color4B;
            float _FlameSway;
            float _FlameSpeed;
            float _FlameSize;
            float _FlameWidth;
            float _FlameIntensity;
            float _AlphaThreshold;
            float _TimeScale;
            float _DebugMode;
            
            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            #define SMOOTHSTEP(a,b,x) smoothstep(a, b, x)
            #define SAMPLE_NOISE(uv) tex2D(_NoiseTex, uv * _NoiseTex_ST.xy + _NoiseTex_ST.zw)
            
            float3 flame(float2 u, float offset, float3 c1, float3 c2, float time)
            {
                float y = SMOOTHSTEP(-0.4, 0.4, u.y);
                float2 baseUV = u * 0.02;
                
                float2 noiseUV1 = baseUV;
                noiseUV1.x += sin(time + offset) * 0.2;
                noiseUV1.y += cos(time * 0.7 + offset) * 0.1;
                float noise1 = SAMPLE_NOISE(frac(noiseUV1)).r;
                
                float2 noiseUV2 = baseUV * 1.5;
                noiseUV2.x += cos(time * 1.2 + offset) * 0.15;
                noiseUV2.y += sin(time * 0.9 + offset) * 0.1;
                float noise2 = SAMPLE_NOISE(frac(noiseUV2)).r;
                
                float finalNoise = (noise1 * 0.7 + noise2 * 0.3);
                
                u += finalNoise * y * float2(_FlameSway, 0.2);
                
                float f = SMOOTHSTEP(0.2, 0.0, length(u) - 0.4);
                f *= SMOOTHSTEP(0.0, 1.0, length(u + float2(0.0, 0.35)));
                
                return f * lerp(c1, c2, y);
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float2 u = (i.uv - 0.5) * float2(_FlameWidth, _FlameSize);
                float time = fmod(_Time.y * _FlameSpeed * 0.5, 6.28318530718);
                
                float3 f1 = flame(u + float2( 3.0, 0.0), 0.1, _Color1A.rgb, _Color1B.rgb, time);
                float3 f2 = flame(u + float2( 1.0, 0.0), 0.2, _Color2A.rgb, _Color2B.rgb, time);
                float3 f3 = flame(u + float2(-1.0, 0.0), 0.3, _Color3A.rgb, _Color3B.rgb, time);
                float3 f4 = flame(u + float2(-3.0, 0.0), 0.4, _Color4A.rgb, _Color4B.rgb, time);
                
                float3 color = (f1 + f2 + f3 + f4) * _FlameIntensity;
                float alpha = max(max(f1.r, f2.r), max(f3.r, f4.r));
                alpha = saturate(alpha - _AlphaThreshold) / (1 - _AlphaThreshold);
                
                return fixed4(color, alpha);
            }
            ENDCG
        }
    }
    FallBack "Transparent/Cutout/Diffuse"
}