// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'


Shader "Custom/UI/UVAnimation"
{
	Properties 
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Total ("total", float) = 1
		_Rows ("rows", float) = 1
		_Cols ("cols", float) = 1
		_Fps ("speed", float) = 1
	}

	SubShader 
	{
		Pass
		{
			Tags { "Queue"="Transparent" "RenderType"="Transparent" }
		
			Lighting Off ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			sampler2D _MainTex;

			float _Total;
			float _Rows;
			float _Cols;
			float _Fps;		

			struct v2f {
				float4  pos : POSITION;
				float2  uv : TEXCOORD0;
			} ;

			v2f vert(appdata_base v)
			{			
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				float floorModTime = floor(fmod( _Time.y  * _Fps, _Total));
				float uIdx = fmod(floorModTime , _Cols);
				float vIdx = _Rows - 1 - floor(floorModTime / _Rows);
				o.uv = float2(v.texcoord.x / _Cols + uIdx / _Cols, v.texcoord.y / _Rows + vIdx / _Rows);							
				return o;
			}
			float4 frag(v2f i) : COLOR
			{
				float4 texCol = tex2D(_MainTex,i.uv);			
				return texCol;
			}
			ENDCG	
		}				
	} 

	Fallback "Mobile/VertexLit"
}
