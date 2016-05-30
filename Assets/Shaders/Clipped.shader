/*Clipped.shader
* When attached to a render object this shader
* will cause the object to be occluded by any
* other render object that has the TransWall
* shader attached to it.
*/
Shader "Clipped" {
Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType"="Opaque" "Queue" = "Geometry+2" }
        LOD 200
        CGPROGRAM
        #pragma surface surf Lambert
        sampler2D _MainTex;
        struct Input {
            float2 uv_MainTex;
        };
        void surf (Input IN, inout SurfaceOutput o) {
            half4 c = tex2D (_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;
            o.Alpha = c.a;
}
ENDCG }
    FallBack "Diffuse"
}