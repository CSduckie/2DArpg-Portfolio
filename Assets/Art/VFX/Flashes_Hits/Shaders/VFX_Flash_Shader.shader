// Upgrade NOTE: upgraded instancing buffer 'VFX_Flash_Shader' to new syntax.

// Made with Amplify Shader Editor v1.9.3.2
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "VFX_Flash_Shader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_R_x_position("R_x_position", Range( -1 , 1)) = 0
		_R_y_position("R_y_position", Range( -1 , 1)) = 0
		_G_x_position("G_x_position", Range( -1 , 1)) = 0
		_G_y_position("G_y_position", Range( -1 , 1)) = 0
		_B_x_position("B_x_position", Range( -1 , 1)) = 0
		_B_y_position("B_y_position", Range( -1 , 1)) = 0
		_Texture0("Texture 0", 2D) = "white" {}
		_Emissive("Emissive", Float) = 1
		_DepthFade("Depth Fade", Float) = 2

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One One
		
		
		Pass
		{
		CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			#define ASE_NEEDS_FRAG_COLOR
			#define ASE_NEEDS_VERT_POSITION
			#pragma multi_compile_instancing


			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform sampler2D _Texture0;
			uniform float _Emissive;
			UNITY_DECLARE_DEPTH_TEXTURE( _CameraDepthTexture );
			uniform float4 _CameraDepthTexture_TexelSize;
			uniform float _DepthFade;
			UNITY_INSTANCING_BUFFER_START(VFX_Flash_Shader)
				UNITY_DEFINE_INSTANCED_PROP(float, _R_x_position)
#define _R_x_position_arr VFX_Flash_Shader
				UNITY_DEFINE_INSTANCED_PROP(float, _R_y_position)
#define _R_y_position_arr VFX_Flash_Shader
				UNITY_DEFINE_INSTANCED_PROP(float, _G_x_position)
#define _G_x_position_arr VFX_Flash_Shader
				UNITY_DEFINE_INSTANCED_PROP(float, _G_y_position)
#define _G_y_position_arr VFX_Flash_Shader
				UNITY_DEFINE_INSTANCED_PROP(float, _B_x_position)
#define _B_x_position_arr VFX_Flash_Shader
				UNITY_DEFINE_INSTANCED_PROP(float, _B_y_position)
#define _B_y_position_arr VFX_Flash_Shader
			UNITY_INSTANCING_BUFFER_END(VFX_Flash_Shader)

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				float4 ase_clipPos = UnityObjectToClipPos(IN.vertex);
				float4 screenPos = ComputeScreenPos(ase_clipPos);
				OUT.ase_texcoord1 = screenPos;
				float3 objectToViewPos = UnityObjectToViewPos(IN.vertex.xyz);
				float eyeDepth = -objectToViewPos.z;
				OUT.ase_texcoord2.x = eyeDepth;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				OUT.ase_texcoord2.yzw = 0;
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float _R_x_position_Instance = UNITY_ACCESS_INSTANCED_PROP(_R_x_position_arr, _R_x_position);
				float2 texCoord2 = IN.texcoord.xy * float2( 1,1 ) + float2( 0,0 );
				float temp_output_3_0 = (texCoord2).x;
				float _R_y_position_Instance = UNITY_ACCESS_INSTANCED_PROP(_R_y_position_arr, _R_y_position);
				float temp_output_11_0 = (texCoord2).y;
				float2 appendResult14 = (float2(( _R_x_position_Instance + temp_output_3_0 ) , ( _R_y_position_Instance + temp_output_11_0 )));
				float _G_x_position_Instance = UNITY_ACCESS_INSTANCED_PROP(_G_x_position_arr, _G_x_position);
				float _G_y_position_Instance = UNITY_ACCESS_INSTANCED_PROP(_G_y_position_arr, _G_y_position);
				float2 appendResult20 = (float2(( _G_x_position_Instance + temp_output_3_0 ) , ( _G_y_position_Instance + temp_output_11_0 )));
				float _B_x_position_Instance = UNITY_ACCESS_INSTANCED_PROP(_B_x_position_arr, _B_x_position);
				float _B_y_position_Instance = UNITY_ACCESS_INSTANCED_PROP(_B_y_position_arr, _B_y_position);
				float2 appendResult24 = (float2(( _B_x_position_Instance + temp_output_3_0 ) , ( _B_y_position_Instance + temp_output_11_0 )));
				float4 appendResult15 = (float4(tex2D( _Texture0, appendResult14 ).r , tex2D( _Texture0, appendResult20 ).g , tex2D( _Texture0, appendResult24 ).b , 0.0));
				float4 screenPos = IN.ase_texcoord1;
				float4 ase_screenPosNorm = screenPos / screenPos.w;
				ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
				float screenDepth39 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE( _CameraDepthTexture, ase_screenPosNorm.xy ));
				float distanceDepth39 = ( screenDepth39 - LinearEyeDepth( ase_screenPosNorm.z ) ) / ( _DepthFade );
				float eyeDepth = IN.ase_texcoord2.x;
				float cameraDepthFade56 = (( eyeDepth -_ProjectionParams.y - 0.0 ) / 1.0);
				float4 appendResult29 = (float4(saturate( ( ( IN.color * appendResult15 ) * _Emissive ) ).rgb , saturate( ( ( IN.color.a * distanceDepth39 ) * cameraDepthFade56 ) )));
				
				fixed4 c = appendResult29;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19302
