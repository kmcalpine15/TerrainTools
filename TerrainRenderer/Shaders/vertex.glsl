#version 330 core

layout (location = 0) in vec3 aPosition;

uniform mat4 matVP;

void main()
{
    vec4 pos = vec4(aPosition, 1.0f);
    gl_Position = matVP * pos;
}

