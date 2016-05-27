/*
* Transwalls.shader
* Any render object that has this shader
* attached will occlude any part of
* another object that has the Clipped
* shader attached to it
*
*/
Shader "TransWalls" {

    SubShader{
        Tags {"Queue" = "Geometry+1"}
        ColorMask 0
        Pass{}
    }

}