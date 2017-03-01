Shader "Custom/Invisible" {
     Properties {
         
     }
     SubShader {
         Tags { "Queue" = "Transparent" } 
         Pass{
             Tags{"LightMode" = "ForwardBase"}
             ZWrite off
             Blend Zero DstAlpha
 
             CGPROGRAM
 
             #pragma vertex vert
             #pragma fragment frag
 
             struct inputVertex{
                 float4 vertex : POSITION;
             };
 
             struct outputVertex{
                 float4 pos : SV_POSITION;
             };
 
             outputVertex vert(inputVertex v){
                 outputVertex o;
                 o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                 return o;
             }
 
             float4 frag(outputVertex i) : COLOR
             {
                 return float4(0,0,0,0);
             }
 
             ENDCG
         }
 
         Pass{
             Tags{"LightMode" = "ForwardAdd"}
             ZWrite off
 
             CGPROGRAM
 
             #pragma vertex vert
             #pragma fragment frag
 
             struct inputVertex{
                 float4 vertex : POSITION;
             };
 
             struct outputVertex{
                 float4 pos : SV_POSITION;
             };
 
             outputVertex vert(inputVertex v){
                 outputVertex o;
                 o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                 return o;
             }
 
             float4 frag(outputVertex i) : COLOR
             {
                 return float4(0,0,0,0);
             }
 
 
             ENDCG
         }
     }
 
 
     FallBack "Diffuse"
 }
 