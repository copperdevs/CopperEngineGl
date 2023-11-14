#version 330 core
in vec2 fUv;
out vec4 FragColor;

uniform vec3 bottomColor;
uniform vec3 middleColor;
uniform vec3 topColor;

uniform float horizon1;
uniform float horizon2;

float smoothstepHermite(float edge0, float edge1, float x) {
    float t = clamp((x - edge0) / (edge1 - edge0), 0.0, 1.0);
    return t * t * (3.0 - 2.0 * t);
}

void main()
{
    float t = fUv.y;

    // Use Hermite interpolation for smoother transitions
    float gradient1 = smoothstepHermite(horizon1, horizon1 + 0.02, t);
    float gradient2 = smoothstepHermite(horizon2, horizon2 + 0.02, t);

    vec3 endColor = mix(bottomColor, middleColor, gradient1);
    endColor = mix(endColor, topColor, gradient2);

    FragColor = vec4(endColor, 1.0);
}
