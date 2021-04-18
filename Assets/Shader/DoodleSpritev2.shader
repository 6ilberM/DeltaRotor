Shader "Custom/Sprites/DoodleUV"
{
    //Author: Gilberto Moreno, @6ilberM
    //of Idea UV Scrolling On BumpMap
    
    //This shader Requires Further Study
    
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" { }
        [PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" { }
        _ScrollMap ("Normal Map", 2D) = "bump" { }
        
        _Color ("Tint", Color) = (1, 1, 1, 1)
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1, 1, 1, 1)
        [HideInInspector] _Flip ("Flip", Vector) = (1, 1, 1, 1)
        [PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
        _Seed ("Seed", Float) = 43758.5453
        _NoiseScale ("Scale of displacement", Float) = 0.0876
        _NoiseSnap ("Time per Second", Range(0, 2)) = 0.5
    }
    
    SubShader
    {
        Tags { "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" "PreviewType" = "Plane" "CanUseSpriteAtlas" = "True" }
        
        Cull Off
        Lighting Off
        ZWrite Off
        Blend One OneMinusSrcAlpha
        
        CGPROGRAM
        
        #pragma surface surf Lambert vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
        #pragma multi_compile_local _ PIXELSNAP_ON
        #pragma multi_compile _ ETC1_EXTERNAL_ALPHA
        #include "UnitySprites.cginc"
        
        float _NoiseScale;
        float _NoiseSnap;
        float _Seed;
        
        sampler2D _ScrollMap;
        
        float2 rand(float2 co)
        {
            return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * _Seed);
        }
        
        struct Input
        {
            float2 uv_MainTex;
            fixed4 color;
        };
        inline float sqr(float v)
        {
            return(v * v);
        }
        
        inline float snap(float x, float snap)
        {
            return snap * round(x / snap);
        }
        
        void vert(inout appdata_full v, out Input o)
        {
            v.vertex = UnityFlipSprite(v.vertex, _Flip);
            // float3 time = snap(_Time.y, _NoiseSnap);
            // float2 noise = rand(v.vertex.xyz + time).xy * _SinTime * _NoiseScale;
            // v.vertex.xy += noise;
            #if defined(PIXELSNAP_ON)
                v.vertex = UnityPixelSnap(v.vertex);
            #endif
            
            UNITY_INITIALIZE_OUTPUT(Input, o);
            o.color = v.color * _Color * _RendererColor;
        }
        
        void surf(Input IN, inout SurfaceOutput o)
        {
            float2 time = snap(_Time.y, _NoiseSnap);
            float2 scrolledUV = IN.uv_MainTex;
            //Create variables that store the individual x and y
            // //components for the uv's scaled by time
            fixed xScrollValue = _NoiseScale * rand(IN.uv_MainTex + time);
            fixed yScrollValue = _NoiseScale * rand(IN.uv_MainTex + time);
            
            scrolledUV += fixed2(xScrollValue, yScrollValue);
            fixed4 c = SampleSpriteTexture(scrolledUV) * IN.color;
            o.Albedo = c.rgb * c.a;
            o.Alpha = c.a;
        }
        ENDCG
        
    }
    
    Fallback "Transparent/VertexLit"
}