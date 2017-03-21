 Shader "Custom/Transparent Color" {
     Properties {
         _TransparentColor ("Transparent Color", Color) = (1,1,1,1)
         _MainTex ("Albedo (RGB)", 2D) = "white" {}
         _MaskTex ("Mask (RGB)", 2D) = "white" {}
     }
     SubShader {
         Tags { "Queue"="Transparent" "RenderType"="Transparent" }
         LOD 200
         
         CGPROGRAM
         #pragma surface surf NoLighting
 
         sampler2D _MainTex;
         sampler2D _MaskTex;
 
         struct Input {
             float2 uv_MainTex;
         };

         fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten)
	     {
	         fixed4 c;
	         c.rgb = s.Albedo; 
	         c.a = s.Alpha;
	         return c;
	     }
 
         fixed4 _TransparentColor;
 
         void surf (Input IN, inout SurfaceOutput o) {
            IN.uv_MainTex.y = 1 - IN.uv_MainTex.y;
            IN.uv_MainTex.x = 1 - IN.uv_MainTex.x;

            half4 main = tex2D (_MainTex, IN.uv_MainTex);
            half4 mask = tex2D (_MaskTex, IN.uv_MainTex);

            half3 diff = _TransparentColor.rgb - mask.rgb;
            if (dot(diff,diff) == 0)
            	discard;

            o.Albedo = main.rgb;
            o.Alpha = main.a;
         }
         ENDCG
     } 
     FallBack "Diffuse"
 }