Shader "Custom/WaterShorelineFoggy"
{
    Properties
    {
        _ShallowColor ("Shallow Water Color", Color) = (0, 0.5, 1, 0.2)
        _DeepColor ("Deep Fog Color", Color) = (0, 0.1, 0.3, 1)
        _FoamColor ("Foam Color", Color) = (1, 1, 1, 1)
        _ShoreThreshold ("Shore Thickness", Range(0.01, 1.0)) = 0.2
        _WaterDensity ("Water Density (Fog)", Range(0.01, 5.0)) = 1.0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };

            sampler2D _CameraDepthTexture;
            float4 _ShallowColor;
            float4 _DeepColor;
            float4 _FoamColor;
            float _ShoreThreshold;
            float _WaterDensity;

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.pos);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // Calculate Depth
                float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.screenPos)));
                float surfaceZ = i.screenPos.w;
                float depthDiff = sceneZ - surfaceZ;

                // 1. Fog Calculation (Deep Water)
                // As depth increases, we transition to DeepColor
                float fogFactor = saturate(depthDiff / _WaterDensity);
                fixed4 waterCol = lerp(_ShallowColor, _DeepColor, fogFactor);

                // 2. Foam Calculation (The white line)
                float foamLine = 1 - saturate(depthDiff / _ShoreThreshold);
                
                // Final Mix: Water with the foam line on top
                fixed4 finalColor = lerp(waterCol, _FoamColor, foamLine);
                
                return finalColor;
            }
            ENDCG
        }
    }
}