Shader "Custom/UISprite" {
	Properties
	{
	   _MainTex("Base (RGB), Alpha (A)", 2D) = "white" {}

	//Ö§³ÖMask ²Ã¼ôµÄ²¿·Ö
	//Start
	_StencilComp("Stencil Comparison", Float) = 8
	_Stencil("Stencil ID", Float) = 0
	_StencilOp("Stencil Operation", Float) = 0
	_StencilWriteMask("Stencil Write Mask", Float) = 255
	_StencilReadMask("Stencil Read Mask", Float) = 255
	_ColorMask("Color Mask", Float) = 15
		//End
	}

		SubShader
	{
	 LOD 200
	 Tags
	 {
	  "Queue" = "Transparent"
	  "IgnoreProjector" = "True"
	  "RenderType" = "Transparent"
	 }
		//Ö§³ÖMask ²Ã¼ôµÄ²¿·Ö
		//Start
		 Stencil
		 {
		   Ref[_Stencil]
		   Comp[_StencilComp]
		   Pass[_StencilOp]
		   ReadMask[_StencilReadMask]
		   WriteMask[_StencilWriteMask]
		 }
		 ColorMask[_ColorMask]
		//End
		Pass
		{
		 Cull Off
		 Lighting Off
		 ZWrite Off
		 Fog { Mode Off }
		 ColorMask RGB
		 AlphaTest Greater .01
		 Blend SrcAlpha OneMinusSrcAlpha
		 ColorMaterial AmbientAndDiffuse

		 CGPROGRAM
		 #pragma vertex vert
		 #pragma fragment frag
		 #include "UnityCG.cginc"
		 sampler2D _MainTex;
		 struct appdata_t
		 {
		  float4 vertex : POSITION;
		  half4 color : COLOR;
		  float2 texcoord : TEXCOORD0;
		 };
		 struct v2f
		 {
		  float4 vertex : POSITION;
		  half4 color : COLOR;
		  float2 texcoord : TEXCOORD0;
		 };
		 v2f vert(appdata_t v)
		 {
		  v2f o;
		  o.vertex = UnityObjectToClipPos(v.vertex);
		  o.color = v.color;
		  o.texcoord = v.texcoord;
		  return o;
		 }
		 half4 frag(v2f IN) : COLOR
		 {
			 half4 col = tex2D(_MainTex, IN.texcoord) * IN.color;
			 float c = 0.299*col.r + 0.587*col.g + 0.184*col.b;
			 col.r = col.g = col.b = c;
			 return col;
			}
			ENDCG
		   }
	}
}