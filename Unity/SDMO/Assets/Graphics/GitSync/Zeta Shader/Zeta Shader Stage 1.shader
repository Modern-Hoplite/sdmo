  Shader "Zeta Shader/Stage 1 (Diffuse only)" {
  	// Stage 1 : Diffuse toon ramp only
  	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Ramp ("2D-3Channel Toon Ramp", 2D) = "blue" {}
	}
    SubShader {
      Tags { "RenderType" = "Opaque" }
      CGPROGRAM
      #pragma surface surf Ramp//BlinnPhong //Lambert

      sampler2D _Ramp;

      half4 LightingRamp (SurfaceOutput s, half3 lightDir, half atten) {
      	half NdotL = dot(s.Normal, lightDir);
      	half diff = NdotL * 0.5 + 0.5;
      	half3 ramp = tex2D(_Ramp, float2(diff, diff)).bbb;
      	half4 c;
      	c.rgb = s.Albedo * _LightColor0.rgb * ramp;
      	c.a = s.Alpha;
      	return c;
      }

      struct Input {
          //float4 color : COLOR;
          float4 baseC;
      };
      fixed4 _Color;

      void surf (Input IN, inout SurfaceOutput o) {
         o.Albedo = _Color;//tex2D(_Ramp, );//_Color;
         //o.Albedo = o.Albedo;
         //o.Albedo = o.Normal;
      }
      ENDCG
    }
    Fallback "Diffuse"
  }