Node;AmplifyShaderEditor.TextureCoordinatesNode;2;-669.6569,276.4724;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;5;-21.74466,-80.57446;Inherit;False;InstancedProperty;_R_x_position;R_x_position;0;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-12.95514,35.98147;Inherit;False;InstancedProperty;_R_y_position;R_y_position;1;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;16;4.916059,198.1093;Inherit;False;InstancedProperty;_G_x_position;G_x_position;2;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;19;13.70564,314.6651;Inherit;False;InstancedProperty;_G_y_position;G_y_position;3;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;3;-408.9993,255.0711;Inherit;False;True;False;True;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ComponentMaskNode;11;-406.3815,342.9414;Inherit;False;False;True;True;True;1;0;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;27;25.88643,581.0249;Inherit;False;InstancedProperty;_B_y_position;B_y_position;5;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;26;17.09685,464.4689;Inherit;False;InstancedProperty;_B_x_position;B_x_position;4;0;Create;True;0;0;0;False;0;False;0;0;-1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;6;275.3317,-57.30586;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;8;284.1211,59.25008;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;17;301.9924,221.3779;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;18;310.7818,337.9337;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;22;314.173,487.7375;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;23;322.9624,604.2933;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;13;-749.4034,-302.4899;Inherit;True;Property;_Texture0;Texture 0;6;0;Create;True;0;0;0;False;0;False;f99a508ed45a4b84793c6920052488f9;f99a508ed45a4b84793c6920052488f9;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.DynamicAppendNode;14;403.698,-18.36169;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;20;430.3586,260.3219;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;24;442.5394,526.6816;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;42;1037.733,459.8892;Inherit;False;Property;_DepthFade;Depth Fade;8;0;Create;True;0;0;0;False;0;False;2;2;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;21;635.7043,210.3646;Inherit;True;Global;Texture1;Texture;0;0;Create;True;0;0;0;False;0;False;-1;f99a508ed45a4b84793c6920052488f9;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;1;652.2252,-56.5611;Inherit;True;Global;Texture;Texture;0;0;Create;True;0;0;0;False;0;False;-1;f99a508ed45a4b84793c6920052488f9;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;25;647.885,476.7244;Inherit;True;Global;Texture2;Texture;0;0;Create;True;0;0;0;False;0;False;-1;f99a508ed45a4b84793c6920052488f9;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;28;1034.078,-177.0199;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.DepthFade;39;1232.164,411.743;Inherit;False;True;False;False;2;1;FLOAT3;0,0,0;False;0;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;15;1052.971,109.8522;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;43;1590.399,380.46;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.CameraDepthFade;56;1481.474,514.9551;Inherit;False;3;2;FLOAT3;0,0,0;False;0;FLOAT;1;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;1236.037,-19.66393;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT4;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;32;1211.217,177.4207;Inherit;False;Property;_Emissive;Emissive;7;0;Create;True;0;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;48;1787.721,370.0903;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;1552.925,70.47509;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;34;1870.732,25.60231;Inherit;False;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SaturateNode;41;2069.878,218.6619;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleSubtractOpNode;40;1838.118,192.0693;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;29;2137.286,71.29469;Inherit;False;FLOAT4;4;0;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;31;2475.385,67.50301;Float;False;True;-1;2;ASEMaterialInspector;0;10;VFX_Flash_Shader;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;True;True;4;1;False;;1;False;;0;4;False;;1;False;;False;False;False;False;False;False;False;False;False;False;False;True;True;2;False;;False;False;False;False;False;False;False;False;False;False;True;True;2;False;;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;3;0;2;0
WireConnection;11;0;2;0
WireConnection;6;0;5;0
WireConnection;6;1;3;0
WireConnection;8;0;7;0
WireConnection;8;1;11;0
WireConnection;17;0;16;0
WireConnection;17;1;3;0
WireConnection;18;0;19;0
WireConnection;18;1;11;0
WireConnection;22;0;26;0
WireConnection;22;1;3;0
WireConnection;23;0;27;0
WireConnection;23;1;11;0
WireConnection;14;0;6;0
WireConnection;14;1;8;0
WireConnection;20;0;17;0
WireConnection;20;1;18;0
WireConnection;24;0;22;0
WireConnection;24;1;23;0
WireConnection;21;0;13;0
WireConnection;21;1;20;0
WireConnection;1;0;13;0
WireConnection;1;1;14;0
WireConnection;25;0;13;0
WireConnection;25;1;24;0
WireConnection;39;0;42;0
WireConnection;15;0;1;1
WireConnection;15;1;21;2
WireConnection;15;2;25;3
WireConnection;43;0;28;4
WireConnection;43;1;39;0
WireConnection;30;0;28;0
WireConnection;30;1;15;0
WireConnection;48;0;43;0
WireConnection;48;1;56;0
WireConnection;33;0;30;0
WireConnection;33;1;32;0
WireConnection;34;0;33;0
WireConnection;41;0;48;0
WireConnection;40;0;39;0
WireConnection;40;1;28;4
WireConnection;29;0;34;0
WireConnection;29;3;41;0
WireConnection;31;0;29;0
ASEEND*/
//CHKSM=FBD758A6F2D82DB2A083A730F05687AC2348EF01