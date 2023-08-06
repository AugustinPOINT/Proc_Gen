Shader "Unlit/NormalsShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            // use "vert" function as the vertex shader
            #pragma vertex vert
            // use "frag" function as the pixel (fragment) shader
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            // vertex shader inputs
            struct appdata
            {
                float4 vertex : POSITION; // vertex position
                float2 uv : TEXCOORD0; // texture coordinate
                float3 normal : NORMAL; // normal?
            };

            // vertex shader outputs ("vertex to fragment")
            struct v2f
            {
                float2 uv : TEXCOORD0; // texture coordinate
                float3 normal : TEXCOORD1; // normal?
                UNITY_FOG_COORDS(1) // this is a macro to define fog coordinate variable
                float4 vertex : SV_POSITION; // clip space position
};

            // texture we will sample
            sampler2D _MainTex;
            float4 _MainTex_ST;

            // vertex shader
            v2f vert (appdata v)
            {
                v2f o;
                // transform position to clip space
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = v.normal;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            // pixel shader; returns low precision ("fixed4" type)
            float4 frag(v2f i) : SV_Target
            {
                return float4(normalize(i.normal), 1); // The result for each pixel is a texture representing its normal
                //// sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);
                //// apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                //return col; // returns a fixed4?
}
            ENDCG
        }
    }
}
