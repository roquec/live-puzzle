Shader "Puzzle Alpha Mask" {
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Top ("Alpha Top (A)", 2D) = "white" {}
        _Right ("Alpha Right (A)", 2D) = "white" {}
        _Bottom ("Alpha Bottom (A)", 2D) = "white" {}
        _Left ("Alpha Left (A)", 2D) = "white" {}
    }
    SubShader {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}
       
        ZWrite Off
       
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask RGB
       
        Pass {
            SetTexture[_MainTex] {
                Combine texture
            }
            SetTexture[_Top] {
                Combine previous + texture, texture
            }
            SetTexture[_Right] {
                Combine previous + texture, texture * previous
            }
            SetTexture[_Bottom] {
                Combine previous + texture, texture * previous
            }
            SetTexture[_Left] {
                Combine previous + texture, texture * previous
            }
        }
    }
}