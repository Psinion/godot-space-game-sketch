shader_type spatial;

uniform float Speed = 0.3f;
uniform float Frequency = 0.5f;
uniform float Amplitude = 1.0f;
uniform sampler2D VertexNoise;

uniform sampler2D ColorNoise;
uniform sampler2D Gradient;
uniform vec4 EmissionColor : source_color = vec4(1, 0.5, 0.1, 1.0);
uniform float EmissionStrength = 10.0f;

uniform sampler2D TransparentNoise;

void vertex() {
	vec3 uv = VERTEX * Frequency;
	float time = TIME * Speed;

	float noiseX = texture(VertexNoise, uv.xy + time).r;
    float noiseY = texture(VertexNoise, uv.xz + time).r;
    float noiseZ = texture(VertexNoise, uv.yz + time).r;

	float noise = (noiseX + noiseY + noiseZ) * 0.33f;

	vec3 displacement = NORMAL * noise * Amplitude;

	VERTEX += displacement;

}

void fragment() {
	vec4 noiseTex = texture(ColorNoise, UV);
	float noiseValue = noiseTex.r;

	vec4 gradientTex = texture(Gradient, vec2(noiseValue));

	ALBEDO = gradientTex.xyz;

	float noiseSmoothed = smoothstep(COLOR.r, 1.0f, noiseValue);
	EMISSION = EmissionColor.xyz * noiseSmoothed * EmissionStrength;

	float transparentNoiseSmoothed = clamp(texture(TransparentNoise, UV).r, 0.0f, 0.9f);
	ALPHA = smoothstep(COLOR.a, 1.0f, transparentNoiseSmoothed);
}