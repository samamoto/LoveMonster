// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ShowInsides" 
{
	Properties
	{
		_MainTex("Texture",2D)="white"{}
		_FrontColor("Front Color",Color) = (1,1,1,1)
		_BackColor("Back Color",Color) = (1,1,1,0.5)
	}


		SubShader
	{
		Tags
	{
		"RenderType" = "Transparent" "Queue" = "Transparent"
	}

	//半透明
		Cull Off		//両面描画

		CGPROGRAM		//Cg言語で記述開始
#pragma surface surf Lambert alpha
		struct Input
	{
		float2 uv_MainTex;
		//float4 color : COLOR;
	};
		sampler2D _MainTex;
		float4 _BackColor;
	void surf(Input IN,inout SurfaceOutput o)
	{
		//o.Albedo = IN.color.rgb;
		o.Albedo = tex2D(_MainTex,IN.uv_MainTex);// * IN.color.rgb;
		o.Alpha = _BackColor.a;
	}
	ENDCG			//Cg言語で記述終了
	}
	FallBack"Diffuse"
}

//		Cull Front
//		Pass
//		{
//			Tags{"RenderType" = "Transparent" "Queue"="Transparent"}
//			Blend SrcAlpha OneMinusSrcAlpha
//
//			CGPROGRAM
//#pragma vertex vert
//#pragma fragment frag
//			fixed4 _BackColor;
//
//
//			float4 vert(float4 v:POSITION) : SV_POSITION
//			{
//				return UnityObjectToClipPos (v);
//			}
//
//			fixed4 frag() : SV_Target
//			{
//				return _BackColor;
//			}
//			ENDCG
//		}
//
//
//	//影
//		Cull Back
//		Pass
//		{
//				ZWrite On
//			Tags { "RenderType" = "Opaque" "LightMode" = "ForwardBase" "Queue" = "Transparent"}
//			CGPROGRAM
//#pragma vertex vert
//#pragma fragment frag
//#pragma multi_compile_fwdbase
//
//#include "UnityCG.cginc"
//#include "AutoLight.cginc"
//			fixed4 _FrontColor;
//			uniform fixed4 _LightColor0;
//
//			struct v2f
//			{
//				float4 pos      : SV_POSITION;
//				float3 lightDir : TEXCOORD0;
//				float3 normal   : TEXCOORD1;
//				LIGHTING_COORDS(3,4)
//			};
//
//			v2f vert(appdata_base v)
//			{
//				v2f o;
//
//				o.pos      = UnityObjectToClipPos(v.vertex);
//				o.lightDir = normalize(ObjSpaceLightDir(v.vertex));
//				o.normal   = normalize(v.normal).xyz;
//
//				TRANSFER_VERTEX_TO_FRAGMENT(o);
//				TRANSFER_SHADOW(o);
//
//				return o;
//			}
//
//			fixed4 frag(v2f i): SV_Target
//			{
//				fixed atten = LIGHT_ATTENUATION(i);
//				fixed3 ambient = UNITY_LIGHTMODEL_AMBIENT;
//				fixed4 lightColor = _LightColor0 * saturate(dot(i.lightDir,i.normal));
//
//				fixed4 c = _FrontColor;
//				c.rgb = (c * lightColor * atten) + ambient;
//				return c;
//			}
//			ENDCG
//		}
//
//	}
//	Fallback "Diffusse"