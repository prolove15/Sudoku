Shader "Custom/TwoSidedPlane" {
    // Properties {
    //     _MainTex ("Texture", 2D) = "white" {}
    // }
    
    // SubShader {
    //     Tags { "RenderType"="Opaque" }
    //     LOD 200
        
    //     Cull Off // Disable backface culling
        
    //     CGPROGRAM
    //     #pragma surface surf Lambert
        
    //     sampler2D _MainTex;
        
    //     struct Input {
    //         float2 uv_MainTex;
    //     };
        
    //     void surf (Input IN, inout SurfaceOutput o) {
    //         // Albedo
    //         float2 flippedUV = IN.uv_MainTex;
    //         flippedUV.x = 1.0 - flippedUV.x; // Flip the X coordinate
    //         flippedUV.y = 1.0 - flippedUV.y; // Flip the Y coordinate
    //         o.Albedo = tex2D(_MainTex, flippedUV).rgb;
            
    //         // Emission
    //         o.Emission = o.Albedo;
    //     }
    //     ENDCG
    // }
    
    // FallBack "Diffuse"

    ////////////////////////////////

     Properties {
        _MainTex ("Texture", 2D) = "white" {}
    }
    
    SubShader {
        Tags { "RenderType"="Opaque" }
        LOD 200
        
        Cull Off // Disable backface culling
        
        CGPROGRAM
        #pragma surface surf Lambert
        
        sampler2D _MainTex;
        
        struct Input {
            float2 uv_MainTex;
        };
        
        void surf (Input IN, inout SurfaceOutput o) {
            // Albedo
            float2 flippedUV = IN.uv_MainTex;
            flippedUV.x = 1.0 - flippedUV.x; // Flip the X coordinate
            flippedUV.y = 1.0 - flippedUV.y; // Flip the Y coordinate
            o.Albedo = tex2D(_MainTex, flippedUV).rgb;
            
            // Check if the surface is facing the front or back
            // if (dot(fixed4(0, 0, 1, 0), o.Normal) > 0) {
            //     // Front side: Use the texture
            //     o.Albedo = tex2D(_MainTex, flippedUV).rgb;
            // } else {
            //     // Back side: Set to white
            //     o.Albedo = fixed3(1, 1, 1);
            // }

            // Emission
            o.Emission = o.Albedo;
            
            // Enable shadows
            o.Alpha = 1.0; // Ensure the object is fully opaque
            o.Specular = 0.0; // Disable specular highlights
            o.Gloss = 0.0; // Disable glossiness
        }
        ENDCG
    }
    
    FallBack "Diffuse"
}